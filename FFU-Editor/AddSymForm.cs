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
using System.Drawing.Imaging;
using System.Runtime.InteropServices;

using FFULibrary;
using log4net;

namespace FFU_Editor
{
    public partial class AddSymForm : Form
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(MainForm));
        private Cursor IconCursor;
        private readonly Cursor WhitePen = new Cursor(Properties.Resources.pen_white.GetHicon());
        private readonly Cursor BlackPen = new Cursor(Properties.Resources.pen_black.GetHicon());
        private Color CellColor1 { get; set; }
        private Color CellColor2 { get; set; }
        private Color CurrentColor { get; set; }
        private Color BackgroundColor { get; set; } = Color.Transparent;
        private int CurrentColorIndex { get; set; }

        private Bitmap CurrentImage { get; set; }
        public AddSymForm(FFU font)
        {
            InitializeComponent();

            FFU = font;

            SelectPalitteComboBox.Items.Clear();
            if (FFU.Pallettes != null)
            {
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
            CurrentColorTextBox.Maximum = FFU.Header.FontInfo.Codek == CodekType.COLOR8 ? 7 : 15;
            CurrentColor = FFU.GetPalette(0)[0];
            CurrentColorIndex = 0;
        }
        private FFU FFU { get; set; }
        public Sym CurrentSym
        {
            get
            {
                return currentSym;
            }
            set
            {
                currentSym = value;
                ShowSym();
            }
        }
        private int currentCode = -1;
        public int CurrentCode
        {
            get => currentCode;
            set
            {
                currentCode = value;
                SymCodeMaskedTextBox.Text = value.ToString("X");
                CurrentSym = FFU.Symbols[value];
                NextButton.Visible = true;
                PrevButton.Visible = true;
                SaveButton.Visible = false;
            }
        }
        private void ShowSymAt(int index)
        {
            index = (FFU.Symbols.Count + index) % FFU.Symbols.Count;
            CurrentCode = FFU.Symbols.ElementAt(index).Key;
        }
        private void ShowSym()
        {
            if (CurrentSym == null)
            {
                return;
            }
            try
            {
                var palitte = FFU.GetPalette(SelectPalitteComboBox.SelectedIndex,BackgroundColor);
                var text = new char[(CurrentSym.Width + 1) * CurrentSym.Height];
                var pos = 0;
                for (var y = 0; y < CurrentSym.Height; y++,pos++)
                {
                    for (var x = 0; x < CurrentSym.Width; x++, pos++)
                    {
                        try
                        {
                            var value = CurrentSym.Image[x, y];
                            text[pos] = ToChar(FFU.Codek.Decode(value));
                        }catch(Exception ex)
                        {
                            Console.WriteLine();
                        }
                    }
                    text[pos] = '\n';
                }
                richTextBox1.Text = new string(text, 0, text.Length - 1);
                var width = CurrentSym.Width;
                var height = CurrentSym.Height;
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
                CurrentImage = new Bitmap(bigIcon.Width, bigIcon.Height);
                bigIcon.Image = CurrentImage;
                smallIcon.BackgroundImage = CurrentSym.GetBitmap(palitte, MinScale);
                smallIcon.BackgroundImageLayout = ImageLayout.Zoom;
                bigIcon.BackgroundImage = buff;
            }
            catch (Exception ex)
            {
                ShowError("", ex);
            }
        }

        private int SymWidth
        {
            get => (int)txtWidth.Value;
            set => txtWidth.Value = value;
        }

        private int SymHeight
        {
            get => (int)txtHeight.Value;
            set => txtHeight.Value = value;
        }

        private Bitmap buff;
        private Sym currentSym;

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

                if (IsDraw)
                {
                    var pos = GetMousePos(e);
                    CurrentSym.Image[pos.X, pos.Y] = FFU.Codek.Encode((byte)CurrentColorIndex);
                    ShowSym();
                }
                else
                {
                    using(var g = Graphics.FromImage(CurrentImage))
                    {
                        using(Pen pen1 = new Pen(CellColor1,1), pen2 = new Pen(CellColor2,1)) {
                            g.Clear(Color.Transparent);

                            var rect = GetImageCellRect(e);
                            g.DrawLines(pen1, new PointF[] {
                                new PointF(rect.Left,rect.Top + strokeLength),
                                new PointF(rect.Left, rect.Top),
                                new PointF(rect.Left + strokeLength,rect.Top)
                            });
                            g.DrawLine(pen2, rect.Left + strokeLength, rect.Top, rect.Right - strokeLength, rect.Top);
                            g.DrawLines(pen1, new PointF[] {
                                new PointF(rect.Left,rect.Bottom - strokeLength),
                                new PointF(rect.Left, rect.Bottom),
                                new PointF(rect.Left + strokeLength,rect.Bottom)
                            });
                            g.DrawLine(pen2, rect.Left + strokeLength, rect.Bottom, rect.Right - strokeLength, rect.Bottom);
                            g.DrawLines(pen1, new PointF[] {
                                new PointF(rect.Right,rect.Top + strokeLength),
                                new PointF(rect.Right, rect.Top),
                                new PointF(rect.Right - strokeLength,rect.Top)
                            }); 
                            g.DrawLine(pen2, rect.Left, rect.Top + strokeLength, rect.Left, rect.Bottom - strokeLength);

                            g.DrawLines(pen1, new PointF[] {
                                new PointF(rect.Right,rect.Bottom - strokeLength),
                                new PointF(rect.Right, rect.Bottom),
                                new PointF(rect.Right - strokeLength,rect.Bottom)
                            });
                            g.DrawLine(pen2, rect.Right, rect.Top + strokeLength, rect.Right, rect.Bottom - strokeLength);

                        }
                    }
                    bigIcon.Image = CurrentImage;
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
            }
            else if (e.Button == MouseButtons.Right)
            {
                var pos = GetMousePos(e);
                CurrentColorTextBox.Value = FFU.Codek.Decode(CurrentSym.Image[pos.X, pos.Y]);
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
                    BackgroundColor = Color.Black;
                    CellColor1 = Color.White;
                    CellColor2 = Color.Black;
                    IconCursor = WhitePen;
                }
                else if (SelectBackgroundComboBox.Text == "Белый")
                {
                    BackgroundColor = Color.White;
                    CellColor1 = Color.Black;
                    CellColor2 = Color.White;
                    IconCursor = BlackPen;
                }
                ShowSym();
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
                if (e.KeyCode == Keys.Enter && currentCode != -1)
                {
                    CurrentCode = int.Parse(SymCodeMaskedTextBox.Text, System.Globalization.NumberStyles.HexNumber);
                }
            }
            catch (Exception ex)
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
                ShowNextSym();
            }
            else if (e.KeyCode == Keys.Left && e.Control)
            {
                ShowPrevSym();
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

        byte ToByte(char sym)
        {
            if(sym <= '9')
            {
                return (byte)(sym - '0');
            }
            return (byte)(sym - 'A');
        }
        char ToChar(byte sym)
        {
            if(sym <= 9)
            {
                return (char)(sym + '0');
            }
            return (char)(sym + 'A');
        }

        private void DrawText_Click(object sender, EventArgs e)
        {
            try
            {
                var lines = richTextBox1.Text.Split('\n');
                var bytes = lines.Select(x => x.Select(y => FFU.Codek.Encode(ToByte(y))).ToArray()).ToArray();
                var height = lines.Length;
                var width = lines.Max(x => x.Length);
                var temp = new byte[width, height];
                var chars = new char[(width + 1) * height];
                var pos = 0;
                for (var y = 0; y < height; y++, pos++)
                {
                    var line = lines[y];
                    var byteRow = bytes[y];
                    for (var x = 0; x < line.Length; x++, pos++)
                    {
                        chars[pos] = line[x];
                        temp[x, y] = byteRow[x];
                    }
                    for(var x = line.Length; x < width; x++)
                    {
                        chars[pos] = '\0';
                        temp[x, y] = 0;
                    }
                    chars[pos] = '\n';
                }
                richTextBox1.Text = new string(chars, 0, chars.Length - 1);
;               CurrentSym.Image = temp;

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
        private void SetSymSize()
        {
            var regionSize = new Size(SymWidth, SymHeight);
            var symSize = new Size(CurrentSym.Width,CurrentSym.Height);
            if(regionSize.Width < symSize.Width)
            {
                symSize.Width = regionSize.Width;
            }
            if(regionSize.Height < symSize.Height)
            {
                symSize.Height = regionSize.Height;
            }
            new SymWrap(CurrentSym).CopyImage(regionSize,new Point(),new Rectangle(new Point(),symSize));
            ShowSym();
        }

        private void TxtWidth_KeyDown(object sender, KeyEventArgs e)
        {
            SetSymSize();
        }

        private void TxtHeight_KeyDown(object sender, KeyEventArgs e)
        {
            SetSymSize();
        }

        private bool ShowRewriteAttention()
        {
            var result = MessageBox.Show("Символ с таким кодом уже существует.\n Продолжить с заменой символа?", "Внимание", MessageBoxButtons.YesNo);
            if(result == DialogResult.OK)
            {
                return true;
            }
            if(result == DialogResult.No)
            {
                return false;
            }
            throw new NotImplementedException();
        }

        private void SaveButton_Click(object sender, EventArgs e)
        {
            try
            {
                var code = int.Parse(SymCodeMaskedTextBox.Text);

                if (FFU.Symbols.ContainsKey(code) && !ShowRewriteAttention())
                {
                    return;
                }
                FFU.Symbols[code] = CurrentSym;
                CurrentCode = code;
            }
            catch (Exception ex)
            {
                ShowError("Save", ex);
            }
        }

        private void ShowPrevSym()
        {
            SymCodeMaskedTextBox.Focus();
            ShowSymAt(SymPosition - 1);
        }

        private void ShowNextSym()
        {
            SymCodeMaskedTextBox.Focus();
            ShowSymAt(SymPosition + 1);
        }

        private void NextButton_MouseDown(object sender, MouseEventArgs e)
        {
            NextTimer.Interval = 500;
            NextTimer.Start();
        }

        private void NextButton_MouseUp(object sender, MouseEventArgs e)
        {
            NextTimer.Stop();
        }

        private void NextButton_MouseLeave(object sender, EventArgs e)
        {
            NextTimer.Stop();
        }

        private void PrevButton_MouseDown(object sender, MouseEventArgs e)
        {
            PrevTimer.Interval = 500;
            PrevTimer.Start();
        }

        private void PrevButton_MouseLeave(object sender, EventArgs e)
        {
            PrevTimer.Stop();
        }

        private void PrevButton_MouseUp(object sender, MouseEventArgs e)
        {
            PrevTimer.Stop();
        }

        private void PrevTimer_Tick(object sender, EventArgs e)
        {
            PrevTimer.Interval = 16;
            ShowPrevSym();
        }

        private void NextTimer_Tick(object sender, EventArgs e)
        {
            NextTimer.Interval = 16;
            ShowNextSym();
        }

        private void NextButton_MouseClick(object sender, MouseEventArgs e)
        {
            ShowNextSym();
        }

        private void PrevButton_Click(object sender, EventArgs e)
        {
            ShowPrevSym();
        }
    }
}
