using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FFUEditor
{
    public interface ILanguage
    {
        string Name { get; }
        string Code { get; }
    }
    class Language : ILanguage
    {
        public string Name => "Русский";
        public string Code => "ru-ru";
    }
    public class BoolValue
    {
        private Config config;
        public BoolValue(Config config, string name)
        {
            this.config = config;
            Name = name;
        }
        public string Name { get; private set; }
        public bool Value
        {
            get => bool.Parse(config[Name] ?? "false");
            set => config[Name] = value.ToString();
        }
    }
    public class IntValue
    {
        private string name;
        private int __default;
        private Config config;
        internal IntValue(Config config, string name, int @default = 0)
        {
            this.config = config;
            this.name = name;
            __default = @default;

        }
        public int Value
        {

            get
            {
                var str = config[name];
                if (str == null)
                {
                    return __default;
                }
                return int.Parse(config[name]);
            }

            set => config[name] = value.ToString();
        }
    }
    public class StringValue
    {
        private readonly string name;
        private readonly string @default;
        private Config config;
        internal StringValue(Config config, string name, string @default = null)
        {
            this.config = config;
            this.name = name;
            this.@default = @default;
        }
        public string Value
        {

            get
            {
                var str = config[name];
                if (str == null)
                {
                    str = @default;
                }
                return str;
            }

            set => config[name] = value;
        }
    }
    public partial class MainConfig
    {
        private static Config config;
        static MainConfig()
        {
            config = new Config();
            template = new StringValue(config, "Template", DefaultTemplate);
        }
        public ILanguage Language => new Language();
        public static string LastFile
        {
            get => config["lastFile"];
            set => config["lastFile"] = value;
        }
        public static string DefaultTemplate => string.Join("\n", new string[]{
            Properties.Resources.RussiaUpperSymbols + Properties.Resources.RussiaLowerCase,
            Properties.Resources.EnglishUpperCase + Properties.Resources.EnglishLowerCase,
            Properties.Resources.JapaneEnglishUpperCase + Properties.Resources.JapaneEnglishLowerCase
        });

        private static StringValue template;

        public static string Template
        {
            get => template.Value;

            set => template.Value = value;
        }
    }

}
