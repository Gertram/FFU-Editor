using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

using Hjg.Pngcs;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;

namespace DLFontViewer
{
    /*00000000000000000000
00000000000000000000
01111111111111111110
01111111111111111110
01111111111111111110
01111111111111111110
01111111111111111110
01111111111111111110
01111111111111111110
01111111111111111110
01111111111111111110
01111111111111111110
01111111111111111110
01111111111111111110
01111111111111111110
01111111111111111110
01111111111111111110
01111111111111111110
00000000000000000000
00000000000000000000*/
    public partial class AddSymForm : Form
    {
        public AddSymForm()
        {
            InitializeComponent();
        }
        private int code;
        private ISym sym;
        public int Code
        {
            get
            {
                return code;
            }
            set
            {
                code = value;
                maskedTextBox1.Text = string.Format("{0:X}", code);
            }
        }
        private char toChar(int sym)
        {
            if (sym < 10)
            {
                return (char)(sym + 0x30);
            }
            return (char)(sym + 0x37);
        }
        private void ShowSym(IReadOnlyList<Color> pallite = null)
        {
            pictureBox2.Width = SymWidth * scale_size;
            pictureBox2.Height = SymHeight * scale_size;
            pictureBox1.Height = SymHeight * scale_size_min;
            pictureBox1.Width = SymWidth * scale_size_min;
            this.buff = sym.GetBitmap(pictureBox2.Width, pictureBox2.Height, pallite);
            pictureBox1.Image = sym.GetBitmap(pictureBox1.Width, pictureBox1.Height, pallite);

            pictureBox2.Image = this.buff;
        }
        public ISym Sym
        {
            get => sym;
            set
            {
                sym = value;
                txtWidth.Text = string.Format("{0:X}", sym.Width);
                txtHeight.Text = string.Format("{0:X}", sym.Height);
                var text = new List<char>();
                for (int y = 0; y < sym.Height; y++)
                {
                    for (int x = 0; x < sym.Width; x++)
                    {
                        text.Add(toChar(sym.Image[x, y]));
                    }
                    text.Add('\n');
                }

                richTextBox1.Text = new string(text.ToArray());
                var colors = sym.Colors;
                Color0.BackColor = colors[0];
                Color1.BackColor = colors[1];
                Color2.BackColor = colors[2];
                Color3.BackColor = colors[3];
                Color4.BackColor = colors[4];
                Color5.BackColor = colors[5];
                Color6.BackColor = colors[6];
                Color7.BackColor = colors[7];
                if (colors.Count > 8)
                {
                    Color8.BackColor = colors[8];
                    Color9.BackColor = colors[9];
                    Color10.BackColor = colors[10];
                    Color11.BackColor = colors[11];
                    Color12.BackColor = colors[12];
                    Color13.BackColor = colors[13];
                    Color14.BackColor = colors[14];
                    Color15.BackColor = colors[15];

                }
                ShowSym();
            }
        }

