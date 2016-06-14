using System.Net;
using System.Web.Mvc;
using OpenData.Site.Entity;

namespace OpenData.Site.FrontPage.Areas.Sites.Controllers
{
    public class LayoutController : BaseSiteController
    {


        // GET: Site/Layout
        public ActionResult Index()
        {
            return View(db.Entity<SiteLayout>().Query().ToPageList(this.pageIndex, this.pageSize));
        }

        // GET: Site/Layout/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SiteLayout siteLayout = db.Entity<SiteLayout>().Query().Where(m => m.Id, id, Data.CompareType.Equal).First();
            if (siteLayout == null)
            {
                return HttpNotFound();
            }
            return View(siteLayout);
        }

        // GET: Site/Layout/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Site/Layout/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Name,Content,CreatedOn,UpdatedOn,CreatedBy,UpdatedBy,Status")] SiteLayout siteLayout)
        {
            if (ModelState.IsValid)
            {
                db.Entity<SiteLayout>().Insert(siteLayout);
                return RedirectToAction("Index");
            }

            return View(siteLayout);
        }

        // GET: Site/Layout/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SiteLayout siteLayout = db.Entity<SiteLayout>().Query().Where(m => m.Id, id, Data.CompareType.Equal).First();
            if (siteLayout == null)
            {
                return HttpNotFound();
            }
            return View(siteLayout);
        }

        // POST: Site/Layout/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name,Content,CreatedOn,UpdatedOn,CreatedBy,UpdatedBy,Status")] SiteLayout siteLayout)
        {
            if (ModelState.IsValid)
            {
                db.Entity<SiteLayout>().Update(siteLayout);
                return RedirectToAction("Index");
            }
            return View(siteLayout);
        }

        // GET: Site/Layout/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SiteLayout siteLayout = db.Entity<SiteLayout>().Query().Where(m => m.Id, id, Data.CompareType.Equal).First();
            if (siteLayout == null)
            {
                return HttpNotFound();
            }
            return View(siteLayout);
        }

        // POST: Site/Layout/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            db.Entity<SiteLayout>().Delete(id);
            return RedirectToAction("Index");
        }

     
    }
}
