using System;
using System.Net;
using System.Web;
using System.Web.Mvc;
using OpenData.Data.Core;
using OpenData.Framework.Core.Entity;

namespace OpenData.Module.EBook.Controllers
{
    public class PageController : BaseSiteController
    {

        public ActionResult Index()
        {
            var list = this.Site.GetSiteDataBase().Entity<SitePage>().Query().ToList();
            return View(list);
        }
        // GET: Site/Page/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SitePage sitePage = this.Site.GetSiteDataBase().Entity<SitePage>().Query().Where(mbox => mbox.Id, id, CompareType.Equal).First();
            if (sitePage == null)
            {
                return HttpNotFound();
            }
            return View(sitePage);
        }

        // GET: Site/Page/Create
        public ActionResult Create()
        {
            SitePage sitePage = new SitePage()
            {
                EnableScript = false,
                EnableTheming = false,
                CreatedBy = this.User.GetCurrentUser().ID,
                CreatedOn = DateTime.UtcNow,
                UpdatedBy = this.User.GetCurrentUser().ID,
                UpdatedOn = DateTime.UtcNow,
                AllowedRole = string.Empty,
                Author = string.Empty,
                Canonical = string.Empty,
                Content = string.Empty,
                Customs = string.Empty,
                DeniedRole = string.Empty,
                Description = string.Empty,
                DisplayText = string.Empty,
                Duration = 0,
                FileExtension = ".cshtml",
                HtmlTitle = "无标题",
                Keywords = string.Empty,
                LinkTarget = TargetType._self,
                OutputCache = false,
                Title = "无标题",
                MasterVirtualPath = string.Empty,
                PageUrl = string.Empty,
                Published = true,
                ShowInCrumb = true,
                ShowInNavigation = true,
                SortBy = 0,
                Status = 0,
                Name = string.Empty,
            };
            return View(sitePage);
        }

        // POST: Site/Page/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,SiteID,FileExtension,Layout,VirtualPath,ExternalUrl,LinkTarget,MasterVirtualPath,PageUrl,PageType,OutputCache,Duration,Published,ContentTitle,Searchable,EnableTheming,EnableScript,Author,Keywords,Description,Customs,HtmlTitle,Canonical,ShowInNavigation,DisplayText,SortBy,ShowInCrumb,AllowedRole,DeniedRole,CreatedOn,UpdatedOn,CreatedBy,UpdatedBy,Status")] SitePage sitePage)
        {
            if (ModelState.IsValid)
            {
                //sitePage.Content = HttpUtility.HtmlDecode(sitePage.Content);
                this.Site.GetSiteDataBase().Entity<SitePage>().Insert(sitePage);
                return RedirectToAction("Index");
            }

            return View(sitePage);
        }

        // GET: Site/Page/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SitePage sitePage = this.Site.GetSiteDataBase().Entity<SitePage>().Query()
                .Where(mbox => mbox.Id, id, CompareType.Equal)
                .First();
            if (sitePage == null)
            {
                return HttpNotFound();
            }

            return View(sitePage);
        }

        // POST: Site/Page/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(SitePage sitePage)
        {
            if (ModelState.IsValid)
            {
                sitePage.Content = HttpUtility.HtmlDecode(sitePage.Content);

                this.Site.GetSiteDataBase().Entity<SitePage>().Update(sitePage);
                return RedirectToAction("Index");
            }
            return View(sitePage);
        }

        // GET: Site/Page/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SitePage sitePage = this.Site.GetSiteDataBase().Entity<SitePage>().Query().Where(mbox => mbox.Id, id, CompareType.Equal).First();
            if (sitePage == null)
            {
                return HttpNotFound();
            }
            return View(sitePage);
        }

        // POST: Site/Page/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            this.Site.GetSiteDataBase().Entity<SitePage>().Delete(id);
            return RedirectToAction("Index");
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Refresh()
        {

            return View();
        }
    }
}