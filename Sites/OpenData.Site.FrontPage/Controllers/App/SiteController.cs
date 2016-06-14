using OpenData.Site.Entity;
using OpenData.Site.Core;
using OpenData.Data.Core;
using System.Net;
using System.Web.Mvc;
using System.Collections.Generic;
using OpenData.Common;
using OpenData.Globalization;
using OpenData.Site.FrontPage.Models;

namespace OpenData.Site.FrontPage.Controllers.App
{
    public class SiteController : BaseController
    {
        IMemberService memberService = ApplicationEngine.Current.Resolve<IMemberService>();
        ISiteService siteService = ApplicationEngine.Current.Resolve<ISiteService>();

        [Authorize(Roles = "Site")]
        public ActionResult Index(int? pageIndex, int? pageSize)
        {
            pageIndex = pageIndex ?? 1;
            pageSize = pageSize ?? 10;

            var list = new List<Entity.Site>();
            foreach (var item in this.siteService.FindSiteByUserID(this.UserManager.GetCurrentUser().ID))
            {
                list.Add(item);
            }
            var model = new PagedList<Entity.Site>(list, pageIndex.Value, pageSize.Value);
            return View(model);
        }

        [Authorize(Roles = "Site")]
        public ActionResult Edit(string id)
        {
            var model = this.siteService.FindSiteByID(id);

            return View(model);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Entity.Site model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            this.siteService.CreateOrUpdateSite(model, this.UserManager.GetCurrentUser().ID);
            return RedirectToAction("Index");
        }

        public ActionResult Details(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var model = this.siteService.FindSiteByID(id);
            return View(model);
        }
        // GET: Tests/Delete/5
        public ActionResult Delete(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var model = this.siteService.FindSiteByID(id);
            return View(model);
        }

        // POST: Tests/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            this.siteService.DeleteSiteByID(id);
            return RedirectToAction("Index");
        }

        public ActionResult Bind(string id)
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Bind(string id, SearchViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            using (var db = OpenDatabase.GetDatabase())
            {
                var userCard = db.Entity<UserCard>().Query().Where(m => m.CardNumber, model.Name, CompareType.Equal)
                    .Where(m => m.IsUsed, false, CompareType.Equal)
                    .Where(m => m.Status, 0, CompareType.Equal)
                    .First();
                if (userCard == null)
                {
                    ModelState.AddModelError("", "Card is not Existed".Localize());
                    return View(model);
                }
                var user = db.Entity<User>().Query().Where(m => m.Id, id, CompareType.Equal).First();
                if (user == null)
                {
                    return View("Error");
                }
                userCard.UserID = id;
                userCard.IsUsed = true;
                userCard.Status = 1;
                db.Entity<UserCard>().Update(userCard);

                if (user.Grade < userCard.CardGrade)
                {
                    user.Grade = userCard.CardGrade;
                    db.Entity<User>().Update(user);
                }
                return View("Close");
            }
        }

    }
}