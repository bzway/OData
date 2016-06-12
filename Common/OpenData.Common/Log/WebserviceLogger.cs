using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using log4net;
using log4net.Appender;
using log4net.Config;
using log4net.Layout;


namespace OpenData.Log
{
    public class WebserviceLogger
    {
        public string URL { get; set; }
        public string Name { get; set; }
        public string Content { get; set; }
        public string Result { get; set; }

        static ILog log = log4net.LogManager.GetLogger("Bzway.WebserviceLog", typeof(WebserviceLogger));
        static void Log(string url, string content, string source, string returnObject)
        {
            var wsLog = new WebserviceLogger()
            {
                Content = content,
                Name = source,
                Result = returnObject,
                URL = url
            };
            log.Info(wsLog);
        }
    }
}