
using System.Configuration;
namespace OpenData.Utility
{
    /// <summary>
    /// Summary description for Configuration.
    /// </summary>

    public class ConfigSetting
    {

        /// <summary>
        /// Retrieve a configuration Sessing with error management
        /// </summary>
        public static string Get(string key, string defaultString = "")
        {
            string strSetting = ConfigurationManager.AppSettings[key];
            return strSetting == null ? defaultString : strSetting;
        }

        public static int GetInt(string key)
        {
            int i;
            if (int.TryParse(ConfigurationManager.AppSettings[key], out  i))
            {
                return i;
            }
            else
            {
                return 0;
            }
        }
    }
}