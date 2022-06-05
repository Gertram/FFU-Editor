using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.IO;
using System.Threading.Tasks;


namespace DLFontViewer
{
    public class UncompressedSym: ISym
    {
        public byte Width { get; set; }
        public byte Height { get;  set; }
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
        public IReadOnlyList<Color> Colors { get
            {
                return new List<Color>
                {
                    Color.FromArgb(0,0,0,0),
                    Color.FromArgb(0x40,0,0,0),
                    Color.FromArgb(0x80,0,0,0),
                    Color.FromArgb(0xa0,0,0),
                    Color.FromArgb(0xD8,0,0,0),
                    Color.FromArgb(0XFF,0,0,0),
                    Color.FromArgb(0xff,0x28,0x28,0x28),
                    Color.FromArgb(0xff,0x50,0x50,0x50),
                    Color.FromArgb(0xff,0x70,0x70,0x70),
                    Color.FromArgb(0xff,0x88,0x88,0x88),
                    Color.FromArgb(0xff,0x98,0x98,0x98),
                    Color.FromArgb(0xff,0xb0,0xb0,0xb0),
                    Color.FromArgb(0xff,0xc8,0xc8,0xc8),
                    Color.FromArgb(0xff,0xe0,0xe0,0xe0),
                    Color.FromArgb(0xff,0xf0,0xf0,0xf0),
                    Color.FromArgb(0xff,0xff,0xff,0xff)
                }; 
            }
        }
        public Bitmap GetBitmap(int width, int height, IReadOnlyList<Color> palitte = null)
        {
            if(palitte == null)
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
            for(i = 0;i < temp.Length; i++)
            {
                temp[i] = (byte)((arr[i * 2] << 4) + arr[i * 2 + 1]);
            }
            return temp;
            //return Codek.encode(ToArray());
        }
    }
}
