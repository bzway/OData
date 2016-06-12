using OpenData.Business.Model;
using OpenData.Business.Service;
using OpenData.Data;
using OpenData.Data.Mongo;
using OpenData.Globalization;
using OpenData.Utility;
using OpenData.Web.Mvc;
using System;
using System.Data;
using System.IO;
using System.Net;
using System.Web.Mvc;
using System.Linq;
using System.Web;
using System.Collections.Generic;
using OpenData.Web.Mvc.Paging;
using OpenData.Business.Entity;

namespace OpenData.WebSite.WebApp.Controllers
{
    public class SiteController : AdminBaseController
    {
        IMemberService memberService = ApplicationEngine.Current.Resolve<IMemberService>();
        ISiteService siteService = ApplicationEngine.Current.Resolve<ISiteService>();

        [BzwayAuthorize(Roles = "Site")]
        public ActionResult Index(int? pageIndex, int? pageSize)
        {
            pageIndex = pageIndex ?? 1;
            pageSize = pageSize ?? 10;

            var list = new List<ViewSite>();
            foreach (var item in this.siteService.FindSiteByUserID(this.UserManager.GetCurrentUser().ID))
            {
                list.Add(new ViewSite()
                {
                    ConnectionString = item.ConnectionString,
                    DatabaseName = item.DatabaseName,
                    Name = item.Name,
                    ProviderName = item.ProviderName,
                });
            }
            SiteSearchViewModel model = new SiteSearchViewModel() { SearchResult = new PagedList<ViewSite>(list, pageIndex.Value, pageSize.Value) };
            return View(model);
        }



        [BzwayAuthorize(Roles = "Site")]
        public ActionResult Edit(string id)
        {
            var site = this.siteService.FindSiteByID(id);
            if (site == null)
            {
                return View();
            }
            var model = new UserSiteViewModel()
            {
                ConnectionString = site.ConnectionString,
                DatabaseName = site.DatabaseName,
                Name = site.Name,
                ProviderName = site.ProviderName,
                ID = site.UUID,
                Domains = site.Domains,
            };
            return View(model);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(ViewSite model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var site = model.Get<Site>();
            this.siteService.CreateOrUpdateSite(site, this.UserManager.GetCurrentUser().ID);


            return View("Close");
        }

        public ActionResult Details(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            using (var db = DefaultDatabase.GetDatabase())
            {
                var entity = db.Entity<User>().Query().Where(m => m.UUID, id, CompareType.Equal).First();
                if (entity == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                var model = new ViewUser()
                {
                    Name = entity.Name,
                    Gender = entity.Gender,
                    Grade = entity.Grade,
                    Birthday = entity.Birthday,
                    IsLunarBirthday = entity.IsLunarBirthday,
                    IsConfirmed = entity.IsConfirmed,
                    IsLocked = entity.IsLocked,
                    LockedTime = entity.LockedTime,
                    Country = entity.Country,
                    Province = entity.Province,
                    City = entity.City,
                    Distinct = entity.Distinct,
                };
                return View(model);
            }
        }

        // GET: Tests/Delete/5
        public ActionResult Delete(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            using (var db = DefaultDatabase.GetDatabase())
            {
                var entity = db.Entity<User>().Query().Where(m => m.UUID, id, CompareType.Equal).First();
                if (entity == null)
                {
                    return HttpNotFound();
                }
                var model = new ViewUser()
                {
                    Birthday = entity.Birthday,
                    City = entity.City,
                    Country = entity.Country,
                    Distinct = entity.Distinct,
                    Gender = entity.Gender,
                    Grade = entity.Grade,
                    IsConfirmed = entity.IsConfirmed,
                    IsLocked = entity.IsLocked,
                    IsLunarBirthday = entity.IsLunarBirthday,
                    LockedTime = entity.LockedTime,
                    Name = entity.Name,
                    NickName = entity.NickName,
                    Password = entity.Password,
                    UserName = entity.UserName,
                    Province = entity.Province,
                    Roles = entity.Roles,
                };
                return View(model);
            }
        }

        // POST: Tests/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            using (var db = DefaultDatabase.GetDatabase())
            {
                db.Entity<User>().Delete(new EntityBase() { UUID = id });
                return View("Close");
            }
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
            using (var db = DefaultDatabase.GetDatabase())
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
                var user = db.Entity<User>().Query().Where(m => m.UUID, id, CompareType.Equal).First();
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