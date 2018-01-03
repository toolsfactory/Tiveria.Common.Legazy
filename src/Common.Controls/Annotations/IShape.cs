using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace Tiveria.Common.Controls.Annotations
{
    public interface IShape : ICloneable, IOperations
    {
        #region Properties

        /// <summary>
        /// Gets or sets the current location.
        /// </summary>
        PointF Location { get; set; }

        /// <summary>
        /// Gets or sets the current size.
        /// </summary>
        SizeF Dimension { get; set; }

        /// <summary>
        /// Gets or sets the current center.
        /// </summary>
        PointF Center { get; set; }

        /// <summary>
        /// Gets oe sets the selecting.
        /// </summary>
        bool Selected { get; set; }

        /// <summary>
        /// Gets the geometric form of the shape.
        /// </summary>
        GraphicsPath Geometry { get; }

        #endregion

        #region Functions
        /// <summary>
        /// Controls if the point is into this.
        /// </summary>
        /// <param name="point">Point to control.</param>
        /// <returns></returns>
        bool IsHit(Point point);

        void Zoom(float oldzoom, float newzoom);

        void DrawShapeIntoImage(Image buffer);
        #endregion

    }
}

