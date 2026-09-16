using System;
using System.Configuration;

namespace ConfiguracionAppConfig.Configuration
{
    internal class AppSettingsReader
    {
        public string GetString(string key, string defaultValue)
        {
            string value = ConfigurationManager.AppSettings[key];
            return string.IsNullOrWhiteSpace(value) ? defaultValue : value;
        }

        public int GetInt(string key, int defaultValue)
        {
            string value = ConfigurationManager.AppSettings[key];
            return int.TryParse(value, out int result) ? result : defaultValue;
        }

        public bool GetBool(string key, bool defaultValue)
        {
            string value = ConfigurationManager.AppSettings[key];
            return bool.TryParse(value, out bool result) ? result : defaultValue;
        }
    }
}
