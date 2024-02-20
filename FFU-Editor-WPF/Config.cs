using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using log4net;


namespace FFUEditor
{
    public class Config
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(MainWindow));
        public string this[string name] { get => Get(name); set => Set(name, value); }
        public static string Get(string key)
        {
            return ConfigurationManager.AppSettings[key];
        }
        public static void Set(string key, string value)
        {
            try
            {
                var configFile = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
                var settings = configFile.AppSettings.Settings;
                if (settings[key] == null)
                {
                    settings.Add(key, value);
                }
                else
                {
                    settings[key].Value = value;
                }
                configFile.Save(ConfigurationSaveMode.Modified);
                ConfigurationManager.RefreshSection(configFile.AppSettings.SectionInformation.Name);
            }
            catch (ConfigurationErrorsException ex)
            {
                log.Error("Error writing app settings",ex);
            }
        }
    }
}