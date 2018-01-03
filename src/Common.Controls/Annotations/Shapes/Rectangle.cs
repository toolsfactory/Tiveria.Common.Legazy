using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace Tiveria.Common.Controls.Annotations.Shapes
{
    public class Rectangle : Shape
    {
        /// <summary>
        /// Copy constructor.
        /// </summary>
        /// <param name="shape">Shape to copy.</param>
        public Rectangle(Shape shape)
            : base(shape)
        {

        }

        public Rectangle(int x, int y, int w, int h)
            : base()
        {
            _Geometry.AddRectangle(new System.Drawing.Rectangle(x,y,w,h));
        }


        public override object Clone()
        {
            return new Rectangle(this);
        }

        protected override RectangleF GetNormalizedBounds()
        {
            return base.GetNormalizedBounds();
        }
    }
}
