using OpenData.Common.AppEngine;
using OpenData.Common.Caching;
using OpenData.Message;
using OpenData.Framework.Core;

namespace OpenData.Sites.FrontPage
{
    public class ServiceConfig : IDependencyRegistrar
    {

        public void Register(IContainerManager containerManager, ITypeFinder typeFinder)
        {
            containerManager.RegisterType<ICacheManager, MemoryCacheManager>();
            containerManager.RegisterType<ISMSService, SMSLogService>();
            containerManager.RegisterType<IUserService, UserService>();
            containerManager.RegisterType<ISiteService, SiteService>();
            containerManager.RegisterType<IMemberService, MemberService>();

            //SMTPService smtp = new SMTPService() { UserName = "g2gstock@sina.com", Host = "smtp.sina.com", Port = 25, Password = "stockg2g" };
            //containerManager.AddComponentInstance<ISMTPService>(smtp);
            //containerManager.AddComponent<ISMTPService, APIService>();
            containerManager.RegisterType<ISMTPService, SMTPLogService>();
            //var Name = "TestDB";
            //var AppID = "wx35d8f9a93970e96e";
            //var AppSecret = "d748ece56f1994800aa421a249e6050d";
            //var IsServiceAccount = true;
            //var Token = "bzwaytoken";
            //var OpenId = "gh_216c64e3da19";
            //WechatSetting.Add(Name, AppID, AppSecret, Token, OpenId, IsServiceAccount);
        }

        public int Order
        {
            get { return 100; }
        }
    }
}
