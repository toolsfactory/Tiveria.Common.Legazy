using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace Tiveria.Common.Controls
{
    public class MultiSelectRegionMap : ScrollableControl
    {
        #region private members
        private bool _AutoZoom;
        private Image _Image;
        private Font _Font;
        private Point _Offset;
        float _Zoom;
        private Graphics _Graphics;
        private List<ImageRegion> _Regions = new List<ImageRegion>();
        private ImageRegion _SelectedRegion = null;

        private Rectangle _EditRegionRectangle;
        private Point _EditRegionStartPoint;
        private EditModeType _EditMode = EditModeType.None;
        private HitAreaType _ResizeMode = HitAreaType.None;


        private Cursor[] _CursorsByHitType = new Cursor[]
		{
			Cursors.SizeNWSE, 
			Cursors.SizeNESW, 
			Cursors.SizeNWSE, 
			Cursors.SizeNESW, 
			Cursors.SizeWE, 
			Cursors.SizeWE, 
			Cursors.SizeNS, 
			Cursors.SizeNS, 
			Cursors.SizeAll, 
			Cursors.Default
		};


        #endregion

        #region Public properties

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

                if (value < 0 || value < 0.001)
                    value = 0.001f;

                if (value > 25)
                    value = 25f;

                _Zoom = value;
                UpdateAndRedraw();
            }
        }

        public Point Offset { get { return _Offset; } }
        internal List<ImageRegion> Regions { get { return _Regions; } }

        [DefaultValue(InterpolationMode.HighQualityBicubic)]
        [Category("Appearance"), Description("The interpolation mode used when zooming.")]
        public InterpolationMode InterpolationMode { get; set; }

        [DefaultValue(false)]
        [Category("Appearance"), Description("Is the not selected part of the image shaded?")]
        public bool ShadeOnEdit { get; set; }

        public int SelectedIndex
        {
            get
            {
                if (_Regions.Count > 0 && _SelectedRegion != null)
                    return _Regions.IndexOf(_SelectedRegion);
                else
                    return -1;
            }

            set
            {
                if (_Regions.Count <= value)
                    throw new IndexOutOfRangeException();
                if (value <= -1)
                    _SelectedRegion = null;
                else
                    _SelectedRegion = _Regions[value];
                Invalidate();
            }
        }
        #endregion

        #region Public Events
        public event EventHandler<RegionChangedEventArgs> RegionCreated;
        public event EventHandler<RegionChangedEventArgs> RegionDeleted;
        public event EventHandler<RegionChangedEventArgs> SelectedRegionChanged;

        protected virtual void OnRegionCreated(int index, Rectangle rect)
        {
            EventHandler<RegionChangedEventArgs> handler = RegionCreated;
            if (handler != null)
                handler(this, new RegionChangedEventArgs(index, rect, "create"));
        }
        protected virtual void OnRegionDeleted(int index, Rectangle rect)
        {
            EventHandler<RegionChangedEventArgs> handler = RegionDeleted;
            if (handler != null)
                handler(this, new RegionChangedEventArgs(index, rect, "delete"));
        }
        protected virtual void OnRegionSelected(int index, Rectangle rect)
        {
            EventHandler<RegionChangedEventArgs> handler = SelectedRegionChanged;
            if (handler != null)
                handler(this, new RegionChangedEventArgs(index, rect, "select"));
        }
        #endregion

        #region Public Region management Methods
        public void ClearRegions()
        {
            _Regions.Clear();
            _SelectedRegion = null;
            Invalidate();
        }

        public void DeleteSelectedRegion()
        {
            if (_SelectedRegion == null)
                return;

            var idx = _Regions.IndexOf(_SelectedRegion);
            if (idx == -1)
                return;

            var rgn = _SelectedRegion;
            _SelectedRegion = null;
            _Regions.Remove(rgn);
            Invalidate();
            OnRegionDeleted(idx, rgn.Rectangle);
        }

        public void DeleteRegion(int index)
        {
            if (index >= 0 && index < _Regions.Count)
            {
                if (_Regions[index] == _SelectedRegion)
                {
                    _SelectedRegion = null;
                }
                var rgn = _Regions[index];
                _Regions.RemoveAt(index);
                Invalidate();
                OnRegionDeleted(index, rgn.Rectangle);
            }
        }

        public bool AddRegion(Rectangle rect)
        {
            if (_Image == null)
            {
                throw new InvalidOperationException("Cannot add regions as no image is set");
            }

            if (rect.X < 0 || rect.Y < 0 || rect.X + rect.Width > _Image.Width || rect.Y + rect.Height > _Image.Height)
            {
                return false;
            }

            _Regions.Add(new ImageRegion(this, rect));
            Invalidate();
            return true;
        }

        public List<Rectangle> ExportRegions()
        {
            var list = new List<Rectangle>();
            foreach (var item in _Regions)
                list.Add(item.Rectangle);

            return list;
        }
        #endregion

        #region Initialize & Dispose
        public MultiSelectRegionMap()
        {
            _Image = null;
            _Zoom = 1.0f;
            _Graphics = Graphics.FromHwnd(Handle);
            _Font = (Font) SystemFonts.DefaultFont.Clone();

            SetStyle(ControlStyles.AllPaintingInWmPaint |
              ControlStyles.UserPaint |
              ControlStyles.ResizeRedraw |
              ControlStyles.UserPaint |
              ControlStyles.DoubleBuffer, true);

            AutoScroll = true;
            AutoZoom = true;
            ShadeOnEdit = false;
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
        }
        #endregion

        #region Internal Updating
        private void UpdateAndRedraw()
        {
            UpdateAutoZoom();
            UpdateScaleFactor();
            UpdateOffset();
            Invalidate();
        }

        private void UpdateOffset()
        {
            _Offset = CalcXYOffset();
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

        private Point CalcXYOffset()
        {
            if (_Image == null)
            {
                return new Point(0,0);
            }

            float xpos, ypos;

            if ((_Image.Width * _Zoom) < ClientSize.Width)
                xpos = (ClientSize.Width - (_Image.Width * _Zoom)) / 2;
            else
                xpos = this.AutoScrollPosition.X / _Zoom;
            if ((_Image.Height * _Zoom) < ClientSize.Height)
                ypos = (ClientSize.Height - (_Image.Height * _Zoom)) / 2;
            else
                ypos = this.AutoScrollPosition.Y / _Zoom;

            return new Point((int) xpos, (int)ypos);        }
        #endregion

        #region overwritten Event Triggers
        protected override void OnSizeChanged(EventArgs e)
        {
            base.OnSizeChanged(e);
            UpdateAutoZoom();
            UpdateOffset();
        }

        protected override void OnScroll(ScrollEventArgs se)
        {
            base.OnScroll(se);
            Invalidate();
        }

        protected override void OnMouseLeave(EventArgs e)
        {
            Invalidate();
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);

            if (_Image == null)
                return;

            switch(_EditMode)
            {
                case EditModeType.Create :  ResizeNewRegion(e); break;
                case EditModeType.Drag: MoveDragRegion(e); break;
                case EditModeType.Resize: MoveResizeRegion(e); break;
                default: DetectSelectedRegionHit(e); break;
            } 
        }

        private void DetectSelectedRegionHit(MouseEventArgs e)
        {
            if (_SelectedRegion == null)
                return;

            var hit = _SelectedRegion.HitTest(e.Location);
            Cursor = _CursorsByHitType[(int)hit];
            Refresh();
        }

        private HitAreaType DetectHit(MouseEventArgs e)
        {
            HitAreaType hit = HitAreaType.None;
            for(int i= 0; i<_Regions.Count; i++)
            {
                hit = _Regions[i].HitTest(e.Location);
                if (hit != HitAreaType.None)
                {
                    _SelectedRegion = _Regions[i];
                    Cursor = _CursorsByHitType[(int)hit];
                    Refresh();
                    OnRegionSelected(i, _Regions[i].Rectangle);
                    break;
                }
            }
            if (_SelectedRegion != null && hit == HitAreaType.None)
            {
                Cursor = Cursors.Default;
                _SelectedRegion = null;
                Refresh();
            }

            return hit;
        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            base.OnMouseDown(e);
            var hit = DetectHit(e);

            if(hit == HitAreaType.None)
            {
                StartCreateNewRegion(e); 
            } 
            else
            {
                if(hit == HitAreaType.Middle)
                {
                    if (e.Button == System.Windows.Forms.MouseButtons.Left)
                    {
                        StartDragRegion(e);
                    }

                    if(e.Button == System.Windows.Forms.MouseButtons.Right)
                    {
                        CancelDragRegion(e);
                    }
                } 
                else
                    if(hit != HitAreaType.None)
                    {
                        StartResizeRegion(e, hit);
                    }
            }
        }

        protected override void OnMouseUp(MouseEventArgs e)
        {
            base.OnMouseUp(e);
            switch(_EditMode)
            {
                case EditModeType.Create: FinalizeCreateNewRegion(e); break;
                case EditModeType.Drag: EndDragRegion(e); break;
                case EditModeType.Resize: EndResizeRegion(e); break;
                default: break;
            }
            Refresh();
        }

        protected override void OnMouseClick(MouseEventArgs e)
        {
            base.OnMouseClick(e);
        }

        #endregion

        #region Create New Region
        private void StartCreateNewRegion(MouseEventArgs e)
        {
            if (e.Button != System.Windows.Forms.MouseButtons.Left)
                return;

            _EditMode = EditModeType.Create;
            _EditRegionRectangle = Rectangle.Empty;
            _EditRegionRectangle.Offset(e.Location);
            _EditRegionStartPoint = e.Location;
            Cursor = Cursors.Cross;
        }

        private void ResizeNewRegion(MouseEventArgs mea)
        {
            Size sz = new Size(mea.Location.X - _EditRegionStartPoint.X, mea.Location.Y - _EditRegionStartPoint.Y);
            _EditRegionRectangle = NormaliseRect(_EditRegionStartPoint, sz);
            var refrect = CalcImageRectangle();
            _EditRegionRectangle.Intersect(refrect);
            Refresh();
        }

        private void FinalizeCreateNewRegion(MouseEventArgs e)
        {
            var region = ImageRegion.FromScreenCoordinates(this, _EditRegionRectangle);
            _Regions.Add(region);
            _EditMode = EditModeType.None;
            _SelectedRegion = region;
            Cursor = Cursors.Default;
            OnRegionCreated(_Regions.Count - 1, region.Rectangle);
        }

        #endregion

        #region Drag Region
        private void StartDragRegion(MouseEventArgs e)
        {
            _EditMode = EditModeType.Drag;
            _EditRegionStartPoint = e.Location;
            _EditRegionRectangle = _SelectedRegion.ScreenRectangle;
            Cursor = Cursors.SizeAll;
            Refresh();
        }

        private void MoveDragRegion(MouseEventArgs e)
        {
            _EditRegionRectangle = _SelectedRegion.ScreenRectangle;
            _EditRegionRectangle.Offset(e.Location.X - _EditRegionStartPoint.X, e.Location.Y - _EditRegionStartPoint.Y);
            var refrect = CalcImageRectangle();
            _EditRegionRectangle.Intersect(refrect);
            Refresh();
        }

        private void EndDragRegion(MouseEventArgs e)
        {
            _EditMode = EditModeType.None;
            Cursor = Cursors.Default;
            _SelectedRegion.ScreenRectangle = _EditRegionRectangle;
            Refresh();
        }

        private void CancelDragRegion(MouseEventArgs e)
        {
            _EditMode = EditModeType.None;
            Cursor = Cursors.Default;
            Refresh();
        }
        #endregion

        #region Resize Region
        private void StartResizeRegion(MouseEventArgs e, HitAreaType hitType)
        {
            _EditMode = EditModeType.Resize;
            _EditRegionStartPoint = e.Location;
            _EditRegionRectangle = _SelectedRegion.ScreenRectangle;
            Cursor = _CursorsByHitType[(int)hitType];
            _ResizeMode = hitType;
            Refresh();
        }

        private void MoveResizeRegion(MouseEventArgs e)
        {
            Rectangle rc = _EditRegionRectangle;
            int x = rc.X,y = rc.Y,w = rc.Width ,h= rc.Height;
            if(_ResizeMode == HitAreaType.BottomLeft || _ResizeMode == HitAreaType.BottomMiddle || _ResizeMode == HitAreaType.BottomRight)
            {
                y = rc.Top;
                h = e.Location.Y - y;

                if(h<0)
                {
                    y = e.Location.Y;
                    h = -h;
                    switch(_ResizeMode)
                    {
                        case HitAreaType.BottomLeft: _ResizeMode = HitAreaType.TopLeft; break;
                        case HitAreaType.BottomMiddle: _ResizeMode = HitAreaType.TopMiddle; break;
                        case HitAreaType.BottomRight: _ResizeMode = HitAreaType.TopRight; break;
                        default: break;
                    }
                }
            } 
            else
            if (_ResizeMode == HitAreaType.TopLeft || _ResizeMode == HitAreaType.TopMiddle || _ResizeMode == HitAreaType.TopRight)
            {
                y = e.Location.Y;
                h = rc.Bottom - y;

                if (h < 0)
                {
                    y = rc.Top;
                    h = -h;
                    switch (_ResizeMode)
                    {
                        case HitAreaType.TopLeft: _ResizeMode = HitAreaType.BottomLeft; break;
                        case HitAreaType.TopMiddle: _ResizeMode = HitAreaType.BottomMiddle; break;
                        case HitAreaType.TopRight: _ResizeMode = HitAreaType.BottomRight; break;
                        default: break;
                    }
                }
            }

            if (_ResizeMode == HitAreaType.BottomRight || _ResizeMode == HitAreaType.TopRight || _ResizeMode == HitAreaType.MidRight)
            {
                x = rc.Left;
                w = e.Location.X - x;
                if(w<0)
                {
                    x = e.Location.X;
                    w = -w;
                    switch(_ResizeMode)
                    {
                        case HitAreaType.TopRight: _ResizeMode = HitAreaType.TopLeft; break;
                        case HitAreaType.MidRight: _ResizeMode = HitAreaType.MidLeft; break;
                        case HitAreaType.BottomRight: _ResizeMode = HitAreaType.BottomLeft; break;
                        default: break;
                    }
                }
            }
            else
            if (_ResizeMode == HitAreaType.BottomLeft || _ResizeMode == HitAreaType.TopLeft || _ResizeMode == HitAreaType.MidLeft)
            {
                x = e.Location.X;
                w = rc.Right - x;
                if (w < 0)
                {
                    x = rc.Right;
                    w = -w;
                    switch (_ResizeMode)
                    {
                        case HitAreaType.TopLeft: _ResizeMode = HitAreaType.TopRight; break;
                        case HitAreaType.MidLeft: _ResizeMode = HitAreaType.MidRight; break;
                        case HitAreaType.BottomLeft: _ResizeMode = HitAreaType.BottomRight; break;
                        default: break;
                    }
                }
            }

            _EditRegionRectangle = new Rectangle(x, y, w, h);
            var refrect = CalcImageRectangle();
            _EditRegionRectangle.Intersect(refrect);
            Refresh();

            return;

        }

        private void EndResizeRegion(MouseEventArgs e)
        {
            _EditMode = EditModeType.None;
            Cursor = Cursors.Default;
            _SelectedRegion.ScreenRectangle = _EditRegionRectangle;
            Refresh();
        }
        #endregion

        #region Painting
        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaintBackground(e);
            UpdateOffset();

            //if no image, don't bother
            if (_Image == null)
            {
                return;
            }

            //ApplyZoomMatrix(e.Graphics);
            DrawImage(e.Graphics);

            if(_EditMode != EditModeType.None)
            {
                DrawEditRegion(e.Graphics);
                if(!ShadeOnEdit)
                {
                    DrawRegions(e.Graphics);
                }
            }

            if (_EditMode == EditModeType.None)
            {
                DrawRegions(e.Graphics);
                DrawSelectedRegion(e.Graphics);
            }
        }

        private void DrawEditRegion(Graphics graphics)
        {
            Region clip = graphics.Clip;
            Rectangle rectangle = _EditRegionRectangle;
            if (ShadeOnEdit)
            {
                graphics.ExcludeClip(rectangle);
                graphics.FillRectangle(new SolidBrush(Color.FromArgb(100, Color.Black)), CalcImageRectangle());
                graphics.Clip = clip;
            }
            using (var pen = new Pen(Color.White) { DashStyle = System.Drawing.Drawing2D.DashStyle.Dot })
                graphics.DrawRectangle(pen, rectangle);
        }

        private void DrawImage(Graphics g)
        {

            g.InterpolationMode = InterpolationMode;
            g.DrawImage(_Image, CalcImageRectangle(), 0, 0, _Image.Width, _Image.Height, GraphicsUnit.Pixel);
        }

        private void DrawRegions(Graphics g)
        {
            foreach(var region in _Regions)
            {
                if (region != _SelectedRegion)
                    region.Draw(g);
            }
        }

        private void DrawSelectedRegion(Graphics g)
        {
            if (_SelectedRegion != null)
                _SelectedRegion.DrawWithHandles(g);
        }

        #endregion

        #region Internal helpers
        private Rectangle CalcImageRectangle()
        {
            return new Rectangle((int) (_Offset.X * _Zoom), (int) (_Offset.Y * _Zoom), (int)(_Image.Width * _Zoom), (int)(_Image.Height * _Zoom));
        }

        private Rectangle NormaliseRect(Point pt, Size sz)
        {
            int val = pt.X + sz.Width;
            int val2 = pt.Y + sz.Height;
            return Rectangle.FromLTRB(System.Math.Min(pt.X, val), System.Math.Min(pt.Y, val2), System.Math.Max(pt.X, val), System.Math.Max(pt.Y, val2));
        }

        private Rectangle NormalizeRectangle(Rectangle source)
        {
            Rectangle rc = source;

            if (source.Right < source.Left)
            {
                rc.X = source.Right;
                rc.Width = source.Left - source.Right;
            }

            if (source.Bottom < source.Top)
            {
                rc.Y = source.Bottom;
                rc.Height = source.Top - source.Bottom;
            }

            return rc;
        }
        #endregion
    }

    class ImageRegion
    {
        private static int handleSize = 6;
        private Rectangle _Rectangle;
        private MultiSelectRegionMap _Owner;

        internal ImageRegion(MultiSelectRegionMap owner)
        {
            _Owner = owner; 
        }

        internal ImageRegion(MultiSelectRegionMap owner, Rectangle rect)
            : this (owner)
        {
            _Rectangle = rect;
        }

        public static ImageRegion FromScreenCoordinates(MultiSelectRegionMap owner, Rectangle rect)
        {
            return new ImageRegion(owner) { ScreenRectangle = rect };
        }

        public Rectangle Rectangle { get { return _Rectangle; } set { _Rectangle = value; } }
        public Rectangle ScreenRectangle { get { return ImageToScreenCoords(_Rectangle); } set { _Rectangle = ScreenToImageCoords(value); } }

        public void Draw(Graphics gr)
        {
            using (var pen = new Pen(Color.Red, 2.0f) { DashStyle = System.Drawing.Drawing2D.DashStyle.Dot })
                gr.DrawRectangle(pen, ImageToScreenCoords(_Rectangle));

            DrawCounterFlag(gr, _Owner.Regions.IndexOf(this) + 1);
        }

        private void DrawCounterFlag(Graphics gr, int p)
        {
            var pos = CalcFlagRectangle(gr, p);

            gr.FillRectangle(Brushes.Red, pos);
            gr.DrawString(p.ToString(), _Owner.Font, Brushes.Black, new PointF( pos.X+3, pos.Y + 3 ));
        }

        private RectangleF CalcFlagRectangle(Graphics gr, int p)
        {
            var textSize = gr.MeasureString(p.ToString(), _Owner.Font);
            var rect = ImageToScreenCoords(_Rectangle);
            RectangleF rc;
            if (_Rectangle.Y >= textSize.Height + 6 )
                rc = new RectangleF(rect.Left, rect.Top - textSize.Height - 6, textSize.Width + 6, textSize.Height + 6);
            else if(_Rectangle.Bottom + textSize.Height+6< _Owner.Image.Height)
                rc = new RectangleF(rect.Left, rect.Bottom, textSize.Width + 6, textSize.Height + 6);
            else
                rc = new RectangleF(rect.Right - textSize.Width - 6, rect.Bottom - textSize.Height - 6, textSize.Width + 6, textSize.Height + 6);
            return rc;
        }

        public void DrawWithHandles(Graphics gr)
        {
            Draw(gr);
            DrawHandles(gr, _Owner.Offset, _Owner.Zoom);
        }

        public bool IsHit(Point pt)
        {
            return ImageToScreenCoords(_Rectangle).Contains(pt);
        }

        public HitAreaType HitTest(Point pt)
        {
            Rectangle rc = ImageToScreenCoords(_Rectangle);
            Rectangle[] array = CalculateHandleLocations(rc);
            for (int i = 0; i < array.Length; i++)
            {
                if (array[i].Contains(pt))
                {
                    return (HitAreaType)i;
                }
            }
            if (!rc.Contains(pt))
            {
                return HitAreaType.None;
            }
//            rc.Inflate(-handleSize, -handleSize);
            if (rc.IsEmpty || rc.Contains(pt))
            {
                return HitAreaType.Middle;
            }
            return HitAreaType.None;
        }


        private void DrawHandles(Graphics gr, Point offset, float zoom)
        {
            var rect = ImageToScreenCoords(_Rectangle);
            var handles = CalculateHandleLocations(rect);

            for (int i = handles.Length - 1; i >= 0; i--)
            {
                gr.FillRectangle(Brushes.White, handles[i]);
                gr.DrawRectangle(Pens.Black, handles[i]);
            }
        }   

        private Rectangle[] CalculateHandleLocations(Rectangle rc)
        {
            Rectangle[] array = new Rectangle[8];
            Size size = new Size(handleSize, handleSize);
            array[2] = new Rectangle(new Point(rc.Left, rc.Top), size);
            array[1] = new Rectangle(new Point(rc.Right - handleSize, rc.Top), size);
            array[6] = new Rectangle(new Point((rc.Left + rc.Right - handleSize) / 2, rc.Top), size);
            array[4] = new Rectangle(new Point(rc.Left, (rc.Top + rc.Bottom - handleSize) / 2), size);
            array[3] = new Rectangle(new Point(rc.Left, rc.Bottom - handleSize), size);
            array[0] = new Rectangle(new Point(rc.Right - handleSize, rc.Bottom - handleSize), size);
            array[7] = new Rectangle(new Point((rc.Left + rc.Right - handleSize) / 2, rc.Bottom - handleSize), size);
            array[5] = new Rectangle(new Point(rc.Right - handleSize, (rc.Top + rc.Bottom - handleSize) / 2), size);
            return array;
        }

        private Rectangle ImageToScreenCoords(Rectangle rc)
        {
            return new Rectangle(ImageToScreenCoords(rc.Location, _Owner.Offset, _Owner.Zoom), ImageToScreenCoords(rc.Size, _Owner.Zoom));
        }

        private Point ImageToScreenCoords(Point pt, Point offset, float zoom)
        {
            return new Point((int)System.Math.Round((double)(pt.X + offset.X) * zoom), (int)System.Math.Round((double)(pt.Y + offset.Y) * zoom));
        }

        private Size ImageToScreenCoords(Size sz, float zoom)
        {
            return new Size((int)System.Math.Round((double)sz.Width * zoom), (int)System.Math.Round((double)sz.Height * zoom));
        }

        private Rectangle ScreenToImageCoords(Rectangle rc)
        {
            rc.Offset(-(int) (_Owner.Offset.X * _Owner.Zoom), - (int) (_Owner.Offset.Y * _Owner.Zoom));
            var pt = new Point((int)System.Math.Round((double)rc.X / _Owner.Zoom), (int)System.Math.Round((double)rc.Y / _Owner.Zoom));
            var sz = new Size((int)System.Math.Round((double)rc.Width / _Owner.Zoom), (int)System.Math.Round((double)rc.Height / _Owner.Zoom));
            return new Rectangle(pt, sz);
        }

    }

    enum HitAreaType
    {
        BottomRight,
        TopRight,
        TopLeft,
        BottomLeft,
        MidLeft,
        MidRight,
        TopMiddle,
        BottomMiddle,
        Middle,
        None
    }

    enum EditModeType
    {
        None,
        Create,
        Drag,
        Resize,
        Rotate
    }

    public class RegionChangedEventArgs : EventArgs
    {
        public int Index { get; private set; }
        public Rectangle Rectangle { get; private set; }
        public string Action { get; private set; }
        public RegionChangedEventArgs(int index, Rectangle rect, string action = "create")
        {
            Index = index;
            Rectangle = rect;
            Action = action;
        }
    }

}
