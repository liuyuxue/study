using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonLib.Tools
{
    public  class ConfigTools
    {
        public static string GetAppConfig(string strKey)
        {
            string result;
            try
            {
                foreach (string key in ConfigurationManager.AppSettings)
                {
                    if (key == strKey)
                    {
                        result = ConfigurationManager.AppSettings[strKey];
                        return result;
                    }
                }
                result = null;
            }
            catch
            {
                throw;
            }
            return result;
        }

        public static void UpdateAppConfig(string newKey, string newValue)
        {
            try
            {
                bool isModified = false;
                foreach (string key in ConfigurationManager.AppSettings)
                {
                    if (key == newKey)
                    {
                        isModified = true;
                    }
                }
                Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
                if (isModified)
                {
                    config.AppSettings.Settings.Remove(newKey);
                }
                config.AppSettings.Settings.Add(newKey, newValue);
                config.Save(ConfigurationSaveMode.Modified);
                ConfigurationManager.RefreshSection("appSettings");
            }
            catch
            {
                throw;
            }
        }

    }
}
