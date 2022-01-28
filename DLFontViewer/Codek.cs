using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DLFontViewer
{
    internal class Codek
    {
        internal static byte[] encode(byte[] data)
        {
            var buff = new List<byte>();
            while (data.Length > 0)
            {
                var i = 0;
                while (i + 1 != data.Length && data[i] == data[i + 1])
                {
                    i++;
                }
                if (i > 3)
                {
                    buff.Add((byte)(data[0] + 8));
                    i++;
                    buff.Add((byte)(i / 256));
                    buff.Add((byte)(i / 16));
                    buff.Add((byte)(i % 16));
                    data = data[i..];
                }
                else if (i > 0)
                {
                    for (var j = 0; j <= i; j++)
                    {
                        buff.Add(((byte)(data[0])));
                    }
                    data = data[(i + 1)..];
                }
                else
                {
                    buff.Add(((byte)(data[i])));
                    data = data[1..];
                }
            }
            if (buff.Count % 2 == 1)
            {
                buff.Add(0);
            }
            var buff2 = new byte[buff.Count / 2];
            for (var i = 0; i < buff.Count / 2; i++)
            {
                var val1 = buff[i * 2] << 4;
                var val2 = buff[i * 2 + 1];
                buff2[i] = (byte)(val1 | val2);
            }
            return buff2;
        }

        internal static byte[] decode(byte[] data)
        {
            var buff2 = new List<byte>();
            foreach (var item in data)
            {
                buff2.Add(BitConverter.GetBytes((item & 0xf0) >> 4)[0]);
                buff2.Add(BitConverter.GetBytes(item & 0x0f)[0]);
            }
            var image = new List<byte>();
            var idx = 0;
            while (idx < buff2.Count)
            {
                var i = buff2[idx];
                if (i < 8)
                {
                    image.Add(i);
                    idx += 1;
                }
                else
                {
                    var len = buff2[idx + 1] * 256 + buff2[idx + 2] * 16 + buff2[idx + 3];
                    var range = new byte[len].Select(x => BitConverter.GetBytes(i - 8)[0]);
                    image.InsertRange(image.Count, range);
                    idx += 4;
                }
            }
            return image.ToArray();
        }
    }
}
