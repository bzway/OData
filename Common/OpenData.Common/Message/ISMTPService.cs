using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Mail;
namespace OpenData.Message
{
    public interface ISMTPService
    {
        void SendMail(string from, string to, string subject, string body);
    }
}