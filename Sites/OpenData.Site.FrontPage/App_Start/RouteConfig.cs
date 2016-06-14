using System.Web.Mvc;
using System.Web.Routing;

namespace OpenData.Framework.WebApp
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
             
            //Front Page
            routes.MapRoute(
                name: "Page",
                url: "{*PageUrl}",
                defaults: new { controller = "FrontPage", action = "Index", PageUrl = "" },
                namespaces: new[] { "Bzway.WebSite.WebApp.Controllers" }
            );
        }
    }
}
