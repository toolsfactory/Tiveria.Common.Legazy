using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using Tiveria.Common.Controls.Annotations;

namespace Tiveria.Common.Controls
{
    public class AnnotationPanel : ScrollableControl, IDocument
    {
        #region private members
        private ShapeCollection _Shapes;
        private bool _AutoZoom;
        private Image _Image;
        float _Zoom;
        float _OldZoom;
        private Graphics _Graphics;
        private OperationsManager _OperationsManager;
        #endregion

        #region Public properties
        #region IDocument Properties
        public Control DrawingControl
        {
            get { return this; }
        }

        public ShapeCollection Shapes
        {
            get
            {
                return _Shapes;
            }
        }
        #endregion
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

                _OldZoom = _Zoom;
                _Zoom = value;
                UpdateAndRedraw();
            }
        }

        [DefaultValue(InterpolationMode.HighQualityBicubic)]
        [Category("Appearance"), Description("The interpolation mode used when zooming.")]
        public InterpolationMode InterpolationMode { get; set; }
        #endregion

        #region Initialize & Dispose
        public AnnotationPanel()
        {
            _Image = null;
            _Zoom = _OldZoom = 1.0f;
            _Graphics = Graphics.FromHwnd(Handle);
            _OperationsManager = new OperationsManager(this);
            _Shapes = new ShapeCollection();
            SetStyle(ControlStyles.AllPaintingInWmPaint |
              ControlStyles.UserPaint |
              ControlStyles.ResizeRedraw |
              ControlStyles.UserPaint |
              ControlStyles.DoubleBuffer, true);

            AutoScroll = true;
            _Shapes.ItemInserted += _Shapes_ItemInserted;
        }

        void _Shapes_ItemInserted(object sender, ShapeChangedEventArgs e)
        {
            Invalidate();
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
            ApplyZoomOnShapes();
            Invalidate();
        }

        private void ApplyZoomOnShapes()
        {
            foreach (var shape in Shapes)
                shape.Zoom(_OldZoom, _Zoom);
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

        private void CalcXYPositions(out float xpos, out float ypos)
        {
            if (_Image == null)
            {
                xpos = ypos = 0;
                return;
            }

            if ((_Image.Width * _Zoom) < ClientSize.Width)
                xpos = (ClientSize.Width - (_Image.Width * _Zoom)) / 2;
            else
                xpos = this.AutoScrollPosition.X;
            if ((_Image.Height * _Zoom) < ClientSize.Height)
                ypos = (ClientSize.Height - (_Image.Height * _Zoom)) / 2;
            else
                ypos = this.AutoScrollPosition.Y;
        }
        #endregion

        #region overwritten Event Triggers
        protected override void OnScroll(ScrollEventArgs se)
        {
            base.OnScroll(se);
        }

        protected override void OnSizeChanged(EventArgs e)
        {
            base.OnSizeChanged(e);
            UpdateAutoZoom();
        }

        protected override void OnMouseLeave(EventArgs e)
        {
            Invalidate();
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);
            _OperationsManager.MouseMove(this, e);
        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            base.OnMouseDown(e);
            _OperationsManager.MouseDown(this, e);
        }

        protected override void OnMouseUp(MouseEventArgs e)
        {
            base.OnMouseUp(e);
            _OperationsManager.MouseUp(this, e);
        }

        protected override void OnMouseClick(MouseEventArgs e)
        {
            base.OnMouseClick(e);
            _OperationsManager.MouseClick(this, e);
        }

        #endregion

        #region Painting
        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaintBackground(e);

            float xpos;
            float ypos;
            CalcXYPositions(out xpos, out ypos);
            e.Graphics.TranslateTransform(xpos, ypos);

            DrawImageZoom(e.Graphics);
            _OperationsManager.Paint(this, e);
        }

        private void DrawImage(Graphics g)
        {
            if (_Image == null)
                return;

            g.InterpolationMode = InterpolationMode;
            g.DrawImage(_Image, new Rectangle(0, 0, _Image.Width, _Image.Height), 0, 0, _Image.Width, _Image.Height, GraphicsUnit.Pixel);
        }

        private void DrawImageZoom(Graphics g)
        {
            if (_Image == null)
                return;

            g.InterpolationMode = InterpolationMode;
            g.DrawImage(_Image, new Rectangle(0, 0, (int)(_Zoom * _Image.Width), (int)(_Zoom * _Image.Height)), 0, 0, _Image.Width, _Image.Height, GraphicsUnit.Pixel);
        }

        #endregion
    }
}
