
using System.Configuration;
using System.Web;

namespace OpenData.Site.FrontPage
{
    public static class Log4NetConfig
    {
        public static void Register(HttpApplication context)
        {
            string configfile = context.Server.MapPath("~") + "\\web.config";
            System.IO.FileInfo fileInfo = new System.IO.FileInfo(configfile);
            log4net.Config.XmlConfigurator.ConfigureAndWatch(fileInfo);

            log4net.Repository.ILoggerRepository hier = log4net.LogManager.GetRepository();
            if (hier != null)
            {
                foreach (log4net.Appender.IAppender appender in hier.GetAppenders())
                {
                    if (appender.Name != "ADONetAppender")
                    {
                        continue;
                    }
                    log4net.Appender.AdoNetAppender adoAppender = (log4net.Appender.AdoNetAppender)appender;
                    adoAppender.ConnectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
                    //refresh settings of appender
                    adoAppender.ActivateOptions();
                }
            }
        }
    }
}
