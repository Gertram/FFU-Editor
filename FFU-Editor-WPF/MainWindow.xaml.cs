using MahApps.Metro.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Media;
using System.Windows.Input;
using System.Windows.Navigation;
using System.IO;
using System.Runtime.InteropServices;
using System.Globalization;
using System.ComponentModel;
using Forms = System.Windows.Forms;
using log4net;
using ControlzEx.Standard;
using System.Reflection;
using System.Web.UI;


namespace FFUEditor
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : MetroWindow
    {
        private static ILog Log { get; } = LogManager.GetLogger(typeof(MainWindow));
        private char currentCharacter;

        private bool ShouldSave { get; set; } = false;
        private FFULibrary.FFU FFUFont { get; set; }
        private char CurrentCharacter
        {
            get => currentCharacter;
            set
            {
                currentCharacter = value;
                ShowCurrentCharacter();
            }
        }
        private int CurrentCharacterIndex
        {
            get
            {
                return FFUFont.Symbols.Keys.ToList().IndexOf(CurrentCharacter);
            }

            set
            {
                value = (value + FFUFont.Symbols.Count) % FFUFont.Symbols.Count;
                var pair = FFUFont.Symbols.ElementAt(value);
                CurrentCharacter = pair.Key;
            }
        }
        private string FileName { get; set; }
        public class FontEncodingItem
        {
            public bool IsSelected { get; set; }
            public Encoding Item { get; set; }
        }
        public MainWindow()
        {
            InitializeComponent();
            InitializeLanguages();
        }
        private void InitializeLanguages()
        {
            App.LanguageChanged += App_LanguageChanged; ;

            CultureInfo currLang = App.Language;

            //Заполняем меню смены языка:
            menuLanguage.Items.Clear();
            foreach (var lang in App.Languages)
            {
                MenuItem menuLang = new MenuItem();
                menuLang.Header = lang.DisplayName;
                menuLang.Tag = lang;
                menuLang.IsChecked = lang.Equals(currLang);
                menuLang.Click += MenuLang_Click; ;
                menuLanguage.Items.Add(menuLang);
            }
        }
        private void OpenLastFile()
        {
            var lastFile = Config.Get("LastFile");
            if (lastFile == null)
            {
                return;
            }
            OpenFile(lastFile);
        }
        private bool AskSave()
        {
            if (!ShouldSave || FFUFont == null)
            {
                return true;
            }
            var res = MessageBox.Show(this.GetResourceString("m_UnsavedChanges"),this.GetResourceString("m_Attention"), MessageBoxButton.YesNoCancel);

            if (res == MessageBoxResult.Cancel)
            {
                return false;
            }
            if (res == MessageBoxResult.No)
            {
                return true;
            }
            if (FileName != null)
            {
                return Save(FileName);
            }
            return SaveAs();
        }
        private void UpdateEncoding()
        {
            if(FFUFont.Header.FontInfo.Encoding == FFULibrary.FontEncoding.SHIFT_JIS)
            {
                EncodingComboBox.SelectedIndex = 0;
            }
            else if(FFUFont.Header.FontInfo.Encoding == FFULibrary.FontEncoding.UTF8)
            {
                EncodingComboBox.SelectedIndex = 1;
            }
        }
        private void OpenFile(string filename)
        {
            try
            {
                if (!AskSave())
                {
                    return;
                }
                FFUFont = new FFULibrary.FFU(filename);
                //File.WriteAllText("temp.txt", string.Join("\n", FFUFont.Symbols.Keys));
                FileName = filename;
                Config.Set("LastFile", FileName);
                PaletteComboBox.Items.Clear();
                if (FFUFont.Pallettes != null)
                {
                    foreach (var index in FFUFont.Pallettes.Select((x, index) => index))
                    {
                        PaletteComboBox.Items.Add(index);
                    }
                    PaletteComboBox.SelectedIndex = 0;
                }
                BackgroundComboBox.SelectedIndex = 0;
                ShowTemplateImage();
                CurrentCharacterIndex = 0;
                Title = FileName;
            }
            catch (Exception ex)
            {
                ShowError(nameof(OpenFile), ex);
            }
        }

        private Color[] GetPalette()
        {
            return GetPalette(PaletteComboBox.SelectedIndex);
        }

        private Color[] GetPalette(int index)
        {
            return FFUFont.GetPalette(index).Select(
                color => Color.FromArgb(color.A, color.G, color.R, color.B)
                ).ToArray();
        }

        private void ShowTemplateImage(string template = null)
        {
            try
            {
                if (template == null)
                {
                    template = MainConfig.Template;
                }
                var lines = template.Split('\n')
                    .Select(line => line.Where(x => FFUFont.Symbols.ContainsKey(x)).Select(x => FFUFont.Symbols[x]).ToList());
                if(TemplateImageWrap == null)
                {
                    return;
                }
                int width = (int)TemplateImageWrap.ActualWidth;
                int height = (int)TemplateImageWrap.ActualHeight;

                var raw = new byte[width * height];

                var bitmapPalette = new System.Windows.Media.Imaging.BitmapPalette(GetPalette());
                try
                {
                    if (lines.Any(x => x.Any()))
                    {
                        BitmapHelper.DrawLines(raw, GetPalette(), FFUFont.Header.FontInfo.SymHeight, width, lines);
                    }
                }
                catch
                {

                }
                TemplateImage.Source = System.Windows.Media.Imaging.BitmapSource.Create(width, height, 96, 96, PixelFormats.Indexed8, bitmapPalette, raw, width);
                TemplateImage.Width = width;
                TemplateImage.Height = height;
            }
            catch (Exception ex)
            {
                ShowError(nameof(ShowTemplateImage), ex);
            }
        }

        private void OpenFile()
        {
            var ofd = new Forms.OpenFileDialog();
            if (ofd.ShowDialog() != Forms.DialogResult.OK)
            {
                return;
            }

            OpenFile(ofd.FileName);
            FileName = ofd.FileName;
        }

        private void MetroWindow_Loaded(object sender, RoutedEventArgs e)
        {
            OpenLastFile();
        }
        private void ShowCurrentCharacter()
        {
            mtxtCharacterCode.Text = $"{string.Join("",FFUFont.CurrentEncoding.GetBytes(CurrentCharacter.ToString()).Select(x=>x.ToString("X")))}";
            mtxtCharacter.Text = CurrentCharacter.ToString();
            mtxtCharacterIndex.Text = CurrentCharacterIndex.ToString();
            if(!FFUFont.Symbols.TryGetValue(CurrentCharacter,out var sym)){
                return;
            }
            CharacterHeaderTextBox.Text = $"{sym.HeaderAddress:X}";
            CharacterBodyTextBox.Text = $"{sym.Address:X}";
            var palette = GetPalette();

            var colors = new System.Windows.Media.Imaging.BitmapPalette(palette);
            //Buffer.BlockCopy(sym.Image, 0, temp, 0, temp.Length);
            int scale = 3;
            int stride = (sym.Width * PixelFormats.Indexed8.BitsPerPixel + 7) / 8 * scale;
            var buff = sym.Draw(scale);
            var bitmapSource = System.Windows.Media.Imaging.BitmapSource.Create(
                sym.Width * scale, sym.Height * scale,
                96, 96,
                PixelFormats.Indexed8, colors,
                buff, stride);

            Miniature.ImageSource = bitmapSource;
        }

        private void ShowError(Exception ex)
        {
            ShowError("", ex);
        }
        private void ShowError(string message, Exception ex)
        {
            MessageBox.Show(message + ex.ToString(), $"{this.GetResourceString("m_Error")}:");
            Log.Error(ex.ToString());
            StatusBar1.Content = $"{this.GetResourceString("m_Error")}: " + message;
        }

        private bool Save(string filename)
        {
            try
            {
                int filesize = FFUFont.CalcFileSize();
                if (filesize > FFUFont.Header.FileSize
                    && MessageBox.Show(
                         this.GetResourceString("m_FileBiggerThanSource_Attention"),
                         this.GetResourceString("m_Attention"), 
                         MessageBoxButton.YesNo) != MessageBoxResult.Yes)
                {
                    return false;
                }

                FFUFont.Save(filename);
                FileName = filename;
                Title = FileName;
                return true;
            }
            catch (Exception ex)
            {
                ShowError(nameof(Save), ex);
                return false;
            }
        }

        private bool SaveAs()
        {
            var sfd = new Forms.SaveFileDialog();
            if (sfd.ShowDialog() != Forms.DialogResult.OK)
            {
                return false;
            }
            return Save(sfd.FileName);
        }

        private void UpdateStatus()
        {
            var filesize = FFUFont.CalcFileSize();
            if (filesize > FFUFont.Header.FileSize)
            {
                StatusBar1.Content = this.GetResourceString("m_FileBiggerThanSource");
            }
            else if (filesize < FFUFont.Header.FileSize)
            {
                StatusBar1.Content = this.GetResourceString("m_FileSmallerThanSource");
            }
            else
            {
                StatusBar1.Content = this.GetResourceString("m_FileMathchWithSource");
            }
        }

        private int ToInt(char sym)
        {
            var bytes = FFUFont.CurrentEncoding.GetBytes(new char[] { sym });
            if (bytes.Length == 2)
            {
                return BitConverter.ToUInt16(bytes.Reverse().ToArray(), 0);
            }
            return bytes[0];
        }

        private void MainForm_DragDrop(object sender, DragEventArgs e)
        {
            if (!e.Data.GetDataPresent(DataFormats.FileDrop, false))
            {
                return;
            }
            var filename = (e.Data.GetData(DataFormats.FileDrop, false) as string[])[0];

            OpenFile(filename);
        }

        private void MainForm_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                e.Effects = DragDropEffects.Copy;
            }
        }

        private void ExportXML(string filename)
        {
            try
            {
                XMLUtility.Export(FFUFont, filename);
            }
            catch (Exception ex)
            {
                ShowError(this.GetResourceString("m_XMLSaveError"), ex);
            }
        }

        private void ExportPNG(string filename)
        {
            try
            {
                PNGUtility.Export(FFUFont, filename, PaletteComboBox.SelectedIndex);
            }
            catch (Exception ex)
            {
                ShowError(this.GetResourceString("m_PNGSaveError"), ex);
            }
        }

        private void MenuLang_Click(object sender, RoutedEventArgs e)
        {
            try {
            MenuItem mi = sender as MenuItem;
            if (mi != null)
            {
                CultureInfo lang = mi.Tag as CultureInfo;
                if (lang != null)
                {
                    App.Language = lang;
                }
            }
            }
            catch (Exception ex)
            {
                ShowError(ex);
            }
        }

        private void App_LanguageChanged(object sender, EventArgs e)
        {
            CultureInfo currLang = App.Language;

            //Отмечаем нужный пункт смены языка как выбранный язык
            foreach (MenuItem i in menuLanguage.Items)
            {
                CultureInfo ci = i.Tag as CultureInfo;
                i.IsChecked = ci != null && ci.Equals(currLang);
            }
        }

        private void MetroWindow_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (FFUFont == null)
            {
                return;
            }
            if (e.KeyboardDevice.Modifiers == ModifierKeys.None)
            {
                return;
            }
            if (!e.KeyboardDevice.Modifiers.HasFlag(ModifierKeys.Control))
            {
                return;
            }
            else if (e.Key == Key.Right)
            {
                CurrentCharacterIndex++;
            }
            else if (e.Key == Key.Left)
            {
                CurrentCharacterIndex--;
            }
            else if (e.Key == Key.Up)
            {
                MiniatureWrap.Width = MiniatureWrap.Height += 10;
            }
            else if (e.Key == Key.Down)
            {
                if (MiniatureWrap.Width > 10 && MiniatureWrap.Height > 10)
                {
                    MiniatureWrap.Width = MiniatureWrap.Height -= 10;
                }
            }
            else
            {
                return;
            }
            e.Handled = true;
        }

        private void ImportCommandBinding_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            if (e.Parameter is ImportFormats format)
            {
                if (format == ImportFormats.XML)
                {

                    try
                    {
                        if (!AskSave())
                        {
                            return;
                        }
                        var ofd = new Forms.OpenFileDialog();
                        ofd.Filter = this.GetResourceString("m_XMLFiles") +" (*.xml)|*.xml";
                        if (ofd.ShowDialog() != Forms.DialogResult.OK)
                        {
                            return;
                        }
                        var info = XMLUtility.Import(ofd.FileName);

                        FFUFont = PNGUtility.Import(info);
                        FileName = Path.GetFileNameWithoutExtension(ofd.FileName);
                        ShowTemplateImage();
                    }
                    catch (Exception ex)
                    {
                        ShowError(ex);
                    }
                }
                //else if(format == ImportFormats.FFU)
                //{
                //    try
                //    {
                //        var ofd = new Forms.OpenFileDialog();
                //        if (ofd.ShowDialog() != Forms.DialogResult.OK)
                //        {
                //            return;
                //        }
                //        var tempFont = new FFU(ofd.FileName);

                //        FFUFont.Symbols = new SortedDictionary<int, Sym>(FFUFont.Symbols.Take(FFUFont.Symbols.Count/* / 4*/).ToDictionary(x => x.Key, x => x.Value));

                //        foreach (var pair in tempFont.Symbols)
                //        {
                //            if (FFUFont.Symbols.ContainsKey(pair.Key) || pair.Key >= 0x8440 && pair.Key <= 0x8491)
                //            {
                //                FFUFont.Symbols[pair.Key] = pair.Value;
                //            }
                //        }

                //        var buff = new List<int>();
                //        foreach (var item in FFUFont.Symbols)
                //        {
                //            if (!tempFont.Symbols.ContainsKey(item.Key))
                //            {
                //                buff.Add(item.Key);
                //            }
                //        }
                //        foreach (var item in buff)
                //        {
                //            FFUFont.Symbols[item].Image = new byte[tempFont.Header.FontInfo.SymWidth, tempFont.Header.FontInfo.SymHeight];
                //        }
                //        ShowTemplateImage();
                //    }
                //    catch (Exception ex)
                //    {
                //        ShowError("", ex);
                //    }
                //}
            }
        }

        private void ExportCommandBinding_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            if (e.Parameter is ExportFormats format)
            {
                if (format == ExportFormats.PNG)
                {
                    var ofd = new Forms.SaveFileDialog
                    {
                        AddExtension = true,
                        DefaultExt = ".png",
                        Filter = "PNG|*.png",
                        FileName = Path.ChangeExtension(Path.GetFileName(FileName),"png"),
                        Title = this.GetResourceString("m_ChooseFileName"),
                        ValidateNames = true
                    };
                    if (ofd.ShowDialog() != Forms.DialogResult.OK)
                    {
                        return;
                    }
                    ExportPNG(ofd.FileName);
                }
                else if (format == ExportFormats.XML)
                {
                    var ofd = new Forms.SaveFileDialog
                    {
                        FileName=Path.ChangeExtension(Path.GetFileName(FileName),"xml"),
                        DefaultExt = ".xml",
                        Filter = this.GetResourceString("m_XMLFiles") +"(.xml)|*.xml",
                        Title = this.GetResourceString("m_ChooseFileName"),
                    };
                    if (ofd.ShowDialog() != Forms.DialogResult.OK)
                    {
                        return;
                    }
                    var filename = ofd.FileName;
                    ExportXML(Path.ChangeExtension(filename, "xml"));
                    ExportPNG(Path.ChangeExtension(filename, "png"));
                }
            }
        }

        private void ExportCommandBinding_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = FFUFont != null;
        }

        private void PaletteComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                if (FFUFont == null)
                {
                    return;
                }
                var palette = GetPalette();
                if (palette == null)
                {
                    return;
                }
                ShowCurrentCharacter();
                ShowTemplateImage();
            }
            catch (Exception ex)
            {
                ShowError(nameof(PaletteComboBox_SelectionChanged), ex);
            }
        }

        private void BackgroundComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                if (BackgroundComboBox.SelectedIndex == 0)
                {
                    TemplateImageWrap.Background = System.Windows.Media.Brushes.Black;
                    MiniatureWrap.Background = System.Windows.Media.Brushes.Black;
                }
                else if (BackgroundComboBox.SelectedIndex == 1)
                {
                    TemplateImageWrap.Background = System.Windows.Media.Brushes.White;
                    MiniatureWrap.Background = System.Windows.Media.Brushes.White;
                }
            }
            catch (Exception ex)
            {
                ShowError(nameof(BackgroundComboBox_SelectionChanged), ex);
            }
        }

        private void MtxtCharacterCode_TextChanged(object sender, TextChangedEventArgs e)
        {
            mtxtCharacterCode.Background = System.Windows.Media.Brushes.Transparent;
            mtxtCharacter.Background = System.Windows.Media.Brushes.Transparent;
        }
        private void MtxtCharacter_TextChanged(object sender, TextChangedEventArgs e)
        {
            mtxtCharacterCode.Background = System.Windows.Media.Brushes.Transparent;
            mtxtCharacter.Background = System.Windows.Media.Brushes.Transparent;
        }

        private void OpenCommandBinding_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            OpenFile();
        }

        private void SaveCommandBinding_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            Save(FileName);
        }

        private void SaveAsCommandBinding_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            SaveAs();
        }

        private void AddCharacterCommandBinding_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            try
            {
                var form = new AddCharacterWindow(FFUFont);
                form.CurrentSym = new FFULibrary.Sym(
                    new byte[FFUFont.Header.FontInfo.SymWidth * FFUFont.Header.FontInfo.SymHeight],
                    FFUFont.Header.FontInfo.SymWidth,
                    FFUFont.Header.FontInfo.SymHeight);
                if (!(bool)form.ShowDialog())
                {
                    return;
                }
                CurrentCharacter = form.CurrentCharacter;
                UpdateStatus();
            }
            catch (Exception ex)
            {
                ShowError(this.GetResourceString("m_AddCharacterError"), ex);
            }
        }

        private void ShowCharacterByCodeCommandBinding_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            try
            {
                var characterCode = int.Parse(mtxtCharacterCode.Text,NumberStyles.HexNumber);
                var characterBytes = BitConverter.GetBytes(characterCode);
                var character = FFUFont.CurrentEncoding.GetChars(characterBytes)[0];
                var sym = FFUFont.Symbols[character];
                CurrentCharacter = character;
                mtxtCharacterCode.Background = System.Windows.Media.Brushes.Transparent;
            }
            catch
            {
                mtxtCharacterCode.Background = System.Windows.Media.Brushes.Red;
            }
        }
        private void ShowCharacterByIndexCommandBinding_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            try
            {
                var index = int.Parse(mtxtCharacterIndex.Text);
                if (index < 0 || index >= FFUFont.Symbols.Count)
                {
                    mtxtCharacterIndex.Background = System.Windows.Media.Brushes.Red;
                    return;
                }
                CurrentCharacterIndex = index;
                mtxtCharacterIndex.Background = System.Windows.Media.Brushes.Transparent;
            }
            catch
            {
                mtxtCharacterIndex.Background = System.Windows.Media.Brushes.Red;
            }
        }

        private void ShowCharacterCommandBinding_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            try
            {
                var character = mtxtCharacter.Text[0];
                var sym = FFUFont.Symbols[character];
                CurrentCharacter = character;
                mtxtCharacter.Background = System.Windows.Media.Brushes.Transparent;
            }
            catch
            {
                mtxtCharacter.Background = System.Windows.Media.Brushes.Red;
            }
        }

        private void RedactCharacterCommandBinding_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            try
            {
                var form = new AddCharacterWindow(FFUFont);
                form.CurrentCharacter = currentCharacter;
                if (!(bool)form.ShowDialog())
                {
                    CurrentCharacter = form.CurrentCharacter;
                    return;
                }
                CurrentCharacter = form.CurrentCharacter;
                UpdateStatus();
            }
            catch (Exception ex)
            {
                ShowError(this.GetResourceString("m_RedactCharacterError"), ex);
            }
        }

        private void RemoveCharacterCommandBinding_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            try
            {
                var index = CurrentCharacterIndex;
                FFUFont.Symbols.Remove(CurrentCharacter);
                CurrentCharacterIndex = index;
                UpdateStatus();
            }
            catch (Exception ex)
            {
                ShowError(this.GetResourceString("m_RemoveCharacterError"), ex);
            }
        }

        private void FontSettingsCharacterCommandBinding_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            try
            {
                var form = new FontInfoWindow(FFUFont);
                form.ShowDialog();
            }
            catch (Exception ex)
            {
                ShowError(ex);
            }
        }

        private void CharactersListCharacterCommandBinding_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            try
            {
                var form = new CharactersListWindow(FFUFont);
                form.ShowDialog();
            }
            catch (Exception ex)
            {
                ShowError(ex);
            }
        }

        private ModifiableCharactersWindow CharactersForm { get; set; }

        private void SymbolsForm_FormClosed(object sender, EventArgs e)
        {
            if (CharactersForm.DialogResult is bool result && result)
            {
                MainConfig.Template = CharactersForm.Characters;
            }
            ShowTemplateImage();
            CharactersForm = null;
        }

        private void SymbolsForm_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(CharactersForm.Characters) && CharactersForm != null)
            {
                ShowTemplateImage(CharactersForm.Characters);
            }
        }

        private void ModifiableCharactersCommandBinding_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            if (CharactersForm != null)
            {
                CharactersForm.Activate();
                return;
            }
            CharactersForm = new ModifiableCharactersWindow();
            CharactersForm.Characters = MainConfig.Template;
            CharactersForm.PropertyChanged += SymbolsForm_PropertyChanged;
            CharactersForm.Closed += SymbolsForm_FormClosed;
            CharactersForm.Show();
        }

        private void ToolsCommandBinding_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            var tool = e.Parameter as string;
            try
            {
                var tools = new Tools(FFUFont);
                switch (tool)
                {
                    case "ImportPadding":
                        tools.ImportPadding();
                        break;
                    case "ExpandPadding":
                        tools.ExpandPadding();
                        break;
                    case "SetPadding":
                        tools.SetPadding();
                        break;
                    case "AddStroke":
                        tools.AddStroke();
                        break;
                    case "GenerateFont":
                        tools.GenerateFont();
                        break;
                    case "CopyRange":
                        tools.CopyRange();
                        break;
                    case "MoveRange":
                        tools.MoveRange();
                        break;
                    default:
                        break;
                }
                ShowTemplateImage();
            }
            catch (Exception ex)
            {
                ShowError(tool, ex);
            }
        }

        private void EncodingComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if(EncodingComboBox.SelectedIndex == 0)
            {
                FFUFont.Header.FontInfo.Encoding = FFULibrary.FontEncoding.SHIFT_JIS;
            }
            else if(EncodingComboBox.SelectedIndex == 1)
            {
                FFUFont.Header.FontInfo.Encoding = FFULibrary.FontEncoding.UTF8;
            }
            UpdateEncoding();
            ShowTemplateImage();
        }

    }
}
