using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Tiveria.Common.Controls.Annotations
{
    public class OperationsManager : IOperations
    {
        private IDocument _Document;
        private ITool _CurrentTool;
        private IShape _CurrentShape;
        private IList<ITool> _Tools;

        public OperationsManager(IDocument document)
        {
            _Document = document;
            _Tools = new List<ITool>();
        }

        public void MouseDown(IDocument document, System.Windows.Forms.MouseEventArgs e)
        {
            if (_CurrentTool != null)
                _CurrentTool.MouseDown(_Document, e);
        }

        public void MouseUp(IDocument document, System.Windows.Forms.MouseEventArgs e)
        {
            if (_CurrentTool != null)
                _CurrentTool.MouseUp(document, e);
        }

        public void MouseClick(IDocument document, System.Windows.Forms.MouseEventArgs e)
        {
            if (_CurrentTool != null)
                _CurrentTool.MouseClick(_Document, e);
        }

        public void MouseDoubleClick(IDocument document, System.Windows.Forms.MouseEventArgs e)
        {
            if (_CurrentTool != null)
                _CurrentTool.MouseDoubleClick(_Document, e);
        }

        public void MouseMove(IDocument document, System.Windows.Forms.MouseEventArgs e)
        {
            if (_CurrentTool == null)
                FindToolAndShapeAt(e.X, e.Y);

            if (_CurrentTool != null)
                _CurrentTool.MouseMove(_Document, e);
        }

        public void Paint(IDocument document, System.Windows.Forms.PaintEventArgs e)
        {
            foreach (var shape in _Document.Shapes)
                shape.Paint(document, e);

            if (_CurrentTool != null)
                _CurrentTool.Paint(document, e);
        }

        bool FindToolAndShapeAt(int x, int y)
        {
            foreach (var shape in _Document.Shapes)
            {
                foreach (var tool in _Tools)
                    if (tool.IsToolOfShape(shape))
                    {
                        _CurrentShape = shape;
                        _CurrentTool = tool;
                        return true;
                    }
            }
            return false;
        }
    }
}
