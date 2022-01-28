using System;
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
    public struct Sym
    {
        public byte width;
        public byte height;
        public byte[] image;
    }
    public partial class MainForm : Form
    {
        DLFont font;
        private int counter = 0;
        private int current = 0;
        struct DLFont
        {
            public Header header;
            public Color[] palitte;
            public SortedDictionary<int, Sym> symbols;
        }
        struct Header
        {
            public short SectionNumbers;
            public int SymCount;
            public byte[] FontInfo;
            public int FileSize;
            public int SectionAddr;
            public int SymAddr;
            public int SymImageAddr;
            public int par1;
            public int par2;
        }
        
        private string? filename = null;
        public MainForm()
        {
            InitializeComponent();
            pictureBox1.BorderStyle = BorderStyle.None;
            groupBox2.KeyDown += MainForm_KeyDown;
        }
        private void LoadFont(byte[] data)
        {
            font = new DLFont
            {
                header = LoadHeader(data),
                palitte = LoadPallite(data, 0x28, 256)
            };
            font.symbols = new SortedDictionary<int, Sym>(LoadSymbols(data, font.header));
            ShowSym(font.symbols.ElementAt(0).Key);
        }
        private Header LoadHeader(byte[] data)
        {
            var header = new Header();
            var h = Encoding.ASCII.GetString(data[0..2]);
            if (h != "UF")
            {
                throw new FormatException("Неверный формат файла(не найден \"UF\" в началае файла");
            }
            header.SectionNumbers = BitConverter.ToInt16(data[2..4]);
            header.SymCount = BitConverter.ToInt32(data[4..8]);
            header.FontInfo = data[8..16];
            header.FileSize = BitConverter.ToInt32(data[16..20]);
            header.SectionAddr = BitConverter.ToInt32(data[20..24]);
            header.SymAddr = BitConverter.ToInt32(data[24..28]);
            header.SymImageAddr = BitConverter.ToInt32(data[28..32]);
            header.par1 = BitConverter.ToInt32(data[32..36]);
            header.par2 = BitConverter.ToInt32(data[36..40]);
            return header;
        }
        private Color[] LoadPallite(byte[] data, int seek, int count)
        {
            var colors = new Color[count];

            for (int i = 0; i < count; i++)
            {
                colors[i] = Color.FromArgb(data[seek + i*4 + 3],data[seek + i*4], data[seek + i*4 + 1], data[seek + i*4 + 2]);
            }

            return colors;
        }
        private Dictionary<int, Sym> LoadSymbols(byte[] data, Header header)
        {
            Dictionary<int, Sym> symbols = new Dictionary<int, Sym>();

            var seek = header.SectionAddr;

            for (int i = 0; i < header.SectionNumbers; i++)
            {
                symbols = symbols.Concat(LoadSection(data, data[(seek + i * 12)..(seek + i * 12 + 12)], header)).ToDictionary(x => x.Key, x => x.Value);
            }

            return symbols;
        }
        private Dictionary<int, Sym> LoadSection(byte[] data, byte[] sectionData, Header header)
        {
            var startCode = BitConverter.ToInt32(sectionData[0..4]);
            var endCode = BitConverter.ToInt32(sectionData[4..8]);
            var seek = BitConverter.ToInt32(sectionData[8..12]) * 8 + header.SymAddr;

            var syms = new Dictionary<int, Sym>(endCode - startCode);

            for (var i = 0; i < endCode - startCode; i++)
            {
                var start = seek + i * 8;
                var end = seek + i * 8 + 8;

                syms[i + startCode] = LoadSym(data, data[start..end], header);
            }

            return syms;
        }
        private Sym LoadSym(byte[] data, byte[] symData, Header header)
        {
            var sym = new Sym
            {
                width = symData[0],
                height = symData[1]
            };
            var length = BitConverter.ToInt16(symData.AsSpan()[2..4]);
            var seek = BitConverter.ToInt32(symData.AsSpan()[4..8]) + header.SymImageAddr;
            sym.image = data[seek..(seek + length)];

            counter++;

            return sym;
        }

        private void LoadFromFile(string filename)
        {
            var data = File.ReadAllBytes(filename);
            LoadFont(data);
            this.filename = filename;
        }
        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var ofd = new OpenFileDialog();
            if (ofd.ShowDialog() != DialogResult.OK)
            {
                return;
            }

            LoadFromFile(ofd.FileName);
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            //LoadFromFile("Z:\\DL\\font\\0");
            //saveToolStripMenuItem_Click(this, null);
        }

        private void MainForm_KeyPress(object sender, KeyPressEventArgs e)
        {

        }

        private void ShowSym(int symcode)
        {
            var sym = font.symbols[symcode];
            mtxtSymCode.Text = string.Format("{0:X}", symcode);
            var bitmap = new Bitmap(Convert.ToInt32(sym.width), Convert.ToInt32(sym.height));
            var buff2 = new List<byte>();
            foreach (var item in sym.image)
            {
                buff2.Add(BitConverter.GetBytes((item & 0xf0) >> 4)[0]);
                buff2.Add(BitConverter.GetBytes(item & 0x0f)[0]);
            }
            var image = new List<byte>();
            var idx = 0;
            while (idx < buff2.Count)
            {
                var i = buff2[idx];
                if (i < 8)
                {
                    image.Add(i);
                    idx += 1;
                }
                else
                {
                    var len = buff2[idx + 1] * 256 + buff2[idx + 2] * 16 + buff2[idx + 3];
                    var range = new byte[len].Select(x => BitConverter.GetBytes(i - 8)[0]);
                    image.InsertRange(image.Count, range);
                    idx += 4;
                }
            }

            var buff = image.GetRange(0, sym.width * sym.height).ToArray();
            for (var y = 0; y < sym.height; y++)
            {
                for (var x = 0; x < sym.width; x++)
                {
                    var val = buff[y * sym.width + x] * 255 / 7;
                    bitmap.SetPixel(x, y, Color.FromArgb(val, val, val));
                }
            }
            pictureBox1.Image = bitmap;
            pictureBox1.SizeMode = PictureBoxSizeMode.Zoom;
        }

        private void MainForm_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Up)
                pictureBox1.Size = new Size(pictureBox1.Size.Width + 10, pictureBox1.Size.Height + 10);
            if (e.KeyCode == Keys.Down && pictureBox1.Size.Width > 10 && pictureBox1.Size.Height > 10)
                pictureBox1.Size = new Size(pictureBox1.Size.Width - 10, pictureBox1.Size.Height - 10);
            if (e.KeyCode == Keys.Right)
            {
                current++;
                if(current == font.symbols.Count)
                {
                    current = 0;
                }
                ShowSym(font.symbols.ElementAt(current).Key);
                
            }
            if (e.KeyCode == Keys.Left)
            {
                current--;
                if(current == -1)
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

        }

        private void MainForm_Click(object sender, EventArgs e)
        {
            groupBox2.Focus();
        }

        private void mtxtSymCode_MaskInputRejected(object sender, MaskInputRejectedEventArgs e)
        {

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
                    if (section[^1] + 1 != sym.Key)
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

            return addrSymImage + font.symbols.Select(x => x.Value.image.Length).Sum();
        }
        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var sections = CalcSections();

            int addrPalitte = 0x28;
            int addrSection = addrPalitte + font.palitte.Length * 4;
            int addrSym = addrSection + sections.Count * 12;
            int addrSymImage = addrSym + font.symbols.Count * 8;

            int filesize = addrSymImage + font.symbols.Select(x => x.Value.image.Length).Sum();

            if (filesize > font.header.FileSize)
            {
                if(MessageBox.Show("Итоговой размер файла превышает исходный!Хотите продолжить?","Внимание",MessageBoxButtons.YesNo) != DialogResult.Yes)
                {
                    return;
                }
            }

            var sfd = new SaveFileDialog();
            if(sfd.ShowDialog() != DialogResult.OK)
            {
                return;
            }
            //sfd.FileName = "Z:\\DL\\font\\out.txt";
            var fileInfo = new FileInfo(sfd.FileName);

            var fileMode = fileInfo.Exists ? FileMode.Truncate : FileMode.CreateNew;
            BinaryWriter bw = new(File.Open(sfd.FileName, fileMode,FileAccess.Write, FileShare.None));

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

            foreach(var color in font.palitte)
            {
                bw.Write(color.R);
                bw.Write(color.G);
                bw.Write(color.B);
                bw.Write(color.A);
            }
            var idx = 0;
            foreach(var sec in sections)
            {
                bw.Write(sec[0]);
                bw.Write(sec[^1] + 1);
                bw.Write(idx);
                idx += sec[^1] - sec[0] + 1;
            }
            idx = 0;
            foreach(var sym in font.symbols)
            {
                bw.Write(sym.Value.width);
                bw.Write(sym.Value.height);
                bw.Write((short)sym.Value.image.Length);
                bw.Write(idx);
                idx += sym.Value.image.Length;
            }
            foreach(var sym in font.symbols)
            {
                bw.Write(sym.Value.image);
            }

            if (filesize < font.header.FileSize)
            {
                bw.Write(new byte[font.header.FileSize - filesize]);//Размер файла
            }

            bw.Close();
        }

        private void UpdateStatus()
        {
            var filesize = CalcFileSize();
            if (filesize > font.header.FileSize)
            {
                toolStripStatusLabel1.Text = "Итоговой размер файла превышает исходный";
            }else if(filesize < font.header.FileSize)
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
            var form = new AddSymForm();
            if(form.ShowDialog() != DialogResult.OK)
            {
                return;
            }
            font.symbols[form.Code] = form.Sym;
            UpdateStatus();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            var form = new AddSymForm();
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
            if(current == font.symbols.Count)
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

        private void importToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var ofd = new OpenFileDialog();
            if(ofd.ShowDialog() != DialogResult.OK)
            {
                return;
            }

            var stream = File.OpenRead(ofd.FileName);
            stream.Seek(0x57e40, SeekOrigin.Begin);
            var reader = new BinaryReader(stream);
            const int swidth = 16;
            const int sheight = 24;
            const int ssize = swidth * sheight;
            if(font.symbols == null)
            {
                font.symbols = new SortedDictionary<int, Sym>();
            }
            for (int i = 0; i < 66; i++)
            {
                var buffer = new byte[ssize];
                reader.BaseStream.Seek(swidth * 7, SeekOrigin.Current);
                reader.Read(buffer, 0, ssize);
                Bitmap bitmap = null;
                if (i != 7 && i != 0x19 && i != 0x1a && i != 0x1b 
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
                }
                if(i <= 32)
                    bitmap = ResizeBitmap(bitmap, 14, 20);
                else
                    bitmap = ResizeBitmap(bitmap, 10, 20);
                buffer = new byte[bitmap.Width * bitmap.Height];
                for (int y = 0; y < bitmap.Height; y++)
                {
                    for (int x = 0; x < bitmap.Width; x++)
                    {
                        var c = bitmap.GetPixel(x, y);
                        buffer[y * bitmap.Width + x] = (byte)(Math.Max(Math.Max(c.R,c.R),c.B)/32);
                    }
                }
                //buffer = Codek.decode(Codek.encode(buffer));
                for (int y = 0; y < bitmap.Height; y++)
                {
                    for (int x = 0; x < bitmap.Width; x++)
                    {
                        var val = buffer[y * bitmap.Width + x]*32;
                        bitmap.SetPixel(x, y, Color.FromArgb(val, val, val));
                    }
                }
                pictureBox1.Image = bitmap;
                pictureBox1.SizeMode = PictureBoxSizeMode.Zoom;
                Update();
                var sym = new Sym
                {
                    width = (byte)bitmap.Width,
                    height = (byte)bitmap.Height,
                    image = Codek.encode(buffer)
                };
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
                reader.BaseStream.Seek(swidth * 6, SeekOrigin.Current);
            }
            mtxtSymCode.Text = "0xbe";
            btnEnterSymCode_Click(null, null);
        }

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
                var bitmap = new Bitmap(Convert.ToInt32(sym.width), Convert.ToInt32(sym.height));

                for (var y = 0; y < sym.height; y++)
                {
                    for (var x = 0; x < sym.width; x++)
                    {
                        var val = sym.image[y * sym.width + x];
                        bitmap.SetPixel(x, y, Color.FromArgb(val, val, val));
                    }
                }
                pictureBox1.Image = bitmap;
                pictureBox1.SizeMode = PictureBoxSizeMode.Zoom;
            }catch (Exception ex)
            {
                toolStripStatusLabel1.Text = ex.Message;
            }
        }
    }
}
