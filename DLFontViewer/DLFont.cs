using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Threading.Tasks;

namespace DLFontViewer
{
    public class DLFont
    {
        public struct Header
        {
            public short SectionNumbers;
            public int SymCount;
            public byte[] FontInfo;
            public int FileSize;
            public int SectionAddr;
            public int SymAddr;
            public int SymImageAddr;
            public int par1;
            public int par2;
        }
        public Header header;
        public Color[] palitte;
        public SortedDictionary<int, ISym> symbols;
        public byte[] Footer = new byte[0];
        public static int counter = 0;
        private static int last_seek;
        public Color[] GetPalitte(int index = 0)
        {
            return palitte.Skip(index * 16).Take(16).ToArray();
        }
        public static DLFont LoadFont<T>(byte[] data) where T : ISym, new()
        {
            var font = new DLFont
            {
                header = LoadHeader(data),
                palitte = LoadPallite(data, 0x28, 256)
            };
            font.symbols = new SortedDictionary<int, ISym>(LoadSymbols<T>(data, font.header));
            if (last_seek != data.Length)
            {
                var temp = new byte[data.Length - last_seek];
                Array.Copy(data, last_seek, temp, 0, data.Length - last_seek);
                font.Footer = temp;
            }
            return font;
        }
        private static Header LoadHeader(byte[] data)
        {
            var header = new Header();
            var h = Encoding.ASCII.GetString(data,0,2);
            if (h != "UF")
            {
                throw new FormatException("Неверный формат файла(не найден \"UF\" в началае файла");
            }
            header.SectionNumbers = BitConverter.ToInt16(data,2);
            header.SymCount = BitConverter.ToInt32(data,4);
            //data[15] уменьшение шрифта
            //data[14] увеличение шрифта
            header.FontInfo = data.Skip(8).Take(8).ToArray();
            header.FileSize = BitConverter.ToInt32(data,16);
            header.SectionAddr = BitConverter.ToInt32(data,20);
            header.SymAddr = BitConverter.ToInt32(data,24);
            header.SymImageAddr = BitConverter.ToInt32(data,28);
            header.par1 = BitConverter.ToInt32(data,32);
            header.par2 = BitConverter.ToInt32(data,36);
            return header;  
        }
        private static Color[] LoadPallite(byte[] data, int seek, int count)
        {
            var colors = new Color[count];

            for (int i = 0; i < count; i++)
            {
                colors[i] = Color.FromArgb(data[seek + i * 4 + 3], data[seek + i * 4], data[seek + i * 4 + 1], data[seek + i * 4 + 2]);
            }

            return colors;
        }
        private static Dictionary<int, ISym> LoadSymbols<T>(byte[] data, Header header) where T : ISym, new()
        {
            Dictionary<int, ISym> symbols = new Dictionary<int, ISym>();

            var seek = header.SectionAddr;

            for (int i = 0; i < header.SectionNumbers; i++)
            {
                var temp = LoadSection<T>(data, seek + i * 12, header);
                foreach(var item in temp)
                {
                    symbols.Add(item.Key, item.Value);
                }
            }
            var ts1 = Codek.First.Elapsed;
            string elapsedTime1 = String.Format("{0:00}:{1:00}:{2:00}.{3:00}",
          ts1.Hours, ts1.Minutes, ts1.Seconds,
          ts1.Milliseconds / 10);
            var ts2 = Codek.Second.Elapsed;
            string elapsedTime2 = String.Format("{0:00}:{1:00}:{2:00}.{3:00}",
          ts2.Hours, ts2.Minutes, ts2.Seconds,
          ts2.Milliseconds / 10);

            return symbols;
        }
        private static Dictionary<int, ISym> LoadSection<T>(byte[] data, int header_seek, Header header) where T : ISym, new()
        {
            var startCode = BitConverter.ToInt32(data,header_seek);
            var endCode = BitConverter.ToInt32(data, header_seek+4);
            var seek = BitConverter.ToInt32(data, header_seek+8) * 8 + header.SymAddr;

            var syms = new Dictionary<int, ISym>(endCode - startCode);

            for (var i = 0; i < endCode - startCode; i++)
            {
                var start = seek + i * 8;
                if(i > 1000)
                {
                    Console.WriteLine("SOmething wornh");
                }
                syms[i + startCode] = LoadSym<T>(data, start, header);
            }

            return syms;
        }
        private static ISym LoadSym<T>(byte[] data, int sym_header, Header header) where T : ISym, new()
        {
            var length = BitConverter.ToInt16(data,sym_header+2);
            var seek = BitConverter.ToInt32(data, sym_header+4) + header.SymImageAddr;
            //var decoded = Codek.decode(data, seek, length);
            var sym = new T();
            sym.Load(data,seek,length/*Codek.decode(data,seek,length)*/, data[sym_header], data[sym_header+1]);
            //var sym = Sym.FromArray(decoded,0,length, data[sym_header], data[sym_header+1]);
            sym.HeaderAddress = sym_header;
            sym.Address = seek;
            last_seek = seek + length;
            counter++;
            
            return sym;
        }
    }
}
