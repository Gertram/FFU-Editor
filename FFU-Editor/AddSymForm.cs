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

using FFULibrary;
using log4net;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;

namespace FFU_Editor
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
        private static readonly ILog log = LogManager.GetLogger(typeof(MainForm));
        private Cursor IconCursor;
        private readonly Cursor WhitePen = new Cursor(Properties.Resources.pen_white.GetHicon());
        private readonly Cursor BlackPen = new Cursor(Properties.Resources.pen_black.GetHicon());
        private Color CellColor1 { get; set; }
        private Color CellColor2 { get; set; }
        private Color CurrentColor { get; set; }
        private int CurrentColorIndex { get; set; }
        public AddSymForm(FFU font)
        {
            InitializeComponent();

            FFU = font;

            SelectPalitteComboBox.Items.Clear();
            if (FFU.Pallettes != null) {
                foreach (var index in FFU.Pallettes.Select((x, index) => index))
                {
                    SelectPalitteComboBox.Items.Add(index);
                }
            }
            else
            {
                SelectPalitteComboBox.Visible = false;
                SelectPalitteLabel.Visible = false;
                SelectPalitteComboBox.Items.Add(0);
            }
            SelectPalitteComboBox.SelectedIndex = 0;
            SelectBackgroundComboBox.SelectedIndex = 0;
            CurrentColorTextBox.Maximum = FFU.GetPalette(0).Length - 1;
            CurrentColor = FFU.GetPalette(0)[0];
            CurrentColorIndex = 0;
        }
        private FFU FFU { get; set; }
        private int currentCode = -1;
        private Sym CurrentSym { get; set; }
        public int CurrentCode
        {
            get
            {
                return currentCode;
            }
            set
            {
                ShowSym(value);
            }
        }
        private char ToChar(int sym)
        {
            if (sym < 10)
            {
                return (char)(sym + 0x30);
            }
            return (char)(sym + 0x37);
        }
        private void ShowSymAt(int index)
        {
            index = (FFU.Symbols.Count + index) % FFU.Symbols.Count;
            CurrentCode = FFU.Symbols.ElementAt(index).Key;
        }
        private void ShowSym(int code = -1)
        {
            if (code == -1)
            {
                if (currentCode == -1)
                {
                    return;
                }
                code = currentCode;
            }
            try
            {
                currentCode = code;
                SymCodeMaskedTextBox.Text = code.ToString("X");
                var palitte = FFU.GetPalette(SelectPalitteComboBox.SelectedIndex);
                CurrentSym = FFU.Symbols[code];
                var text = new char[(CurrentSym.Width + 1) * CurrentSym.Height];
                var pos = 0;
                for (var y = 0; y < CurrentSym.Height; y++)
                {
                    for (var x = 0; x < CurrentSym.Width; x++, pos++)
                    {
                        text[pos] = CurrentSym.Image[x, y].ToString("X")[0];
                    }
                    text[pos] = '\n';
                    pos++;
                }
                richTextBox1.Text = new string(text).Trim();
                var width = CurrentSym.Width;
                var height = CurrentSym.Height;
                txtWidth.Text = $"{width:X}";
                txtHeight.Text = $"{height:X}";
                SymWidth = width;
                SymHeight = height;
                bigIcon.Width = width * BigScale;
                bigIcon.Height = height * BigScale;
                smallIcon.Height = height * MinScale;
                smallIcon.Width = height * MinScale;
                var wrap = new SymWrap(CurrentSym);
                var left = wrap.LeftPadding();
                if (left != -1)
                {
                    PaddingLeftTextBox.Value = left;
                    PaddingRightTextBox.Value = wrap.RightPadding();
                    PaddingTopTextBox.Value = wrap.TopPadding();
                    PaddingBottomTextBox.Value = wrap.BottomPadding();
                    PaddingLeftTextBox.Enabled = PaddingBottomTextBox.Enabled = PaddingRightTextBox.Enabled = PaddingTopTextBox.Enabled = true;
                }
                else
                {
                    PaddingLeftTextBox.Value = PaddingRightTextBox.Value = PaddingTopTextBox.Value = PaddingBottomTextBox.Value = 0;
                    PaddingLeftTextBox.Enabled = PaddingBottomTextBox.Enabled = PaddingRightTextBox.Enabled = PaddingTopTextBox.Enabled = false;
                }

                buff = CurrentSym.GetBitmap(palitte, BigScale);
                smallIcon.Image = CurrentSym.GetBitmap(palitte, MinScale);
                bigIcon.Image = buff;
            }
            catch (Exception ex)
            {
                ShowError("", ex);
            }
        }

        private int SymWidth
        {
            get => (byte)int.Parse(txtWidth.Text, System.Globalization.NumberStyles.HexNumber);
            set => txtWidth.Text = value.ToString("X");
        }

        private int SymHeight
        {
            get => (byte)int.Parse(txtHeight.Text, System.Globalization.NumberStyles.HexNumber);
            set => txtHeight.Text = value.ToString("X");
        }

        private Bitmap buff;
        private int BigScale { get; set; } = 12;
        private int MinScale { get; set; } = 2;
        private Point GetMousePos(MouseEventArgs e)
        {
            return new Point(e.X / BigScale, e.Y / BigScale);
        }
        private Rectangle GetImageCellRect(MouseEventArgs e)
        {
            var pos = GetMousePos(e);
            var startx = BigScale * pos.X;
            var starty = BigScale * pos.Y;
            return new Rectangle(startx, starty, BigScale, BigScale);
        }
        private void BigIcon_MouseEnter(object sender, EventArgs e)
        {
            Cursor = IconCursor;
        }
        private void PictureBox_MouseMove(object sender, MouseEventArgs e)
        {
            try
            {
                var strokeLength = BigScale / 3;

                var bitmap = buff.Clone() as Bitmap;
                if (IsDraw)
                {
                    var pos = GetMousePos(e);
                    CurrentSym.Image[pos.X, pos.Y] = (byte)CurrentColorIndex;
                    ShowSym();
                }
                else
                {
                    var rect = GetImageCellRect(e);
                    for (int x = rect.Left; x < rect.Right; x += strokeLength * 2)
                    {
                        for (int tx = x; tx < x + strokeLength && tx < rect.Right; tx++)
                        {
                            bitmap.SetPixel(tx, rect.Top, CellColor1);
                            bitmap.SetPixel(tx, rect.Bottom - 1, CellColor1);
                        }
                        for (int tx = x + strokeLength; tx < x + 2 * strokeLength && tx < rect.Right; tx++)
                        {
                            bitmap.SetPixel(tx, rect.Top, CellColor2);
                            bitmap.SetPixel(tx, rect.Bottom - 1, CellColor2);
                        }
                    }
                    for (int y = rect.Top; y < rect.Bottom; y += strokeLength * 2)
                    {
                        for (int ty = y; ty < y + strokeLength && ty < rect.Bottom; ty++)
                        {
                            bitmap.SetPixel(rect.Left, ty, CellColor1);
                            bitmap.SetPixel(rect.Right - 1, ty, CellColor1);
                        }
                        for (int ty = y + strokeLength; ty < y + 2 * strokeLength && ty < rect.Bottom; ty++)
                        {
                            bitmap.SetPixel(rect.Left, ty, CellColor2);
                            bitmap.SetPixel(rect.Right - 1, ty, CellColor2);
                        }
                    }
                    bigIcon.Image = bitmap;
                }
            }
            catch (Exception ex)
            {
                ShowError("", ex);
            }
        }
        private bool IsDraw { get; set; } = false;
        private void BigIcon_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                IsDraw = true;
                PictureBox_MouseMove(sender, e);
            }else if (e.Button == MouseButtons.Right)
            {
                var pos = GetMousePos(e);
                CurrentColorTextBox.Value = CurrentSym.Image[pos.X, pos.Y];
            }
        }

        private void BigIcon_MouseLeave(object sender, EventArgs e)
        {
            IsDraw = false;
            Cursor = Cursors.Default;
        }

        private void BigIcon_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                IsDraw = false;
            }
        }

        private void CutButton_Click(object sender, EventArgs e)
        {
            try
            {
                new SymWrap(CurrentSym).RemovePadding((int)CutLeftTextBox.Value, (int)CutTopTextBox.Value,
                        (int)CutRightTextBox.Value, (int)CutBottomTextBox.Value);
                ShowSym();
            }
            catch (Exception ex)
            {
                ShowError("", ex);
            }
        }

        private void ExpandButton_Click(object sender, EventArgs e)
        {
            try
            {
                new SymWrap(CurrentSym).AddPadding((int)ExpandLeftTextBox.Value, (int)ExpandTopTextBox.Value,
                    (int)ExpandRightTextBox.Value, (int)ExpandBottomTextBox.Value);
                ShowSym();
            }
            catch (Exception ex)
            {
                ShowError("", ex);
            }
        }
        private int SymPosition => FFU.Symbols.Keys.ToList().IndexOf(CurrentCode);
        private void NextButton_Click(object sender, EventArgs e)
        {
            SymCodeMaskedTextBox.Focus();
            ShowSymAt(SymPosition + 1);
        }

        private void PrevButton_Click(object sender, EventArgs e)
        {
            SymCodeMaskedTextBox.Focus();
            ShowSymAt(SymPosition - 1);
        }
        private void SelectPalitteComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (FFU == null)
                {
                    return;
                }
                var palitte = FFU.GetPalette(SelectPalitteComboBox.SelectedIndex);
                if (palitte == null)
                {
                    return;
                }
                ShowSym();
                CurrentColorTextBox_ValueChanged(this, null);
            }
            catch (Exception ex)
            {
                ShowError(nameof(SelectPalitteComboBox_SelectedIndexChanged), ex);
            }
        }

        private void SelectBackgroundComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (SelectBackgroundComboBox.Text == "Черный")
                {
                    bigIcon.BackColor = Color.Black;
                    smallIcon.BackColor = Color.Black;
                    CellColor1 = Color.White;
                    CellColor2 = Color.Black;
                    IconCursor = WhitePen;
                }
                else if (SelectBackgroundComboBox.Text == "Белый")
                {
                    bigIcon.BackColor = Color.White;
                    smallIcon.BackColor = Color.White;
                    CellColor1 = Color.Black;
                    CellColor2 = Color.White;
                    IconCursor = BlackPen;
                }
            }
            catch (Exception ex)
            {
                ShowError(nameof(SelectBackgroundComboBox_SelectedIndexChanged), ex);
            }
        }
        private void SymCodeMaskedTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Enter)
                {
                    CurrentCode = int.Parse(SymCodeMaskedTextBox.Text, System.Globalization.NumberStyles.HexNumber);
                }
            }catch(Exception ex)
            {
                ShowError("", ex);
            }
        }
        private void ShowError(string message, Exception ex)
        {
            MessageBox.Show(message + ex.ToString(), "Ошибка:");
            log.Error(ex.ToString());
        }

        private void AddSymForm_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Right && e.Control)
            {
                NextButton_Click(this, null);
            }
            else if (e.KeyCode == Keys.Left && e.Control)
            {
                PrevButton_Click(this, null);
            }
        }

        private void PaddingButton_Click(object sender, EventArgs e)
        {
            try
            {
                new SymWrap(CurrentSym).SetPadding((int)PaddingLeftTextBox.Value, (int)PaddingTopTextBox.Value,
                        (int)PaddingRightTextBox.Value, (int)PaddingBottomTextBox.Value);
                ShowSym();
            }
            catch (Exception ex)
            {
                ShowError("", ex);
            }
        }
        ~AddSymForm()
        {
            WhitePen.Dispose();
            BlackPen.Dispose();
        }

        private void DrawText_Click(object sender, EventArgs e)
        {
            try
            {
                var lines = richTextBox1.Text.Split('\n');
                var height = lines.Length;
                var width = lines.Max(x => x.Length);
                var temp = new byte[width, height];
                char[] text = new char[width * height + height];
                int pos = 0;
                for (var y = 0; y < height; y++)
                {
                    var line = lines[y];
                    for (int x = 0; x < width; x++, pos++)
                    {
                        var value = 0;
                        if (x < line.Length && int.TryParse(line[x].ToString(), System.Globalization.NumberStyles.HexNumber, null, out int result))
                        {
                            value = result;
                        }
                        text[pos] = value.ToString("X")[0];

                        temp[x, y] = (byte)value;
                    }
                    if (y != height - 1)
                    {
                        text[pos] = '\n';
                        pos++;
                    }
                }
                CurrentSym.Image = temp;

                ShowSym();
            }
            catch (Exception ex)
            {
                ShowError("", ex);
            }
        }

        private void CurrentColorTextBox_ValueChanged(object sender, EventArgs e)
        {
            CurrentColorIndex = (int)CurrentColorTextBox.Value;
            CurrentColor = FFU.GetPalette(SelectPalitteComboBox.SelectedIndex)[CurrentColorIndex];
            CurrentColorPictureBox.BackColor = CurrentColor;
        }

        private void ScaleTextBox_ValueChanged(object sender, EventArgs e)
        {
            BigScale = (int)ScaleTextBox.Value;
            ShowSym();
        }
    }
}
