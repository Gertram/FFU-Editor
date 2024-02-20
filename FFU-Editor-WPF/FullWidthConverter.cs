using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FFUEditor
{
    public static class FullWidthConverter
    {
        private const string FullWidthAscii = "　！＂＃＄％＆＇（）＊＋，－．／０１２３４５６７８９：；＜＝＞？＠ＡＢＣＤＥＦＧＨＩＪＫＬＭＮＯＰＱＲＳＴＵＶＷＸＹＺ［＼］＾＿｀ａｂｃｄｅｆｇｈｉｊｋｌｍｎｏｐｑｒｓｔｕｖｗｘｙｚ｛｜｝～｟｠";
        private const string StandartWidthAscii = " !\"#$%&'()*+,-./0123456789:;<=>?@ABCDEFGHIJKLMNOPQRSTUVWXYZ[\\]^_`abcdefghijklmnopqrstuvwxyz{|}~⦅⦆";
        private const string HalfwidthJapanese = " ｡｢｣､･ｦｧｨｩｪｫｬｭｮｯｰｱｲｳｴｵｶｷｸｹｺｻｼｽｾｿﾀﾁﾂﾃﾄﾅﾆﾇﾈﾉﾊﾋﾌﾍﾎﾏﾐﾑﾒﾓﾔﾕﾖﾗﾘﾙﾚﾛﾜﾝﾞﾟ";
        private const string StandartwidthJapanese = "　。「」、・ヲァィゥェォャュョッーアイウエカキクケコサシスセソタチツテトナニヌネノハヒフヘホマミムメモヤユヨラリルレロワン゛゜";
        private const string Syms = "ﾠﾡﾢﾣﾤﾥﾦﾧﾨﾩﾪﾫﾬﾭﾮﾯﾰﾱﾲﾳﾴﾵﾶﾷﾸﾹﾺﾻﾼﾽﾾ﾿￀￁ￂￃￄￅￆￇ￈￉ￊￋￌￍￎￏ￐￑ￒￓￔￕￖￗ￘￙ￚￛￜ￝￞￟￠￡￢￣￤￥￦￧￨￩￪￫￬￭￮";

        public static string ToFullWidth(this string text, bool withSpace = true)
        {
            return text.ToFullWidth(0, text.Length, withSpace);
        }
        public static string ToFullWidth(this string text, int start, int length, bool withSpace = true)
        {
            var line = text.ToCharArray();
            if (!withSpace)
            {
                for (var i = start; i < start + length; i++)
                {
                    var sym = text[i];
                    if (sym == ' ')
                    {
                        continue;
                    }
                    if (StandartWidthAscii.Contains(sym))
                    {
                        sym = FullWidthAscii[StandartWidthAscii.IndexOf(sym)];
                    }
                    line[i] = sym;
                }
            }
            else
            {
                for (var i = start; i < start + length; i++)
                {
                    var sym = text[i];
                    if (StandartWidthAscii.Contains(sym))
                    {
                        sym = FullWidthAscii[StandartWidthAscii.IndexOf(sym)];
                    }
                    line[i] = sym;
                }
            }
            return new string(line);
        }
        public static string ToHalfWidth(this string text)
        {
            return text.ToHalfWidth(0, text.Length);
        }
        public static string ToHalfWidth(this string text, int start, int length)
        {
            var line = text.ToCharArray();
            for (var i = start; i < start + length; i++)
            {
                var sym = text[i];
                if (StandartwidthJapanese.Contains(sym))
                {
                    sym = HalfwidthJapanese[StandartwidthJapanese.IndexOf(sym)];
                }
                line[i] = sym;
            }
            return new string(line);
        }
        public static string ToStandartWidth(this string text)
        {
            return text.ToStandartWidth(0, text.Length);
        }
        public static string ToStandartWidth(this string text, int start, int length)
        {
            var line = text.ToCharArray();
            for (var i = start; i < start + length; i++)
            {
                var sym = text[i];
                if (HalfwidthJapanese.Contains(sym))
                {
                    sym = StandartwidthJapanese[HalfwidthJapanese.IndexOf(sym)];
                }
                else if (FullWidthAscii.Contains(sym))
                {
                    sym = StandartWidthAscii[FullWidthAscii.IndexOf(sym)];
                }
                line[i] = sym;
            }
            return new string(line);
        }
    }
}
