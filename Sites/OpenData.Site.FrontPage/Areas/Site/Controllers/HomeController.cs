using OpenData.Framework.Core;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OpenData.Sites.FrontPage.Areas.Sites.Controllers
{
    public class HomeController : BaseSiteController
    {
        public ActionResult Index()
        {
            var list = this.SiteService.FindSiteByUserID(this.User.GetCurrentUser().ID);
            return View(list);
        }

        public ActionResult Details(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                var site = this.SiteService.FindSiteByUserID(this.User.GetCurrentUser().ID).FirstOrDefault();
                if (site == null)
                {
                    this.Warning("test");
                    return Redirect("/User");
                }
                this.CurrentSite = site.Id;
            }
            this.CurrentSite = id;

            return View(this.Site.GetSite());
        }

        public void Modules()
        {
            HttpApplication httpApps = ControllerContext.HttpContext.ApplicationInstance;
            // 获取所有 http module
            HttpModuleCollection httpModules = httpApps.Modules;

            Response.Write(string.Format("一共有{0}个 HttpModule</br>", httpModules.Count.ToString()));
            foreach (string activeModule in httpModules.AllKeys)
            {
                Response.Write(activeModule + "</br>");
            }
        }
    }
}