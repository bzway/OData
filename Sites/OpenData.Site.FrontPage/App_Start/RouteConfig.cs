using System.Web.Mvc;
using System.Web.Routing;

namespace OpenData.Sites.FrontPage
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
                namespaces: new[] { "OpenData.Sites.FrontPage" }
            );
        }
    }
}
