using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;


namespace Tiveria.Common.Controls.Annotations
{
    public interface IOperations
    {
        void MouseDown(IDocument document, MouseEventArgs e);
        void MouseUp(IDocument document, MouseEventArgs e);
        void MouseClick(IDocument document, MouseEventArgs e);
        void MouseDoubleClick(IDocument document, MouseEventArgs e);
        void MouseMove(IDocument document, MouseEventArgs e);
        void Paint(IDocument document, PaintEventArgs e);
    }

    public interface ITool : IOperations
    {
        bool IsToolOfShape(IShape shape);
    }
}

