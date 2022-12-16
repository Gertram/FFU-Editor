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
    public partial class SymbolsListForm : Form
    {
        private static Encoding Encoding = Encoding.GetEncoding("shift-jis");
        private DataTable dataTable;
        private FFU FFU;
        public SymbolsListForm(FFU font)
        {
            InitializeComponent();
            FFU = font;
            dataTable = new DataTable();
            dataTable.Columns.Add("Name", typeof(char));
            dataTable.Columns.Add("Code", typeof(int));
            dataTable.Columns.Add("Image", typeof(Image));
            var palitte = font.GetPalette(0);
            palitte[0] = Color.FromArgb(0xff, palitte[0]);
            foreach(var sym in font.Symbols)
            {
                dataTable.Rows.Add(ToChar(sym.Key),sym.Key,sym.Value.GetBitmap(palitte));
            }
            dataGridView1.RowTemplate.Height = font.Header.FontInfo.SymHeight;
            dataGridView1.DataSource = dataTable;
        }

        private void SymbolsListForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            var syms = new List<int>();
            foreach(DataRow row in dataTable.Rows)
            {
                syms.Add((int)row.ItemArray[1]);
            }
            var dict = FFU.Symbols.Where(x => syms.Contains(x.Key)).ToDictionary(x => x.Key, x => x.Value);
            FFU.Symbols = new SortedDictionary<int, Sym>(dict);
        }
        private static char ToChar(int num)
        {
            if (num < 256)
            {
                return Encoding.GetChars(new byte[] { (byte)(num) })[0];
            }
            return Encoding.GetChars(new byte[] { (byte)(num >> 8), (byte)(num & 0xff) })[0];
        }
    }
}
