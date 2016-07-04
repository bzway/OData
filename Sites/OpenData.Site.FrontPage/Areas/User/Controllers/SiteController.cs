using OpenData.Framework.Core.Entity;
using System.Net;
using System.Web.Mvc;

namespace OpenData.Sites.FrontPage.Areas.Users.Controllers
{
    public class SiteController : BaseController
    {

        [Authorize(Roles = "Site")]
        public ActionResult Index()
        {
            var userId = this.User.GetCurrentUser().ID;
            var model = this.SiteService.FindSiteByUserID(userId);
            return View(model);
        }

        [Authorize(Roles = "Site")]
        public ActionResult Edit(string id)
        {
            var model = this.SiteService.FindSiteByID(id);
            return View(model);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Site model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            this.SiteService.CreateOrUpdateSite(model, this.User.GetCurrentUser().ID);
            return RedirectToAction("Index");
        }

        public ActionResult Details(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var model = this.SiteService.FindSiteByID(id);
            return View(model);
        }
        // GET: Tests/Delete/5
        public ActionResult Delete(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var model = this.SiteService.FindSiteByID(id);
            return View(model);
        }

        // POST: Tests/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            this.SiteService.DeleteSiteByID(id);
            return RedirectToAction("Index");
        }
    }
}