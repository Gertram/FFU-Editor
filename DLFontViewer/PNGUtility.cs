using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading.Tasks;
using FFULibrary;

namespace FFU_Editor
{
    internal static class PNGUtility
    {
        internal static void Export(FFU font, string filename)
        {
            var syms = font.Symbols.Select(x => x.Value).ToArray();
            var symInRow = (int)Math.Ceiling(Math.Sqrt(syms.Length));
            var rows = (int)Math.Ceiling((float)(syms.Length) / (float)(symInRow));
            var symWidth = font.Header.FontInfo.SymWidth;
            var symHeight = font.Header.FontInfo.SymHeight;
            var width = symInRow * symWidth;
            var height = rows * symHeight;
            var info = new Hjg.Pngcs.ImageInfo(width, height, 8, false, false, true);

            var chunk = new Hjg.Pngcs.Chunks.PngChunkPLTE(info);
            var colors = font.GetPalette(0);
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

                var writer = new Hjg.Pngcs.PngWriter(stream, info);
                writer.GetChunksList().Queue(chunk);
                writer.GetChunksList().Queue(tchunk);
                writer.CompLevel = 9;
                WritePNG(writer, symHeight, symWidth, symInRow, width, rows, syms);
            }
        }
        private static void WritePNG(Hjg.Pngcs.PngWriter writer, int symHeight, int symWidth, int symInRow, int cols, int rows, Sym[] syms)
        {
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
                        sym.WriteRow(y, raw, padLeft + x_offset);
                    }
                    writer.WriteRowByte(raw, y + j * symHeight);
                }
            }
            writer.End();
        }
    }
}
