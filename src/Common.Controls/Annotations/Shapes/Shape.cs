using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace Tiveria.Common.Controls.Annotations.Shapes
{
    public abstract class Shape : IShape, IDisposable
    {
        protected System.Drawing.Drawing2D.GraphicsPath _Geometry;
        internal float _Rotation = 0f;

        protected Shape()
        {
            _Geometry = new GraphicsPath();
        }

        /// <summary>
        /// Copy constructor.
        /// </summary>
        /// <param name="shape">Shape to copy.</param>
        protected Shape(Shape shape)
        {
            _Geometry = shape.Geometry.Clone() as GraphicsPath;
        }

        public void Dispose()
        {
            if (_Geometry != null)
            {
                _Geometry.Dispose();
                _Geometry = null;
            }
        }
        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="geometric">Reference GraphicsPath.</param>
        protected Shape(IDocument owner, GraphicsPath geometric)
        {
            _Geometry = geometric.Clone() as GraphicsPath;
        }

        public PointF Location
        {
            get {
                return Geometry.GetBounds().Location;
                //return GetNormalizedBounds().Location;
            }

            set
            {
                if (value.IsEmpty)
                    return;

                if (float.IsNaN(value.X) || float.IsNaN(value.Y) ||
                    float.IsInfinity(value.Y) || float.IsInfinity(value.Y))
                    return;

                float offsetX = value.X - this.Location.X;
                float offsetY = value.Y - this.Location.Y;
                //                Geometry.Translate(offsetX, offsetY);
            }
        }

        public SizeF Dimension
        {
            get {
                return Geometry.GetBounds().Size;
                //return GetNormalizedBounds().Size;
            }

            set
            {
                if (value.IsEmpty)
                    return;

                // Never this.Dimension.Width and this.Dimension.Height are zero
                float scaleX = value.Width / this.Dimension.Width;
                float scaleY = value.Height / this.Dimension.Height;

                //                _transformer.Scale(scaleX, scaleY);
            }
        }

        public PointF Center
        {
            get
            {
                float x = this.Location.X + this.Dimension.Width / 2f;
                float y = this.Location.Y + this.Dimension.Height / 2f;

                return new PointF(x, y);
            }

            set
            {
                float offsetX = value.X - this.Location.X - this.Dimension.Width / 2f;
                float offsetY = value.Y - this.Location.Y - this.Dimension.Height / 2f; ;

                //                _transformer.Translate(offsetX, offsetY);
            }
        }

        public float Rotation
        {
            get { return _Rotation; }

            set
            {
                _Rotation += value;
                //                _transformer.Rotate(value);
            }
        }

        public bool Selected
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        public System.Drawing.Drawing2D.GraphicsPath Geometry
        {
            get
            {
                return _Geometry;
            }
        }

        public bool IsHit(System.Drawing.Point point)
        {
            return _Geometry.IsVisible(point);
        }

        public abstract object Clone();

        #region Mouse Actions
        public void MouseDown(IDocument document, System.Windows.Forms.MouseEventArgs e)
        {
        }

        public void MouseUp(IDocument document, System.Windows.Forms.MouseEventArgs e)
        {
        }

        public void MouseClick(IDocument document, System.Windows.Forms.MouseEventArgs e)
        {
        }

        public void MouseDoubleClick(IDocument document, System.Windows.Forms.MouseEventArgs e)
        {
        }

        public void MouseMove(IDocument document, System.Windows.Forms.MouseEventArgs e)
        {
        }
        #endregion

        public void Paint(IDocument document, System.Windows.Forms.PaintEventArgs e)
        {
            e.Graphics.DrawPath(Pens.Red, Geometry);
        }

        protected virtual RectangleF GetNormalizedBounds()
        {
             return new RectangleF();
            /*
            using (var path = Geometry.Clone() as GraphicsPath)
            using (var mx = new Matrix(0f, 0f, 1 / _Document.Zoom, 0f, 0f, 1 / _Document.Zoom))
            {
                path.Transform(mx);
                return path.GetBounds();
            }
             */
        }


        public void Zoom(float oldzoom, float newzoom)
        {
            var newfact = (1 / oldzoom) * newzoom;
            using (var mx = new Matrix(newfact, 0f, 0f, newfact, 0f, 0f))
            {
                _Geometry.Transform(mx);
            }
        }

        #region IShape Member


        public void DrawShapeIntoImage(Image buffer)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
