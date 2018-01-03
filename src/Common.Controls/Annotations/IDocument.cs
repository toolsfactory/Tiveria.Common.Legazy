using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace Tiveria.Common.Controls.Annotations
{
    public interface IDocument
    {
        Control DrawingControl { get; }
        ShapeCollection Shapes { get; }
    }
}
