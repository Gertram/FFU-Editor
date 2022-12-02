using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FFU_Editor
{
    public partial class GenerateFontWindow : Form
    {
        public GenerateFontWindow()
        {
            InitializeComponent();
            FontFamilyComboBox.Items.AddRange(FontFamily.Families.Select(x => x.Name).ToArray());
        }

        private void FontFamilyComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            ShowFont();
        }

        private void FontSizeNumericUpDown_ValueChanged(object sender, EventArgs e)
        {
            ShowFont();
        }

        private void FontBorderSIzeNumericUpDown_ValueChanged(object sender, EventArgs e)
        {
            ShowFont();
        }
        private Bitmap GenerateSym(char sym,Font font,Color fontColor,int symHeight,int border,Color borderColor, int padding)
        {
            using (var brush = new SolidBrush(fontColor)) {
                var width = symHeight*2;
                var height = symHeight;
                var bitmap = new Bitmap(width, height);
                var g = Graphics.FromImage(bitmap);
                g.Clear(Color.Transparent);
                var rect = new RectangleF(0, 0, width, height);
                g.DrawString(new string(sym, 1), font, brush, rect, new StringFormat() { LineAlignment=StringAlignment.Center});
                g.Flush();
                var left = BitmapHelper.FindLeftBorder(bitmap);
                var right = BitmapHelper.FindRightBorder(bitmap);
                var imageWidth = right - left + 1;
                var temp = new Bitmap(imageWidth, bitmap.Height);
                for (int y = 0; y < bitmap.Height; y++)
                {
                    for (int x = 0; x < imageWidth; x++)
                    {
                        var pixel = bitmap.GetPixel(x + left, y);
                        temp.SetPixel(x, y, pixel);
                    }
                }
                var image = new Bitmap(imageWidth + padding * 2 + border * 2, bitmap.Height);
                for (int y = 0; y < bitmap.Height; y++)
                {
                    for (int x = 0; x < imageWidth; x++)
                    {
                        var pixel = temp.GetPixel(x, y);
                        if (!pixel.Equals(BitmapHelper.TransparentBlack))
                        {
                            pixel = fontColor;
                        }
                        image.SetPixel(x + padding + border, y, pixel);
                    }
                }
                for (int i = 0; i < border; i++)
                {
                    var color = Color.FromArgb(255 / border * (border - i), borderColor);
                    if (!FindColor(Colors, color, out var _))
                    {
                        Colors.Add(color, (byte)(border - i+4));
                    }
                    image = CreateBorder(image, color, BitmapHelper.TransparentBlack);
                }
                return image;
            }
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
        private static string Rus = "АБВГДЕЁЖЗИЙКЛМНОПРСТУФХЦЧШЩЪЫЬЭЮЯабвгдеёжзийклмнопрстуфхцчшщъыьэюя";
        internal Dictionary<char, Bitmap> Syms { get; private set; } = new Dictionary<char, Bitmap>();
        internal Dictionary<Color,byte> Colors { get; set; } = new Dictionary<Color,byte>();
        private void ShowFont()
        {
            if (FontFamilyComboBox.SelectedItem == null || FontColorButton.BackColor == null || FontBorderColorButton.BackColor == null)
            {
                return;
            }
            var fontSize = (int)FontSizeNumericUpDown.Value;
            try
            {
                var font = new Font(FontFamilyComboBox.SelectedItem as string, (float)fontSize,GraphicsUnit.Pixel);
                RectangleF region = pictureBox1.Bounds;
                var borderColor = FontBorderColorButton.BackColor;
                var drawFormat = new StringFormat();
                drawFormat.FormatFlags = StringFormatFlags.DirectionVertical;
                var syms = Rus.Concat(OthersRichTextBox.Text.ToCharArray()).ToArray();
                var symHeight = (int)SymHeightNumericUpDown.Value;
                Colors.Clear();
                Colors.Add(BitmapHelper.TransparentBlack, 4);
                Colors.Add(FontColorButton.BackColor,1);
                if(symHeight < fontSize+4)
                {
                    symHeight = fontSize + 4;
                }
                Syms.Clear();
                var border = (int)FontBorderSIzeNumericUpDown.Value;
                for (int i = 0; i < syms.Length; i++)
                {
                    var sym = syms[i];
                    var temp = GenerateSym(sym, font, FontColorButton.BackColor, symHeight,border,borderColor,0);
                    Syms[sym] = temp;
                }

                var text = richTextBox1.Text;
                if (string.IsNullOrWhiteSpace(text))
                {
                    text = new string(syms);
                }
                ShowText(text,symHeight,region);
                
            }catch(Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
        private void ShowText(string text,int symHeight, RectangleF region)
        {
            var bitmap = new Bitmap(pictureBox1.Width, pictureBox1.Height);
            var x_offset = 0;
            var y_offset = 0;
            for (int i = 0; i < text.Length; i++)
            {
                if (y_offset + symHeight >= region.Height)
                {
                    return;
                }
                if (!Syms.TryGetValue(text[i],out var temp)) {
                    temp = new Bitmap(10, symHeight);
                    var g = Graphics.FromImage(temp);
                    g.Clear(BitmapHelper.TransparentBlack);
                    g.Flush();
                }

                if (x_offset + temp.Width >= region.Width)
                {
                    x_offset = 0;
                    y_offset += symHeight;
                }
                for (int y = 0; y < temp.Height; y++)
                {
                    for (int x = 0; x < temp.Width; x++)
                    {
                        bitmap.SetPixel(x + x_offset, y + y_offset, temp.GetPixel(x, y));
                    }
                }
                x_offset += temp.Width;
            }
            pictureBox1.Image = bitmap;
        }
        private static Bitmap CreateBorder(Bitmap image, Color border, Color back)
        {
            var temp = new Bitmap(image.Width, image.Height);
            for (int y = 0; y < image.Height; y++)
            {
                for (int x = 0; x < image.Width; x++)
                {
                    temp.SetPixel(x,y,back);
                }
            }
            for (int y = 1; y < image.Height - 1; y++)
            {
                for (int x = 1; x < image.Width - 1; x++)
                {
                    var pixel = image.GetPixel(x,y);
                    if (pixel == back)
                    {
                        continue;
                    }
                    temp.SetPixel(x,y, pixel);
                    var left = image.GetPixel(x - 1,y);
                    if (left == back)
                    {
                        temp.SetPixel(x - 1,y,border);
                    }
                    var right = image.GetPixel(x + 1,y);
                    if (right == back)
                    {
                        temp.SetPixel(x + 1,y,border);
                    }
                    var top = image.GetPixel(x,y - 1);
                    if (top == back)
                    {
                        temp.SetPixel(x,y - 1,border);
                    }
                    var bottom = image.GetPixel(x,y + 1);
                    if (bottom == back)
                    {
                        temp.SetPixel(x,y + 1,border);
                    }
                }
            }
            return temp;
        }
        private void FontColorButton_Click(object sender, EventArgs e)
        {
            if (sender is Button button && FontColorDialog.ShowDialog() != DialogResult.OK)
            {
                return;
            }
            FontColorButton.BackColor = FontColorDialog.Color;
            ShowFont();
        }

        private void FontBorderColorButton_Click(object sender, EventArgs e)
        {
            if (sender is Button button && FontBorderColorDialog.ShowDialog() != DialogResult.OK)
            {
                return;
            }
            FontBorderColorButton.BackColor = FontBorderColorDialog.Color;
            ShowFont();
        }

        private void SymHeightNumericUpDown_ValueChanged(object sender, EventArgs e)
        {
            ShowFont();
        }

        private void SaveButton_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.OK;
        }

        private void richTextBox1_TextChanged(object sender, EventArgs e)
        {
            ShowFont();
        }

        private void OthersRichTextBox_TextChanged(object sender, EventArgs e)
        {
            ShowFont();
        }
    }
}
