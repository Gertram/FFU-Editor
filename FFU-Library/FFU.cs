using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.IO;
using System.Threading.Tasks;

namespace FFULibrary
{
    public enum CodekType
    {
        COLOR8,
        COLOR16
    };
    public class Header
    {
        public short SectionNumbers { get; set; }
        public int SymCount { get; set; }
        public byte[] FontInfo { get; set; }
        public int FileSize { get; set; }
        public int SectionAddr { get; set; }
        public int SymAddr { get; set; }
        public int SymImageAddr { get; set; }
        public int Par1 { get; set; }
        public int Par2 { get; set; }
        public CodekType Codek;
    }
    public class FFU
    {
        public Header Header { get; private set; }
        public Color[][] Pallettes { get; set; }
        private ICodek codek;
        private ICodek Codek
        {
            get
            {
                if (codek == null)
                {
                    if (Header.Codek == CodekType.COLOR8)
                    {
                        codek = new CompressedCodek();
                    }
                    else if (Header.Codek == CodekType.COLOR16)
                    {
                        codek = new UncompressedCodek();
                    }
                    else
                    {
                        throw new NotImplementedException();
                    }
                }
                
                return codek;
            }
        }
        public SortedDictionary<int, Sym> Symbols { get; set; }
        public byte[] Footer = new byte[0];
        public static int counter = 0;
        private static int last_seek;
        public FFU(string filename) : this(File.ReadAllBytes(filename)) { }
        public FFU(byte[] data)
        {
            Header = LoadHeader(data);
            Pallettes = LoadPallettes(data, 0x28);
            Symbols = new SortedDictionary<int, Sym>(LoadSymbols(data, Header));
            if (last_seek != data.Length)
            {
                var temp = new byte[data.Length - last_seek];
                Array.Copy(data, last_seek, temp, 0, data.Length - last_seek);
                Footer = temp;
            }
        }
        private static Header LoadHeader(byte[] data)
        {
            var header = new Header();
            var h = Encoding.ASCII.GetString(data, 0, 2);
            if (h != "UF")
            {
                throw new FormatException("Неверный формат файла(не найден \"UF\" в началае файла");
            }
            header.SectionNumbers = BitConverter.ToInt16(data, 2);
            header.SymCount = BitConverter.ToInt32(data, 4);
            //data[15] уменьшение шрифта
            //data[14] увеличение шрифта
            header.FontInfo = data.Skip(8).Take(8).ToArray();
            if(header.FontInfo[0] == 5)
            {
                header.Codek = CodekType.COLOR8;
            }
            else if(header.FontInfo[0] == 3)
            {
                header.Codek = CodekType.COLOR16;
            }
            else
            {
                throw new NotImplementedException("Неизвестный тип кодирования");
            }
            header.FileSize = BitConverter.ToInt32(data, 16);
            header.SectionAddr = BitConverter.ToInt32(data, 20);
            header.SymAddr = BitConverter.ToInt32(data, 24);
            header.SymImageAddr = BitConverter.ToInt32(data, 28);
            header.Par1 = BitConverter.ToInt32(data, 32);
            header.Par2 = BitConverter.ToInt32(data, 36);
            return header;
        }
        public Color[] GetPalette(int index)
        {
            if (Pallettes != null)
            {
                return Pallettes[index];
            }

            var colorCount = Header.Codek == CodekType.COLOR8 ? 8 : 16;
            var colors = new Color[colorCount];
            for (var i = 0; i < colorCount; i++)
            {
                colors[i] = Color.FromArgb(0xff / (colorCount - 1) * i, 0xff, 0xff, 0xff);
            }
            return colors;
        }
        private Color[][] LoadPallettes(byte[] data, int seek)
        {
            if(Header.SectionAddr == 0x20)
            {
                return null;
            }
            var palettes = new Color[16][];
            for (int i = 0; i < 16; i++)
            {
                var colors = new Color[16];

                for (int j = 0; j < 16; j++, seek += 4)
                {
                    colors[j] = Color.FromArgb(data[seek + 3], data[seek], data[seek + 1], data[seek + 2]);
                }
                palettes[i] = colors;
            }


            return palettes;
        }
        private Dictionary<int, Sym> LoadSymbols(byte[] data, Header header)
        {
            Dictionary<int, Sym> symbols = new Dictionary<int, Sym>();

            var seek = header.SectionAddr;

            const int sectionSize = sizeof(int) * 3;
            for (int i = 0; i < header.SectionNumbers; i++)
            {
                var temp = LoadSection(data, seek + i*sectionSize, header, Codek);
                foreach (var item in temp)
                {
                    symbols.Add(item.Key, item.Value);
                }
            }

            return symbols;
        }
        private static Dictionary<int, Sym> LoadSection(byte[] data, int header_seek, Header header, ICodek codek)
        {
            var startCode = BitConverter.ToInt32(data, header_seek);
            var endCode = BitConverter.ToInt32(data, header_seek + 4);
            var seek = BitConverter.ToInt32(data, header_seek + 8) * 8 + header.SymAddr;

            var syms = new Dictionary<int, Sym>(endCode - startCode);

            for (var i = 0; i < endCode - startCode; i++)
            {
                var start = seek + i * 8;
                if (i > 1000)
                {
                    Console.WriteLine("SOmething wornh");
                }
                syms[i + startCode] = LoadSym(data, start, header, codek);
            }

            return syms;
        }
        private static Sym LoadSym(byte[] data, int sym_header, Header header, ICodek codek)
        {
            var length = BitConverter.ToInt16(data, sym_header + 2);
            var seek = BitConverter.ToInt32(data, sym_header + 4) + header.SymImageAddr;
            var decoded = codek.Decode(data, seek, length);
            var height = data[sym_header + 1];
            var width = (byte)(decoded.Length / height);
            var temp = new byte[width, height];
            for (int y = 0, pos = 0; y < height; y++)
            {
                for (var x = 0; x < width; x++, pos++)
                {
                    temp[x, y] = decoded[pos];
                }
            }
            var sym = new Sym
            {
                Image = temp,
                HeaderAddress = sym_header,
                Address = seek
            };
            last_seek = seek + length;
            counter++;

            return sym;
        }

