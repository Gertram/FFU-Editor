using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.IO;
using System.Threading.Tasks;


namespace DLFontViewer
{
    public class UncompressedSym : ISym
    {
        static UncompressedSym()
        {
            var list = new Color[16];
            for (var i = 0; i < 16; i++)
            {
                list[i] = (Color.FromArgb(0x10 * i, 0xff, 0xff, 0xff));
            }
            list[0] = (Color.FromArgb(0));
            list[15] = (Color.White);
            colors = list;
        }
        public byte Width { get; set; }
        public byte Height { get; set; }
        public byte[,] Image { get; set; }
        public int HeaderAddress { get; set; }
        public int Address { get; set; }
        public UncompressedSym() { }
        public void Load(byte[] data, int seek, int length, byte width, byte height)
        {
            byte[,] matrix;
            width = (byte)(length / height * 2);
            /*
                        if (length == width * height / 2)
                        {*/
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
        private static Color[] colors;

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
        public IReadOnlyList<Color> Colors
        {
            get
            {
                var list = new List<Color>();
                for (var i = 0; i < 0xf0; i += 0x10)
                {
                    list.Add(Color.FromArgb(i, 0xff, 0xff, 0xff));
                }
                list.Add(Color.White);
                return list;
            }
        }
        public Bitmap GetBitmap(int width, int height, IReadOnlyList<Color> palitte = null)
        {
            if (palitte == null)
            {
                palitte = Colors;
            }
            var temp = new Bitmap(width, height);
            var resx = width / Width;
            var resy = height / Height;
            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    var val = Image[x / resx, y / resy];
                    temp.SetPixel(x, y, palitte[val]);
                }
            }

            return temp;
        }
        public Bitmap GetBitmap(IReadOnlyList<Color> palitte = null)
        {

            if (palitte == null)
            {
                palitte = Colors;
            }
            var temp = new Bitmap(Width, Height);
            for (int y = 0; y < Height; y++)
            {
                for (int x = 0; x < Width; x++)
                {
                    var val = Image[x, y];
                    temp.SetPixel(x, y, palitte[val]);
                }
            }

            return temp;
        }
        public void writeRow(int row, byte[] dest, int start)
        {
            for (int x = 0; x < Width; x++)
            {
                var val = colors[Image[x, row]];
                dest[start + x * 4 + 0] = val.R;
                dest[start + x * 4 + 1] = val.G;
                dest[start + x * 4+2]= val.B;
                dest[start + x * 4 + 3] = val.A;
            }
        }
        public byte[] Encoded()
        {
            var arr = new byte[Width * Height];
            int i = 0;
            for (int y = 0; y < Height; y++)
            {
                for (int x = 0; x < Width; x++, i++)
                {
                    arr[i] = Image[x, y];
                }
            }
            var temp = new byte[arr.Length / 2];
            for (i = 0; i < temp.Length; i++)
            {
                temp[i] = (byte)((arr[i * 2] << 4) + arr[i * 2 + 1]);
            }
            return temp;
            //return Codek.encode(ToArray());
        }
    }
}
