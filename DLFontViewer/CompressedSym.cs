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

            /*var temp = new List<byte>();
            for (int i = 0; i < length; i++)
            {
                var val = data[i + seek];
                temp.Add((byte)(val >> 4));
                temp.Add((byte)(val & 0x0f));
            }*/
            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    matrix[x, y] = temp[y * width + x];
                }
            }

            /* }
             else
             {*//*
                 var buf = width;
                 width = height;
                 height = buf;*//*
                 matrix = new byte[width, height];

                 var temp = new List<byte>();
                 for (int i = 0; i < length; i++)
                 {
                     var val = data[i + seek];
                     temp.Add((byte)(val >> 4));
                     temp.Add((byte)(val & 0x0f));
                 }

                 for (int y = 0; y < height; y++)
                 {
                     for (int x = 0; x < width; x++)
                     {
                         matrix[x, y] = temp[y * width + x];
                     }
                 }
             }*/
            this.Width = width;
            this.Height = height;
            this.Image = matrix;
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
            Color.FromArgb(0,0,0,0),
            Color.FromArgb(0x80,0,0,0),
            Color.FromArgb(0xd8,0,0,0),
            Color.FromArgb(0xff,0,0,0),
            Color.FromArgb(0x88,0x88,0x88),
            Color.FromArgb(0xb0,0xb0,0xb0),
            Color.FromArgb(0xe0,0xe0,0xe0),
            Color.FromArgb(0xff,0xff,0xff)
        };
        public IReadOnlyList<Color> Colors { get => Palitte; }
        public Bitmap GetBitmap(int width, int height, IReadOnlyList<Color> palitte = null)
        {
            if (palitte == null)
            {
                palitte = Palitte;
            }
            var temp = new Bitmap(width, height);
            var resx = width / Width;
            var resy = height / Height;
            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    temp.SetPixel(x, y, palitte[Image[x / resx, y / resy]]);
                }
            }

            return temp;
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
    }
}
