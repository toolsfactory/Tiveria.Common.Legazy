using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tiveria.Common.Controls.Annotations.Tools
{
    public class PointerTool: ITool
    {
        #region ITool Member

        public bool IsToolOfShape(IShape shape)
        {
            return true;
        }

        #endregion

        #region IOperations Member

        public void MouseDown(IDocument document, System.Windows.Forms.MouseEventArgs e)
        {
            throw new NotImplementedException();
        }

        public void MouseUp(IDocument document, System.Windows.Forms.MouseEventArgs e)
        {
            throw new NotImplementedException();
        }

        public void MouseClick(IDocument document, System.Windows.Forms.MouseEventArgs e)
        {
            throw new NotImplementedException();
        }

        public void MouseDoubleClick(IDocument document, System.Windows.Forms.MouseEventArgs e)
        {
            throw new NotImplementedException();
        }

        public void MouseMove(IDocument document, System.Windows.Forms.MouseEventArgs e)
        {
            throw new NotImplementedException();
        }

        public void Paint(IDocument document, System.Windows.Forms.PaintEventArgs e)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
