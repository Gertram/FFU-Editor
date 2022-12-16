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
    public partial class SetPaddingForm : Form
    {
        public SetPaddingForm()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if(numericUpDown1.Value == 0)
            {
                return;
            }
            Value = (int)numericUpDown1.Value;
            DialogResult = DialogResult.OK;
        }
        public int Value { get;private set; }  = 0;
    }
}
