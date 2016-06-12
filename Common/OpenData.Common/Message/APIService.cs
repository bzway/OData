
//using CodeScales.Http.Methods;
//using CodeScales.Http.Entity;
//using CodeScales.Http.Entity.Mime;
//using CodeScales.Http.Common;
//using CodeScales;
//using CodeScales.Http.Network;
//using CodeScales.Http.Protocol;
//using CodeScales.Http.Cookies;
//using CodeScales.Http;
//using System;
//using System.Text;
//namespace Bzway.Message
//{

//    public class APIService : ISMTPService
//    {
//        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
//        public static void WebAPISendMail()
//        {

//        }

//        public void SendMail(string from, string to, string subject, string body)
//        {
//            HttpClient client = new HttpClient();
//            HttpPost postMethod = new HttpPost(new Uri("http://api.emailcar.net/mail.trigger.xml/"));

//            MultipartEntity multipartEntity = new MultipartEntity();
//            postMethod.Entity = multipartEntity;
//            multipartEntity.AddBody(new StringBody(Encoding.UTF8, "api_user", "zhumingwu"));
//            multipartEntity.AddBody(new StringBody(Encoding.UTF8, "api_pwd", "2850143d"));
//            multipartEntity.AddBody(new StringBody(Encoding.UTF8, "from", "from@EmailCar.net"));
//            multipartEntity.AddBody(new StringBody(Encoding.UTF8, "replyto", "zhumingwu@126.com"));
//            multipartEntity.AddBody(new StringBody(Encoding.UTF8, "fromname", "EmailCar"));
//            multipartEntity.AddBody(new StringBody(Encoding.UTF8, "to", to));
//            multipartEntity.AddBody(new StringBody(Encoding.UTF8, "subject", "C#调用WebAPI主题"));
//            multipartEntity.AddBody(new StringBody(Encoding.UTF8, "html", "C# html内容"));

//            HttpResponse response = client.Execute(postMethod);

//            Console.WriteLine("Response Code: " + response.ResponseCode);
//            string aaa = ("Response Content: " + EntityUtils.ToString(response.Entity));

//        }
//    }
//}