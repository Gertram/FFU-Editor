using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Drawing;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
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
            var root = new XElement("font");
            var doc = new XDocument(root);
            var info = new XElement("info");
            info.SetAttributeValue("face", "Desyrel");
            info.SetAttributeValue("size", symHeight);
            info.SetAttributeValue("charset", "unicode");
            var common = new XElement("common");
            common.SetAttributeValue("lineHeight", symHeight);
            common.SetAttributeValue("base", symHeight);
            common.SetAttributeValue("scaleW", symWidth);
            common.SetAttributeValue("scaleH", symHeight);
            common.SetAttributeValue("pages", 1);
            var pages = new XElement("pages");
            var page = new XElement("page");
            page.SetAttributeValue("id", 0);
            page.SetAttributeValue("file", Path.Combine(Path.GetDirectoryName(filename),Path.ChangeExtension(filename,"png")));
            pages.Add(page);
            var chars = new XElement("chars");
            chars.SetAttributeValue("count", syms.Length);
            doc.Root.Add(info);
            doc.Root.Add(common);
            doc.Root.Add(pages);
            var palette = new XElement("palette");
            var index = 0;
            if (FFUFont.Pallettes != null)
            {
                foreach (var item in FFUFont.Pallettes)
                {
                    foreach (var color in item)
                    {
                        var temp = new XElement("color");
                        temp.SetAttributeValue("id", index);
                        temp.SetAttributeValue("ARGB", color.ToArgb().ToString("X"));
                        palette.Add(temp);
                        index++;
                    }
                }
                doc.Root.Add(palette);
            }
            var num = 0;
            foreach (var sym in syms)
            {
                var temp = new XElement("char");
                var letter = ToChar(sym.Key);
                var swidth = sym.Value.Width;
                var padLeft = (symWidth - swidth) / 2;
                temp.SetAttributeValue("id", (int)letter);
                temp.SetAttributeValue("x", (num % symInRow) * symWidth + padLeft);
                temp.SetAttributeValue("y", num / symInRow * symHeight);
                temp.SetAttributeValue("width", swidth);
                temp.SetAttributeValue("height", symHeight);
                temp.SetAttributeValue("x_offset", 0);
                temp.SetAttributeValue("y_offset", 0);
                temp.SetAttributeValue("xadvance", swidth);
                chars.Add(temp);
                num++;
            }
            doc.Root.Add(chars);
            doc.Save(filename);
        }
        private static char ToChar(int num)
        {
            if (num < 256)
            {
                return encoding.GetChars(new byte[] { (byte)(num) })[0];
            }
            return encoding.GetChars(new byte[] { (byte)(num >> 8), (byte)(num & 0xff) })[0];
        }

        internal class FontInfo
        {
            public int Width { get; set; }
            public int Height { get; set; }
            public string FileName { get; set; }

            public Color[][] Palettes { get; set; }
            public SortedDictionary<char,Rectangle> Symbols { get; set; }
        }

        private static int GetIntValue(XElement element,string name)
        {
            var value = GetValue(element, name);
            return int.Parse(value);
        }
        private static string GetValue(XElement element,string name)
        {
            var attr = element.Attribute(name);
            if (attr == null)
            {
                throw new Exception($"Attribute {name} in {element.Name.LocalName} not found");
            }
            return attr.Value;
        }
        private static XElement GetChild(XElement element,string name)
        {
            var temp = element.Element(name);
            if (temp == null)
            {
                throw new Exception($"\"{name}\" element not found");
            }
            return temp;
        }
        internal static FontInfo Import(string filename)
        {
            var doc = XDocument.Load(filename);

            if (!string.Equals(doc.Root.Name.LocalName, "font", StringComparison.OrdinalIgnoreCase))
            {
                throw new Exception("Root element must be \"font\"");
            }
            var root = doc.Root;
            var info = GetChild(root,"info");
            var common = GetChild(root, "common");
            var fontInfo = new FontInfo();
            fontInfo.Width = GetIntValue(common, "scaleW");
            fontInfo.Height = GetIntValue(common, "scaleH");
            var pageCount = GetIntValue(common, "pages");
            var pages = GetChild(root, "pages");
            var page = GetChild(pages, "page");
            var file = GetValue(page, "file");
            var path = Path.Combine(Path.GetDirectoryName(filename), file);
            if (!File.Exists(path))
            {
                path = file;
            }
            fontInfo.FileName = path;
            var chars = GetChild(root, "chars");
            var symbols = new Dictionary<char, Rectangle>();
            var palette = root.Element("palette");
            if(palette != null)
            {
                var palettes = new Color[16][];
                for(var i = 0;i < palettes.Length; i++)
                {
                    palettes[i] = new Color[16];
                }
                foreach(var element in palette.Elements("color"))
                {
                    var index = GetIntValue(element, "id");
                    var collectionIndex = index / 16;
                    var colorIndex = index % 16;
                    palettes[collectionIndex][colorIndex] = Color.FromArgb(int.Parse(element.Attribute("ARGB").Value,System.Globalization.NumberStyles.HexNumber));
                }
                fontInfo.Palettes = palettes;
            }

            foreach(var element in chars.Elements("char"))
            {
                var sym = (char)GetIntValue(element, "id");
                var x = GetIntValue(element, "x");
                var y = GetIntValue(element, "y");
                var width = GetIntValue(element, "width");
                var height = GetIntValue(element, "height");
                var xAttribute = element.Attribute("xadvance");
                if (xAttribute.Value != null && int.TryParse(xAttribute.Value,out var xadvance))
                {
                    width = xadvance;
                }
                var yAttribute = element.Attribute("xadvance");
                if (yAttribute.Value != null && int.TryParse(yAttribute.Value,out var yadvance))
                {
                    width = yadvance;
                }
                var rect = new Rectangle(x,y,width,height);
                symbols.Add(sym, rect);
            }
            fontInfo.Symbols = new SortedDictionary<char, Rectangle>(symbols);
            return fontInfo;
        }
        
    }
}
