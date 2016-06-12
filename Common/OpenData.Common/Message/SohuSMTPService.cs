using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Mail;
using System.Net;
using System.Web;
using System.Net.Http;
using OpenData.Utility;
using Newtonsoft.Json;
namespace OpenData.Message
{

    public class SohuSMTPService : ISMTPService
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public void SendMail(string from, string to, string subject, string body)
        {

            if (string.IsNullOrWhiteSpace(subject))
            {
                return;
            }
            if (string.IsNullOrWhiteSpace(body))
            {
                return;
            }
            if (string.IsNullOrWhiteSpace(to))
            {
                return;
            }
            var url = "http://api.sendcloud.net/apiv2/mail/send";

            String api_user = "8DACD09B90384E97A51CB6EE6A13ABB5";
            String api_key = "arCzn5Q5nAvwuLJ9";
            string fromname = "TestEmail";



            try
            {
                WebClient client = new WebClient();
                client.Proxy = WebProxyHelper.CreateWebProxy();
                client.Encoding = Encoding.UTF8;
                var data = new
                {
                    api_user = api_user,
                    api_key = api_key,
                    from = from,
                    to = to,
                    fromname = fromname,
                    subject = subject,
                    //template_invoke_name = "test_template_send",
                    HtmlString = body,
                };

                var res = client.UploadString(url, "post", JsonConvert.SerializeObject(data));
            }
            catch (Exception e)
            {
                Console.WriteLine("\nException Caught!");
                Console.WriteLine("Message :{0} ", e.Message);
            }
        }
    }
}