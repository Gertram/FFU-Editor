﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DLFontViewer
{
    public partial class MainForm : Form
    {
        DLFont font;
        private int counter = 0;
        private int current = 0;


        private string filename = null;
        public MainForm()
        {
            InitializeComponent();
            pictureBox1.BorderStyle = BorderStyle.None;
            groupBox2.KeyDown += MainForm_KeyDown;
            mtxtSymCode.KeyDown += MtxtSymCode_KeyDown;
            Focus();
        }

        private void MtxtSymCode_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.R && e.Modifiers == Keys.Control)
            {
                var form = new AddSymForm(font);
                var sym = font.symbols.ElementAt(current);
                form.Code = sym.Key;
                form.Sym = sym.Value;
                if (form.ShowDialog() != DialogResult.OK)
                {
                    return;
                }

                font.symbols[form.Code] = form.Sym;
                UpdateStatus();
            }
            else if (e.KeyCode == Keys.Right && e.Control)
            {
                current++;
                if (current == font.symbols.Count)
                {
                    current = 0;
                }
                ShowSym(font.symbols.ElementAt(current).Key);

            }
            else if (e.KeyCode == Keys.Left && e.Control)
            {
                current--;
                if (current == -1)
                {
                    current = font.symbols.Count - 1;
                }
                ShowSym(font.symbols.ElementAt(current).Key);
            }
        }

        internal DLFont LoadFont<T>(byte[] data) where T : ISym, new()
        {
            return DLFont.LoadFont<T>(data);
        }
        private int FindLeftBorder(ISym sym)
        {
            for (int x = 0; x < sym.Width; x++)
            {
                for (int y = 0; y < sym.Height; y++)
                {
                    if (sym.Image[x, y] != 0)
                    {
                        return x;
                    }
                }
            }
            return -1;
        }
        private int FindRightBorder(ISym sym)
        {
            for (int x = sym.Width - 1; x >= 0; x--)
            {
                for (int y = 0; y < sym.Height; y++)
                {
                    if (sym.Image[x, y] != 0)
                    {
                        return x;
                    }
                }
            }
            return -1;
        }
        private void SymPadding(ISym sym, int left, int right)
        {
            var image = sym.Image;
            var leftBorder = FindLeftBorder(sym);

            int rightBorder = FindRightBorder(sym);

            var symWidth = rightBorder - leftBorder + 1;
            var width = (byte)(symWidth + left + right);
            var temp = new byte[width, sym.Height];
            for (int y = 0; y < sym.Height; y++)
            {
                for (int x = 0; x < symWidth; x++)
                {
                    temp[left + x, y] = sym.Image[x + leftBorder, y];
                }
            }
            sym.Image = temp;
            sym.Width = width;
        }
        private void SymPadding(ISym sym, int border)
        {
            var image = sym.Image;
            var leftBorder = FindLeftBorder(sym);

            int rightBorder = FindRightBorder(sym);

            var symWidth = rightBorder - leftBorder + 1;
            var width = (byte)(symWidth + border * 2);
            var temp = new byte[width, sym.Height];
            for (int y = 0; y < sym.Height; y++)
            {
                for (int x = 0; x < symWidth; x++)
                {
                    temp[border + x, y] = sym.Image[x + leftBorder, y];
                }
            }
            sym.Image = temp;
            sym.Width = width;
            /*if (leftBorder != 0 || rightBorder != sym.Width)
            {
                var width = sym.Width - leftBorder - (sym.Width - rightBorder - 1);
                if (width % 2 == 1)
                {
                    if (rightBorder != sym.Width)
                    {
                        if (rightBorder + 1 == sym.Width)
                        {
                            if (leftBorder == 0)
                            {
                                return;
                            }
                            else {
                                leftBorder--;
                            }
                        }
                        else
                        {
                            rightBorder++;
                        }
                    }
                    else if (leftBorder > 1)
                    {

                        leftBorder--;
                    }
                    else
                    {
                        return;
                    }
                    width++;
                }
                var temp = new byte[width, sym.Height];
                for (int y = 0; y < sym.Height; y++)
                {
                    for (int x = 0; x < width; x++)
                    {
                        temp[x, y] = sym.Image[x + leftBorder, y];
                    }
                }
                var final_width = width + (border - 1) * 2;
                
                var ftemp = new byte[final_width, sym.Height];
                for(int y = 0;y < sym.Height; y++)
                {
                    for(int x = 0; x < width; x++)
                    {
                        ftemp[x + border - 1, y] = temp[x, y];
                    }
                }
                sym.Image = ftemp;
                sym.Width = (byte)final_width;
            }*/
        }
        private void LoadFromFile<T>(string filename) where T : ISym, new()
        {
            var data = File.ReadAllBytes(filename);
            font = LoadFont<T>(data);
            ShowSym(font.symbols.ElementAt(0).Key);
            this.filename = filename;
            ShowRusSymsImage();
        }
        private void ShowRusSymsImage()
        {
            var syms = font.symbols.Where(x => x.Key >= 0x8440 && x.Key <= 0x8491).ToList();
            var bitmap = new Bitmap(850, 810);
            if (!syms.Any())
            {
                pbSymbols.Image = bitmap;
                return;
            }
            ShowSym(0x8440);
            var x_off = 0;
            var y_off = 0;
            foreach (var sym in syms)
            {
                var buff = sym.Value.GetBitmap();
                for (var y = 0; y < buff.Height; y++)
                {
                    for (var x = 0; x < buff.Width; x++)
                    {
                        bitmap.SetPixel(x_off + x, y_off + y, buff.GetPixel(x, y));
                    }
                }

                if ((x_off + buff.Width) / 800 > 0)
                {
                    y_off += buff.Height;
                }
                x_off = (x_off + buff.Width) % 800;
            }
            pbSymbols.Image = bitmap;
            pbSymbols.Width = bitmap.Width;
            pbSymbols.Height = bitmap.Height;
        }
        private void OpenFile<T>() where T : ISym, new()
        {
            var ofd = new OpenFileDialog();
            if (ofd.ShowDialog() != DialogResult.OK)
            {
                return;
            }

            LoadFromFile<T>(ofd.FileName);
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            //LoadFromFile<CompressedSym>("D:\\Diabolik lovers\\Work\\DL\\font\\0");
            // LoadFromFile<CompressedSym>("D:\\PSP\\Princess\\system\\900");
            //button1_Click(null, null);
        }

        private Bitmap GetSymBitmap(ISym sym)
        {
            var bitmap = new Bitmap(sym.Width, sym.Height);

            for (int y = 0; y < sym.Height; y++)
            {
                for (int x = 0; x < sym.Width; x++)
                {
                    var val = sym.Image[x, y]/**35*/;
                    bitmap.SetPixel(x, y, Color.FromArgb(val, val, val));
                }
            }
            return bitmap;
        }

        private void ShowSym(int symcode)
        {
            var sym = font.symbols[symcode];
            mtxtSymCode.Text = string.Format("{0:X}", symcode);
            int i = 0;
            foreach (var item in font.symbols)
            {
                if (item.Key == symcode)
                {
                    break;
                }
                i++;
            }
            current = i;
            SymHeaderAddress.Text = $"{sym.HeaderAddress:X}";
            SymAddressLabel.Text = $"{sym.Address:X}";

            pictureBox1.Image = sym.GetBitmap();
            pictureBox1.SizeMode = PictureBoxSizeMode.Zoom;
        }

        private void MainForm_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.O && e.Modifiers == Keys.Control)
            {
                OpenFile<UncompressedSym>();
                return;
            }
            if (e.KeyCode == Keys.Up)
                pictureBox1.Size = new Size(pictureBox1.Size.Width + 10, pictureBox1.Size.Height + 10);
            if (e.KeyCode == Keys.Down && pictureBox1.Size.Width > 10 && pictureBox1.Size.Height > 10)
                pictureBox1.Size = new Size(pictureBox1.Size.Width - 10, pictureBox1.Size.Height - 10);
            if (e.KeyCode == Keys.Right)
            {
                current++;
                if (current == font.symbols.Count)
                {
                    current = 0;
                }
                ShowSym(font.symbols.ElementAt(current).Key);

            }
            if (e.KeyCode == Keys.Left)
            {
                current--;
                if (current == -1)
                {
                    current = font.symbols.Count - 1;
                }
                ShowSym(font.symbols.ElementAt(current).Key);
            }

        }

        private void btnEnterSymCode_Click(object sender, EventArgs e)
        {
            try
            {
                var sym = int.Parse(mtxtSymCode.Text, System.Globalization.NumberStyles.HexNumber);
                ShowSym(sym);
                for (int i = 0; i < font.symbols.Count; i++)
                {
                    if (font.symbols.ElementAt(i).Key == sym)
                    {
                        current = i;
                        break;
                    }
                }
                mtxtSymCode.BackColor = Color.White;
            }
            catch
            {
                mtxtSymCode.BackColor = Color.Red;
            }
        }

        private void groupBox1_Enter(object sender, EventArgs e)
        {
            try
            {
                var sym = int.Parse(mtxtSymCode.Text, System.Globalization.NumberStyles.HexNumber);
                ShowSym(sym);
                for (int i = 0; i < font.symbols.Count; i++)
                {
                    if (font.symbols.ElementAt(i).Key == sym)
                    {
                        current = i;
                        break;
                    }
                }
                mtxtSymCode.BackColor = Color.White;
            }
            catch
            {
                mtxtSymCode.BackColor = Color.Red;
            }
        }

        private void MainForm_Click(object sender, EventArgs e)
        {
            groupBox2.Focus();
        }

        private void mtxtSymCode_TextChanged(object sender, EventArgs e)
        {
            mtxtSymCode.BackColor = Color.White;
        }

        private List<List<int>> CalcSections()
        {
            var sections = new List<List<int>>();

            var section = new List<int>();

            foreach (var sym in font.symbols)
            {
                if (section.Count == 0)
                {
                    section.Add(sym.Key);
                }
                else
                {
                    if (section.Last() + 1 != sym.Key)
                    {

                        sections.Add(section);
                        section = new List<int>();
                        section.Add(sym.Key);
                    }
                    else
                    {
                        section.Add(sym.Key);
                    }
                }
            }
            if (section.Count > 0)
            {
                sections.Add(section);
            }
            return sections;
        }

        private int CalcFileSize()
        {
            var sections = CalcSections();

            int addrPalitte = 0x28;
            int addrSection = addrPalitte + font.palitte.Length * 4;
            int addrSym = addrSection + sections.Count * 12;
            int addrSymImage = addrSym + font.symbols.Count * 8;

            return addrSymImage + font.symbols.Select(x => x.Value.Encoded().Length).Sum();
        }
        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var sections = CalcSections();

            int addrPalitte = 0x28;
            int addrSection = addrPalitte + font.palitte.Length * 4;
            int addrSym = addrSection + sections.Count * 12;
            int addrSymImage = addrSym + font.symbols.Count * 8;

            int filesize = addrSymImage + font.symbols.Select(x => x.Value.Encoded().Length).Sum();

            if (filesize > font.header.FileSize)
            {
                if (MessageBox.Show("Итоговой размер файла превышает исходный!Хотите продолжить?", "Внимание", MessageBoxButtons.YesNo) != DialogResult.Yes)
                {
                    return;
                }
            }

            var sfd = new SaveFileDialog();
            if (sfd.ShowDialog() != DialogResult.OK)
            {
                return;
            }
            //sfd.FileName = "Z:\\DL\\font\\out.txt";
            var fileInfo = new FileInfo(sfd.FileName);

            var fileMode = fileInfo.Exists ? FileMode.Truncate : FileMode.CreateNew;
            BinaryWriter bw = new BinaryWriter(File.Open(sfd.FileName, fileMode, FileAccess.Write, FileShare.None));

            bw.Write(Encoding.ASCII.GetBytes("UF"));

            bw.Write((short)sections.Count);

            bw.Write(font.symbols.Count);

            bw.Write(font.header.FontInfo);

            if (filesize < font.header.FileSize)
            {
                bw.Write(BitConverter.GetBytes(font.header.FileSize));//Размер файла
            }
            else
            {
                bw.Write(BitConverter.GetBytes(filesize));//Размер файла
            }


            bw.Write(BitConverter.GetBytes(addrSection));//Сдвиг на адрес таблицы секций

            bw.Write(BitConverter.GetBytes(addrSym));//Сдвиг на адрес адрес таблицы символов

            bw.Write(BitConverter.GetBytes(addrSymImage));//Сдивг на адрес изображений символов

            bw.Write(font.header.par1);

            bw.Write(font.header.par2);

            foreach (var color in font.palitte)
            {
                bw.Write(color.R);
                bw.Write(color.G);
                bw.Write(color.B);
                bw.Write(color.A);
            }
            var idx = 0;
            foreach (var sec in sections)
            {
                bw.Write(sec[0]);
                bw.Write(sec.Last() + 1);
                bw.Write(idx);
                idx += sec.Last() - sec[0] + 1;
            }
            idx = 0;
            foreach (var sym in font.symbols)
            {
                var encoded = sym.Value.Encoded();
                bw.Write(sym.Value.Width);
                bw.Write(sym.Value.Height);
                bw.Write((short)encoded.Length);
                bw.Write(idx);
                idx += encoded.Length;
            }
            foreach (var sym in font.symbols)
            {
                bw.Write(sym.Value.Encoded());
            }

            if (filesize < font.header.FileSize)
            {
                bw.Write(new byte[font.header.FileSize - filesize]);//Размер файла
            }

            bw.Write(font.Footer);


            bw.Close();
        }

        private void UpdateStatus()
        {
            var filesize = CalcFileSize();
            if (filesize > font.header.FileSize)
            {
                toolStripStatusLabel1.Text = "Итоговой размер файла превышает исходный";
            }
            else if (filesize < font.header.FileSize)
            {
                toolStripStatusLabel1.Text = "Итоговый размер файл меньше исходного, будут добавлены символы в конце файла";
            }
            else
            {
                toolStripStatusLabel1.Text = "Итоговый размер файла совпадат с исходным";
            }
        }

        private void btnAddNew_Click(object sender, EventArgs e)
        {
            var form = new AddSymForm(font);
            if (form.ShowDialog() != DialogResult.OK)
            {
                return;
            }
            font.symbols[form.Code] = form.Sym;
            UpdateStatus();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            var form = new AddSymForm(font);
            var sym = font.symbols.ElementAt(current);
            form.Code = sym.Key;
            form.Sym = sym.Value;
            if (form.ShowDialog() != DialogResult.OK)
            {
                return;
            }

            font.symbols[form.Code] = form.Sym;
            UpdateStatus();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            var sym = font.symbols.ElementAt(current);
            font.symbols.Remove(sym.Key);
            if (current == font.symbols.Count)
            {
                current = font.symbols.Count - 1;
            }
            else
            {
                current++;
            }
            UpdateStatus();
            ShowSym(font.symbols.ElementAt(current).Key);
        }
        private int GetStartPos(byte[] buffer, int swidth, int sheight)
        {
            for (int x = 0; x < swidth; x++)
            {
                for (int y = 0; y < sheight; y++)
                {
                    if (buffer[y * swidth + x] != 0)
                    {
                        return x;
                    }
                }
            }
            return -1;
        }
        private int GetEndPos(byte[] buffer, int swidth, int sheight)
        {
            for (int x = swidth - 1; x >= 0; x--)
            {
                for (int y = 0; y < sheight; y++)
                {
                    if (buffer[y * swidth + x] != 0)
                    {
                        return x;
                    }
                }
            }
            return -1;
        }
        private void SymWidth(byte[] buffer, int swidth, int sheight, out int width, out int pos)
        {
            pos = GetStartPos(buffer, swidth, sheight);
            width = GetEndPos(buffer, swidth, sheight) - pos + 1;
        }/*
        private void importToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var ofd = new OpenFileDialog();
            if(ofd.ShowDialog() != DialogResult.OK)
            {
                return;
            }

            var stream = File.OpenRead(ofd.FileName);
            //stream.Seek(0x57e40, SeekOrigin.Begin);
            stream.Seek(0x3D1D8, SeekOrigin.Begin);
            var reader = new BinaryReader(stream);
            const int swidth = 12;
            const int sheight = 30;
            const int ssize = swidth * sheight;
            if(font.symbols == null)
            {
                font.symbols = new SortedDictionary<int, Sym>();
            }
            for (int i = 0; i < 66; i++)
            {
                var buffer = new byte[ssize];
                reader.Read(buffer, 0, ssize);
                
                SymWidth(buffer, swidth, sheight, out int width, out int pos);

                Bitmap bitmap = new Bitmap(width, sheight);
                var s = Sym.FromArray(buffer,0,buffer.Length, swidth, sheight);

                for (int y = 0;y < sheight; y++)
                {
                    for(int x = 0;x < width; x++)
                    {
                        var val = s.Image[x+pos, y];
                        bitmap.SetPixel(x, y, Color.FromArgb(val, val, val));
                    }
                }

                if(i > 32)
                {
                    var buff = new Bitmap(bitmap.Width + 2, bitmap.Height);
                    for (int y = 0; y < sheight; y++)
                    {
                        for (int x = 0; x < width; x++)
                        {
                            var val = bitmap.GetPixel(x, y);
                            buff.SetPixel(x+1, y, val);
                        }
                    }
                    bitmap = buff;
                }

                *//*if (i != 7 && i != 0x19 && i != 0x1a && i != 0x1b 
                    && i!=0x1c && i != 1f) {
                    bitmap = new Bitmap(swidth - 4, sheight);
                    for (int y = 0; y < bitmap.Height; y++)
                    {
                        for (int x = 0; x < bitmap.Width; x++)
                        {
                            var val = buffer[y * swidth + x+1];
                            bitmap.SetPixel(x, y, Color.FromArgb(val, val, val));
                        }
                    }
                }
                else if (i <= 32) {
                    bitmap = new Bitmap(swidth - 2, sheight);
                    for (int y = 0; y < bitmap.Height; y++)
                    {
                        for (int x = 0; x < bitmap.Width; x++)
                        {
                            var val = buffer[y * swidth + x];
                            bitmap.SetPixel(x, y, Color.FromArgb(val, val, val));
                        }
                    }
                }
                else {
                    bitmap = new Bitmap(swidth - 6, sheight);
                    for (int y = 0; y < bitmap.Height; y++)
                    {
                        for (int x = 0; x < bitmap.Width; x++)
                        {
                            var val = buffer[y * swidth + x + 2];
                            bitmap.SetPixel(x, y, Color.FromArgb(val, val, val));
                        }
                    }
                }*//*
                if (i <= 32)
                    bitmap = ResizeBitmap(bitmap, 10, 20);
                else
                    bitmap = ResizeBitmap(bitmap, 10, 20);
                buffer = new byte[bitmap.Width * bitmap.Height];
                for (int y = 0; y < bitmap.Height; y++)
                {
                    for (int x = 0; x < bitmap.Width; x++)
                    {
                        var c = bitmap.GetPixel(x, y);
                        buffer[y * bitmap.Width + x] = (byte)(Math.Max(Math.Max(c.R,c.R),c.B)/35);
                    }
                }
                //buffer = Codek.decode(Codek.encode(buffer));
                *//*for (int y = 0; y < bitmap.Height; y++)
                {
                    for (int x = 0; x < bitmap.Width; x++)
                    {
                        var val = buffer[y * bitmap.Width + x]*32;
                        bitmap.SetPixel(x, y, Color.FromArgb(val, val, val));
                    }
                }*//*
                pictureBox1.Image = bitmap;
                pictureBox1.SizeMode = PictureBoxSizeMode.Zoom;
                Update();
                var sym = Sym.FromArray(buffer,0,buffer.Length,(byte)bitmap.Width, (byte)bitmap.Height);
                if (i <= 32)
                {
                    font.symbols[i + 0x8440] = sym;
                }
                else if(i <= 47)
                {
                    font.symbols[i + 0x844F] = sym;
                }
                else
                {
                    font.symbols[i + 0x8450] = sym;
                }
            }
            mtxtSymCode.Text = "8470";
            btnEnterSymCode_Click(null, null);
        }*/

        public Bitmap ResizeBitmap(Bitmap bmp, int width, int height)
        {
            Bitmap result = new Bitmap(width, height);
            using (Graphics g = Graphics.FromImage(result))
            {
                g.DrawImage(bmp, 0, 0, width, height);
            }

            return result;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            try
            {
                var symcode = int.Parse(mtxtSymCode.Text, System.Globalization.NumberStyles.HexNumber);
                var sym = font.symbols[symcode];
                var bitmap = new Bitmap(Convert.ToInt32(sym.Width), Convert.ToInt32(sym.Height));

                for (var y = 0; y < sym.Height; y++)
                {
                    for (var x = 0; x < sym.Width; x++)
                    {
                        var val = sym.Image[x, y];
                        bitmap.SetPixel(x, y, Color.FromArgb(val, val, val));
                    }
                }
                pictureBox1.Image = bitmap;
                pictureBox1.SizeMode = PictureBoxSizeMode.Zoom;
            }
            catch (Exception ex)
            {
                toolStripStatusLabel1.Text = ex.Message;
            }
        }

        private void mtxtSymCode_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {

                e.Handled = true;
            }
        }

        private void compressedToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFile<CompressedSym>();
        }

        private void uncompressedToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFile<UncompressedSym>();
        }
        private byte[,] Invert(byte[,] image)
        {
            var temp = (byte[,])image.Clone();
            var width = temp.GetLength(1);
            var height = temp.GetLength(0);
            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    if (temp[y, x] == 0)
                    {
                        temp[y, x] = 4;
                    }
                    else
                    {
                        temp[y, x] = 1;
                    }
                }
            }
            return temp;
        }
        private void anotherCompressedToolStripMenuItem_Click(object sender, EventArgs e)
        {
            /*var ofd = new OpenFileDialog();
            if(ofd.ShowDialog() != DialogResult.OK)
            {
                return;
            }
            var data = File.ReadAllBytes(ofd.FileName);*/
            var data = File.ReadAllBytes("D:\\Diabolik lovers\\Work\\DL\\font\\0");
            var tempFont = LoadFont<CompressedSym>(data);

            foreach (var item in tempFont.symbols.Where(x => x.Key >= 0x8440 && x.Key <= 0x8491))
            {
                font.symbols[item.Key] = item.Value;
                item.Value.Image = Invert(item.Value.Image);
            }
            ShowRusSymsImage();
        }

        /* private void CutSymsToolStripMenuItem_Click(object sender, EventArgs e)
         {
             var win = new CutSymsForm();
             if(win.ShowDialog() != DialogResult.OK)
             {
                 return;
             }
             foreach (var sym in font.symbols.Where(x => (x.Key >= 0x8440 && x.Key <= 0x8491) || (x.Key >= 0x41 && x.Key <= 0x7a)))
             {
                 SymPadding(sym.Value,(int)win.Value);
             }
             ShowRusSymsImage();
         }*/
        private void SetBorder(ISym sym, int border)
        {
            var leftBorder = FindLeftBorder(sym);

            int rightBorder = FindRightBorder(sym);

            var symWidth = rightBorder - leftBorder + 1 + border * 2;
            var temp = new byte[symWidth, sym.Height];
            for (int y = 0; y < sym.Height; y++)
            {
                for (int x = leftBorder; x <= rightBorder; x++)
                {
                    temp[x - leftBorder + border, y] = sym.Image[x, y];
                }
            }
            sym.Image = temp;
            sym.Width = (byte)symWidth;
        }
        private void CutSym(ISym sym, int border)
        {
            var leftBorder = FindLeftBorder(sym);
            if (leftBorder > border)
            {
                leftBorder = border;
            }

            int rightBorder = FindRightBorder(sym);
            if (rightBorder < sym.Width - border)
            {
                rightBorder = sym.Width - border - 1;
            }

            //var symWidth = rightBorder - leftBorder + 1;
            var width = (byte)(rightBorder - leftBorder + 1);
            var temp = new byte[width, sym.Height];
            for (int y = 0; y < sym.Height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    temp[x, y] = sym.Image[x + leftBorder, y];
                }
            }
            sym.Image = temp;
            sym.Width = width;
        }
        int FindBottomBorder(ISym sym)
        {
            for (int y = 0; y < sym.Height; y++)
            {
                var found = false;
                for (int x = sym.Width - 1; x >= 0; x--)
                {
                    found = sym.Image[x, y] != 0;
                }
                if (!found)
                {
                    return y;
                }
            }
            
            return -1;
        }
        private void CutSym(ISym sym, int width, int height)
        {
            if (width > sym.Width)
            {
                width = sym.Width;
            }
       
            var temp = new byte[width, height];

            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    temp[x, y] = sym.Image[x, y];
                }
            }
            sym.Image = temp;
            sym.Height = (byte)height;
            sym.Width = (byte)width;
        }
        private void CutSymsToolStripMenuItem_Click(object sender, EventArgs e)
        {/*
            var win = new CutSymsForm();
            if (win.ShowDialog() != DialogResult.OK)
            {
                return;
            }*/

            /*foreach (var sym in font.symbols.Where(x => x.Key >= 0x8440 && x.Key <= 0x8460))
            {
                CutSym(sym.Value, 4);
            }
            foreach (var sym in font.symbols.Where(x => x.Key >= 0x8470 && x.Key <= 0x8491))
            {
                CutSym(sym.Value, 7);
            }*/
            foreach (var sym in font.symbols.Where(x => x.Key >= 'A' && x.Key <= 'Z'))
            {
                SetBorder(sym.Value, 2);
            }
            /*foreach (var sym in font.symbols.Where(x => x.Key >= 'a' && x.Key <= 'z'))
            {
                SetBorder(sym.Value, 1);
            }*/
            ShowRusSymsImage();
        }
        private byte[,] NewPalitte(byte[,] image)
        {
            var height = image.GetLength(0);
            var width = image.GetLength(1);
            var temp = new byte[height, width];
            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    var value = image[y, x];
                    if (value != 0)
                    {
                        temp[y, x] = (byte)(value * 7 / 15);//(byte)(value >> 1);
                    }
                }
            }
            return temp;
        }
        private void anotherUncompressToolStripMenuItem_Click(object sender, EventArgs e)
        {

            //var data = File.ReadAllBytes("D:\\Diabolik lovers\\Work\\DL\\font\\advfont24x30.ffu");
            var data = File.ReadAllBytes("D:\\PSP\\Wolves\\VITA\\font\\window\\font\\sysfont.ffu");
            var tempFont = LoadFont<UncompressedSym>(data);

            //font.symbols = new SortedDictionary<int, ISym>(font.symbols.Take(font.symbols.Count / 4).ToDictionary(x => x.Key, x => x.Value));


            foreach (var pair in tempFont.symbols)
            {
                if (font.symbols.ContainsKey(pair.Key) || pair.Key >= 0x8440 && pair.Key <= 0x8491)
                {
                    var temp = new CompressedSym();
                    temp.Width = pair.Value.Width;
                    temp.Image = NewPalitte(tempFont.symbols[pair.Key].Image);
                    CutSym(temp, 0x20,0x1e);
                    font.symbols[pair.Key] = temp;
                }
            }

            
            var buff = new List<int>();
            foreach (var item in font.symbols)
            {
                if (!tempFont.symbols.ContainsKey(item.Key))
                {
                    buff.Add(item.Key);
                }
            }
            foreach (var item in buff)
            {
                font.symbols[item].Width = 0x10;
                font.symbols[item].Image = new byte[0x10, 0x1e];
                font.symbols[item].Height = 0x1e;
            }
            ShowRusSymsImage();
        }
        private bool FindColor(IDictionary<Color, byte> colors, Color color, out int value)
        {
            foreach (var item in colors)
            {
                if (item.Key.ToArgb() == color.ToArgb())
                {
                    value = item.Value;
                    return true;
                }
            }
            value = 0;
            return false;
        }
        private ISym ConvertToSym<T>(char sym, Bitmap bitmap, Dictionary<Color, byte> colors) where T : ISym, new()
        {
            var data = new byte[bitmap.Width, bitmap.Height];

            for (int y = 0; y < bitmap.Height; y++)
            {
                for (int x = 0; x < bitmap.Width; x++)
                {
                    var pixel = bitmap.GetPixel(x, y);

                    if (FindColor(colors, pixel, out var value))
                    {
                        data[x, y] = (byte)value;
                    }
                    else
                    {
                        return null;
                    }
                }
            }
            var temp = new T
            {
                Image = data,
                Width = (byte)bitmap.Width,
                Height = (byte)bitmap.Height
            };
            return temp;
        }
        private static Encoding encoding = Encoding.GetEncoding("shift-jis");
        private int ToInt(char sym)
        {
            var bytes = encoding.GetBytes(new char[] { sym });
            if (bytes.Length == 2)
            {
                return BitConverter.ToUInt16(bytes.Reverse().ToArray(), 0);
            }
            return bytes[0];
        }
        private void GenerateFontToolStripMenuItem_Click(object sender, EventArgs e)
        {

            var win = new GenerateFontWindow();
            if (win.ShowDialog() != DialogResult.OK)
            {
                return;
            }

            foreach (var item in win.Syms)
            {
                var sym = ConvertToSym<CompressedSym>(item.Key, item.Value, win.Colors);


                var key = ToInt(item.Key);
                font.symbols[key] = sym;
            }
            ShowRusSymsImage();
            /* 
             var bitmap = new Bitmap(600, 600);
             var graphics = Graphics.FromImage(bitmap);

             var font = new Font("Cambria", 16);
             var brush = new SolidBrush(Color.Black);
             var drawFormat = new StringFormat();
             drawFormat.FormatFlags = StringFormatFlags.DirectionVertical;
             var rectf = new RectangleF(0, 0, 300, 200);
             graphics.FillRectangle(Brushes.Transparent, rectf);
             graphics.DrawString("АБСДЕ", font, brush, rectf);
             graphics.Flush();
             pbSymbols.BackColor = Color.Transparent;
             pbSymbols.Image = bitmap;*/
        }

        private void MainForm_DragDrop(object sender, DragEventArgs e)
        {
            if (!e.Data.GetDataPresent(DataFormats.FileDrop, false))
            {
                return;
            }
            var filename = (e.Data.GetData(DataFormats.FileDrop, false) as string[])[0];

            LoadFromFile<UncompressedSym>(filename);
        }

        private void MainForm_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop)) e.Effect = DragDropEffects.Copy;
        }
        private char ToChar(int num)
        {
            if(num < 256)
            {
                return encoding.GetChars(new byte[] { (byte)(num) })[0];
            }
            return encoding.GetChars(new byte[] { (byte)(num >> 8), (byte)(num & 0xff) })[0];
        }
        private void pNGToolStripMenuItem_Click(object sender, EventArgs e)
        {
            /*foreach(var sym in this.font.symbols)
            {
                var bitmap = sym.Value.GetBitmap();
            }*/
            var symbols = font.symbols/*.Where(x => x.Key <=127 || x.Key >= 0x8440 && x.Key <= 0x8494)*/.Select(x => x.Value).ToArray();
            var symbols2 = font.symbols/*.Where(x => x.Key <= 127 || x.Key >= 0x8440 && x.Key <= 0x8494)*/.ToArray();
            int symInRow = (int)Math.Ceiling(Math.Sqrt(symbols.Length));
            int rows = (int)Math.Ceiling((float)(symbols.Length) / (float)(symInRow));
            const int symWidth = 32;//29;
            const int symHeight = 37;//0x4a; ;
            int width = symInRow * symWidth;
            int height = rows * symHeight;
            //var image = Indexed();
            var info = new Hjg.Pngcs.ImageInfo(width, height, 8, false, false, true);

            var chunk = new Hjg.Pngcs.Chunks.PngChunkPLTE(info);
            chunk.SetNentries(UncompressedSym.colors.Length);
            var tchunk = new Hjg.Pngcs.Chunks.PngChunkTRNS(info);
            var alpha = new int[UncompressedSym.colors.Length];
            for (int i = 0; i < UncompressedSym.colors.Length; i++)
            {
                var color = UncompressedSym.colors[i];
                alpha[i] = color.A;
                chunk.SetEntry(i, color.R, color.G, color.B);
            }
            tchunk.SetPalletteAlpha(alpha);
            var stream = File.OpenWrite("export.png");
            using (var file = new StreamWriter("export.xml",false))
            {
                file.WriteLine("<font>");
                file.WriteLine($"<info face =\"Desyrel\" size=\"{symHeight}\" charset=\"unicode\" />");
                file.WriteLine($"<common lineHeight=\"{symHeight}\" base=\"{symHeight}\" scaleW=\"{width}\" scaleH=\"{height}\" pages=\"1\" />");
                file.WriteLine("<pages>");
                file.WriteLine("<page id=\"0\" file=\"export.png\" />");
                file.WriteLine("</pages >");
                file.WriteLine($"<chars count=\"{symbols.Length}\" >");
                var num = 0;
                foreach(var sym in symbols2)
                {
                    var letter = ToChar(sym.Key);
                    var swidth = sym.Value.Width;
                    var padLeft = (symWidth - swidth)/2;
                    var x = (num % symInRow) * symWidth+padLeft;
                    var y = num / symInRow * symHeight;
                    file.WriteLine($"<char id=\"{(int)letter}\" x=\"{x}\" y=\"{y}\" width=\"{swidth}\" height=\"{symHeight}\" xoffset=\"0\" yoffset=\"0\" xadvance=\"{swidth}\" />");
                    num++;
                }
                file.WriteLine("</chars>");
                file.WriteLine("</font>");
                file.Close();
                var writer = new Hjg.Pngcs.PngWriter(stream, info);
                try
                {
                    writer.GetChunksList().Queue(chunk);
                    writer.GetChunksList().Queue(tchunk);
                    writer.CompLevel = 9;
                    var raw = new byte[width];
                    var line = new byte[width / 2];
                    for (int j = 0; j < rows; j++)
                    {
                        for (var y = 0; y < symHeight; y++)
                        {
                            for (int i = 0; i < symInRow && j * symInRow + i < symbols.Length; i++)
                            {
                                var sym = symbols[i + symInRow * j];
                                var padLeft = (symWidth - sym.Width) / 2;
                                var padRight = (symWidth - sym.Width - padLeft);
                                var x_offset = i * symWidth;

                                for (var x = 0; x < padLeft; x++)
                                {
                                    raw[x + x_offset] = 0;
                                }
                                for (var x = (symWidth - padRight); x < symWidth; x++)
                                {
                                    raw[x + x_offset] = 0;
                                }
                                sym.writeRow(y, raw, padLeft + x_offset);
                            }
                            /*for(var x = 0; x < width/2; x++)
                            {
                                line[x] = (byte)((raw[x] << 4) + raw[x + 1]);
                            }*/
                            writer.WriteRowByte(raw, y + j * symHeight);
                        }
                    }
                    writer.End();
                }
                finally
                {
                    stream.Close();
                }
            }
        }
    }
}
