using System;
using System.Collections.Generic;
using System.Linq;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace Tiveria.Common.Controls.Annotations.Adorners
{
    public class RectangleAdorner : IAdorner
    {
        private readonly Shapes.Rectangle _Shape;
        private readonly IDocument _Document;
        
        public RectangleAdorner(Shapes.Rectangle shape, IDocument document)
        {
            _Document = document;
            _Shape = shape;
        }

        #region IAdorner Member

        public void Paint(System.Windows.Forms.PaintEventArgs e)
        {
            foreach (PointF point in _Shape.Geometry.PathPoints)
            {
                /*
                RectangleF rect = new RectangleF(point.X - MarkerDimension / 2, point.Y - MarkerDimension / 2, MarkerDimension, MarkerDimension);
                using (Brush brush = new SolidBrush(MarkerColor))
                {
                    e.Graphics.FillRectangle(brush, Rectangle.Round(rect));
                }
                 */ 
            }
        }

        public AdornerHitType IsHit(System.Drawing.PointF point)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
