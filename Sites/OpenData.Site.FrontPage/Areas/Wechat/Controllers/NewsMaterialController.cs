using System.Net;
using System.Web.Mvc;
using OpenData.Site.Entity;
using OpenData.Data.Core;

namespace OpenData.Site.FrontPage.Areas.Wechats.Controllers
{
    public class NewsMaterialController : BaseWechatManageController
    {
     

        public ActionResult Index(string id)
        {
            var list = this.db.Entity<WechatNewsMaterial>().Query()
                .Where(m => m.OfficialAccount, this.CurrentOfficialAccount, CompareType.Equal)
                .Where(m => m.MaterialID, id, CompareType.Equal)
                .ToList();
            return View(list);
        }

        // GET: Wechat/NewsMaterial/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            WechatNewsMaterial wechatNewsMaterial = this.db.Entity<WechatNewsMaterial>().Query()
                .Where(m => m.Id, id, CompareType.Equal).First();
            if (wechatNewsMaterial == null)
            {
                return HttpNotFound();
            }
            return View(wechatNewsMaterial);
        }

        // GET: Wechat/NewsMaterial/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Wechat/NewsMaterial/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,OfficialAccount,MaterialID,MediaId,SortBy,Title,ThumbMediaId,ShowCoverPicture,Author,Digest,Content,Url,ContentSourceUrl,LastUpdateTime,CreatedOn,UpdatedOn,CreatedBy,UpdatedBy,Status")] WechatNewsMaterial wechatNewsMaterial)
        {
            if (ModelState.IsValid)
            {
                this.db.Entity<WechatNewsMaterial>().Insert(wechatNewsMaterial);
                return RedirectToAction("Index");
            }

            return View(wechatNewsMaterial);
        }

        // GET: Wechat/NewsMaterial/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            WechatNewsMaterial wechatNewsMaterial = this.db.Entity<WechatNewsMaterial>().Query()
                .Where(m => m.Id, id, CompareType.Equal)
                .First();
            if (wechatNewsMaterial == null)
            {
                return HttpNotFound();
            }
            return View(wechatNewsMaterial);
        }

        // POST: Wechat/NewsMaterial/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,OfficialAccount,MaterialID,MediaId,SortBy,Title,ThumbMediaId,ShowCoverPicture,Author,Digest,Content,Url,ContentSourceUrl,LastUpdateTime,CreatedOn,UpdatedOn,CreatedBy,UpdatedBy,Status")] WechatNewsMaterial wechatNewsMaterial)
        {
            if (ModelState.IsValid)
            {
                this.db.Entity<WechatNewsMaterial>().Update(wechatNewsMaterial);
                return RedirectToAction("Index");
            }
            return View(wechatNewsMaterial);
        }

        // GET: Wechat/NewsMaterial/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            WechatNewsMaterial wechatNewsMaterial = this.db.Entity<WechatNewsMaterial>().Query().Where(m => m.Id, id, CompareType.Equal).First();
            if (wechatNewsMaterial == null)
            {
                return HttpNotFound();
            }
            return View(wechatNewsMaterial);
        }

        // POST: Wechat/NewsMaterial/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            this.db.Entity<WechatNewsMaterial>().Delete(id);
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
