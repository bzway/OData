using System;
using System.Net;
using System.Net.Security;
using System.Reflection;

namespace OpenData.Utility
{
    public class WebProxyHelper
    {
        public static readonly string WebProxy = ConfigSetting.Get("WebProxyURL");
        public static readonly string WebProxyPort = ConfigSetting.Get("WebProxyPort");
        public static readonly string Username = ConfigSetting.Get("WebProxyUserName");
        public static readonly string Password = ConfigSetting.Get("WebProxyPassword");
        public static readonly string UseWebProxy = ConfigSetting.Get("UseWebProxy");
        public static readonly string Domain = ConfigSetting.Get("Domain");

        public static WebProxy CreateWebProxy()
        {
            if ("1".Equals(UseWebProxy))
            {
                WebProxy proxyObject = new WebProxy(WebProxy, int.Parse(WebProxyPort));
                proxyObject.Credentials = new NetworkCredential(Username, Password, Domain);

                return proxyObject;
            }
            return null;
        }

        //Https 不验证证书
        public static void CancelCertificateValidate()
        {
            ServicePointManager.ServerCertificateValidationCallback = new RemoteCertificateValidationCallback(delegate { return true; });
        }
    }
}