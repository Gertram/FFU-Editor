using System.Windows;
using System.Windows.Controls;
using System.ComponentModel;
using MahApps.Metro.Controls;

namespace FFUEditor
{
    /// <summary>
    /// Логика взаимодействия для SymbolsListWindow.xaml
    /// </summary>
    public partial class ModifiableCharactersWindow : MetroWindow, INotifyPropertyChanged
    {
        public ModifiableCharactersWindow()
        {
            InitializeComponent();
        }
        public string Characters
        {
            get
            {
                return CharactersTextBox.Text.Trim();
            }
            set
            {
                CharactersTextBox.Text = value;
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void CharactersTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Characters)));
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            //DialogResult = true;
            Close();
        }
        private void DefaultButton_Click(object sender, RoutedEventArgs e)
        {
            Characters = MainConfig.DefaultTemplate;
        }
    }
}
