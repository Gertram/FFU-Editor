using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Configuration;
using System.Text;
using System.IO;
using System.Windows.Forms;
using log4net;
using FFULibrary;

namespace FFU_Editor
{
    public partial class MainForm : Form
    {
        private static readonly Encoding encoding = Encoding.GetEncoding("shift-jis");
        private static readonly ILog log = LogManager.GetLogger(typeof(MainForm));
        private FFU FFUFont { get; set; }
        private int Current { get; set; }
        private string FileName { get; set; }
        public MainForm()
        {
            InitializeComponent();
            Focus();
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
            if(FFUFont == null)
            {
                return true;
            }
            var res = MessageBox.Show("Есть несохраненный файл. Сохранить?", "Внимание", MessageBoxButtons.YesNoCancel);
            if(res == DialogResult.Cancel)
            {
                return false;
            }
            if(res == DialogResult.No)
            {
                return true;
            }
            if(FileName != null)
            {
                return Save(FileName);
            }
            return SaveAs();
        }
        private void OpenFile(string filename)
        {
            try
            {
                if (!AskSave())
                {
                    return;
                }
                FFUFont = new FFU(filename);
                FileName = filename;
                Config.Set("LastFile", FileName);
                PalitteComboBox.Items.Clear();
                foreach (var index in FFUFont.Pallettes.Select((x, index) => index))
                {
                    PalitteComboBox.Items.Add(index);
                }
                PalitteComboBox.SelectedIndex = 0;
                BackgroundComboBox.SelectedIndex = 0;
                ShowTemplateImage();
                ShowSym(FFUFont.Symbols.ElementAt(0).Key);
                Text = FileName;
                SaveToolStripMenuItem.Enabled = true;
                SaveAsToolStripMenuItem.Enabled = true;
                ExportToolStripMenuItem.Enabled = true;
            }
            catch (Exception ex)
            {
                ShowError(nameof(OpenFile), ex);
            }
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
                    .Select(line => line.Select(x => ToInt(x)).Where(x => FFUFont.Symbols.ContainsKey(x)).Select(x => FFUFont.Symbols[x]).ToList());
                var bitmap = new Bitmap(TemplateSymbolsBox.Width, TemplateSymbolsBox.Height);
                if (!lines.Any(x => x.Any()))
                {
                    pbSymbols.Image = bitmap;
                    return;
                }
                var x_off = 0;
                var y_off = 0;
                var palitte = FFUFont.GetPalette(PalitteComboBox.SelectedIndex);
                foreach (var line in lines)
                {
                    foreach (var sym in line)
                    {
                        var buff = sym.GetBitmap(palitte);

                        for (var y = 0; y < buff.Height && y + y_off < bitmap.Height; y++)
                        {
                            for (var x = 0; x < buff.Width; x++)
                            {
                                if (x_off + x >= bitmap.Width)
                                {
                                    Console.WriteLine();
                                }
                                bitmap.SetPixel(x_off + x, y_off + y, buff.GetPixel(x, y));
                            }
                        }

                        x_off = (x_off + buff.Width) % bitmap.Width;
                        if ((x_off + buff.Width) / bitmap.Width > 0)
                        {
                            y_off += buff.Height;
                            x_off = 0;
                        }
                    }
                    y_off += FFUFont.Header.FontInfo.SymHeight;
                    x_off = 0;
                }
                pbSymbols.Image = bitmap;
                pbSymbols.Width = bitmap.Width;
                pbSymbols.Height = bitmap.Height;
            }
            catch (Exception ex)
            {
                ShowError(nameof(ShowTemplateImage), ex);
            }
        }
        private void OpenFile()
        {
            var ofd = new OpenFileDialog();
            if (ofd.ShowDialog() != DialogResult.OK)
            {
                return;
            }

            OpenFile(ofd.FileName);
            FileName = ofd.FileName;
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            OpenLastFile();
        }
        private void ShowSym(int code)
        {
            var keys = FFUFont.Symbols.Keys.ToList();
            var index = keys.IndexOf(code);
            if (index == -1)
            {
                return;
            }
            ShowSymAt(index);
        }
        private void ShowSymAt(int index)
        {
            try
            {
                index = (index + FFUFont.Symbols.Count) % FFUFont.Symbols.Count;
                var pair = FFUFont.Symbols.ElementAt(index);
                var sym = pair.Value;
                mtxtSymCode.Text = string.Format("{0:X}", pair.Key);
                Current = index;
                SymHeaderAddressTextBox.Text = $"{sym.HeaderAddress:X}";
                SymImageAddressTextBox.Text = $"{sym.Address:X}";
                var palitte = FFUFont.GetPalette(PalitteComboBox.SelectedIndex);
                smallIcon.Image = sym.GetBitmap(palitte);
                smallIcon.SizeMode = PictureBoxSizeMode.Zoom;
            }
            catch (Exception ex)
            {
                ShowError(nameof(ShowSymAt), ex);
            }
        }
        private void ShowError(string message, Exception ex)
        {
            MessageBox.Show(message + ex.ToString(), "Ошибка:");
            log.Error(ex.ToString());
            statusStrip1.Text = "Ошибка: " + message;
        }
        private void ButtonEnterSymCode_Click(object sender, EventArgs e)
        {
            try
            {
                var sym = int.Parse(mtxtSymCode.Text, System.Globalization.NumberStyles.HexNumber);
                ShowSym(sym);
                for (var i = 0; i < FFUFont.Symbols.Count; i++)
                {
                    if (FFUFont.Symbols.ElementAt(i).Key == sym)
                    {
                        Current = i;
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
        private void MtxtSymCode_TextChanged(object sender, EventArgs e)
        {
            mtxtSymCode.BackColor = Color.White;
        }
        private bool Save(string filename)
        {
            try
            {
                if (FFUFont.CalcFileSize() > FFUFont.Header.FileSize
                    && MessageBox.Show("Итоговой размер файла превышает исходный!Хотите продолжить?", "Внимание", MessageBoxButtons.YesNo) != DialogResult.Yes)
                {
                    return false;
                }

                FFUFont.Save(filename);
                FileName = filename;
                Text = FileName;
                return true;
            }
            catch (Exception ex)
            {
                ShowError(nameof(Save), ex);
                return false;
            }
        }
        private void OpenToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFile();
        }
        private bool SaveAs()
        {
            var sfd = new SaveFileDialog();
            if (sfd.ShowDialog() != DialogResult.OK)
            {
                return false;
            }
            return Save(sfd.FileName);
        }
        private void SaveAsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveAs();
        }
        private void SaveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Save(FileName);
        }
        private void UpdateStatus()
        {
            var filesize = FFUFont.CalcFileSize();
            if (filesize > FFUFont.Header.FileSize)
            {
                toolStripStatusLabel1.Text = "Итоговой размер файла превышает исходный";
            }
            else if (filesize < FFUFont.Header.FileSize)
            {
                toolStripStatusLabel1.Text = "Итоговый размер файл меньше исходного, будут добавлены символы в конце файла";
            }
            else
            {
                toolStripStatusLabel1.Text = "Итоговый размер файла совпадат с исходным";
            }
        }

        private void ButtonAddNew_Click(object sender, EventArgs e)
        {
            try
            {
                var form = new AddSymForm(FFUFont);
                form.CurrentSym = new Sym(new byte[FFUFont.Header.FontInfo.SymWidth, FFUFont.Header.FontInfo.SymHeight]);
                if (form.ShowDialog() != DialogResult.OK)
                {
                    return;
                }
                UpdateStatus();
            }
            catch (Exception ex)
            {
                ShowError(nameof(ButtonAddNew_Click), ex);
            }
        }
        private void ShowRedactForm()
        {
            try
            {
                var form = new AddSymForm(FFUFont);
                var sym = FFUFont.Symbols.ElementAt(Current);
                form.CurrentCode = sym.Key;
                if (form.ShowDialog() != DialogResult.OK)
                {
                    return;
                }

                UpdateStatus();
            }
            catch (Exception ex)
            {
                ShowError(nameof(ButtonRedact_Click), ex);
            }
        }
        private void ButtonRedact_Click(object sender, EventArgs e)
        {
            ShowRedactForm();
        }

        private void ButtonRemove_Click(object sender, EventArgs e)
        {
            try
            {
                var sym = FFUFont.Symbols.ElementAt(Current);
                FFUFont.Symbols.Remove(sym.Key);
                if (Current == FFUFont.Symbols.Count)
                {
                    Current = FFUFont.Symbols.Count - 1;
                }
                else
                {
                    Current++;
                }
                UpdateStatus();
                ShowSym(FFUFont.Symbols.ElementAt(Current).Key);
            }
            catch (Exception ex)
            {
                ShowError(nameof(ButtonRemove_Click), ex);
            }
        }
        private void MtxtSymCode_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter && int.TryParse(mtxtSymCode.Text, System.Globalization.NumberStyles.HexNumber, null, out var result))
            {
                ShowSym(result);
                e.Handled = true;
            }
        }
        private void ImportPadding(int start, int end, FFU tempFont)
        {
            var ratio = FFUFont.Header.FontInfo.SymWidth / tempFont.Header.FontInfo.SymWidth;
            foreach (var sym in FFUFont.Symbols.Where(x => x.Key >= start && x.Key <= end))
            {
                var temp = new SymWrap(tempFont.Symbols[sym.Key]);
                var leftBorder = temp.FindLeftPosition() * ratio;
                var rightBorder = (temp.Sym.Width - temp.FindRightPosition() - 1) * ratio;
                temp.SetHorizontalPadding(leftBorder, rightBorder);
            }
        }
        private void SetPadding(int start, int end, int border = 1)
        {
            foreach (var sym in FFUFont.Symbols.Where(x => x.Key >= start && x.Key <= end))
            {
                var temp = new SymWrap(sym.Value);
                temp.SetPadding(border);
            }
        }
        private void SwitchSym(int first, int second)
        {
            var temp1 = FFUFont.Symbols[first];
            var temp2 = FFUFont.Symbols[second];
            FFUFont.Symbols[first] = temp2;
            FFUFont.Symbols[second] = temp1;
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
        private Sym ConvertToSym(char sym, Bitmap bitmap, Dictionary<Color, byte> colors)
        {
            var data = new byte[bitmap.Width, bitmap.Height];

            for (var y = 0; y < bitmap.Height; y++)
            {
                for (var x = 0; x < bitmap.Width; x++)
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
            return new Sym(data);
        }
        private int ToInt(char sym)
        {
            var bytes = encoding.GetBytes(new char[] { sym });
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
                e.Effect = DragDropEffects.Copy;
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
                ShowError("WriteXML Error", ex);
            }
        }

        private void ExportPNG(string filename)
        {
            try
            {
                PNGUtility.Export(FFUFont, filename, PalitteComboBox.SelectedIndex);
            }
            catch (Exception ex)
            {
                ShowError("WritePNG Error", ex);
            }
        }
        private void ExportFontToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var ofd = new SaveFileDialog
            {
                DefaultExt = ".xml",
                Filter = "Xml files (.xml)|*.xml",
                Title = "Выберите имя файла",
                InitialDirectory = Environment.CurrentDirectory
            };
            if (ofd.ShowDialog() != DialogResult.OK)
            {
                return;
            }
            var filename = Path.GetFileNameWithoutExtension(ofd.FileName);
            ExportXML(Path.ChangeExtension(filename, "xml"));
            ExportPNG(Path.ChangeExtension(filename, "png"));
        }
        private void ExportPNGToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var ofd = new SaveFileDialog
            {
                AddExtension = true,
                DefaultExt = ".png",
                Title = "Выберите имя файла",
                ValidateNames = true,
                InitialDirectory = Environment.CurrentDirectory
            };
            if (ofd.ShowDialog() != DialogResult.OK)
            {
                return;
            }
            ExportPNG(ofd.FileName);
        }

        private void SmallIconButton_Click(object sender, EventArgs e)
        {
            try
            {
                var symcode = int.Parse(mtxtSymCode.Text, System.Globalization.NumberStyles.HexNumber);
                var sym = FFUFont.Symbols[symcode];
                var bitmap = new Bitmap(Convert.ToInt32(sym.Width), Convert.ToInt32(sym.Height));

                for (var y = 0; y < sym.Height; y++)
                {
                    for (var x = 0; x < sym.Width; x++)
                    {
                        var val = sym.Image[x, y];
                        bitmap.SetPixel(x, y, Color.FromArgb(val, val, val));
                    }
                }
                smallIcon.Image = bitmap;
                smallIcon.SizeMode = PictureBoxSizeMode.Zoom;
            }
            catch (Exception ex)
            {
                ShowError(nameof(SmallIconButton_Click), ex);
            }
        }
        private void MainForm_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.R && e.Modifiers == Keys.Control)
            {
                ShowRedactForm();
            }
            else if (e.KeyCode == Keys.Right && e.Control)
            {
                ShowSymAt(Current + 1);
            }
            else if (e.KeyCode == Keys.Left && e.Control)
            {
                ShowSymAt(Current - 1);
            }
            else if (e.KeyCode == Keys.T && e.Control && e.Shift)
            {
                SaveAsToolStripMenuItem_Click(this, null);
            }
            else if (e.KeyCode == Keys.S && e.Control)
            {
                SaveToolStripMenuItem_Click(this, null);
            }
            else if (e.KeyCode == Keys.O && e.Modifiers == Keys.Control)
            {
                OpenFile();
                return;
            }
            else if (e.KeyCode == Keys.Up && e.Control)
            {
                smallIcon.Size = new Size(smallIcon.Size.Width + 10, smallIcon.Size.Height + 10);
            }
            else if (e.KeyCode == Keys.Down && e.Control && smallIcon.Size.Width > 10 && smallIcon.Size.Height > 10)
            {
                smallIcon.Size = new Size(smallIcon.Size.Width - 10, smallIcon.Size.Height - 10);
            }
        }

        private void PalitteComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (FFUFont == null)
                {
                    return;
                }
                var palitte = FFUFont.GetPalette(PalitteComboBox.SelectedIndex);
                if (palitte == null)
                {
                    return;
                }
                ShowSymAt(Current);
                ShowTemplateImage();
            }
            catch (Exception ex)
            {
                ShowError(nameof(PalitteComboBox_SelectedIndexChanged), ex);
            }
        }

        private void BackgroundComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (BackgroundComboBox.Text == "Черный")
                {
                    pbSymbols.BackColor = Color.Black;
                    smallIcon.BackColor = Color.Black;
                }
                else if (BackgroundComboBox.Text == "Белый")
                {
                    pbSymbols.BackColor = Color.White;
                    smallIcon.BackColor = Color.White;
                }
            }
            catch (Exception ex)
            {
                ShowError(nameof(BackgroundComboBox_SelectedIndexChanged), ex);
            }
        }

        private void ImportPaddingToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var ofd = new OpenFileDialog();
            if (ofd.ShowDialog() != DialogResult.OK)
            {
                return;
            }
            try
            {
                var tempFont = new FFU(ofd.FileName);

                ImportPadding(0x8440, 0x8460, tempFont);
                ImportPadding(0x8470, 0x8491, tempFont);
                ImportPadding(0x8260, 0x8279, tempFont);
                ImportPadding(0x8281, 0x829a, tempFont);
                ImportPadding('A', 'Z', tempFont);
                ImportPadding('a', 'z', tempFont);
                ShowTemplateImage();
            }
            catch (Exception ex)
            {
                ShowError("ImportPadding", ex);
            }
        }

        private void GenerateFontToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                var win = new GenerateFontWindow();
                if (win.ShowDialog() != DialogResult.OK)
                {
                    return;
                }

                foreach (var item in win.Syms)
                {
                    var sym = ConvertToSym(item.Key, item.Value, win.Colors);


                    var key = ToInt(item.Key);
                    FFUFont.Symbols[key] = sym;
                }
                ShowTemplateImage();
            }
            catch (Exception ex)
            {
                ShowError("GenerateFont", ex);
            }
        }

        private void SetPaddingToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                var form = new SetPaddingForm();
                if (form.ShowDialog() != DialogResult.OK)
                {
                    return;
                }
                var padding = form.Value;
                SetPadding('A', 'Z', padding);
                SetPadding('a', 'z', padding);

                SetPadding(0x8260, 0x8279, padding);
                SetPadding(0x8281, 0x829a, padding);

                SetPadding(0x8440, 0x8460, padding);
                SetPadding(0x8470, 0x8491, padding);
                ShowTemplateImage();
            }
            catch (Exception ex)
            {
                ShowError("RemovePadding", ex);
            }
        }
        private SetSymbolsForm SymbolsForm { get; set; }
        private void SetSymbolsButton_Click(object sender, EventArgs e)
        {
            if (SymbolsForm != null)
            {
                return;
            }
            SymbolsForm = new SetSymbolsForm();
            SymbolsForm.Symbols = MainConfig.Template;
            SymbolsForm.PropertyChanged += SymbolsForm_PropertyChanged;
            SymbolsForm.FormClosed += SymbolsForm_FormClosed;
            SymbolsForm.Show();
        }

        private void SymbolsForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (SymbolsForm.DialogResult == DialogResult.OK)
            {
                MainConfig.Template = SymbolsForm.Symbols;
            }
            ShowTemplateImage();
            SymbolsForm = null;
        }

        private void SymbolsForm_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(SymbolsForm.Symbols) && SymbolsForm != null)
            {
                ShowTemplateImage(SymbolsForm.Symbols);
            }
        }

        private void FontOptionsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var form = new FontInfoForm(FFUFont);
            form.ShowDialog();
        }
        //Не знаю нужно ли
        private void ImportFromFFUToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                var ofd = new OpenFileDialog();
                if (ofd.ShowDialog() != DialogResult.OK)
                {
                    return;
                }
                var tempFont = new FFU(ofd.FileName);

                FFUFont.Symbols = new SortedDictionary<int, Sym>(FFUFont.Symbols.Take(FFUFont.Symbols.Count/* / 4*/).ToDictionary(x => x.Key, x => x.Value));

                foreach (var pair in tempFont.Symbols)
                {
                    if (FFUFont.Symbols.ContainsKey(pair.Key) || pair.Key >= 0x8440 && pair.Key <= 0x8491)
                    {
                        FFUFont.Symbols[pair.Key] = pair.Value;
                    }
                }

                var buff = new List<int>();
                foreach (var item in FFUFont.Symbols)
                {
                    if (!tempFont.Symbols.ContainsKey(item.Key))
                    {
                        buff.Add(item.Key);
                    }
                }
                foreach (var item in buff)
                {
                    FFUFont.Symbols[item].Image = new byte[tempFont.Header.FontInfo.SymWidth, tempFont.Header.FontInfo.SymHeight];
                }
                ShowTemplateImage();
            }
            catch (Exception ex)
            {
                ShowError("", ex);
            }
        }

        private void ImportXMLFontToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                if (!AskSave())
                {
                    return;
                }
                var ofd = new OpenFileDialog();
                ofd.Filter = "XML файлы (*.xml)|*.xml";
                if (ofd.ShowDialog() != DialogResult.OK)
                {
                    return;
                }
                var info = XMLUtility.Import(ofd.FileName);

                FFUFont = PNGUtility.Import(info);
                ShowTemplateImage();
            }
            catch (Exception ex)
            {
                ShowError("", ex);
            }
        }

        private void SymbolsListToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var form = new SymbolsListForm(FFUFont);
            form.ShowDialog();
        }
    }
}
