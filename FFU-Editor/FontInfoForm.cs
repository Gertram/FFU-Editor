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
    public partial class FontInfoForm : Form
    {
        private FFU FFU { get; set; }
        private FontInfo FontInfo { get; set; }
        public FontInfoForm(FFU font)
        {
            InitializeComponent();
            FFU = font;
            FontInfo = font.Header.FontInfo;
            if(FontInfo.Codek == CodekType.COLOR8)
            {
                CodekComboBox.SelectedIndex = 0;
            }
            else if(FontInfo.Codek == CodekType.COLOR16)
            {
                CodekComboBox.SelectedIndex = 1;
            }
            else
            {
                throw new NotImplementedException();
            }
            WidthTextBox.Text = FontInfo.SymWidth.ToString("X");
            HeightTextBox.Text = FontInfo.SymHeight.ToString("X");
            if (FontInfo.LittleEndian)
            {
                EndingTextBox.Text = "LittleEndian";
            }
            else
            {
                EndingTextBox.Text = "BigEndian";
            }
            PlusScaleTextBox.Text = FontInfo.ScalePlus.ToString("X");
            MinusScaleTextBox.Value = FontInfo.ScaleMinus;
        }

        private void SaveButton_Click(object sender, EventArgs e)
        {
            try
            {
                FontInfo.ScaleMinus = (byte)MinusScaleTextBox.Value;
                if(CodekComboBox.SelectedIndex == 0)
                {
                    if(FontInfo.Codek != CodekType.COLOR8)
                    {

                    }
                    FontInfo.Codek = CodekType.COLOR8;
                }
                else if(CodekComboBox.SelectedIndex == 1)
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
