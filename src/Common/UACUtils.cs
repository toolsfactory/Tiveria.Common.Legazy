using System;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using System.Drawing;

namespace Tiveria.Common
{
    internal static class UACUtils
    {
        public static bool AtLeastVista()
        {
            return (Environment.OSVersion.Platform == PlatformID.Win32NT && Environment.OSVersion.Version.Major > 5);
        }

        public static void SetButtonShield(Button btn, bool showShield)
        {
            //Note: make sure the button FlatStyle = FlatStyle.System
            // BCM_SETSHIELD = 0x0000160C
            NativeMethods.SendMessage(new HandleRef(btn, btn.Handle), 0x160C, IntPtr.Zero, showShield ? new IntPtr(1) : IntPtr.Zero);
        }

        // Return a bitmap containing the UAC shield.
        private static Bitmap _ShieldBitmap = null;
        public static Bitmap GetUacShieldBitmap()
        {
            if (_ShieldBitmap != null) return _ShieldBitmap;

            const int width = 50;
            const int height = 50;
            const int margin = 4;

            // Make the button. For some reason, it must have text or the UAC shield won't appear.
            Button button = new Button() { Text = " ", FlatStyle = FlatStyle.System, Size = new Size(width, height) };
            SetButtonShield(button,true );

            // Draw the button onto a bitmap.
            Bitmap bm = new Bitmap(width, height);
            button.Refresh();
            button.DrawToBitmap(bm, new Rectangle(0, 0, width, height));

            // Find the part containing the shield.
            int min_x = width, max_x = 0, min_y = height, max_y = 0;

            // Fill on the left.
            for (int y = margin; y < height - margin; y++)
            {
                // Get the leftmost pixel's color.
                Color target_color = bm.GetPixel(margin, y);

                // Fill in with this color as long as we see the target.
                for (int x = margin; x < width - margin; x++)
                {
                    // See if this pixel is part of the shield.
                    if (bm.GetPixel(x, y).Equals(target_color))
                    {
                        // It's not part of the shield.
                        // Clear the pixel.
                        bm.SetPixel(x, y, Color.Transparent);
                    }
                    else
                    {
                        // It's part of the shield.
                        if (min_y > y) min_y = y;
                        if (min_x > x) min_x = x;
                        if (max_y < y) max_y = y;
                        if (max_x < x) max_x = x;
                    }
                }
            }

            // Clip out the shield part.
            int wShield = max_x - min_x + 1;
            int hShield = max_y - min_y + 1;

            // check if shield was found
            if ((wShield < 1) || (hShield < 1))
                return null;

            _ShieldBitmap = new Bitmap(wShield, hShield);
            using (Graphics shield_gr = Graphics.FromImage(_ShieldBitmap))
            {
                shield_gr.DrawImage(bm, 0, 0, new Rectangle(min_x, min_y, wShield, hShield), GraphicsUnit.Pixel);
            }

            // Return the shield.
            return _ShieldBitmap;
        }
    }
}
