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
        private Image GetClipboardImage()
        {

            // Try to paste PNG data.
            if (Clipboard.ContainsData("PNG"))
            {
                Object png_object = Clipboard.GetData("PNG");
                if (png_object is MemoryStream)
                {
                    var png_stream = png_object as MemoryStream;
                    return Image.FromStream(png_stream);
                }
            }

            // Try to paste bitmap data.
            if (Clipboard.ContainsImage())
            {
                return Clipboard.GetImage();
            }

            // We couldn't find anything useful. Return null.
            return null;
        }
        private Color[,] Temp;
        private Color[] Palette;
        private byte[] Picture;
        private void PrintArray(int[,] buff)
        {
            var writer = new BinaryWriter(File.OpenWrite("temp2.txt"));
            for (int y = 0; y < buff.GetLength(0); y++)
            {
                for (int x = 0; x < buff.GetLength(1); x++)
                {
                    var val = buff[y, x];
                    if (val != 0)
                    {
                        Console.WriteLine("fse");
                    }
                    var temp = (byte)val;
                    writer.Write(temp);
                }
            }
            writer.Close();
        }
        private void PrintBitmap(Bitmap buff)
        {
            var writer = new BinaryWriter(File.OpenWrite("temp2.txt"));
            for (int y = 0; y < buff.Height; y++)
            {
                for (int x = 0; x < buff.Width; x++)
                {
                    var val = GetIndex(buff.GetPixel(x, y));
                    var temp = (byte)val;
                    writer.Write(temp);
                }
            }
            writer.Close();
        }
        private void LoadImage(ref Bitmap image)
        {
            var temp = new int[image.Height, image.Width];
            var t2 = new Color[image.Height, image.Width];
            for (int i = 0; i < image.Width; i++)
            {
                for (int j = 0; j < image.Height; j++)
                {
                    if (image.GetPixel(i, j).R >= 250)
                    {
                        t2[j, i] = Color.FromArgb(0, 0, 0, 0);
                    }
                    else
                    {
                        t2[j, i] = image.GetPixel(i, j);
                    }
                }
            }

            for (int x = 0; x < image.Width; x++)
            {
                for (int y = 0; y < image.Height; y++)
                {
                    var pixel = t2[y, x];
                    var ind = GetIndex(pixel);
                    if (ind == -1)
                    {
                        MessageBox.Show("Color not found");
                        return;
                    }
                    temp[y, x] = ind;
                }
            }
            PrintArray(temp);
            var white = (byte)GetIndex(Color.FromArgb(151, 251, 248, 247));
            for (int y = 1; y < image.Height - 1; y++)
            {
                for (int x = 1; x < image.Width - 1; x++)
                {
                    var pixel = Palette[temp[y, x]];
                    if (pixel.A == 0 || pixel.A == 151)
                    {
                        continue;
                    }
                    var left = Palette[temp[y, x - 1]];
                    if (left.A == 0)
                    {
                        temp[y, x - 1] = white;
                    }
                    var right = Palette[temp[y, x + 1]];
                    if (right.A == 0)
                    {
                        temp[y, x + 1] = white;
                    }
                    var top = Palette[temp[y - 1, x]];
                    if (top.A == 0)
                    {
                        temp[y - 1, x] = white;
                    }
                    var bottom = Palette[temp[y + 1, x]];
                    if (bottom.A == 0)
                    {
                        temp[y + 1, x] = white;
                    }
                }
            }
            PrintArray(temp);
            var buff = new int[image.Height, image.Width];
            for (int j = 1; j < image.Height - 1; j++)
            {
                for (int i = 1; i < image.Width - 1; i++)
                {
                    var pixel = Palette[temp[j, i]];
                    if (pixel.A != 0)
                    {
                        buff[j, i] = temp[j, i];
                        continue;
                    }
                    var left = Palette[temp[j, i - 1]];
                    var right = Palette[temp[j, i + 1]];
                    var top = Palette[temp[j - 1, i]];
                    var bottom = Palette[temp[j + 1, i]];
                    var val = (left.A + right.A + top.A + bottom.A) / 4;
                    if (val > 0)
                    {
                        for (byte k = 0; k < Palette.Length; k++)
                        {
                            if (Palette[k].A >= val)
                            {
                                buff[j, i] = k;
                                break;
                            }
                        }
                    }
                    else
                    {
                        buff[j, i] = 255;
                    }
                }
            }
            var raw = new byte[buff.Length];
            PrintArray(buff);
            for (int y = 0, i = 0; y < image.Height; y++)
            {
                for (int x = 0; x < image.Width; x++, i++)
                {
                    raw[i] = (byte)buff[y, x];
                }
            }
            image = image.Clone(new Rectangle(0, 0, image.Width, image.Height), PixelFormat.Format8bppIndexed);
            try
            {
                var pal = image.Palette;
                for (int i = 0; i < pal.Entries.Length; i++)
                {
                    pal.Entries[i] = Palette[i];
                }
                image.Palette = pal;
                var data = image.LockBits(new Rectangle(0, 0, image.Width, image.Height), System.Drawing.Imaging.ImageLockMode.ReadWrite, System.Drawing.Imaging.PixelFormat.Format8bppIndexed);

                IntPtr scan0 = data.Scan0;
                Marshal.Copy(raw, 0, scan0, raw.Length);
                scan0 = new IntPtr(scan0.ToInt64() + data.Stride);


                image.UnlockBits(data);
                pictureBox2.Image = image;
                pictureBox2.Width = image.Width * 3;
                pictureBox2.Height = image.Height * 3;
                pictureBox2.SizeMode = PictureBoxSizeMode.Zoom;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }

        }
        private Bitmap Indexed()
        {
            int height = Temp.GetLength(0);
            int width = Temp.GetLength(1);
            var image = new Bitmap(width, height, PixelFormat.Format8bppIndexed);
            var data = image.LockBits(new Rectangle(0, 0, image.Width, image.Height), System.Drawing.Imaging.ImageLockMode.WriteOnly, System.Drawing.Imaging.PixelFormat.Format8bppIndexed);

            var raw = new byte[image.Width];

            var pal = image.Palette;
            for (int i = 0; i < pal.Entries.Length; i++)
            {
                pal.Entries[i] = Palette[i];
            }
            image.Palette = pal;
            IntPtr scan0 = data.Scan0;
            for (int y = 0; y < image.Height; y++)
            {
                for (int x = 0; x < image.Width; x++)
                {
                    raw[x] = (byte)GetIndex(Temp[y, x]);
                }

                Marshal.Copy(raw, 0, scan0, raw.Length);
                scan0 = new IntPtr(scan0.ToInt64() + data.Stride);
            }


            image.UnlockBits(data);
            return image;
        }
        private int GetIndex(Color color)
        {
            for (int i = 0; i < Palette.Length; i++)
            {
                if (Palette[i].Equals(color))
                {
                    return i;
                }
            }
            return -1;
        }
        private void LoadPalette()
        {
            var file = new FileInfo("C:\\Users\\artem\\OneDrive\\Рабочий стол\\palitte.bap");
            var stream = file.OpenRead();
            var reader = new BinaryReader(stream);
            reader.BaseStream.Seek(16, SeekOrigin.Begin);
            var size = (stream.Length - 16) >> 2;
            var table = new Color[size];

            for (int i = 0; i < size; i++)
            {
                var red = reader.ReadByte();
                var green = reader.ReadByte();
                var blue = reader.ReadByte();
                var alpha = reader.ReadByte();
                var color = Color.FromArgb(alpha, red, green, blue);
                table[i] = color;
            }
            reader.Close();
            colorDialog1.CustomColors = table.Select(x => x.ToArgb()).ToArray();
            table = table.OrderBy(x => x.A).ToArray();
            Color0.BackColor = table.Last();
            Palette = table;
        }
        private static Color[,] Smooth(Color[,] temp, IList<Color> colors)
        {
            int width = temp.GetLength(1);
            int height = temp.GetLength(0);
            var buff = new Color[height, width];
            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    buff[y, x] = Color.FromArgb(0, 0, 0, 0);
                }
            }
            for (int j = 1; j < height - 1; j++)
            {
                for (int i = 1; i < width - 1; i++)
                {
                    var pixel = temp[j, i];
                    if (pixel.A != 0)
                    {
                        buff[j, i] = temp[j, i];
                        continue;
                    }
                    var left = temp[j, i - 1];
                    var right = temp[j, i + 1];
                    var top = temp[j - 1, i];
                    var bottom = temp[j + 1, i];
                    var val = (left.A + right.A + top.A + bottom.A) / 4;
                    if (val > 0)
                    {
                        for (int k = 0; k < colors.Count; k++)
                        {
                            if (colors[k].A >= val)
                            {
                                var color = colors[k];
                                buff[j, i] = color;
                                break;
                            }
                        }
                    }
                    else
                    {
                        buff[j, i] = Color.FromArgb(0, 0, 0, 0);
                    }
                }
            }
            return buff;
        }
        private void InsertFromClipBoard()
        {
            LoadPalette();
            var bitmap = new Bitmap(GetClipboardImage());
            //LoadImage(ref bitmap);
            //e.Handled = true;
            //return;

            var white = Palette.OrderByDescending(x => x.R + x.G + x.B + x.A).First();
            var temp = new Color[bitmap.Height, bitmap.Width];
            for (int i = 0; i < bitmap.Width; i++)
            {
                for (int j = 0; j < bitmap.Height; j++)
                {
                    var pixel = bitmap.GetPixel(i, j);
                    if (pixel.Equals(white))
                    {
                        temp[j, i] = Palette.First();
                    }
                    else
                    {
                        temp[j, i] = bitmap.GetPixel(i, j);
                    }
                }
            }
            for (int j = 1; j < bitmap.Height - 1; j++)
            {
                for (int i = 1; i < bitmap.Width - 1; i++)
                {
                    var pixel = temp[j, i];
                    if (pixel.A == 0 || pixel.A == 151)
                    {
                        continue;
                    }
                    var left = temp[j, i - 1];
                    if (left.A == 0)
                    {
                        temp[j, i - 1] = white;
                    }
                    var right = temp[j, i + 1];
                    if (right.A == 0)
                    {
                        temp[j, i + 1] = white;
                    }
                    var top = temp[j - 1, i];
                    if (top.A == 0)
                    {
                        temp[j - 1, i] = white;
                    }
                    var bottom = temp[j + 1, i];
                    if (bottom.A == 0)
                    {
                        temp[j + 1, i] = white;
                    }
                }
            }
            var border = int.Parse(BorderWidth.Text);
            var buff = temp;
            /*for (int i = 0; i < border - 1; i++)
            {

                buff = Smooth(buff);
            }*/

            for (int y = 0; y < bitmap.Height; y++)
            {
                for (int x = 0; x < bitmap.Width; x++)
                {
                    bitmap.SetPixel(x, y, buff[y, x]);
                }
            }
            Temp = buff;
            pictureBox2.Image = bitmap;
            pictureBox2.Width = bitmap.Width;
            pictureBox2.Height = bitmap.Height;
        }
        private void AddSymForm_KeyDown(object sender, KeyEventArgs e)
        {
            /*try
            {*/
            if (e.KeyCode == Keys.V && e.Control && Clipboard.ContainsImage())
            {
                InsertFromClipBoard();
                e.Handled = true;
                return;
            }
            if (e.KeyCode == Keys.S && e.Control)
            {
                int width = Temp.GetLength(1);
                int height = Temp.GetLength(0);
                //var image = Indexed();
                var info = new Hjg.Pngcs.ImageInfo(width, height, 8, false, false, true);

                var chunk = new Hjg.Pngcs.Chunks.PngChunkPLTE(info);
                chunk.SetNentries(Palette.Length);
                var tchunk = new Hjg.Pngcs.Chunks.PngChunkTRNS(info);
                var alpha = new int[Palette.Length];
                for (int i = 0; i < Palette.Length; i++)
                {
                    var color = Palette[i];
                    alpha[i] = color.A;
                    chunk.SetEntry(i, color.R, color.G, color.B);
                }
                tchunk.SetPalletteAlpha(alpha);
                var stream = File.OpenWrite("export.png");
                var writer = new Hjg.Pngcs.PngWriter(stream, info);
                try
                {
                    writer.GetChunksList().Queue(chunk);
                    writer.GetChunksList().Queue(tchunk);
                    writer.CompLevel = 0;
                    byte[] raw = new byte[width];
                    for (int y = 0; y < height; y++)
                    {
                        for (int x = 0; x < width; x++)
                        {
                            raw[x] = (byte)GetIndex(Temp[y, x]);
                        }
                        writer.WriteRowByte(raw, y);
                    }
                    writer.End();
                }
                finally
                {
                    stream.Close();
                }
            }
            if (e.KeyCode == Keys.C && e.Control)
            {

                var image = Indexed();
                Clipboard.SetData("PNG", image);
                Clipboard.SetImage(image);
                Clipboard.SetDataObject(image);
            }/*
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "Ошибка!");
            }*/

        }/*
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
        private bool IsDraw = false;
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
        private int[,] CreateBorder(int[,] image, int color, int back = 0)
        {
            var height = image.GetLength(0);
            var width = image.GetLength(1);
            var temp = new int[height, width];
            for (int j = 1; j < height; j++)
            {
                for (int i = 1; i < width; i++)
                {
                    temp[j, i] = back;
                }
            }
            for (int j = 1; j < height - 1; j++)
            {
                for (int i = 1; i < width - 1; i++)
                {
                    var pixel = image[j, i];
                    if (pixel == back)
                    {
                        continue;
                    }
                    temp[j, i] = pixel;
                    var left = image[j, i - 1];
                    if (left == back)
                    {
                        temp[j, i - 1] = color;
                    }
                    var right = image[j, i + 1];
                    if (right == back)
                    {
                        temp[j, i + 1] = color;
                    }
                    var top = image[j - 1, i];
                    if (top == back)
                    {
                        temp[j - 1, i] = color;
                    }
                    var bottom = image[j + 1, i];
                    if (bottom == back)
                    {
                        temp[j + 1, i] = color;
                    }
                }
            }
            return temp;
        }
        private void LoadSimplePng(string filename)
        {
            Color[,] image;
            using (var stream = File.OpenRead(filename))
            {
                var reader = new PngReader(stream);
                var info = reader.ImgInfo;
                if (info.Indexed)
                {
                    return;
                }
                image = new Color[info.Rows, info.Cols];
                for (int i = 0; i < info.Rows; i++)
                {
                    var line = reader.ReadRowByte(i);
                    var row = Hjg.Pngcs.ImageLineHelper.Unpack(info, line.ScanlineB, null, false);
                    for (int j = 0; j < info.Cols; j++)
                    {
                        var offset = j << 2;
                        var red = row[offset];
                        var green = row[offset + 1];
                        var blue = row[offset + 2];
                        var alpha = row[offset + 3];
                        if (alpha == 0)
                        {
                            red = green = blue = 0;
                        }
                        image[i, j] = Color.FromArgb(alpha, red, green, blue);
                    }
                }

            }

            using (var stream = File.OpenWrite("export.png"))
            {
                int width = image.GetLength(1);
                int height = image.GetLength(0);
                var info = new Hjg.Pngcs.ImageInfo(width, height, 8, true, false, false);
                var writer = new Hjg.Pngcs.PngWriter(stream, info);

                writer.CompLevel = 0;
                byte[] raw = new byte[width << 2];
                for (int y = 0; y < height; y++)
                {
                    for (int x = 0; x < width; x++)
                    {
                        var offset = x << 2;
                        var pixel = image[y, x];
                        raw[offset] = pixel.R;
                        raw[offset + 1] = pixel.G;
                        raw[offset + 2] = pixel.B;
                        raw[offset + 3] = pixel.A;
                    }
                    writer.WriteRowByte(raw, y);
                }
                writer.End();

            }
        }
        private void AddSymForm_DragDrop(object sender, DragEventArgs e)
        {
            if (!e.Data.GetDataPresent(DataFormats.FileDrop, false))
            {
                return;
            }
            var filename = (e.Data.GetData(DataFormats.FileDrop, false) as string[])[0];
            LoadSimplePng(filename);
            return;
            using (var stream = File.OpenRead(filename))
            {
                var reader = new PngReader(stream);
                var plte = reader.GetChunksList().GetById1(Hjg.Pngcs.Chunks.PngChunkPLTE.ID) as Hjg.Pngcs.Chunks.PngChunkPLTE;
                var trns = reader.GetChunksList().GetById1(Hjg.Pngcs.Chunks.PngChunkTRNS.ID) as Hjg.Pngcs.Chunks.PngChunkTRNS;
                var info = reader.ImgInfo;
                var palette = new Color[plte.GetNentries()];
                var alpha = trns.GetPalletteAlpha();
                for (int i = 0; i < palette.Length; i++)
                {
                    palette[i] = Color.FromArgb(plte.GetEntry(i));
                    palette[i] = Color.FromArgb(alpha[i], palette[i]);
                }
                Palette = palette;
                var alphaPalette = new List<int>();
                Color BaseWhite = palette.Last();
                for (int i = palette.Length - 1; i >= 0; i--)
                {
                    var color = palette[i];
                    if (color.R != BaseWhite.R || color.B != BaseWhite.B || color.G != BaseWhite.G)
                    {
                        break;
                    }
                    alphaPalette.Add(i);
                }
                alphaPalette.Reverse();
                var image = new int[info.Rows, info.Cols];
                for (int i = 0; i < info.Rows; i++)
                {
                    var line = reader.ReadRowByte(i);
                    var row = Hjg.Pngcs.ImageLineHelper.Unpack(info, line.ScanlineB, null, false);
                    for (int j = 0; j < info.Cols; j++)
                    {
                        image[i, j] = row[j];
                    }
                }
                var white = palette.OrderByDescending(x => x.R + x.G + x.B + x.A).First();
                int width = info.Cols;
                int height = info.Rows;
                var border = int.Parse(BorderWidth.Text);
                var borderPalette = new int[border + 1];
                var step = BaseWhite.A / border;
                for (int i = 0; i < border; i++)
                {
                    var need = step * (border - i);
                    for (int j = 0; j < alphaPalette.Count; j++)
                    {
                        var color = palette[alphaPalette[j]];
                        if (color.A >= need)
                        {
                            if (i > 0 && borderPalette[i - 1] == alphaPalette[j] && j > 0)
                            {
                                borderPalette[i] = alphaPalette[j - 1];
                            }
                            else
                            {
                                borderPalette[i] = alphaPalette[j];
                            }
                            break;
                        }
                    }
                }
                borderPalette[0] = 0xea;
                border = 0;
                //borderPalette[1] = 3;
                //borderPalette[0] = 0x8;
                //borderPalette[border] = alphaPalette[0];

                for (int i = 0; i <= border; i++)
                {
                    image = CreateBorder(image, borderPalette[i], 0);
                }
                var temp = new Color[height, width];
                for (int i = 0; i < width; i++)
                {
                    for (int j = 0; j < height; j++)
                    {
                        temp[j, i] = palette[image[j, i]];
                    }
                }

                var bitmap = new Bitmap(width, height);
                for (int y = 0; y < height; y++)
                {
                    for (int x = 0; x < width; x++)
                    {
                        bitmap.SetPixel(x, y, temp[y, x]);
                    }
                }
                PrintBitmap(bitmap);
                Temp = temp;
                this.buff = bitmap;
                pictureBox2.Image = bitmap;
                pictureBox2.Width = bitmap.Width;
                pictureBox2.Height = bitmap.Height;
            }

            //InsertFromClipBoard();
        }

        private void AddSymForm_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop)) e.Effect = DragDropEffects.Copy;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            var left = (int)LeftPadding.Value;
            var right = (int)RightPadding.Value;
            var width = sym.Width + left+right;

            var temp = new byte[width, sym.Height];
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
