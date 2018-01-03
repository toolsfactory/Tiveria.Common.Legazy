using System;
using System.Runtime.InteropServices;

namespace Tiveria.Common
{
    internal static class NativeMethods
    {
        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        internal static extern IntPtr SendMessage(IntPtr hWnd, UInt32 Msg, Int32 wParam, Int32 lParam);

        [DllImport("user32.dll", CharSet = CharSet.Unicode)]
        internal static extern IntPtr SendMessage(HandleRef hWnd, UInt32 Msg, IntPtr wParam, IntPtr lParam);


        [DllImport("user32.dll", SetLastError = true)]
        internal static extern IntPtr SetParent(IntPtr hWndChild, IntPtr hWndNewParent);

        [DllImport("user32.dll")]
        internal static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);

        [DllImport("user32.dll", ExactSpelling = true, CharSet = CharSet.Auto)]
        internal static extern IntPtr GetParent(IntPtr hWnd);

        // check if user is an admin for Windows 2000 and above
        [DllImport("shell32.dll", EntryPoint = "#680", CharSet = CharSet.Unicode)]
        internal static extern bool IsUserAnAdmin();

        internal const int WM_SYSCOMMAND = 274;
        internal const int SC_MAXIMIZE = 61488;
    }
}
