using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using FFULibrary;
using Forms = System.Windows.Forms;
using Hjg.Pngcs;
using System.Runtime.InteropServices;

namespace FFUEditor
{
    internal class Tools
    {
        public Tools(FFU fFUFont)
        {
            FFUFont = fFUFont;
        }

        private FFU FFUFont { get; set; }

        private void ImportPadding(FFU tempFont)
        {
            float k = 0.6f;// (float)FFUFont.Header.FontInfo.SymHeight / tempFont.Header.FontInfo.SymHeight;
            foreach (var pair in FFUFont.Symbols)
            {
                if (tempFont.Symbols.TryGetValue(pair.Key, out var sym))
                {
                    var temp = new SymWrap(sym);
                    var leftBorder = temp.FindLeftPosition();
                    var rightPosition = temp.FindRightPosition();
                    if (rightPosition == -1 || leftBorder == -1)
                    {
                        var width = (int)(sym.Width * k);
                        if(width % 2 == 0)
                        {
                            width++;
                        }
                        var t = new Sym(width, pair.Value.Height);
                        pair.Value.Raw = t.Raw;
                        pair.Value.Width = t.Width;
                        Console.WriteLine();
                        continue;
                    }
                    //var topBorder = temp.FindTopPosition();
                    //var bottomPosition = temp.FindBottomPosition();
                    //var bottomBorder = temp.Sym.Height - bottomPosition - 1;
                    //temp = new SymWrap(pair.Value);
                    //temp.SetVerticalPadding(topBorder, bottomBorder);
                    var rightBorder = temp.Sym.Width - rightPosition - 1;
                    temp = new SymWrap(pair.Value);
                    leftBorder = (int)(leftBorder * k);
                    rightBorder = (int)(rightBorder * k);

                    temp.SetHorizontalPadding(leftBorder, rightBorder);
                    if (temp.Sym.Width % 2 != 0)
                    {
                        temp.AddPadding(0, 0, 1, 0);
                    }
                }
            }
        }
        private void ImportPadding(int start, int end, FFU tempFont)
        {
            //float ratio = (float)FFUFont.Header.FontInfo.SymWidth / tempFont.Header.FontInfo.SymWidth;
            foreach (var sym in FFUFont.Symbols.Where(x => x.Key >= start && x.Key <= end))
            {
                var temp = new SymWrap(tempFont.Symbols[sym.Key]);
                var leftBorder = temp.FindLeftPosition();
                var rightPosition = temp.FindRightPosition();
                if (rightPosition == -1 || leftBorder == -1)
                {
                    Console.WriteLine();
                    continue;
                }
                var rightBorder = temp.Sym.Width - rightPosition - 1;
                temp = new SymWrap(sym.Value);
                temp.SetHorizontalPadding(leftBorder, rightBorder);
            }
        }
        private void SetPadding(char start, char end, int border = 1)
        {
            foreach (var sym in FFUFont.Symbols.Where(x => x.Key >= start && x.Key <= end))
            {
                var temp = new SymWrap(sym.Value);
                temp.SetHorizontalPadding(border, border);
            }
        }

        private static void AddStroke(Sym sym, int width, int height, byte color)
        {
            var newImage = new Sym(new byte[width*height],width,height);
            for (var y = 0; y < height; y++)
            {
                for (var x = 0; x < width; x++)
                {
                    var pixel = sym.GetPixel(x, y);
                    if (pixel != 0)
                    {
                        newImage.SetPixel(x, y,pixel);
                        continue;
                    }

                    if (x + 1 < width - 1 && sym.GetPixel(x + 1, y) != 0
                        || x - 1 > 0 && sym.GetPixel(x - 1, y) != 0
                        || y + 1 < height - 1 && sym.GetPixel(x, y + 1) != 0
                        || y - 1 > 0 && sym.GetPixel(x, y - 1) != 0)
                    {
                        newImage.SetPixel(x, y,color);
                    }
                    else
                    {
                        newImage.SetPixel(x, y,pixel);
                    }
                }
            }
            sym.Raw = newImage.Raw;
        }

        private void AddStroke(int start, int end, byte[] colors)
        {
            foreach (var item in FFUFont.Symbols.Where(x => x.Key >= start && x.Key <= end))
            {
                var sym = item.Value;
                foreach (var color in colors)
                {
                    AddStroke(sym, sym.Width, sym.Height, color);
                }
            }
        }

