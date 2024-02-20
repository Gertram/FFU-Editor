using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace FFUEditor
{
    internal static class BitmapHelper
    {
        internal static Color TransparentBlack = Color.FromArgb(0, 0, 0, 0);
        internal static System.Windows.Media.Imaging.BitmapSource ConvertTo(System.Drawing.Bitmap bitmap)
        {
            var bitmapData = bitmap.LockBits(
                new System.Drawing.Rectangle(0, 0, bitmap.Width, bitmap.Height),
                System.Drawing.Imaging.ImageLockMode.ReadOnly, bitmap.PixelFormat);

            var bitmapSource = System.Windows.Media.Imaging.BitmapSource.Create(
                bitmapData.Width, bitmapData.Height,
                bitmap.HorizontalResolution, bitmap.VerticalResolution,
                PixelFormats.Bgra32, null,
                bitmapData.Scan0, bitmapData.Stride * bitmapData.Height, bitmapData.Stride);

            bitmap.UnlockBits(bitmapData);

            return bitmapSource;
        }
        internal static void DrawLines(byte[] raw, Color[] palette, int symHeight, int rowWidth,
            IEnumerable<IEnumerable<FFULibrary.Sym>> lines)
        {
            var x_off = 0;
            var y_off = 0;

            foreach (var line in lines)
            {
                foreach (var sym in line)
                {
                    sym.DrawTo(raw, x_off, y_off, rowWidth);

                    x_off = (x_off + sym.Width) % rowWidth;
                    if ((x_off + sym.Width) / rowWidth > 0)
                    {
                        y_off += symHeight;
                        x_off = 0;
                    }
                }
                y_off += symHeight;
                x_off = 0;
            }
        }
    }
}
