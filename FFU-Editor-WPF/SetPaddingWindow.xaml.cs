using System;
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
using MahApps.Metro.Controls;

namespace FFUEditor
{
    /// <summary>
    /// Логика взаимодействия для SetPaddingWindow.xaml
    /// </summary>
    public partial class SetPaddingWindow : MetroWindow
    {
        public SetPaddingWindow()
        {
            InitializeComponent();
        }
        public int Value { get; private set; } = 0;

        private void OkButton_Click(object sender, RoutedEventArgs e)
        {
            if(!int.TryParse(ValueTextBox.Text,out var value) || value == 0)
            {
                return;
            }
            Value = value;
            DialogResult = true;
        }
    }
}
