using System.Web.Mvc;

namespace OpenData.Sites.FrontPage.Areas.Sites
{
    public class SitesAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "Site";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                "Site_default",
                "Site/{controller}/{action}/{id}",
                new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}