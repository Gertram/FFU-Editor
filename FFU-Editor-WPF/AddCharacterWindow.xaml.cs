using System;
using System.Linq;
using System.ComponentModel;
using System.Windows.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using FFULibrary;
using MahApps.Metro.Controls;
using log4net;
using FFUEditor.ColorExtension;
using System.Globalization;

namespace FFUEditor
{
    public class Padding
    {
        public int Left { get; set; }
        public int Right { get; set; }
        public int Top { get; set; }
        public int Bottom { get; set; }
    }
    /// <summary>
    /// Логика взаимодействия для AddCharacterWindow.xaml
    /// </summary>
    public partial class AddCharacterWindow : MetroWindow, INotifyPropertyChanged
    {
        public static RoutedCommand SwitchCharacterCommand { get; } = new RoutedCommand();
        private Padding setPaddings;
        private Padding expandPaddings = new Padding();
        private Padding cutPaddings = new Padding();
        private char currentCode = '\0';
        private Sym currentSym;
        private DispatcherTimer NextTimer { get; } = new DispatcherTimer();
        private DispatcherTimer PrevTimer { get; } = new DispatcherTimer();
        private int BigScale { get; set; } = 12;
        private int MinScale { get; set; } = ScaleStep;
        private const int ScaleStep = 3;
        private const int StartTimeStep = 500;
        private const int WorkTimeStep = 16;
        public bool UnLockPadding
        {
            get
            {
                return unLockPadding;
            }
            set
            {
                unLockPadding = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(UnLockPadding)));
            }
        }
        private static readonly ILog log = LogManager.GetLogger(typeof(AddCharacterWindow));
        private Cursor IconCursor { get; set; }
        private bool unLockPadding = true;
        private SolidColorBrush backgroundColor = Brushes.Black;
        private readonly Cursor WhitePen = WinFXCursorFromBitmap.CreateCursor(Properties.Resources.pen_white);
        private readonly Cursor BlackPen = WinFXCursorFromBitmap.CreateCursor(Properties.Resources.pen_black);
        private System.Drawing.Color CellColor1 { get; set; }
        private System.Drawing.Color CellColor2 { get; set; }
        private System.Drawing.Color CurrentColor { get; set; }
        public SolidColorBrush BackgroundColor
        {
            get
            {
                return backgroundColor;
            }
            set
            {
                backgroundColor = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(BackgroundColor)));
            }
        }
        private int CurrentColorIndex { get; set; }

        private FFU FFU { get; set; }
        private System.Drawing.Bitmap CurrentImage { get; set; }

        public Padding SetPaddings
        {
            get
            {
                return setPaddings;
            }
            private set
            {
                setPaddings = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(SetPaddings)));
            }
        }
        public Padding ExpandPaddings
        {
            get
            {
                return expandPaddings;
            }
            private set
            {
                expandPaddings = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(ExpandPaddings)));
            }
        }
        public Padding CutPaddings
        {
            get
            {
                return cutPaddings;
            }
            private set
            {
                cutPaddings = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(CutPaddings)));
            }
        }
        public Sym CurrentSym
        {
            get
            {
                return currentSym;
            }
            set
            {
                currentSym = value;
                ShowCurrentCharacter();
            }
        }
        private char currentCharacter;
        public char CurrentCharacter
        {
            get => currentCharacter;
            set
            {
                currentCharacter = value;
                CurrentSym = FFU.Symbols[value];
            }
        }
        private int CurrentCharacterIndex
        {
            get => FFU.Symbols.Keys.ToList().IndexOf(CurrentCharacter);
            set
            {
                value = (value + FFU.Symbols.Count) % FFU.Symbols.Count;
                var pair = FFU.Symbols.ElementAt(value);
                CurrentCharacter = pair.Key;
            }
        }

        public AddCharacterWindow(FFU font)
        {
            InitializeComponent();
            IconCursor = WhitePen;
            NextTimer.Tick += NextTimer_Tick;
            PrevTimer.Tick += PrevTimer_Tick;

            FFU = font;

            PaletteComboBox.Items.Clear();
            if (FFU.Pallettes != null)
            {
                foreach (var index in FFU.Pallettes.Select((x, index) => index))
                {
                    PaletteComboBox.Items.Add(index);
                }
            }
            else
            {
                PaletteComboBox.IsEnabled = false;
                PaletteComboBox.IsEnabled = false;
                PaletteComboBox.Items.Add(0);
            }
            PaletteComboBox.SelectedIndex = 0;
            BackgroundComboBox.SelectedIndex = 0;
            CurrentColorComboBox.Items.Clear();
            var palette = FFU.GetPalette(0);
            for (var i = 0; i < palette.Length; i++)
            {
                CurrentColorComboBox.Items.Add(i);
            }
            var color = FFU.GetPalette(0)[0];
            CurrentColor = System.Drawing.Color.FromArgb(color.A, color.R, color.G, color.B);
            CurrentColorIndex = 0;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void PaletteComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                if (FFU == null)
                {
                    return;
                }
                var palitte = FFU.GetPalette(PaletteComboBox.SelectedIndex);
                if (palitte == null)
                {
                    return;
                }
                ShowCurrentCharacter();
                CurrentColorComboBox_SelectionChanged(this, null);
            }
            catch (Exception ex)
            {
                ShowError("", ex);
            }
        }

        private void BackgoundComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                if (BackgroundComboBox.SelectedIndex == 0)
                {
                    BackgroundColor = Brushes.Black;
                    CellColor1 = System.Drawing.Color.White;
                    CellColor2 = System.Drawing.Color.Black;
                    IconCursor = WhitePen;
                }
                else if (BackgroundComboBox.SelectedIndex == 1)
                {
                    BackgroundColor = Brushes.White;
                    CellColor1 = System.Drawing.Color.Black;
                    CellColor2 = System.Drawing.Color.White;
                    IconCursor = BlackPen;
                }
                ShowCurrentCharacter();
            }
            catch (Exception ex)
            {
                ShowError("", ex);
            }
        }

        private void CurrentColorComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            CurrentColorIndex = CurrentColorComboBox.SelectedIndex;
            if (CurrentColorIndex == -1)
            {
                CurrentColorIndex = 0;
            }
            var palette = FFU.GetPalette(PaletteComboBox.SelectedIndex);
            var color = palette[CurrentColorIndex];
            CurrentColor = System.Drawing.Color.FromArgb(color.A, color.R, color.G, color.B);
            CurrentColorWrap.Background = new SolidColorBrush(System.Windows.Media.Color.FromArgb(CurrentColor.A, CurrentColor.R, CurrentColor.G, CurrentColor.B));
        }

        private void ScaleComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var scale = (ScaleComboBox.SelectedIndex + 1) * ScaleStep;
            if (scale <= 0)
            {
                scale = ScaleStep * 3;
            }
            BigScale = scale;
            ShowCurrentCharacter();

        }

        private void SwitchCharacterCommandBinding_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            var direction = e.Parameter as string;
            if (direction == "Next")
            {
                ShowNextSym();
            }
            else if (direction == "Prev")
            {
                ShowPrevSym();
            }
        }

        private void Window_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Right && e.KeyboardDevice.Modifiers.HasFlag(ModifierKeys.Control))
            {
                ShowNextSym();
            }
            else if (e.Key == Key.Left && e.KeyboardDevice.Modifiers.HasFlag(ModifierKeys.Control))
            {
                ShowPrevSym();
            }
            else
            {
                return;
            }
            e.Handled = true;
        }
        private void UpdateTextView()
        {
            var lines = new string[CurrentSym.Height];

            for (var y = 0; y < CurrentSym.Height; y++)
            {
                var line = new char[CurrentSym.Width];
                for (var x = 0; x < CurrentSym.Width; x++)
                {
                    line[x] = ToChar(CurrentSym.GetPixel(x, y));
                }
                lines[y] = new string(line);
            }
            TextViewTextBox.Text = string.Join("\n", lines);
        }
        private void UpdatePadding()
        {
            var wrap = new SymWrap(CurrentSym);
            var left = wrap.FindLeftPosition();
            if (left != -1)
            {
                var padding = new Padding
                {
                    Left = wrap.LeftPadding(),
                    Right = wrap.RightPadding(),
                    Top = wrap.TopPadding(),
                    Bottom = wrap.BottomPadding()
                };
                SetPaddings = padding;
                UnLockPadding = true;
            }
            else
            {
                var padding = new Padding();
                padding.Left = padding.Right = padding.Top = padding.Bottom = 0;
                SetPaddings = padding;
                UnLockPadding = false;
            }
        }
        private void UpdateImage()
        {
            var palette = FFU.GetPalette(PaletteComboBox.SelectedIndex);
            var width = CurrentSym.Width;
            var height = CurrentSym.Height;
            CharacterWidth = width;
            CharacterHeight = height;
            //var dpiTransform = new ScaleTransform(1 / GetWindowsScaling(), 1 / GetWindowsScaling());
            //if (dpiTransform.CanFreeze)
            //    dpiTransform.Freeze();
            //BigIconWrap.RenderTransform = dpiTransform;
            //BigIconWrap.LayoutTransform = dpiTransform;
            BigIconWrap.Width = width * BigScale;
            BigIconWrap.Height = height * BigScale;
            var source = CurrentSym.ToBitmapSource(palette, BigScale);
            CurrentImage = new System.Drawing.Bitmap((int)BigIconWrap.Width, (int)BigIconWrap.Height);
            BigIconMask.Source = BitmapHelper.ConvertTo(CurrentImage);
            Miniature.ImageSource = source;
            BigIconBack.Source = source;
        }
        private void ShowCurrentCharacter()
        {
            if (CurrentSym == null)
            {
                return;
            }
            try
            {
                CurrentCharacterCodeTextBox.Text = $"{string.Join("", FFU.CurrentEncoding.GetBytes(CurrentCharacter.ToString()).Select(x => x.ToString("X")))}";
                CurrentCharacterTextBox.Text = CurrentCharacter.ToString();
                CurrentCharacterIndexTextBox.Text = CurrentCharacterIndex.ToString();

                CurrentCharacterCodeTextBox.Background = System.Windows.Media.Brushes.Transparent;
                CurrentCharacterTextBox.Background = System.Windows.Media.Brushes.Transparent;
                CurrentCharacterIndexTextBox.Background = System.Windows.Media.Brushes.Transparent;
                UpdateTextView();
                UpdatePadding();
                UpdateImage();
            }
            catch (Exception ex)
            {
                ShowError("",ex);
            }
        }

        private int CharacterWidth
        {
            get
            {
                return int.Parse(txtWidth.Text);
            }

            set
            {
                txtWidth.Text = value.ToString();
            }
        }

        private int CharacterHeight
        {
            get
            {
                return int.Parse(txtHeight.Text);
            }

            set
            {
                txtHeight.Text = value.ToString();
            }
        }
        private System.Drawing.Point GetMousePos(Point location)
        {
            var scale = BigIconMask.ActualWidth / CurrentImage.Width;
            return new System.Drawing.Point(
                (int)(location.X / BigScale / scale),
                (int)(location.Y / BigScale / scale)
                );
        }
        private System.Drawing.Rectangle GetImageCellRect(Point location)
        {
            var pos = GetMousePos(location);
            var startx = BigScale * pos.X;
            var starty = BigScale * pos.Y;
            return new System.Drawing.Rectangle(startx, starty, BigScale, BigScale);
        }

        private void ShowError(string message, Exception ex)
        {
            MessageBox.Show(message + ex.ToString(), "Ошибка:");
            log.Error(ex.ToString());
        }
        ~AddCharacterWindow()
        {
            WhitePen.Dispose();
            BlackPen.Dispose();
        }

        private byte ToByte(char sym)
        {
            return sym <= '9' ? (byte)(sym - '0') : (byte)(sym - 'A' + 10);
        }

        private char ToChar(byte sym)
        {
            return sym <= 9 ? (char)(sym + '0') : (char)(sym + 'A' - 10);
        }

        private void SetSymSize()
        {
            var regionSize = new System.Drawing.Size(CharacterWidth, CharacterHeight);
            var symSize = new System.Drawing.Size(CurrentSym.Width, CurrentSym.Height);
            if (regionSize.Width < symSize.Width)
            {
                symSize.Width = regionSize.Width;
            }
            if (regionSize.Height < symSize.Height)
            {
                symSize.Height = regionSize.Height;
            }
            new SymWrap(CurrentSym).CopyImage(regionSize, new System.Drawing.Point(), new System.Drawing.Rectangle(new System.Drawing.Point(), symSize));
            ShowCurrentCharacter();
        }

        private void TxtWidth_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Return)
            {
                SetSymSize();
                e.Handled = true;
            }
        }

        private bool ShowRewriteAttention()
        {
            var result = MessageBox.Show("Символ с таким кодом уже существует.\n Продолжить с заменой символа?", "Внимание", MessageBoxButton.YesNo);
            if (result == MessageBoxResult.Yes)
            {
                return true;
            }
            if (result == MessageBoxResult.No)
            {
                return false;
            }
            throw new NotImplementedException();
        }

        private void ShowPrevSym()
        {
            CurrentCharacterIndexTextBox.Focus();
            CurrentCharacterIndex--;
        }

        private void ShowNextSym()
        {
            CurrentCharacterIndexTextBox.Focus();
            CurrentCharacterIndex++;
        }

        private void NextButton_MouseDown(object sender, MouseEventArgs e)
        {
            NextTimer.Interval = new TimeSpan(0, 0, 0, 0, StartTimeStep);
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
            PrevTimer.Interval = new TimeSpan(0, 0, 0, 0, StartTimeStep);
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
            PrevTimer.Interval = new TimeSpan(0, 0, 0, 0, WorkTimeStep);
            ShowPrevSym();
        }

        private void NextTimer_Tick(object sender, EventArgs e)
        {
            NextTimer.Interval = new TimeSpan(0, 0, 0, 0, WorkTimeStep);
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

        private void CurrentCharacterIndexTextBox_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                var index = int.Parse(CurrentCharacterIndexTextBox.Text);
                if (index < 0 || index >= FFU.Symbols.Count)
                {
                    CurrentCharacterIndexTextBox.Background = System.Windows.Media.Brushes.Red;
                    return;
                }
                CurrentCharacterIndex = index;
                CurrentCharacterIndexTextBox.Background = System.Windows.Media.Brushes.Transparent;
            }
            catch
            {
                CurrentCharacterIndexTextBox.Background = System.Windows.Media.Brushes.Red;
            }
        }

        private void CurrentCharacterextBox_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                var character = CurrentCharacterTextBox.Text[0];
                var sym = FFU.Symbols[character];
                CurrentCharacter = character;
                CurrentCharacterTextBox.Background = System.Windows.Media.Brushes.Transparent;
            }
            catch
            {
                CurrentCharacterTextBox.Background = System.Windows.Media.Brushes.Red;
            }
        }

        private void CurrentCharacterCodeTextBox_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.Key == Key.Enter && currentCode != -1)
                {
                    var characterCode = int.Parse(CurrentCharacterCodeTextBox.Text, NumberStyles.HexNumber);
                    var characterBytes = BitConverter.GetBytes(characterCode);
                    var character = FFU.CurrentEncoding.GetChars(characterBytes)[0];
                    CurrentCharacter = character;
                }
            }
            catch (Exception ex)
            {
                ShowError("", ex);
            }
        }
        private bool IsDraw { get; set; } = false;
        private void BigIcon_MouseDown(object sender, MouseButtonEventArgs e)
        {
            var location = e.GetPosition(sender as IInputElement);
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                IsDraw = true;
                BigIcon_MouseMove(sender, e);
            }
            else if (e.RightButton == MouseButtonState.Pressed)
            {
                var pos = GetMousePos(location);
                CurrentColorComboBox.SelectedIndex = CurrentSym.GetPixel(pos.X, pos.Y);
            }
        }

        private void SetPaddingButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                new SymWrap(CurrentSym).SetPadding(SetPaddings.Left, SetPaddings.Top, SetPaddings.Right, SetPaddings.Bottom);
                ShowCurrentCharacter();
            }
            catch (Exception ex)
            {
                ShowError("", ex);
            }
        }

        private void ExpandPaddingButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                new SymWrap(CurrentSym).AddPadding(ExpandPaddings.Left, ExpandPaddings.Top, ExpandPaddings.Right, ExpandPaddings.Bottom);
                ShowCurrentCharacter();
            }
            catch (Exception ex)
            {
                ShowError("", ex);
            }
        }

        private void CutPaddingButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                new SymWrap(CurrentSym).RemovePadding(CutPaddings.Left, CutPaddings.Top, CutPaddings.Right, CutPaddings.Bottom);
                ShowCurrentCharacter();
            }
            catch (Exception ex)
            {
                ShowError("", ex);
            }
        }

        private void BigIcon_MouseUp(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Released)
            {
                IsDraw = false;
            }
        }

        private void BigIcon_MouseEnter(object sender, MouseEventArgs e)
        {
            Cursor = IconCursor;
        }

        private void BigIcon_MouseMove(object sender, MouseEventArgs e)
        {
            var location = e.GetPosition(sender as IInputElement);
            try
            {
                var strokeLength = BigScale / ScaleStep;

                if (IsDraw)
                {
                    var pos = GetMousePos(location);
                    CurrentSym.SetPixel(pos.X, pos.Y, (byte)CurrentColorIndex);
                    ShowCurrentCharacter();
                }
                else
                {
                    using (var g = System.Drawing.Graphics.FromImage(CurrentImage))
                    {
                        using (System.Drawing.Pen pen1 = new System.Drawing.Pen(CellColor1, 1), pen2 = new System.Drawing.Pen(CellColor2, 1))
                        {
                            g.Clear(System.Drawing.Color.Transparent);

                            var rect = GetImageCellRect(location);
                            g.DrawLines(pen1, new System.Drawing.PointF[] {
                                new System.Drawing.PointF(rect.Left,rect.Top + strokeLength),
                                new System.Drawing.PointF(rect.Left, rect.Top),
                                new System.Drawing.PointF(rect.Left + strokeLength,rect.Top)
                            });
                            g.DrawLine(pen2, rect.Left + strokeLength, rect.Top, rect.Right - strokeLength, rect.Top);
                            g.DrawLines(pen1, new System.Drawing.PointF[] {
                                new System.Drawing.PointF(rect.Left,rect.Bottom - strokeLength),
                                new System.Drawing.PointF(rect.Left, rect.Bottom),
                                new System.Drawing.PointF(rect.Left + strokeLength,rect.Bottom)
                            });
                            g.DrawLine(pen2, rect.Left + strokeLength, rect.Bottom, rect.Right - strokeLength, rect.Bottom);
                            g.DrawLines(pen1, new System.Drawing.PointF[] {
                                new System.Drawing.PointF(rect.Right,rect.Top + strokeLength),
                                new System.Drawing.PointF(rect.Right, rect.Top),
                                new System.Drawing.PointF(rect.Right - strokeLength,rect.Top)
                            });
                            g.DrawLine(pen2, rect.Left, rect.Top + strokeLength, rect.Left, rect.Bottom - strokeLength);

                            g.DrawLines(pen1, new System.Drawing.PointF[] {
                                new System.Drawing.PointF(rect.Right,rect.Bottom - strokeLength),
                                new System.Drawing.PointF(rect.Right, rect.Bottom),
                                new System.Drawing.PointF(rect.Right - strokeLength,rect.Bottom)
                            });
                            g.DrawLine(pen2, rect.Right, rect.Top + strokeLength, rect.Right, rect.Bottom - strokeLength);

                        }
                    }
                    BigIconMask.Source = BitmapHelper.ConvertTo(CurrentImage);
                }
            }
            catch (Exception ex)
            {
                ShowError("", ex);
            }
        }

        private void BigIcon_MouseLeave(object sender, MouseEventArgs e)
        {
            IsDraw = false;
            Cursor = Cursors.Arrow;
        }

        private void UpdateImageButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var lines = TextViewTextBox.Text.Split('\n').Select(x=>x.TrimEnd('\r')).ToArray();
                var width = lines.Max(x => x.Length);
                var height = lines.Length;
                var temp = new Sym(width, height);
                for (var y = 0; y < height; y++)
                {
                    var line = lines[y];
                    for (var x = 0; x < line.Length; x++)
                    {
                        var value = ToByte(line[x]);
                        if (value >= CurrentColorComboBox.Items.Count)
                        {
                            throw new Exception("Color was upper then max");
                        }
                        temp.SetPixel(x, y, value);
                    }
                }
                CurrentSym.Raw = temp.Raw;
                CurrentSym.Width = temp.Width;
                CurrentSym.Height = temp.Height;

                ShowCurrentCharacter();
            }
            catch (Exception ex)
            {
                ShowError("", ex);
            }
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                //var code = int.Parse(CurrentCodeTextBox.Text);
                var character = CurrentCharacterTextBox.Text[0];

                if (FFU.Symbols.ContainsKey(character) && !ShowRewriteAttention())
                {
                    return;
                }
                FFU.Symbols[character] = CurrentSym;
                CurrentCharacter = character;
            }
            catch (Exception ex)
            {
                ShowError("Save", ex);
            }
        }

        private void SkewSetTextBox_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                int angle = int.Parse(SkewXTextBox.Text);
                var tanx = Math.Tan(Math.PI / 180 * angle);
                var xoffset = (int)Math.Round(currentSym.Height * tanx);
                var sym = new Sym(currentSym.Width + Math.Abs(xoffset), currentSym.Height);
                if (xoffset < 0)
                {
                    xoffset = -xoffset;
                }
                else
                {
                    xoffset = 0;
                }
                for (int y = 0; y < currentSym.Height; y++)
                {
                    for (int x = 0; x < currentSym.Width; x++)
                    {
                        var pixel = currentSym.GetPixel(x, y);

                        int _x = (int)Math.Round(x + y * tanx);
                        _x += xoffset;
                        if (_x < 0)
                        {
                            continue;
                        }
                        sym.SetPixel(_x, y, pixel);
                    }
                }
                CurrentSym = sym;
                FFU.Symbols[CurrentCharacter] = sym;
                ShowCurrentCharacter();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void ImportMenuItem_Click(object sender, RoutedEventArgs e)
        {

        }

        private void ExportMenuItem_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
