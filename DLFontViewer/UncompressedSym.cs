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
            /*for (var i = 0; i < 16; i++)
            {
                list[i] = (Color.FromArgb(0x10 * i, 0xff, 0xff, 0xff));
            }
            list[0] = (Color.FromArgb(0));
            list[15] = (Color.White);*/
/*          //RED
            list = new Color[]
            {
                Color.FromArgb(0,0,0,0),
                Color.FromArgb(0x40,0,0,0),
                Color.FromArgb(0x80,0,0,0),
                Color.FromArgb(0xa0,0,0,0),
                Color.FromArgb(0xd8,0,0,0),
                Color.FromArgb(0xff,0,0,0),
                Color.FromArgb(0xff,0x0a,0x0a,0x14),
                Color.FromArgb(0xff,0x15,0x14,0x28),
                Color.FromArgb(0xff,0x20,0x1e,0x3c),
                Color.FromArgb(0xff,0x2a,0x28,0x50),
                Color.FromArgb(0xff,0x35,0x32,0x65),
                Color.FromArgb(0xff,0x3f,0x3c,79),
                Color.FromArgb(0xff,0x4a,0x46,0x8d),
                Color.FromArgb(0xff,0x54,0x50,0xa1),
                Color.FromArgb(0xff,0x5f,0x5b,0xb6),
                Color.FromArgb(0xff,0x6a,0x65,0xca)
            };*/
            //BLUE
            list = new Color[]
            {
                Color.FromArgb(0,0,0,0),
                Color.FromArgb(0x40,0,0,0),
                Color.FromArgb(0x80,0,0,0),
                Color.FromArgb(0xa0,0,0,0),
                Color.FromArgb(0xd8,0,0,0),
                Color.FromArgb(0xff,0,0,0),
                Color.FromArgb(0xff,0x17,0x14,0x0e),
                Color.FromArgb(0xff,0x2e,0x28,0x1c),
                Color.FromArgb(0xff,0x45,0x3c,0x2b),
                Color.FromArgb(0xff,0x5c,0x50,0x39),
                Color.FromArgb(0xff,0x73,0x64,0x47),
                Color.FromArgb(0xff,0x8a,0x78,0x55),
                Color.FromArgb(0xff,0xa2,0x8c,0x64),
                Color.FromArgb(0xff,0xb8,0xa0,0x72),
                Color.FromArgb(0xff,0xd0,0xb4,0x80),
                Color.FromArgb(0xff,0xe7,0xc8,0x8f),
            };

            for(var i = 0;i < 16; i++)
            {
                var color = list[i];
                list[i] = Color.FromArgb(color.A,color.B, color.G, color.R);
            }

            colors = list;
        }
        public static Color[] colors;
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
                dest[start + x] = Image[x, row];
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
