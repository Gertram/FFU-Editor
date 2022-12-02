using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using FFULibrary;

namespace FFU_Editor
{
    internal static class XMLUtility
    {
        private static Encoding encoding = Encoding.GetEncoding("shift-jis");
        internal static void Export(FFU FFUFont,string filename)
        {
            var syms = FFUFont.Symbols.ToArray();
            var symInRow = (int)Math.Ceiling(Math.Sqrt(syms.Length));
            var rows = (int)Math.Ceiling((float)(syms.Length) / (float)(symInRow));
            var symWidth = FFUFont.Header.FontInfo.SymWidth;
            var symHeight = FFUFont.Header.FontInfo.SymHeight;
            var width = symInRow * symWidth;
            var height = rows * symHeight;
            using (var file = new StreamWriter(filename, false))
            {
                file.WriteLine("<font>");
                file.WriteLine($"<info face =\"Desyrel\" size=\"{symHeight}\" charset=\"unicode\" />");
                file.WriteLine($"<common lineHeight=\"{symHeight}\" base=\"{symHeight}\" scaleW=\"{width}\" scaleH=\"{height}\" pages=\"1\" />");
                file.WriteLine("<pages>");
                file.WriteLine("<page id=\"0\" file=\"export.png\" />");
                file.WriteLine("</pages >");
                file.WriteLine($"<chars count=\"{syms.Length}\" >");
                var num = 0;
                foreach (var sym in syms)
                {
                    var letter = ToChar(sym.Key);
                    var swidth = sym.Value.Width;
                    var padLeft = (symWidth - swidth) / 2;
                    var x = (num % symInRow) * symWidth + padLeft;
                    var y = num / symInRow * symHeight;
                    file.WriteLine($"<char id=\"{(int)letter}\" x=\"{x}\" y=\"{y}\" width=\"{swidth}\" height=\"{symHeight}\" xoffset=\"0\" yoffset=\"0\" xadvance=\"{swidth}\" />");
                    num++;
                }
                file.WriteLine("</chars>");
                file.WriteLine("</font>");
                file.Close();
            }
        }
        private static char ToChar(int num)
        {
            if (num < 256)
            {
                return encoding.GetChars(new byte[] { (byte)(num) })[0];
            }
            return encoding.GetChars(new byte[] { (byte)(num >> 8), (byte)(num & 0xff) })[0];
        }
    }
}
