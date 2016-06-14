using System.Web.Mvc;

namespace OpenData.Framework.WebApp.Areas.Wechats
{
    public class WechatsAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "Wechat";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                "Wechat_default",
                "Wechat/{controller}/{action}/{id}",
                new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}