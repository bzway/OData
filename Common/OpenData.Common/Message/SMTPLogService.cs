using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Mail;
using OpenData.Log;
namespace OpenData.Message
{
    public class SMTPLogService : ISMTPService
    {
        static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public void SendMail(string from, string to, string subject, string body)
        {
            StringBuilder email = new StringBuilder();
            email.AppendLine("from:" + from);
            email.AppendLine("to:" + to);
            email.AppendLine("subject:" + subject);
            email.AppendLine("body:" + body);
            log.Info(email.ToString());
            //EmailLogger.Log(from, to, subject, body);
        }
    }
}
