using System.Linq;
using OpenData.Framework.Core;
using System.Web.Mvc;

namespace OpenData.Framework.WebApp.Areas.Sites.Controllers
{
    public class HomeController : BaseSiteController
    {

        [Authorize(Roles = "Site")]
        public ActionResult Index(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                var site = this.SiteService.FindSiteByUserID(this.UserManager.GetCurrentUser().ID).FirstOrDefault();
                if (site == null)
                {
                    return Redirect("/User");
                }
                this.CurrentSite = site.Id;
            }
            this.CurrentSite = id;

            return View(this.SiteManager.GetSite());
        }


    }
}