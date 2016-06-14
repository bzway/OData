using OpenData.AppEngine.Dependency;
using OpenData.Caching;
using OpenData.Message;
using OpenData.Framework.Core;

namespace OpenData.Sites.FrontPage
{
    public class ServiceConfig : IDependencyRegistrar
    {

        public void Register(ContainerManager containerManager, AppEngine.ITypeFinder typeFinder)
        {
            containerManager.AddComponent<ICacheManager, MemoryCacheManager>();
            containerManager.AddComponent<ISMSService, SMSLogService>();
            containerManager.AddComponent<IUserService, UserService>();
            containerManager.AddComponent<ISiteService, SiteService>();
            containerManager.AddComponent<IMemberService, MemberService>();

            //SMTPService smtp = new SMTPService() { UserName = "g2gstock@sina.com", Host = "smtp.sina.com", Port = 25, Password = "stockg2g" };
            //containerManager.AddComponentInstance<ISMTPService>(smtp);
            //containerManager.AddComponent<ISMTPService, APIService>();
            containerManager.AddComponent<ISMTPService, SMTPLogService>();
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
