using System.Web.Mvc;

namespace OpenData.Module.EBook
{
    public class SitesAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "EBook";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                "EBook_default",
                "EBook/{controller}/{action}/{id}",
                new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}