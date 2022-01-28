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
        private Sym sym;
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
            if(sym < 10)
            {
                return (char)(sym + 0x30);
            }
            return (char)(sym + 0x37);
        }
        public Sym Sym
        {
            get => sym;
            set
            {
                sym = value;
                txtWidth.Text = string.Format("{0:X}", sym.width);
                txtHeight.Text = string.Format("{0:X}", sym.height);
                var buff = new List<char>();
                foreach(var s in sym.image)
                {
                    var left = toChar((s & 0xf0) >> 4);
                    var right = toChar(s & 0x0f);
                    buff.Add(left);
                    buff.Add(right);
                }

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

                var buff3 = image.GetRange(0, sym.width * sym.height).ToArray();

                var list = new List<string>();

                for(int i = 0; i < image.Count/sym.width; i++)
                {
                    list.Add(new string(buff3[(i * sym.width)..(i * sym.width + sym.width)].Select(x => toChar(x)).ToArray()));
                }

                richTextBox1.Text = string.Join("\n", list);
                Update();
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Code = int.Parse(maskedTextBox1.Text, System.Globalization.NumberStyles.HexNumber);
            var buff = richTextBox1.Text.Replace("\n", "");
            var data = new List<byte>();
            while (buff.Length > 0)
            {
                var i = 0;
                while (i + 1 != buff.Length && buff[i] == buff[i + 1])
                {
                    i++;
                }
                if (i > 3)
                {
                    data.Add(((byte)(buff[0] - 40)));
                    i++;
                    data.Add((byte)(i / 256));
                    data.Add((byte)(i / 16));
                    data.Add((byte)(i % 16));
                    buff = buff[i..];
                }
                else if (i > 0)
                {
                    for (var j = 0; j <= i; j++)
                    {
                        data.Add(((byte)(buff[0] - 48)));
                    }
                    buff = buff[(i + 1)..];
                }
                else
                {
                    data.Add(((byte)(buff[i] - 48)));
                    buff = buff[1..];
                }
            }
            if (data.Count % 2 == 1)
            {
                data.Add(0);
            }
            var buff2 = new byte[data.Count / 2];
            for (var i = 0; i < data.Count / 2; i++)
            {
                var val1 = data[i * 2] << 4;
                var val2 = data[i * 2 + 1];
                buff2[i] = (byte)(val1 | val2);
            }
            Sym = new Sym
            {
                width = (byte)int.Parse(txtWidth.Text, System.Globalization.NumberStyles.HexNumber),
                height = (byte)int.Parse(txtHeight.Text, System.Globalization.NumberStyles.HexNumber),
                image = buff2
            };
            DialogResult = DialogResult.OK;
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
        private void AddSymForm_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.KeyCode == Keys.V && e.Control && Clipboard.ContainsImage())
            {
                var bitmap = new Bitmap(GetClipboardImage());
                var str = new List<char>();
                for (var i = 0;i < bitmap.Height; i++)
                {
                    for(var j = 0;j < bitmap.Width; j++)
                    {
                        var c = bitmap.GetPixel(j, i);
                        c = Color.FromArgb(c.A, c.A, c.A);
                        bitmap.SetPixel(j, i, c);
                        str.Add(toChar(c.R / 36));
                    }
                    str.Add('\n');
                }
                txtWidth.Text = String.Format("{0:X}", bitmap.Width);
                txtHeight.Text = String.Format("{0:X}", bitmap.Height);
                richTextBox1.Text = (new string(str.ToArray()));
                e.Handled = true;
            }
        }

        private void richTextBox1_TextChanged(object sender, EventArgs e)
        {

            var width = (byte)int.Parse(txtWidth.Text, System.Globalization.NumberStyles.HexNumber);
            var height = (byte)int.Parse(txtHeight.Text, System.Globalization.NumberStyles.HexNumber);
            var buff = richTextBox1.Text.Trim().Replace("\n", "").PadRight(width*height,'0');
            
            var bitmap = new Bitmap(Convert.ToInt32(width), Convert.ToInt32(height));
            for (var y = 0; y < height; y++)
            {
                for (var x = 0; x < width; x++)
                {
                    var val = (buff[y * width + x] - 48) * 255 / 7;
                    bitmap.SetPixel(x, y, Color.FromArgb(val, val, val));
                }
            }
            pictureBox1.Image = bitmap;
            pictureBox1.SizeMode = PictureBoxSizeMode.Zoom;
        }
    }
}
