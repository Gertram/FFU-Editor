using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Windows.Forms;
using System.IO;
using System.Threading.Tasks;
using FFULibrary;
using Hjg.Pngcs;

namespace FFU_Editor
{
    internal static class PNGUtility
    {
        private static readonly Encoding encoding = Encoding.GetEncoding("shift-jis");
        internal static void Export(FFU font, string filename, int palttteIndex)
        {
            var symInRow = (int)Math.Ceiling(Math.Sqrt(font.Symbols.Count));
            var rows = (int)Math.Ceiling((float)(font.Symbols.Count) / (float)(symInRow));
            var symWidth = font.Header.FontInfo.SymWidth;
            var symHeight = font.Header.FontInfo.SymHeight;
            var width = symInRow * symWidth;
            var height = rows * symHeight;
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
                var index = font.Codek.Decode((byte)i);
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
                WritePNG(writer, symHeight, symWidth, symInRow, width, rows, font);
            }
        }
        private static void WritePNG(PngWriter writer, int symHeight, int symWidth, int symInRow, int cols, int rows, FFU font)
        {
            var syms = font.Symbols.Select(x => x.Value).ToArray();
            var raw = new byte[cols];
            for (var j = 0; j < rows; j++)
            {
                for (var y = 0; y < symHeight; y++)
                {
                    for (var i = 0; i < symInRow && j * symInRow + i < syms.Length; i++)
                    {
                        var sym = syms[i + symInRow * j];
                        var padLeft = (symWidth - sym.Width) / 2;
                        var padRight = (symWidth - sym.Width - padLeft);
                        var x_offset = i * symWidth;

                        for (var x = 0; x < padLeft; x++)
                        {
                            raw[x + x_offset] = 0;
                        }
                        for (var x = (symWidth - padRight); x < symWidth; x++)
                        {
                            raw[x + x_offset] = 0;
                        }
                        //WriteRow(sym,y, raw, padLeft + x_offset);
                        for (int x = 0; x < sym.Width; x++)
                        {
                            raw[padLeft+x_offset + x] = font.Codek.Decode(sym.Image[x, y]);
                        }
                    }
                    writer.WriteRowByte(raw, y + j * symHeight);
                }
            }
            writer.End();
        }

        //private static void WriteRow(Sym sym,int row, byte[] dest, int start)
        //{
        //    for (int x = 0; x < sym.Width; x++)
        //    {
        //        dest[start + x] = sym.Image[x, row];
        //    }
        //}
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
                        data[y] = row.ScanlineB.Clone() as byte[];
                    }
                }
                var font = new FFU(new FontInfo(codek, fontInfo.Width, fontInfo.Height, true), new SortedDictionary<int, Sym>());
                foreach (var item in fontInfo.Symbols)
                {
                    var rect = item.Value;

                    var temp = new byte[rect.Width, rect.Height];
                    for(var y = 0;y < rect.Height; y++)
                    {
                        var row = data[rect.Top + y];
                        for (var x = 0; x < rect.Width; x++)
                        {
                            var value = row[rect.Left + x];
                            temp[x, y] = font.Codek.Encode(value);
                        }
                    }
                    var key = ToInt(item.Key);
                   font.Symbols[key] = new Sym(temp);
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
