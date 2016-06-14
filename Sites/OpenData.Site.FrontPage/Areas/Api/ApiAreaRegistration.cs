using System.Web.Mvc;

namespace OpenData.Framework.WebApp.Areas.Api
{
    public class ApiAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "api";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                "Api_default",
                "api/{controller}/{action}/{id}",
                 new { controller = "Home", action = "Index", id = UrlParameter.Optional });

        }
    }
}