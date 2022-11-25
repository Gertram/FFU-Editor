using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Threading.Tasks;

namespace DLFontViewer
{
    public class CompressedSym : ISym
    {
        static CompressedSym()
        {
            /*var list = new Color[8];
            for (var i = 0; i < 8; i++)
            {
                list[i] = (Color.FromArgb(0x20 * i, 0xff, 0xff, 0xff));
            }
            list[0] = (Color.FromArgb(0));
            list[15] = (Color.White);
            colors = list;*/
            colors = CompressedSym.Palitte;
        }
        public static Color[] colors;
        public byte Width { get; set; }
        public byte Height { get;  set; }
        public byte[,] Image { get; set; }
        public int HeaderAddress { get; set; }
        public int Address { get; set; }
        public CompressedSym() { }
        public void Load(byte[] data, int seek, int length, byte width, byte height)
        {
            var temp = Codek.decode(data, seek, length);

            width = (byte)(temp.Length / height);
            byte[,] matrix = new byte[width, height];

           
            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    matrix[x, y] = temp[y * width + x];
                }
            }
          
            Width = width;
            Height = height;
            Image = matrix;
        }

        internal byte[] ToArray()
        {
            var data = new byte[Width * Height];

            for (int y = 0; y < Height; y++)
            {
                for (int x = 0; x < Width; x++)
                {
                    data[y * Width + x] = Image[x, y];
                }
            }

            return data;
        }
        private static Color[] Palitte = new Color[]
        {
            Color.FromArgb(0,0,0,0),//32
            Color.FromArgb(0x80,0,0,0),//96
            Color.FromArgb(0xd8,0,0,0),//192
            Color.FromArgb(0xff,0,0,0),
            Color.FromArgb(0xff,0x50,0x50,0x50),
            Color.FromArgb(0xff,0x98,0x98,0x98),
            Color.FromArgb(0xff,0xe0,0xe0,0xe0),
            Color.FromArgb(0xff,0xff,0xff,0xff)
        };
        public IReadOnlyList<Color> Colors { get => Palitte; }
        public Bitmap GetBitmap(int width, int height, IReadOnlyList<Color> palitte = null)
        {
            if (palitte == null)
            {
                palitte = Palitte;
            }
            if(palitte.Count == 16)
            {
                var temp = new Color[8];
                temp[0] = palitte[0];
                temp[1] = palitte[2];
                temp[2] = palitte[4];
                temp[3] = palitte[5];
                temp[4] = palitte[7];
                temp[5] = palitte[10];
                temp[6] = palitte[13];
                temp[7] = palitte[15];
                palitte = temp;

            }
            var bitmap = new Bitmap(width, height);
            var resx = width / Width;
            var resy = height / Height;
            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    bitmap.SetPixel(x, y, palitte[Image[x / resx, y / resy]]);
                }
            }

            return bitmap;
        }
        public Bitmap GetBitmap(IReadOnlyList<Color> palitte = null)
        {
            if(palitte == null)
            {
                palitte = Palitte;
            }
            var temp = new Bitmap(Width, Height);
            for (int y = 0; y < Height; y++)
            {
                for (int x = 0; x < Width; x++)
                {
                    temp.SetPixel(x, y, palitte[Image[x, y]]);
                }
            }

            return temp;
        }
        public byte[] Encoded()
        {
            return Codek.encode(ToArray());
        }

        public void writeRow(int row, byte[] dest, int start)
        {
            for (int x = 0; x < Width; x++)
            {
                dest[start + x] = Image[x, row];
            }
        }
    }
}