        private void ExpandPadding(int start, int end, int padding)
        {
            foreach (var sym in FFUFont.Symbols.Where(x => x.Key >= start && x.Key <= end))
            {
                var temp = new SymWrap(sym.Value);
                temp.AddPadding(0, padding, 0, padding);
            }
        }
        internal void ImportPadding()
        {
            var ofd = new Forms.OpenFileDialog();
            if (ofd.ShowDialog() != Forms.DialogResult.OK)
            {
                return;
            }
            var tempFont = new FFU(ofd.FileName);
            ImportPadding(tempFont);
            //ImportPadding(0x8440, 0x8460, tempFont);
            //ImportPadding(0x8470, 0x8491, tempFont);

            //ImportPadding(0x8141, 0x8258, tempFont);
            //ImportPadding(0x21, 255, tempFont);

            //ImportPadding(0x8260, 0x8279, tempFont);
            //ImportPadding(0x8281, 0x829a, tempFont);
            //ImportPadding(0x829f, 0x8396, tempFont);
        }

        internal void ExpandPadding()
        {
            var form = new SetPaddingWindow();
            if (!(bool)form.ShowDialog())
            {
                return;
            }
            var padding = form.Value;

            ExpandPadding(0, 255, padding);

            ExpandPadding(0x8260, 0x8279, padding);
            ExpandPadding(0x8281, 0x829a, padding);

            ExpandPadding(0x8440, 0x8460, padding);
            ExpandPadding(0x8470, 0x8491, padding);
            //ExpandPadding(0, FFUFont.Symbols.Keys.Last()+1, padding);
            //ExpandPadding(0, 255, padding);

            //ExpandPadding(0x8260, 0x8279, padding);
            //ExpandPadding(0x8281, 0x829a, padding);
            //ExpandPadding(0x829f, 0x8396, padding);

            //ExpandPadding(0x8440, 0x8460, padding);
            //ExpandPadding(0x8470, 0x8491, padding);
            //const char ch = '・';
            //const int end = 0x875d;
            //const int start = 0xf040;
            //var encoding = Encoding.GetEncoding("shift-jis");
            //var font = new FFU("D:\\Diabolik lovers\\Work\\DL\\font\\0");
            //var index = start;
            //int count = 0;
            //using (var writer = new StreamWriter("log.txt"))
            //{
            //    foreach (var sym in font.Symbols.Where(x => x.Key <= end))
            //    {
            //        while (encoding.GetChars(new byte[] { (byte)((index & 0xff00) >> 8), (byte)(index & 0xff) })[0] == ch)
            //        {
            //            index++;
            //        }
            //        char value = '\0';
            //        if (sym.Key < 0x80 || sym.Key > 0xa0 && sym.Key < 0xe0)
            //        {
            //            value = encoding.GetChars(new byte[] { (byte)(sym.Key & 0xff) })[0];
            //        }
            //        else
            //        {
            //            value = encoding.GetChars(new byte[] { (byte)((sym.Key & 0xff00) >> 8), (byte)(sym.Key & 0xff) })[0];
            //        }
            //        FFUFont.Symbols[index] = new Sym(sym.Value.Image);
            //        writer.WriteLine($"'{value}'={index:X}");
            //        index++;
            //        count++;
            //    }
            //}
        }

