using System.Text;
namespace OpenData.Message
{
    public class SMSLogService : ISMSService
    {
        static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public void Send(string from, string to, string subject, string body)
        {
            StringBuilder email = new StringBuilder();
            email.AppendLine("from:" + from);
            email.AppendLine("to:" + to);
            email.AppendLine("subject:" + subject);
            email.AppendLine("body:" + body);
            log.Info(email.ToString());
        }
    }
}