        private void AddButton_Click(object sender, EventArgs e)
        {
            /* try
             {
                 Code = int.Parse(maskedTextBox1.Text, System.Globalization.NumberStyles.HexNumber);
                 var buff = richTextBox1.Text.Replace("\n", "").Select(x => (byte)(x < 'A' ? x - 0x30 : x - 'A' + 10)).ToArray();
                 sym = Sym.FromArray(buff,0,buff.Length, SymWidth, SymHeight);

                 DialogResult = DialogResult.OK;
             }
             catch (Exception ex)
             {
                 MessageBox.Show(ex.ToString(), "Ошибка!");
             }*/
        }
        
        
       /*
        private void Test()
        {
            ImageInfo imi = new ImageInfo(cols, rows, 8, false);
            // 8 bits per channel, no alpha 
            // open image for writing to a output stream 
            PngWriter png = new PngWriter(outputStream, imi);
            // add some optional metadata (chunks) png.getMetadata().setDpi(100.0); 
            png.getMetadata().setTimeNow(0);
            // 0 seconds fron now = now png.getMetadata().setText(PngChunkTextVar.KEY_Title, "just a text image"); 
            png.getMetadata().setText("my key", "my text");
            ImageLineInt iline = new ImageLineInt(imi);
            for (int col = 0; col < imi.cols; col++)
            {
                // this line will be written to all rows
                int r = 255; int g = 127; int b = 255 * col / imi.cols;
                ImageLineHelper.setPixelRGB8(iline, col, r, g, b);
                // orange-ish gradient
            }
            for (int row = 0; row < png.imgInfo.rows; row++)
            {
                png.writeRow(iline);
            }
            png.end();

        }*/
        private byte SymWidth => (byte)int.Parse(txtWidth.Text, System.Globalization.NumberStyles.HexNumber);
        private byte SymHeight => (byte)int.Parse(txtHeight.Text, System.Globalization.NumberStyles.HexNumber);
        private void richTextBox1_TextChanged(object sender, EventArgs e)
        {
            try
            {
                var width = SymWidth;
                var height = SymHeight;
                var buff = richTextBox1.Text.Trim().Replace("\n", "").PadRight(width * height, '0');

                //var bitmap = new Bitmap(Convert.ToInt32(width), Convert.ToInt32(height));
                //for (var y = 0; y < height; y++)
                //{
                //    for (var x = 0; x < width; x++)
                //    {
                //        var val = (buff[y * width + x] - 48) /** 255 / 7*/;
                //        bitmap.SetPixel(x, y, Color.FromArgb(val, val, val));
                //    }
                //}
                //pictureBox2.Width = SymWidth * scale_size;
                //pictureBox2.Height = SymWidth * scale_size;
                //pictureBox1.Height = SymHeight * scale_size_min;
                //pictureBox1.Width = SymWidth * scale_size_min;
                //this.buff = ResizeBitmap(bitmap, pictureBox2.Width, pictureBox2.Height);
                //pictureBox1.Image = ResizeBitmap(bitmap, pictureBox1.Width, pictureBox1.Height); ;
                //pictureBox2.Image = this.buff;

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "Ошибка!");
            }
        }
        private Bitmap buff;
        const int scale_size = 10;
        const int scale_size_min = 2;
        private void PictureBox_MouseMove(object sender, MouseEventArgs e)
        {
            try
            {
                var pos = e.Location;
                var image = pictureBox2.Image;
                var bitmap = new Bitmap(buff);
                var width = pictureBox2.Width;
                var height = pictureBox2.Height;
                var res_x = width / SymWidth;
                var res_y = height / SymHeight;
                var pos_x = pos.X / res_x;
                var pos_y = pos.Y / res_y;
                var startx = res_x * pos_x;
                var endx = res_x * (pos_x + 1) - 1;
                var starty = res_y * pos_y;
                var endy = res_y * (pos_y + 1) - 1;
                var strokeLength = 4;

                if (IsDraw)
                {
                    for (int y = starty; y <= endy; y++)
                    {
                        for (int x = startx; x <= endx; x++)
                        {
                            buff.SetPixel(x, y, Color.White);
                        }
                    }
                    pictureBox2.Image = buff;
                    var text = richTextBox1.Text.Trim().Split('\n').Select(x => x.ToArray()).ToList();
                    text[pos_y][pos_x] = '7';
                    richTextBox1.Text = string.Join("\n", text.Select(x => new string(x)));
                }
                else
                {
                    for (int x = startx; x <= endx; x += strokeLength * 2)
                    {
                        for (int tx = x; tx < x + strokeLength; tx++)
                        {
                            bitmap.SetPixel(tx, starty, Color.White);
                            bitmap.SetPixel(tx, endy, Color.White);
                        }
                    }
                    for (int x = startx + strokeLength; x <= endx; x += strokeLength * 2)
                    {
                        for (int tx = x; tx < x + strokeLength; tx++)
                        {
                            bitmap.SetPixel(tx, starty, Color.Black);
                            bitmap.SetPixel(tx, endy, Color.Black);
                        }
                    }
                    for (int y = starty; y <= endy; y += strokeLength * 2)
                    {
                        for (int ty = y; ty < y + strokeLength; ty++)
                        {
                            bitmap.SetPixel(startx, ty, Color.White);
                            bitmap.SetPixel(endx, ty, Color.White);
                        }
                    }
                    for (int y = starty + strokeLength; y <= endy; y += strokeLength * 2)
                    {
                        for (int ty = y; ty < y + strokeLength; ty++)
                        {
                            bitmap.SetPixel(startx, y, Color.Black);
                            bitmap.SetPixel(endx, y, Color.Black);
                        }
                    }
                    pictureBox2.Image = bitmap;
                }
            }
            catch
            {
                Console.WriteLine("HUI");
            }
        }
        private bool IsDraw { get; set; }= false;
        private void pictureBox2_MouseDown(object sender, MouseEventArgs e)
        {
            IsDraw = true;
            PictureBox_MouseMove(sender, e);
        }

        private void pictureBox2_MouseLeave(object sender, EventArgs e)
        {
            IsDraw = false;
        }

        private void pictureBox2_MouseUp(object sender, MouseEventArgs e)
        {
            IsDraw = false;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            var leftsize = int.Parse(LeftSizeTextBox.Text);
            var rightsize = int.Parse(RightCutSize.Text);
            var width = sym.Width - leftsize - rightsize;

            var temp = new byte[width, sym.Height];
            for (int y = 0; y < sym.Height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    temp[x, y] = sym.Image[x + leftsize, y];
                }
            }
            sym.Image = temp;
            sym.Width = (byte)width;
            txtWidth.Text = $"{width:X}";
            ShowSym();
        }

        private void SelectColor_Click(object sender, EventArgs e)
        {
            if (sender is Button button && colorDialog1.ShowDialog() == DialogResult.OK)
            {
                button.BackColor = colorDialog1.Color;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            var pallite = new List<Color>
            {
                Color0.BackColor,
                Color1.BackColor,
                Color2.BackColor,
                Color3.BackColor,
                Color4.BackColor,
                Color5.BackColor,
                Color6.BackColor,
                Color7.BackColor
            };
            if (sym.Colors.Count > 8)
            {
                pallite.Add(Color8.BackColor);
                pallite.Add(Color9.BackColor);
                pallite.Add(Color10.BackColor);
                pallite.Add(Color11.BackColor);
                pallite.Add(Color12.BackColor);
                pallite.Add(Color13.BackColor);
                pallite.Add(Color14.BackColor);
                pallite.Add(Color15.BackColor);
            }
            ShowSym(pallite);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            var left = (int)LeftPadding.Value;
            var right = (int)RightPadding.Value;
            var width = sym.Width + left+right;

            var temp = new byte[width, sym.Height];
            /*for (int y = 0; y < sym.Height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    temp[x, y] = 4;
                }
            }*/
            for (int y = 0; y < sym.Height; y++)
            {
                for (int x = 0; x < sym.Width; x++)
                {
                    temp[x+left, y] = sym.Image[x, y];
                }
            }
            sym.Image = temp;
            sym.Width = (byte)width;
            txtWidth.Text = $"{width:X}";
            ShowSym();
        }
    }
}
