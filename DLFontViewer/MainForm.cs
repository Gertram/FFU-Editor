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
    public partial class MainForm : Form
    {
        DLFont font;
        private int counter = 0;
        private int current = 0;
        struct DLFont
        {
            public Header header;
            public Color[] palitte;
            public Section[] sections;
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
        struct Section
        {
            public Sym[] syms;
        }
        struct Sym
        {
            public byte width;
            public byte height;
            public int code;
            public byte[] image;
        }
        private string? filename = null;
        public MainForm()
        {
            InitializeComponent();
            pictureBox1.BorderStyle = BorderStyle.None;
        }
        private void LoadFont(byte[] data)
        {
            font = new DLFont
            {
                header = LoadHeader(data),
                palitte = LoadPallite(data, 0x40, 256)
            };
            font.sections = LoadSections(data, font.header);
            ShowSym(0);
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
                colors[i] = Color.FromArgb(data[seek + i], data[seek + i + 1], data[seek + i + 2]);
            }

            return colors;
        }
        private Section[] LoadSections(byte[] data, Header header)
        {
            var sections = new Section[header.SectionNumbers];

            var seek = header.SectionAddr;

            for (int i = 0; i < header.SectionNumbers; i++)
            {
                sections[i] = LoadSection(data,data[(seek + i*12)..(seek + i*12 + 12)] ,header);
            }

            return sections;
        }
        private Section LoadSection(byte[] data,byte[] sectionData,Header header)
        {
            var startCode = BitConverter.ToInt32(sectionData[0..4]);
            var endCode = BitConverter.ToInt32(sectionData[4..8]);
            var seek = BitConverter.ToInt32(sectionData[8..12])*8 + header.SymAddr;

            var syms = new Sym[endCode - startCode];

            for (var i = 0; i < endCode - startCode; i++)
            {
                var start = seek + i * 8;
                var end = seek + i * 8 + 8;
                
                syms[i] = LoadSym(data, i + startCode, data[start..end], header);
            }
            var section = new Section
            {
                syms = syms
            };

            return section;
        }
        private Sym LoadSym(byte[] data,int code,byte[] symData,Header header)
        {
            var sym = new Sym
            {
                code = code,
                width = symData[0],
                height = symData[1]
            };
            var length = BitConverter.ToInt16(symData.AsSpan()[2..4]);
            var seek = BitConverter.ToInt32(symData.AsSpan()[4..8]) + header.SymImageAddr;
            var buff = data[seek..(seek + length)];
            var buff2 = new List<byte>();
            foreach(var item in buff)
            {
                buff2.Add(BitConverter.GetBytes((item & 0xf0) >> 4)[0]);
                buff2.Add(BitConverter.GetBytes(item & 0x0f)[0]);
            }
            var image = new List<byte>();
            var idx = 0;
            while(idx < buff2.Count)
            {
                var i = buff2[idx];
                if(i < 8)
                {
                    image.Add(i);
                    idx += 1;
                }
                else
                {
                    var len = buff2[idx + 1] * 256 + buff2[idx + 2] * 16 + buff2[idx + 3];
                    var range = new byte[len].Select(x => BitConverter.GetBytes(i - 8)[0]);
                    image.InsertRange(image.Count,range);
                    idx += 4;
                }
            }

            sym.image = image.ToArray();

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
            if(ofd.ShowDialog() != DialogResult.OK)
            {
                return;
            }

            LoadFromFile(ofd.FileName);
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            LoadFromFile("Z:\\DL\\font\\0");
        }

        private void MainForm_KeyPress(object sender, KeyPressEventArgs e)
        {
           
           }

        private void ShowSym(int num)
        {
            if(num < 0)
            {
                return;
            }
            Sym sym = new();
            var idx = 0;
            foreach (var section in font.sections)
            {
                foreach(var s in section.syms)
                {
                    if(idx == num)
                    {
                        sym = s;
                    }
                    idx++;
                }
            }
            if(sym.code == 0)
            {
                return;
            }
            current = num;
            var bitmap = new Bitmap(Convert.ToInt32(sym.width), Convert.ToInt32(sym.height));
            for (var y = 0; y < sym.height; y++)
            {
                for (var x = 0; x < sym.width; x++)
                {
                    var val = sym.image[y * sym.width + x] * 255 / 7;
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
                ShowSym(current + 1);
            }
            if (e.KeyCode == Keys.Left)
            {
                ShowSym(current - 1);
            }

        }
    }
}
