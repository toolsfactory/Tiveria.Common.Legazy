using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Tiveria.Common.Extensions
{
    public static class ControlExtensions
    {
        public static void ExecuteInUIContext(this Control ctrl, Action action)
        {
            if (ctrl.InvokeRequired)
                ctrl.Invoke(action);
            else
                action();
        }

        public static void ExecuteInUIContextAndAsync(this Control ctrl, Action action)
        {
            if (ctrl.InvokeRequired)
                ctrl.BeginInvoke(action);
            else
                action();
        }

    }
}
