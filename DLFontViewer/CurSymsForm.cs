using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DLFontViewer
{
    public partial class CutSymsForm : Form
    {
        public CutSymsForm()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if(numericUpDown1.Value == 0)
            {
                return;
            }
            Value = numericUpDown1.Value;
            DialogResult = DialogResult.OK;
        }
        public decimal Value { get;private set; }  = 0;
    }
}
