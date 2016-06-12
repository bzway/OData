using Microsoft.Owin;
using System.Web;

namespace OpenData.Framework.Service
{
    public static class ServiceExtension
    {
        public static UserManager GetUserManager(this HttpContextBase context)
        {
            return new UserManager(context);
        }

        public static WechatManager GetWechatManager(this HttpContextBase context, string uuid)
        {
            var db = context.GetSiteManager().GetSiteDataBase();
            return new WechatManager(db, uuid);
        }
        public static SiteManager GetSiteManager(this HttpContextBase context)
        {
            return new SiteManager(context);
        }
    }
}