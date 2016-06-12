using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Mail;
namespace OpenData.Message
{
    public interface ISMSService
    {
        void Send(string from, string to, string subject, string body);

    }
  
}