        internal void SetPadding()
        {
            float k = 0.6f;
            var s = "ＡＢＣＤＥＦＧＨＩＪＫＬＭＮＯＰＱＲＳＴＵＶＷＸＹＺａｂｃｄｅｆｇｈｉｊｋｌｍｎｏｐｑｒｓｔｕｖｗｘｙｚ";
            //foreach (var pair in FFUFont.Symbols.Where(x=> s.Contains(x.Key)))
            foreach (var pair in FFUFont.Symbols)
            {
                var wrap = new SymWrap(pair.Value);
                var left = wrap.FindLeftPosition();
                if(left < 0)
                {
                    continue;
                }
                var right = wrap.RightPadding();
                var internal_width = wrap.Sym.Width - right - left;
                left = (int)Math.Round(left * k);
                right = (int)Math.Round(right * k);
                if((internal_width + left+right) % 2 == 1)
                {
                    right++;
                }
                wrap.SetHorizontalPadding(left, right);
                
            }

            return;


            var s2 = "｡｢｣､･ｦｧｨｩｪｫｬｭｮｯｰｱｲｳｴｵｶｷｸｹｺｻｼｽｾｿﾀﾁﾂﾃﾄﾅﾆﾇﾈﾉﾊﾋﾌﾍﾎﾏﾐﾑﾒﾓﾔﾕﾖﾗﾘﾙﾚﾛﾜﾝﾞ";
            SetPadding('｡', 'ﾞ',1);
            return;
            //foreach(var sym in FFUFont.Symbols.Where(x=>x.Key > 0x8260 && x.Key < 0x829b))
            //{
            //    var wrap = new SymWrap(sym.Value);
            //    var leftPadding = wrap.LeftPadding();
            //    var rightPadding = wrap.RightPadding();
            //    if(leftPadding == -1 || rightPadding == -1)
            //    {
            //        continue;
            //    }
            //    if(sym.Value.Width < 22)
            //    {
            //        int left = (22 - sym.Value.Width)/2;
            //        var right = 22 - sym.Value.Width - left;

            //        wrap.AddPadding(left,0, right,0);
            //    }
            //}
            //int start = 0x8260;
            //foreach(var sym in FFUFont.Symbols.Where(x=>x.Key >= 'A' && x.Key <= 'Z'))
            //{
            //    FFUFont.Symbols[start].Image = sym.Value.Image.Clone() as byte[,];
            //    start++;
            //}
            //start = 0x8281;
            //foreach(var sym in FFUFont.Symbols.Where(x=>x.Key >= 'a' && x.Key <= 'z'))
            //{
            //    FFUFont.Symbols[start].Image = sym.Value.Image.Clone() as byte[,];
            //    start++;
            //}
            //FFUFont.Symbols[0x8143].Image = FFUFont.Symbols[','].Image;
            //FFUFont.Symbols[0x8144].Image = FFUFont.Symbols['.'].Image;
            //FFUFont.Symbols[0x8146].Image = FFUFont.Symbols[':'].Image;
            //FFUFont.Symbols[0x8147].Image = FFUFont.Symbols[';'].Image;
            //FFUFont.Symbols[0x8148].Image = FFUFont.Symbols['?'].Image;
            //FFUFont.Symbols[0x8149].Image = FFUFont.Symbols['!'].Image;
            //return;
            var form = new SetPaddingWindow();
            if (!(bool)form.ShowDialog())
            {
                return;
            }
            var padding = form.Value;
            //SetPadding(0, 255, padding);

            //SetPadding(0x8260, 0x8279, padding);
            //SetPadding(0x8281, 0x829a, padding);

            //SetPadding(0x8440, 0x8460, padding);
            //SetPadding(0x8470, 0x8491, padding);
        }

        internal void AddStroke()
        {
            int i = 0;
            var palette = FFUFont.GetPalette(0);
            byte black = 0xff;
            //for(byte j = 0; j < palette.Length; j++)
            //{
            //    var color = palette[j];
            //    if(color.A == 0xff && color.R == 0 && color.G == 0 && color.B == 0)
            //    {
            //        black = j;
            //        break;
            //    }
            //}
            //if(black == -1)
            //{
            //    return;
            //}
            var colors = new byte[] { 5, 2 };
            AddStroke(0x8141, 0x8258, colors);
            AddStroke(0, 255, colors);

            AddStroke(0x8260, 0x8279, colors);
            AddStroke(0x8281, 0x829a, colors);
            AddStroke(0x829f, 0x8396, colors);

            AddStroke(0x8440, 0x8460, colors);
            AddStroke(0x8470, 0x8491, colors);
        }

