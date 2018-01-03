using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace Tiveria.Common.Controls
{
    public class ImageMap : ScrollableControl
    {
        #region private members
        private bool _AutoZoom;
        private Image _Image;
        float _Zoom;
        private GraphicsPath _OriginalPath;
        private GraphicsPath _ScaledPath;
        private GraphicsPath _CurrentPath;
        private GraphicsPath _ManualHighlightPath;
        private ArrayList _Keys;
        private Graphics _Graphics;
        private int _CurrentIndex;
        private bool _MouseDownInRegion;
        private int _ManualHighlightIndex;
        private Timer _ManualHighlightTimer;
        #endregion

        #region Public properties
        [DefaultValue(typeof(Color), "Red")]
        [Category("Appearance"), Description("The fill color of the region when highlighted")]
        public Color HighlightFillColor { get; set; }
        [DefaultValue(typeof(Color), "Red")]
        [Category("Appearance"), Description("The border color of the region when highlighted")]
        public Color HighlightBorderColor { get; set; }

        [DefaultValue(typeof(Color), "Blue")]
        [Category("Appearance"), Description("The fill color of the region when clicked")]
        public Color ClickFillColor { get; set; }
        [DefaultValue(typeof(Color), "Blue")]
        [Category("Appearance"), Description("The border color of the region when clicked")]
        public Color ClickBorderColor { get; set; }

        [DefaultValue(typeof(Color), "Green")]
        [Category("Appearance"), Description("The fill color of the region when manually blinking")]
        public Color BlinkFillColor { get; set; }
        [DefaultValue(typeof(Color), "Green")]
        [Category("Appearance"), Description("The border color of the region when manually blinking")]
        public Color BlinkBorderColor { get; set; }

        [DefaultValue(128)]
        [Category("Appearance"), Description("The alpha channel used for all fill operations")]
        public byte FillTransparency { get; set; }

        [DefaultValue(null)]
        [Category("Appearance"), Description("The image to be displayed")]
        public Image Image
        {
            get { return _Image; }
            set
            {
                if (_Image == value)
                    return;

                _Image = value;
                UpdateAndRedraw();
            }
        }

        [DefaultValue(true)]
        [Category("Appearance"), Description("Automatic Zoom mode on or off. If on, the zoom property is ignored.")]
        public bool AutoZoom
        {
            get
            {
                return _AutoZoom;
            }
            set
            {
                if (_AutoZoom == value)
                    return;

                _AutoZoom = value;
                UpdateAndRedraw();
            }
        }

        [DefaultValue(1.0f)]
        [Category("Appearance"), Description("The zoom factor. Less than 1 to reduce. More than 1 to magnify.")]
        public float Zoom
        {
            get { return _Zoom; }
            set
            {
                if (_Zoom == value || AutoZoom)
                    return;

                if (value < 0 || value < 0.001 || value > 25)
                    value = 0.001f;

                if (value > 25)
                    value = 25f;

                _Zoom = value;
                UpdateAndRedraw();
            }
        }

        [DefaultValue(typeof(Cursors), "Hand")]
        [Category("Appearance"), Description("The cursor shown when the mouse is over a region.")]
        public Cursor RegionCursor { get; set; }

        [DefaultValue(InterpolationMode.HighQualityBicubic)]
        [Category("Appearance"), Description("The interpolation mode used when zooming.")]
        public InterpolationMode InterpolationMode { get; set; }

        [DefaultValue(false)]
        [Category("Appearance"), Description("Show a frame arround all defined regions.")]
        public bool ShowRegions { get; set; }

        [DefaultValue(typeof(Color), "Yellow")]
        [Category("Appearance"), Description("The border color of all regions when ShowRegions is true.")]
        public Color RegionsBorderColor { get; set; }

        #endregion

        #region public events
        public event EventHandler<RegionEventArgs> RegionEnter;
        public event EventHandler<RegionEventArgs> RegionLeave;
        public event EventHandler<RegionEventArgs> RegionClick;
        #endregion

        #region Event Triggers
        protected virtual void OnRegionEnter(object sender, RegionEventArgs e)
        {
            EventHandler<RegionEventArgs> handler = RegionEnter;
            if (handler != null)
                handler(sender, e);
        }

        protected virtual void OnRegionLeave(object sender, RegionEventArgs e)
        {
            EventHandler<RegionEventArgs> handler = RegionLeave;
            if (handler != null)
                handler(sender, e);
        }

        protected virtual void OnRegionClick(object sender, RegionEventArgs e)
        {
            EventHandler<RegionEventArgs> handler = RegionClick;
            if (handler != null)
                handler(sender, e);
        }
        #endregion

        #region Initialize & Dispose
        public ImageMap()
        {
            _Image = null;
            _Zoom = 1.0f;
            _Keys = new ArrayList();
            _CurrentIndex = -1;
            _Graphics = Graphics.FromHwnd(Handle);
            _MouseDownInRegion = false;
            _ManualHighlightIndex = -1;
            _ManualHighlightPath = null;
            _ManualHighlightTimer = new Timer();
            _ManualHighlightTimer.Interval = 2500;
            _ManualHighlightTimer.Tick += _ManualHighlightTimer_Tick;
            _ManualHighlightTimer.Start();

            SetStyle(ControlStyles.AllPaintingInWmPaint |
              ControlStyles.UserPaint |
              ControlStyles.ResizeRedraw |
              ControlStyles.UserPaint |
              ControlStyles.DoubleBuffer, true);

            AutoScroll = true;
            RegionCursor = Cursors.Hand;
            HighlightFillColor = HighlightBorderColor = Color.Red;
            ClickFillColor = ClickBorderColor = Color.Blue;
            BlinkFillColor = BlinkBorderColor = Color.Green;
            RegionsBorderColor = Color.Yellow;

            ClearRegions();
            AutoZoom = true;
        }

        protected override void Dispose(bool disposing)
        {
            _Keys = null;
            _CurrentPath = null;
            _OriginalPath = null;
            _ScaledPath = null;
            base.Dispose(disposing);
        }
        #endregion

        #region Map Management Interface
        public int AddElipse(string key, Point center, int radius)
        {
            return this.AddElipse(key, center.X, center.Y, radius);
        }

        public int AddElipse(string key, int x, int y, int radius)
        {
            if (_Keys.Count > 0)
                _OriginalPath.SetMarkers();
            _OriginalPath.AddEllipse(x - radius, y - radius, radius * 2, radius * 2);
            UpdatePath();
            return _Keys.Add(key);
        }

        public int AddRectangle(string key, int x1, int y1, int x2, int y2)
        {
            return AddRectangle(key, new Rectangle(x1, y1, (x2 - x1), (y2 - y1)));
        }

        public int AddRectangle(string key, Rectangle rectangle)
        {
            if (_Keys.Count > 0)
                _OriginalPath.SetMarkers();
            _OriginalPath.AddRectangle(rectangle);
            UpdatePath();
            return _Keys.Add(key);
        }

        public void ClearRegions()
        {
            _Keys.Clear();
            _OriginalPath = new GraphicsPath();
            UpdatePath();
        }

        [Browsable(false)]
        public int RegionCount { get { return _Keys.Count; } }
        #endregion

        #region Manual Highlighting
        public void HighlightRegion(string key, int durationMS = 500)
        {
            var idx = _Keys.IndexOf(key);
            HighlightRegion(idx, durationMS);
        }

        public void HighlightRegion(int index, int durationMS = 500)
        {
            if (index >= _Keys.Count)
                return;

            ExecuteThreadSafeAndAsync(() =>
            {
                _ManualHighlightTimer.Stop();
                _ManualHighlightIndex = index;
                _ManualHighlightPath = GetPathForIndex(index);
                _ManualHighlightTimer.Interval = durationMS;
                _ManualHighlightTimer.Start();
                Invalidate();
            });
        }

        private GraphicsPath GetPathForIndex(int index)
        {
            var path = new GraphicsPath();
            GraphicsPathIterator iterator = new GraphicsPathIterator(_OriginalPath);
            iterator.Rewind();
            for (int i = 0; i <= index; i++)
                iterator.NextMarker(path);
            return path;
        }

        void _ManualHighlightTimer_Tick(object sender, EventArgs e)
        {
            _ManualHighlightTimer.Stop();
            _ManualHighlightIndex = -1;
            Invalidate();
        }

        private void ExecuteThreadSafeAndAsync(Action action)
        {
            if (InvokeRequired)
                BeginInvoke(action);
            else
                action.Invoke();
        }

        #endregion

        #region Internal Updating
        private void UpdateAndRedraw()
        {
            UpdateAutoZoom();
            UpdateScaleFactor();
            UpdatePath();
            Invalidate();
        }

        private void UpdateScaleFactor()
        {
            if (_Image == null)
                this.AutoScrollMinSize = this.Size;
            else
            {
                this.AutoScrollMinSize = new Size(
                  (int)(this._Image.Width * _Zoom + 0.5f),
                  (int)(this._Image.Height * _Zoom + 0.5f)
                  );
            }
        }

        private void UpdateAutoZoom()
        {
            AutoScroll = !_AutoZoom;
            if (!_AutoZoom || Image == null)
                return;

            float h = ((float)ClientSize.Width) / ((float)Image.Width);
            float v = ((float)ClientSize.Height) / ((float)Image.Height);

            _Zoom = Math.Min(h, v);
        }

        private void UpdatePath()
        {
            float xpos;
            float ypos;
            CalcXYPositions(out xpos, out ypos);
            _ScaledPath = (GraphicsPath)_OriginalPath.Clone();
            Matrix mx = new Matrix(_Zoom, 0, 0, _Zoom, 0, 0);
            mx.Translate(xpos, ypos);
            _ScaledPath.Transform(mx);
        }

        private void CalcXYPositions(out float xpos, out float ypos)
        {
            if (_Image == null)
            {
                xpos = ypos = 0;
                return;
            }

            if ((_Image.Width * _Zoom) < ClientSize.Width)
                xpos = (ClientSize.Width - (_Image.Width * _Zoom)) / (2 * _Zoom);
            else
                xpos = this.AutoScrollPosition.X / _Zoom;
            if ((_Image.Height * _Zoom) < ClientSize.Height)
                ypos = (ClientSize.Height - (_Image.Height * _Zoom)) / (2 * _Zoom);
            else
                ypos = this.AutoScrollPosition.Y / _Zoom;
        }
        #endregion

        #region overwritten Event Triggers
        protected override void OnScroll(ScrollEventArgs se)
        {
            base.OnScroll(se);
            UpdatePath();
        }

        protected override void OnSizeChanged(EventArgs e)
        {
            base.OnSizeChanged(e);
            UpdateAutoZoom();
            UpdatePath();
        }

        protected override void OnMouseLeave(EventArgs e)
        {
            RegionExited();
            Invalidate();
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            GraphicsPath path;
            int newIndex = this.getActiveIndexAtPoint(new Point(e.X, e.Y), out path);
            if (newIndex > -1)
                RegionEntered(newIndex, path);
            else
                RegionExited();
            if (_CurrentIndex != newIndex)

                base.OnMouseMove(e);
        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            base.OnMouseDown(e);
            if (_CurrentIndex != -1)
            {
                _MouseDownInRegion = true;
                Invalidate();
            }
        }

        protected override void OnMouseUp(MouseEventArgs e)
        {
            base.OnMouseUp(e);
            if (_MouseDownInRegion)
            {
                _MouseDownInRegion = false;
                Invalidate();
            }
        }

        protected override void OnMouseClick(MouseEventArgs e)
        {
            base.OnMouseClick(e);
            if (_CurrentIndex > -1)
                OnRegionClick(this, new RegionEventArgs(_CurrentIndex, _Keys[_CurrentIndex].ToString()));
        }

        #endregion

        #region Painting
        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaintBackground(e);

            //if no image, don't bother
            if (_Image == null)
            {
                return;
            }

            ApplyZoomMatrix(e.Graphics);
            DrawImage(e.Graphics);

            if (!Enabled)
                return;

            DrawRegions(e.Graphics);
            DrawHighlightRegion(e.Graphics);
            DrawBlinkRegion(e.Graphics);
            DrawClickRegion(e.Graphics);
        }

        private void ApplyZoomMatrix(Graphics g)
        {
            float xpos;
            float ypos;
            CalcXYPositions(out xpos, out ypos);

            Matrix mx = new Matrix(_Zoom, 0, 0, _Zoom, 0, 0);
            mx.Translate(xpos, ypos);
            g.Transform = mx;
        }

        private void DrawImage(Graphics g)
        {
            g.InterpolationMode = InterpolationMode;
            g.DrawImage(_Image, new Rectangle(0, 0, _Image.Width, _Image.Height), 0, 0, _Image.Width, _Image.Height, GraphicsUnit.Pixel);
        }

        private void DrawRegions(Graphics g)
        {
            if (!ShowRegions)
                return;

            g.DrawPath(new Pen(RegionsBorderColor), _OriginalPath);
        }

        private void DrawHighlightRegion(Graphics g)
        {
            if (_CurrentIndex == -1 || _MouseDownInRegion)
                return;

            g.FillPath(new SolidBrush(Color.FromArgb(FillTransparency, HighlightFillColor)), _CurrentPath);
            g.DrawPath(new Pen(HighlightBorderColor), _CurrentPath);
        }

        private void DrawBlinkRegion(Graphics g)
        {
            if (_ManualHighlightIndex == -1 || _ManualHighlightIndex == _CurrentIndex)
                return;

            g.FillPath(new SolidBrush(Color.FromArgb(FillTransparency, BlinkFillColor)), _ManualHighlightPath);
            g.DrawPath(new Pen(BlinkBorderColor), _ManualHighlightPath);
        }

        private void DrawClickRegion(Graphics g)
        {
            if (_CurrentIndex == -1 || !_MouseDownInRegion)
                return;

            g.FillPath(new SolidBrush(Color.FromArgb(FillTransparency, ClickFillColor)), _CurrentPath);
            g.DrawPath(new Pen(ClickBorderColor), _CurrentPath);
        }

        #endregion

        #region Internal helpers
        private void RegionEntered(int newIndex, GraphicsPath path)
        {
            if (newIndex == _CurrentIndex)
                return;

            _CurrentIndex = newIndex;
            _CurrentPath = path;
            Cursor = RegionCursor;
            Invalidate();

            OnRegionEnter(this, new RegionEventArgs(_CurrentIndex, _Keys[_CurrentIndex].ToString()));
        }

        private void RegionExited()
        {
            if (_CurrentIndex == -1)
                return;

            OnRegionLeave(this, new RegionEventArgs(_CurrentIndex, _Keys[_CurrentIndex].ToString()));

            _MouseDownInRegion = false;
            _CurrentIndex = -1;
            Cursor = Cursors.Default;
            Invalidate();

        }

        private int getActiveIndexAtPoint(Point point, out GraphicsPath origpath)
        {
            var path = new GraphicsPath();
            origpath = new GraphicsPath();
            GraphicsPathIterator iterator = new GraphicsPathIterator(_ScaledPath);
            GraphicsPathIterator iteratororig = new GraphicsPathIterator(_OriginalPath);
            iterator.Rewind();
            iteratororig.Rewind();
            for (int current = 0; current < iterator.SubpathCount; current++)
            {
                iterator.NextMarker(path);
                iteratororig.NextMarker(origpath);
                if (path.IsVisible(point, _Graphics))
                    return current;
            }
            return -1;
        }
        #endregion
    }

    public class RegionEventArgs : EventArgs
    {
        public int Index { get; private set; }
        public string Key { get; private set; }
        public RegionEventArgs(int index, string key)
        {
            Index = index;
            Key = key;
        }
    }

}
