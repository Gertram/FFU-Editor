using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Threading.Tasks;

namespace DLFontViewer
{
    public interface ISym
    {
        void Load(byte[] data, int seek, int length, byte width, byte height);
        int HeaderAddress { get; set; }
        int Address { get; set; }
        byte Width { get; set; }
        byte Height { get; set; }
        Bitmap GetBitmap(IReadOnlyList<Color> palitte = null);
        Bitmap GetBitmap(int width,int height, IReadOnlyList<Color> palitte = null);
        IReadOnlyList<Color> Colors { get; }
        byte[,] Image { get; set; }
        byte[] Encoded();

        void writeRow(int row, byte[] dest, int start);
    }
}
