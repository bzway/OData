using System.Web.Mvc;

namespace OpenData.Module.Website
{
    public class SitesAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "Sites";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                "Sites_default",
                "Sites/{controller}/{action}/{id}",
                new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}