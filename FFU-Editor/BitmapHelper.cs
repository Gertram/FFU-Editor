using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace FFU_Editor
{
    internal static class BitmapHelper
    {
        internal static Color TransparentBlack = Color.FromArgb(0, 0, 0, 0);
        internal static int FindLeftBorder(Bitmap bitmap)
        {
            for (var x = 0; x < bitmap.Width; x++)
            {
                for (var y = 0; y < bitmap.Height; y++)
                {
                    var pixel = bitmap.GetPixel(x, y);
                    if (!pixel.Equals(TransparentBlack))
                    {
                        return x;
                    }
                }
            }
            return -1;
        }
        internal static int FindRightBorder(Bitmap bitmap)
        {
            for (var x = bitmap.Width - 1; x >= 0; x--)
            {
                for (var y = 0; y < bitmap.Height; y++)
                {
                    var pixel = bitmap.GetPixel(x, y);
                    if (!pixel.Equals(TransparentBlack))
                    {
                        return x;
                    }
                }
            }
            return -1;
        }
    }
}
