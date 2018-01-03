using System;
using System.Collections.Generic;
using System.Linq;
using System.Drawing;

namespace Tiveria.Common.Extensions
{
    public static class GraphicsExtensions
    {
        public static void DrawImageScaled(this Graphics g, Image image, Rectangle dest, ContentAlignment align)
        {
            if (g == null || image == null || image.Width== 0 || image.Height == 0)
                return;

            Rectangle finaldest = CalcDestination(dest, image.Size, align);
            g.DrawImage(image, finaldest);
        }

        private static Rectangle CalcDestination(Rectangle dest, Size sourcesize, ContentAlignment align)
        {
            Rectangle result = new Rectangle();
            var scaleX = (float)dest.Width / (float)sourcesize.Width;
            var scaleY = (float)dest.Height / (float)sourcesize.Height;
            float scale = Math.Min(scaleX, scaleY);

            if (scaleX < scaleY) // Vertical alignment
            {
                result.X = dest.X;
                result.Width = dest.Width;
                result.Height = (int)(sourcesize.Height * scale);
                switch (align)
                {
                    case ContentAlignment.BottomCenter:
                    case ContentAlignment.BottomLeft:
                    case ContentAlignment.BottomRight: result.Y = dest.Y + dest.Height - result.Height; break;
                    case ContentAlignment.MiddleCenter:
                    case ContentAlignment.MiddleLeft:
                    case ContentAlignment.MiddleRight: result.Y = dest.Y + ((dest.Height - result.Height)/2); break;
                    default: result.Y = dest.Y; break;
                }
            }
            else // Horizontal alignment
            {
                result.Y = dest.Y;
                result.Height = dest.Height;
                result.Width = (int)(sourcesize.Width * scale);
                switch (align)
                {
                    case ContentAlignment.BottomRight: 
                    case ContentAlignment.MiddleRight:
                    case ContentAlignment.TopRight: result.X = dest.X + dest.Width - result.Width; break;
                    case ContentAlignment.BottomCenter:
                    case ContentAlignment.MiddleCenter:
                    case ContentAlignment.TopCenter: result.X = dest.X + ((dest.Width - result.Width) / 2); break;
                    default: result.X = dest.X; break;
                }
            }
            return result;
        }
    }
}
