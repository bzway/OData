using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using log4net;
using log4net.Appender;
using log4net.Config;
using log4net.Layout;


namespace OpenData.Log
{
    public class EmailLogger {
        public string To { get; set; }

        public string From { get; set; }

        public string Subject { get; set; }

        public string Body { get; set; }

        public string Source { get; set; }

        static ILog log = log4net.LogManager.GetLogger("Bzway.Email",typeof(EmailLogger));
        public static void Log(string from, string to, string subject, string body)
        {
            StackTrace st = new StackTrace(1, true);
            StackFrame sf = st.GetFrame(0);
            string source = string.Format("{0}:{1}:{2}", sf.GetFileName(), sf.GetMethod().Name, sf.GetFileLineNumber());
            var maillog = new EmailLogger() { From = from, Body = body, To = to, Subject = subject, Source = source };
            log.Info(maillog);
        }

    }
}