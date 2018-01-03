using System;
using System.Collections.Generic;
using System.Linq;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace Tiveria.Common.Controls.Annotations
{
    public interface IAdorner
    {
        void Paint(PaintEventArgs e);
        AdornerHitType IsHit(PointF point);
    }
}