        private List<List<int>> CalcSections()
        {
            var sections = new List<List<int>>();

            var section = new List<int>();

            foreach (var sym in Symbols)
            {
                if (section.Count == 0)
                {
                    section.Add(sym.Key);
                }
                else
                {
                    if (section.Last() + 1 != sym.Key)
                    {

                        sections.Add(section);
                        section = new List<int>();
                        section.Add(sym.Key);
                    }
                    else
                    {
                        section.Add(sym.Key);
                    }
                }
            }
            if (section.Count > 0)
            {
                sections.Add(section);
            }
            return sections;
        }

        public int CalcFileSize()
        {
            var sections = CalcSections();

            int addrPalitte = 0x28;
            int addrSection = addrPalitte + Pallettes.Sum(x => x.Length) * 4;
            int addrSym = addrSection + sections.Count * 12;
            int addrSymImage = addrSym + Symbols.Count * 8;

            return addrSymImage + Symbols.Select(x => Codek.Encode(x.Value.ToArray()).Length).Sum();
        }

        public byte[] Save()
        {
            return null;
        }
        private void WriteHeader(BinaryWriter bw)
        {
            bw.Write(Encoding.ASCII.GetBytes("UF"));

            bw.Write(Header.SectionNumbers);

            bw.Write(Symbols.Count);

            bw.Write(Header.FontInfo);

            bw.Write(BitConverter.GetBytes(Header.SectionAddr));//Сдвиг на адрес таблицы секций

            bw.Write(BitConverter.GetBytes(Header.SymAddr));//Сдвиг на адрес адрес таблицы символов

            bw.Write(BitConverter.GetBytes(Header.SymImageAddr));//Сдивг на адрес изображений символов

            bw.Write(Header.Par1);

            bw.Write(Header.Par2);
        }
        private void WriteSections(BinaryWriter bw)
        {
            Header.SectionAddr = (int)bw.BaseStream.Position;
            var sections = CalcSections();
            var idx = 0;
            foreach (var sec in sections)
            {
                bw.Write(sec[0]);
                bw.Write(sec.Last() + 1);
                bw.Write(idx);
                idx += sec.Last() - sec[0] + 1;
            }
            Header.SectionNumbers = (short)sections.Count;
        }
        private void WritePalitte(BinaryWriter bw)
        {
            foreach(var palitte in Pallettes)
            {
                foreach(var color in palitte)
                {
                    bw.Write(color.R);
                    bw.Write(color.G);
                    bw.Write(color.B);
                    bw.Write(color.A);
                }
            }
        }

        private void WriteSymbols(BinaryWriter bw)
        {
            Header.SymAddr = (int)bw.BaseStream.Position;
            Header.SymCount = Symbols.Count;
            var idx = 0;

            foreach (var sym in Symbols)
            {
                var encoded = Codek.Encode(sym.Value.ToArray());
                bw.Write(sym.Value.Width);
                bw.Write(sym.Value.Height);
                bw.Write((short)encoded.Length);
                bw.Write(idx);
                idx += encoded.Length;
            }
            Header.SymImageAddr = (int)bw.BaseStream.Position;

            foreach (var sym in Symbols)
            {
                bw.Write(Codek.Encode(sym.Value.ToArray()));
            }

            var filesize = (int)bw.BaseStream.Position;

            if (filesize < Header.FileSize)
            {
                bw.Write(new byte[Header.FileSize - filesize]);//Размер файла
                filesize = (int)bw.BaseStream.Position;
            }

            Header.FileSize = filesize;
            bw.Write(Footer);
        }
        public void Save(string filename)
        {
            BinaryWriter bw = new BinaryWriter(File.Open(filename, FileMode.Create, FileAccess.Write, FileShare.None));

            WriteHeader(bw);

            WritePalitte(bw);

            WriteSections(bw);

            WriteSymbols(bw);

            bw.BaseStream.Position = 0;

            WriteHeader(bw);

            bw.Close();
        }
    }
}
