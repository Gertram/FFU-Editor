using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using FFULibrary;

namespace FFU_Editor
{
    public partial class FontInfoNew : Form
    {
        private FFU FFU { get; set; }
        private FontInfo FontInfo { get; set; }
        public FontInfoNew(FFU font)
        {
            InitializeComponent();
            FFU = font;
            FontInfo = font.Header.FontInfo;
            if (FontInfo.Codek == CodekType.COLOR8)
            {
                CodekComboBox.SelectedIndex = 0;
            }
            else if (FontInfo.Codek == CodekType.COLOR16)
            {
                CodekComboBox.SelectedIndex = 1;
            }
            else
            {
                throw new NotImplementedException();
            }
            SymWidthTextBox.Value = FontInfo.SymWidth;
            SymHeightTextBox.Value = FontInfo.SymHeight;
            if (FontInfo.LittleEndian)
            {
                EndingTextBox.Text = "LittleEndian";
            }
            else
            {
                EndingTextBox.Text = "BigEndian";
            }
            PlusScaleTextBox.Text = FontInfo.ScalePlus.ToString("X");
            LineHeightTextBox.Value = FontInfo.LineHeight;
        }

        private void SaveButton_Click(object sender, EventArgs e)
        {
            try
            {
                FontInfo.SymHeight = (byte)SymHeightTextBox.Value;
                FontInfo.SymWidth = (byte)SymWidthTextBox.Value;
                FontInfo.LineHeight = (byte)LineHeightTextBox.Value;
                if (CodekComboBox.SelectedIndex == 0)
                {
                    if (FontInfo.Codek != CodekType.COLOR8)
                    {

                    }
                    FontInfo.Codek = CodekType.COLOR8;
                }
                else if (CodekComboBox.SelectedIndex == 1)
                {
                    if (FontInfo.Codek != CodekType.COLOR16)
                    {

                    }
                    FontInfo.Codek = CodekType.COLOR16;
                }
                else
                {
                    throw new NotImplementedException();
                }
                DialogResult = DialogResult.OK;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
    }
}
