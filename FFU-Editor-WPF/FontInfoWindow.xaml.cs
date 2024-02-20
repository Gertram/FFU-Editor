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
using FFULibrary;

namespace FFUEditor
{
    /// <summary>
    /// Логика взаимодействия для FontInfo.xaml
    /// </summary>
    public partial class FontInfoWindow : MetroWindow
    {
        private FFU FFU { get; set; }
        public FontInfo FontInfo { get; }
        public FontInfoWindow(FFU font)
        {
            FFU = font;
            var fontInfo = FFU.Header.FontInfo;
            FontInfo = fontInfo;
            InitializeComponent();
            if (fontInfo.Codek == CodekType.COLOR8)
            {
                CodekComboBox.SelectedIndex = 0;
            }
            else if (fontInfo.Codek == CodekType.COLOR16)
            {
                CodekComboBox.SelectedIndex = 1;
            }
            else
            {
                throw new NotImplementedException();
            }
            if (fontInfo.Encoding == FontEncoding.SHIFT_JIS)
            {
                EncodingComboBox.SelectedIndex = 0;
            }
            else if (fontInfo.Encoding == FontEncoding.UTF8)
            {
                EncodingComboBox.SelectedIndex = 1;
            }
            else
            {
                throw new NotImplementedException();
            }
        }

        private CodekType GetCodek()
        {
            if (CodekComboBox.SelectedIndex < 0)
            {
                throw new Exception();
            }
            if (CodekComboBox.SelectedIndex == 0)
            {
                return CodekType.COLOR8;
            }
            if (CodekComboBox.SelectedIndex == 1)
            {
                return CodekType.COLOR16;
            }
            throw new NotImplementedException();
        }
        private FontEncoding GetEncoding()
        {
            if (EncodingComboBox.SelectedIndex < 0)
            {
                throw new Exception();
            }
            if (EncodingComboBox.SelectedIndex == 0)
            {
                return FontEncoding.SHIFT_JIS;
            }
            if (EncodingComboBox.SelectedIndex == 1)
            {
                return FontEncoding.UTF8;
            }
            throw new NotImplementedException();
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void OkButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var codek = GetCodek();
                if(codek != FontInfo.Codek)
                {
                    if(codek == CodekType.COLOR16 &&  FontInfo.Codek == CodekType.COLOR8)
                    {
                        var result = MessageBox.Show(this.GetResourceString("m_QualityAttention"),this.GetResourceString("m_Attention"), MessageBoxButton.YesNo);
                        if(result != MessageBoxResult.Yes)
                        {
                            return;
                        }
                    }

                    FFU.ConvertTo(codek);
                }
                FFU.Header.FontInfo.Encoding = GetEncoding();
                Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
    }
}
