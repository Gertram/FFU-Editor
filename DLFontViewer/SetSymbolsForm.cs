using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FFU_Editor
{
    public partial class SetSymbolsForm : Form,INotifyPropertyChanged
    {
        public SetSymbolsForm()
        {
            InitializeComponent();
        }

        public string Symbols { get => SymbolsRichTextBox.Text.Trim(); set => SymbolsRichTextBox.Text = value; }

        public event PropertyChangedEventHandler PropertyChanged;

        private void SymbolsRichTextBox_TextChanged(object sender, EventArgs e)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Symbols)));
        }

        private void SaveButton_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.OK;
        }

        private void DefaultButton_Click(object sender, EventArgs e)
        {
            Symbols = MainConfig.DefaultTemplate;
        }
    }
}
