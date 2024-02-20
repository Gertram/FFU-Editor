using System;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using FFUEditor.ColorExtension;
using FFULibrary;
using MahApps.Metro.Controls;
using System.ComponentModel;
using ControlzEx.Standard;

namespace FFUEditor
{
    /// <summary>
    /// Логика взаимодействия для SymbolsListWindow.xaml
    /// </summary>
    public partial class CharactersListWindow : MetroWindow
    {
        public static RoutedCommand DeleteCommand { get; } = new RoutedCommand();
        public class CharacterItem : INotifyPropertyChanged
        {
            private char character;

            public CharacterItem(char character, Encoding encoding, BitmapSource image,FFU font)
            {
                this.font = font;
                CurrentEncoding = encoding;
                Character = character;
                Image = image;
            }
            private Encoding CurrentEncoding { get; set; }
            public char Character
            {
                get
                {
                    return character;
                }
                set
                {
                    if(character == value)
                    {
                        return;
                    }
                    var old = character;
                    character = value;
                    if (Code == 0x3f && Character != '?')
                    {
                        State = 1;
                    }
                    if (old != default)
                    {
                        if (font.Symbols.ContainsKey(value))
                        {
                            State = 2;
                        }
                        else
                        {
                            State = 0;
                            var sym = font.Symbols[old];
                            font.Symbols.Remove(old);
                            font.Symbols.Add(value, sym);
                        }
                    }

                    PropertyChanged?.Invoke(this,new PropertyChangedEventArgs(nameof(Character)));
                    PropertyChanged?.Invoke(this,new PropertyChangedEventArgs(nameof(Code)));
                    PropertyChanged?.Invoke(this,new PropertyChangedEventArgs(nameof(HexCode)));
                    PropertyChanged?.Invoke(this,new PropertyChangedEventArgs(nameof(State)));
                }
            }
            private FFU font { get; set; }
            public int Code => ToInt(Character);
            public string HexCode => Code.ToString("X");
            public int State { get; set; } = 0;
            public BitmapSource Image { get; }

            public event PropertyChangedEventHandler PropertyChanged;

            private byte[] PadRight(byte[] bytes, int padding)
            {
                if (bytes.Length == padding)
                {
                    return bytes;
                }
                return bytes.Concat(new byte[padding - bytes.Length]).ToArray();
            }
            private int ToInt(char ch)
            {
                var bytes = CurrentEncoding.GetBytes(ch.ToString());
                return BitConverter.ToInt32(PadRight(bytes.Reverse().ToArray(), 4), 0);
            }
        }
        public ObservableCollection<CharacterItem> Characters { get; }
        private FFU font;
        private bool ShouldSave { get; set; } = false;
        public CharactersListWindow(FFU font)
        {
            this.font = font;
            var characters = new List<CharacterItem>();
            var palette = font.GetPalette(0);
            int index = 0;
            foreach (var sym in font.Symbols)
            {

                characters.Add(new CharacterItem(sym.Key, font.CurrentEncoding, sym.Value.ToBitmapSource(palette),font));
                index++;
            }
            Characters = new ObservableCollection<CharacterItem>(characters);
            InitializeComponent();
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (!ShouldSave)
            {
                return;
            }
            var result = MessageBox.Show("SaveChanges?", "", MessageBoxButton.YesNoCancel);
            if(result == MessageBoxResult.Yes)
            {
                SaveButton_Click(sender, null);
                return;
            }

            e.Cancel = result == MessageBoxResult.Cancel;
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            var syms = new List<int>();
            foreach (var item in Characters)
            {
                syms.Add(item.Code);
            }
            var dict = font.Symbols.Where(x => syms.Contains(x.Key)).ToDictionary(x => x.Key, x => x.Value);
            font.Symbols = new SortedDictionary<char, Sym>(dict);
        }

        private void DeleteCommandBinding_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            var length = CharactersList.SelectedItems.Count;
            var index = CharactersList.SelectedIndex;
            for(var i = 0;i < length; i++)
            {
                Characters.RemoveAt(index);
            }
            CharactersList.SelectedIndex = index;
        }

        private void DeleteCommandBinding_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = CharactersList.SelectedIndex != -1;
        }
    }
}