        internal void GenerateFont()
        {
            using(var stream = File.OpenRead(@"D:/__advfont29x37_new.png"))
            {
                var reader = new PngReader(stream);
                var info = reader.ImgInfo;
                var width = 51;
                var symWidth = 50;
                var height = 57;
                var symCount = 13 * 13;
                var symLength = symWidth * height;
                var data = new byte[symCount][];
                for(var i = 0;i < data.Length; i++)
                {
                    data[i] = new byte[symLength];
                }
                for(var i = 0;i < info.Rows; i++)
                {
                    var line = reader.ReadRowByte(i);
                    line = line.unpackToNewImageLine();
                    var bytes = line.ScanlineB;
                    var y = i / height;
                    for(var j = 0;j < 13; j++)
                    {
                        var cellNumber = y * 13 + j;
                        var srcOffset = j * width;
                        var dstOffset = (i % height) * symWidth;
                        Buffer.BlockCopy(bytes, srcOffset, data[cellNumber], dstOffset, symWidth);
                    }
                }
                var keys = FFUFont.Symbols.Keys.ToArray();
                for(var i = 0;i < keys.Length;i++) 
                {
                    var sym = FFUFont.Symbols[keys[i]];
                    sym.Raw = data[i];
                    sym.Width = symWidth;
                    sym.Height = height;
                }
                //using(var writer = new  StreamWriter("temp.txt")) 
                //{
                //    foreach(var item in data)
                //    {
                //        for (int y = 0, offset = 0; y < height; y++)
                //        {
                //            for (int x = 0; x < width; x++,offset++)
                //            {
                //                writer.Write(item[offset]);
                //            }
                //            writer.WriteLine();
                //        }
                //        writer.WriteLine();
                //    }
                //}
            }
            //try
            //    {
            //        var win = new GenerateFontWindow();
            //        if (!(bool)win.ShowDialog())
            //        {
            //            return;
            //        }

            //        foreach (var item in win.Syms)
            //        {
            //            var sym = ConvertToSym(item.Key, item.Value, win.Colors);


            //            var key = ToInt(item.Key);
            //            FFUFont.Symbols[key] = sym;
            //        }
            //        ShowTemplateImage();
            //    }
            //    catch (Exception ex)
            //    {
            //        ShowError("GenerateFont", ex);
            //    }
        }
        internal void CopyRangeFromFile()
        {
            var regex = new Regex(@"(?<old>.+)=(?<new>.+)");
            //var lines = System.IO.File.ReadAllLines(@"D:\Work\PyCharmProjects\DLSymTableCompare\output.txt");
            var lines = System.IO.File.ReadAllLines(@"D:\Work\VisualStudio\EBOOTStringRedactor\EBOOTStringRedactor\bin\Debug\encoding_config.txt");
            foreach (var line in lines)
            {
                var match = regex.Match(line);
                if (!match.Success)
                {
                    continue;
                }
                var old = match.Groups["old"].Value;
                var _new = match.Groups["new"].Value;
                //var strs = _new.Split(',');
                //var old_code = int.Parse(old, System.Globalization.NumberStyles.HexNumber);
                //var new_codes = strs.Select(x => int.Parse(x, System.Globalization.NumberStyles.HexNumber)).ToList();
                //if (!FFUFont.Symbols.TryGetValue(old_code, out var character))
                //{
                //    continue;
                //}
                //foreach (var code in new_codes)
                //{
                //    FFUFont.Symbols[code] = new Sym(character.Raw, character.Width, character.Height);
                //}
                var old_code = old[0];
                var new_code = _new[0];
                if(FFUFont.Symbols.TryGetValue(new_code, out var sym))
                {
                    FFUFont.Symbols[old_code] = sym;
                }
            }
        }
        private char ToChar(int num)
        {
            var bytes = BitConverter.GetBytes(num).Reverse().SkipWhile(x => x == '\0').ToArray();
            return FFUFont.CurrentEncoding.GetChars(bytes)[0];
        }
        internal void CopyRange()
        {
            //var s = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz!?,.‘’“”'\"";
            var s = "абвгдеёжзийклмнопрстуфхцчшщъыьэюяАБВГДЕЁЖЗИЙКЛМНОПРСТУФХЦЧШЩЪЫЬЭЮЯ";
            //var s2 = "｡｢｣､･ｦｧｨｩｪｫｬｭｮｯｰｱｲｳｴｵｶｷｸｹｺｻｼｽｾｿﾀﾁﾂﾃﾄﾅﾆﾇﾈﾉﾊﾋﾌﾍﾎﾏﾐﾑﾒﾓﾔﾕﾖﾗﾘﾙﾚﾛﾜﾝﾞ";
            //var input = "You’re the weird one, Reiji-san";
            //var output = "";
            //foreach(var c in input)
            //{
            //    var pos = s.IndexOf(c);
            //    if (pos == -1)
            //    {
            //        output += c;
            //    }
            //    else
            //    {
            //        output += s2[pos];
            //    }
            //}

            //return;
            //max offset 0xdf
            int offset = 0xa1;
            var encoding = Encoding.GetEncoding("Shift-jis");
            var symbols = FFUFont.Symbols.ToDictionary(x=>x.Key,x=>x.Value);
            var dict = new Dictionary<char, char>();
            
            foreach (var sym in symbols.Where(x => s.Contains(x.Key)))
            {
                var characters = encoding.GetChars(new byte[] { (byte)offset });
                var character = characters[0];
                dict.Add(sym.Key, character);
                //FFUFont.Symbols[character] = new Sym(sym.Value.Raw,sym.Value.Width,sym.Value.Height);
                offset++;
            }

            var input = "You’re the weird one, Reiji-san";
            var output = "";
            foreach (var c in input)
            {
                if(dict.TryGetValue(c,out var c2))
                {
                    output += c2;
                }
                else
                {
                    output += c;
                }
            }

            return;
            //var s1 = "ＡＢＣＤＥＦＧＨＩＪＫＬＭＮＯＰＱＲＳＴＵＶＷＸＹＺａｂｃｄｅｆｇｈｉｊｋｌｍｎｏｐｑｒｓｔｕｖｗｘｙｚ１２３４５６７８９０";
            //var s2 = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz1234567890";
            //for (int i = 0; i < s2.Length; i++)
            //{
            //    FFUFont.Symbols[s1[i]] = FFUFont.Symbols[s2[i]];
            //}
            //return;

            //const char ch = '・';
            //const int end = 0x875d;
            //var encoding = Encoding.GetEncoding("shift-jis");
            //const int start = 0xf040;
            //var symbols = new SortedDictionary<char,Sym>(FFUFont.Symbols);
            //var index = start;
            //using (var writer = new StreamWriter("log.txt"))
            //{
            //    foreach (var sym in symbols.Where(x => x.Key <= end))
            //    {
            //        while (encoding.GetChars(new byte[] { (byte)((index & 0xff00) >> 8), (byte)(index & 0xff) })[0] == ch)
            //        {
            //            index++;
            //        }
            //        char value = '\0';
            //        if (sym.Key < 0x80 || sym.Key > 0xa0 && sym.Key < 0xe0)
            //        {
            //            value = encoding.GetChars(new byte[] { (byte)(sym.Key & 0xff) })[0];
            //        }
            //        else
            //        {
            //            value = encoding.GetChars(new byte[] { (byte)((sym.Key & 0xff00) >> 8), (byte)(sym.Key & 0xff) })[0];
            //        }
            //        //var wrap = new SymWrap(sym.Value);
            //        //var wrap2 = new SymWrap(FFUFont.Symbols[index]);
            //        //wrap2.SetPadding(wrap.LeftPadding(), wrap.TopPadding(), wrap.RightPadding(), wrap.BottomPadding());
            //        FFUFont.Symbols[index] = new Sym(sym.Value.Raw, sym.Value.Width, sym.Value.Height);
            //        writer.WriteLine($"'{value}'={index:X}");
            //        index++;
            //    }
            //}
            //for (var i = 0x3040; i < 0x306F; i++)
            //{
            //    char ch = (char)i;
            //    if (FFUFont.Symbols.ContainsKey(ch))
            //    {
            //        FFUFont.Symbols.Remove(ch);
            //    }
            //}
            //return;
            //string num = "3";
            var alpha = "абвгдеёжзийклмнопрстуфхцчшщъыьэюяАБВГДЕЁЖЗИЙКЛМНОПРСТУФХЦЧШЩЪЫЬЭЮЯ";
            var font = new FFU("D:\\Downloads\\advfont29x37.ffu");
            foreach(var ch in font.Symbols.Keys)
            {
                if (!alpha.Contains(ch))
                {
                    continue;
                }
                //if (ch < 0x306f || ch > 0x309F)
                //{
                //    continue;
                //}
                //if(ch < 0x30A0 || ch > 0x30FF)
                //{
                //    continue;
                //}
                //if (ch >= 0x4E00 && ch <= 0x9FBF)
                //{
                //    continue;
                //}
                if (!FFUFont.Symbols.ContainsKey(ch)&& font.Symbols.TryGetValue(ch,out var sym))
                {
                    //if (sym.Height < FFUFont.Header.FontInfo.SymHeight)
                    //{
                    //    var wrap = new SymWrap(sym);
                    //    var padding = FFUFont.Header.FontInfo.SymHeight - sym.Height;

                    //    if (padding % 2 == 0)
                    //    {
                    //        wrap.AddPadding(0, padding / 2);
                    //    }
                    //    else
                    //    {
                    //        var topPadding = padding / 2 + 1;
                    //        var bottomPadding = padding - topPadding;
                    //        wrap.AddPadding(0, topPadding, 0, bottomPadding);
                    //    }
                    //}
                    //if (sym.Width < FFUFont.Header.FontInfo.SymWidth)
                    //{
                    //    var wrap = new SymWrap(sym);
                    //    var padding = FFUFont.Header.FontInfo.SymWidth - sym.Width;
                    //    if(padding % 2 == 0)
                    //    {
                    //        wrap.AddPadding(padding / 2,0);
                    //    }
                    //    else
                    //    {
                    //        var leftPadding = padding / 2+1;
                    //        var rightPadding = padding - leftPadding;
                    //        wrap.AddPadding(leftPadding, 0, rightPadding, 0);
                    //    }
                    //}
                    FFUFont.Symbols.Add(ch, sym);
                }
            }

            //const char ch = '・';
            //const int end = 0x875d;
            //const int start = 0xf040;
            //var encoding = Encoding.GetEncoding("shift-jis");
            //var font = new FFU("D:\\Diabolik lovers\\Work\\DL\\font\\0");
            //var index = start;
            //int count = 0;
            //using (var writer = new StreamWriter("log.txt"))
            //{
            //    foreach (var sym in font.Symbols.Where(x => x.Key <= end))
            //    {
            //        while (encoding.GetChars(new byte[] { (byte)((index & 0xff00) >> 8), (byte)(index & 0xff) })[0] == ch)
            //        {
            //            index++;
            //        }
            //        char value = '\0';
            //        if (sym.Key < 0x80 || sym.Key > 0xa0 && sym.Key < 0xe0)
            //        {
            //            value = encoding.GetChars(new byte[] { (byte)(sym.Key & 0xff) })[0];
            //        }
            //        else
            //        {
            //            value = encoding.GetChars(new byte[] { (byte)((sym.Key & 0xff00) >> 8), (byte)(sym.Key & 0xff) })[0];
            //        }
            //        FFUFont.Symbols[index] = new Sym(sym.Value.Image);
            //        writer.WriteLine($"'{value}'={index:X}");
            //        index++;
            //        count++;
            //    }
            //}
            //return;
            //a1-df
            //63
            //int offset = 0xa1;
            //foreach (var sym in FFUFont.Symbols.Where(x => x.Key >= 0x8440 && x.Key <= 0x845a))
            //{
            //    FFUFont.Symbols[offset].Raw = sym.Value.Raw;
            //    offset++;
            //}
            //foreach (var sym in FFUFont.Symbols.Where(x => x.Key >= 0x845e && x.Key <= 0x8460))
            //{
            //    FFUFont.Symbols[offset].Raw = sym.Value.Raw;
            //    offset++;
            //}
            //foreach (var sym in FFUFont.Symbols.Where(x => x.Key >= 0x8470 && x.Key <= 0x8491))
            //{
            //    FFUFont.Symbols[offset].Raw = sym.Value.Raw;
            //    offset++;
            //}
        }
        internal void MoveRange()
        {
            CopyRangeFromFile();
            //var regex = new Regex(@"(?<old>.+)=(?<new>.+)");
            //var lines = System.IO.File.ReadAllLines(@"D:\Work\PyCharmProjects\DLSymTableCompare\symtable.txt");
            //var font = new FFU("D:\\Diabolik lovers\\Work\\PS Vita\\font\\advfont24x30 — копия (2).ffu");
            //foreach (var line in lines)
            //{
            //    var match = regex.Match(line);
            //    if (!match.Success)
            //    {
            //        continue;
            //    }
            //    var _new = match.Groups["new"].Value;
            //    var code = int.Parse(_new, System.Globalization.NumberStyles.HexNumber);
            //    FFUFont.Symbols[code] = font.Symbols[code];
            //}
        }
    }
}
