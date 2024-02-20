using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Threading.Tasks;
using FFULibrary;
using Hjg.Pngcs;

namespace FFUEditor
{
    internal static class PNGUtility
    {
        private static readonly Encoding encoding = Encoding.GetEncoding("shift-jis");
        internal static void Export(FFU font, string filename, int palttteIndex)
        {
            var symInRow = (int)Math.Ceiling(Math.Sqrt(font.Symbols.Count));
            var rows = (int)Math.Ceiling((float)(font.Symbols.Count) / symInRow);
            var ceilWidth = Math.Max(font.Header.FontInfo.SymWidth, font.Symbols.Values.Max(x => x.Width));
            var ceilHeight = Math.Max(font.Header.FontInfo.SymHeight, font.Symbols.Values.Max(x => x.Height));
            var width = symInRow * ceilWidth;
            var height = rows * ceilHeight;
            var info = new ImageInfo(width, height, 8, false, false, true);
            var chunk = new Hjg.Pngcs.Chunks.PngChunkPLTE(info);
            var colorCount = 16;
            if(font.Header.FontInfo.Codek == CodekType.COLOR8)
            {
                colorCount = 8;
            }
            var colors = new Color[colorCount];
            var palette = font.GetPalette(palttteIndex);
            for(var i = 0; i < palette.Length; i++)
            {
                var index = (byte)i;
                if (index != 0xff)
                {
                    colors[index] = palette[i];
                }
            }
            chunk.SetNentries(colors.Length);
            var tchunk = new Hjg.Pngcs.Chunks.PngChunkTRNS(info);
            var alpha = new int[colors.Length];
            for (var i = 0; i < colors.Length; i++)
            {
                var color = colors[i];
                alpha[i] = color.A;
                chunk.SetEntry(i, color.R, color.G, color.B);
            }
            tchunk.SetPalletteAlpha(alpha);

            using (var stream = File.OpenWrite(filename))
            {
                var writer = new PngWriter(stream, info);
                writer.GetChunksList().Queue(chunk);
                writer.GetChunksList().Queue(tchunk);
                writer.CompLevel = 9;
                WritePNG(writer, ceilWidth, ceilHeight, symInRow, width, rows, font);
            }
        }
        private static void WritePNG(PngWriter writer,int ceilWidth,int ceilHeight,int symInRow, int cols, int rows, FFU font)
        {
            var syms = font.Symbols.Select(x => x.Value).ToArray();
            var raw = new byte[cols];
            for (var j = 0; j < rows; j++)
            {
                for (var y = 0; y < ceilHeight; y++)
                {
                    for (var i = 0; i < symInRow && j * symInRow + i < syms.Length; i++)
                    {
                        var sym = syms[i + symInRow * j];
                        var start_y = (ceilHeight - sym.Height) / 2;
                        var end_y = start_y+sym.Height;
                        if(y < start_y || y >= end_y)
                        {
                            continue;
                        }
                        var padLeft = (ceilWidth - sym.Width) / 2;
                        var padRight = ceilWidth - sym.Width - padLeft;
                        var x_offset = i * ceilWidth;

                        for (var x = 0; x < padLeft; x++)
                        {
                            raw[x + x_offset] = 0;
                        }
                        for (var x = ceilWidth - padRight; x < ceilWidth; x++)
                        {
                            raw[x + x_offset] = 0;
                        }
                        //WriteRow(sym,y, raw, padLeft + x_offset);
                        for (int x = 0; x < sym.Width; x++)
                        {
                            raw[padLeft + x_offset + x] = sym.GetPixel(x, y-start_y);
                        }
                    }
                    writer.WriteRowByte(raw, y + j * ceilHeight);
                }
            }
            writer.End();
        }

        public static FFU Import(XMLUtility.FontInfo fontInfo)
        {
            using (var reader = File.OpenRead(fontInfo.FileName))
            {
                var pngReader = new PngReader(reader);
                var info = pngReader.ImgInfo;
                if (!info.Indexed && !info.Alpha)
                {
                    throw new Exception("Поддерживаются только индексиваронные файлы с альфа каналом");
                }
               
                var chunk = pngReader.GetChunksList().GetById1(Hjg.Pngcs.Chunks.PngChunkPLTE.ID);

                var paletteChunk = chunk as Hjg.Pngcs.Chunks.PngChunkPLTE;

                var colorNums = paletteChunk.GetNentries();

                CodekType codek;
                if(colorNums == 8)
                {
                    codek = CodekType.COLOR8;
                }
                else if(colorNums == 16)
                {
                    codek = CodekType.COLOR16;
                }
                else
                {
                    throw new Exception("Размер палитры должен быть 8 или 16 цветов");
                }
                var data = new byte[info.Rows][];
                using (var file = new StreamWriter("out.txt"))
                {
                    for (var y = 0; y < info.Rows; y++)
                    {
                        var row = pngReader.ReadRowByte(y);
                        file.WriteLine(string.Join("",row.ScanlineB.Select(x => x.ToString("X"))).Substring(0,256));
                        var temp = row.unpackToNewImageLine();
                        var bytesRow = temp.ScanlineB.Clone() as byte[];
                        
                        data[y] = bytesRow;
                    }
                }
                var encoding = fontInfo.Encoding.Equals(Encoding.UTF8) ? FFULibrary.FontEncoding.UTF8 : FontEncoding.SHIFT_JIS;
                var _info = new FontInfo(codek, fontInfo.Width, fontInfo.Height, 1,encoding);
                _info.LineHeight = (byte)fontInfo.LineHeight;
                var font = new FFU(_info, new SortedDictionary<char, Sym>());
                foreach (var item in fontInfo.Symbols)
                {
                    var rect = item.Value;

                    var sym = new Sym(rect.Width, rect.Height);
                    for (var y = 0; y < rect.Height; y++)
                    {
                        try
                        {
                            var row = data[rect.Top + y];
                            for (var x = 0; x < rect.Width; x++)
                            {
                                try
                                {
                                    var value = row[rect.Left + x];
                                    sym.SetPixel(x, y, value);

                                }
                                catch (Exception ex)
                                {

                                }
                            }
                        }
                        catch (Exception ex)
                        {

                        }
                    }
                
                    //var key = ToInt(item.Key);
                    font.Symbols[item.Key] = sym;
                }

                font.Pallettes = fontInfo.Palettes;

                return font;
            }
        }

        private static int ToInt(char sym)
        {
            var bytes = encoding.GetBytes(new char[] { sym });
            if (bytes.Length == 2)
            {
                return BitConverter.ToUInt16(bytes.Reverse().ToArray(), 0);
            }
            return bytes[0];
        }
    }
}
