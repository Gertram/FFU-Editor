using System.Linq;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace FFUEditor.ColorExtension
{
    internal static class WpfColorHelper
    {
        public static System.Windows.Media.Color MediaFromArgb(FFULibrary.Color color)
        {
            return System.Windows.Media.Color.FromArgb(color.A, color.R, color.G, color.B);
        }
        public static System.Drawing.Color DrawingFromArgb(FFULibrary.Color color)
        {
            return System.Drawing.Color.FromArgb(color.A, color.R, color.G, color.B);
        }
        public static int ToArgb(this FFULibrary.Color color)
        {
            return (color.A << 24) | (color.R << 16) | (color.G << 8) | color.B;
        }
        public static BitmapSource ToBitmapSource(this FFULibrary.Sym sym,FFULibrary.Color[] palette, int scale)
        {
            var buff = sym.Draw(scale);
            var bitmapPalette = new BitmapPalette(palette.Select(x => MediaFromArgb(x)).ToArray());
            var source = BitmapSource.Create(
                sym.Width * scale,
                sym.Height * scale,
                96, 96,
                PixelFormats.Indexed8, bitmapPalette, buff, sym.Width * scale);
            return source;
        }
        public static BitmapSource ToBitmapSource(this FFULibrary.Sym sym,FFULibrary.Color[] palette)
        {
            var bitmapPalette = new BitmapPalette(palette.Select(x => MediaFromArgb(x)).ToArray());
            var source = BitmapSource.Create(
                sym.Width,
                sym.Height,
                96, 96,
                PixelFormats.Indexed8, bitmapPalette, sym.Raw, sym.Width);
            return source;
        }
    }
}
