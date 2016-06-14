using System.Net;
using System.Web.Mvc;
using OpenData.Data.Core;
using OpenData.Framework.Core;
using OpenData.Framework.Core.Entity;

namespace OpenData.Sites.FrontPage.Areas.Wechats.Controllers
{
    public class MaterialController : BaseWechatManageController
    {
        public ActionResult Index(string id)
        {
            if (string.IsNullOrEmpty(id) && string.IsNullOrEmpty(this.CurrentOfficialAccount))
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            if (string.IsNullOrEmpty(this.CurrentOfficialAccount))
            {
                this.CurrentOfficialAccount = id;
            }
            var list = this.db.Entity<WechatMaterial>().Query()
                .Where(m => m.OfficialAccount, this.CurrentOfficialAccount, CompareType.Equal)
                .ToPageList(this.pageIndex, this.pageSize);
            return View(list);
        }



        // GET: Wechat/Material/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            WechatMaterial wechatMaterial = this.db.Entity<WechatMaterial>().Query().Where(m => m.Id, id, CompareType.Equal).First();
            if (wechatMaterial == null)
            {
                return HttpNotFound();
            }
            return View(wechatMaterial);
        }

        // GET: Wechat/Material/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Wechat/Material/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,OfficialAccount,Type,MediaId,Name,Description,Url,LastUpdateTime,IsReleased,CreatedOn,UpdatedOn,CreatedBy,UpdatedBy,Status")] WechatMaterial wechatMaterial)
        {
            if (ModelState.IsValid)
            {
                this.db.Entity<WechatMaterial>().Insert(wechatMaterial);
                return RedirectToAction("Index");
            }

            return View(wechatMaterial);
        }

        // GET: Wechat/Material/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            WechatMaterial wechatMaterial = this.db.Entity<WechatMaterial>().Query()
                .Where(m => m.Id, id, CompareType.Equal)
                .First();
            if (wechatMaterial == null)
            {
                return HttpNotFound();
            }
            return View(wechatMaterial);
        }

        // POST: Wechat/Material/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,OfficialAccount,Type,MediaId,Name,Description,Url,LastUpdateTime,IsReleased,CreatedOn,UpdatedOn,CreatedBy,UpdatedBy,Status")] WechatMaterial wechatMaterial)
        {
            if (ModelState.IsValid)
            {
                this.db.Entity<WechatMaterial>().Update(wechatMaterial);
                return RedirectToAction("Index");
            }
            return View(wechatMaterial);
        }

        // GET: Wechat/Material/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            WechatMaterial wechatMaterial = this.db.Entity<WechatMaterial>().Query()
                .Where(m => m.Id, id, CompareType.Equal)
                .First();
            if (wechatMaterial == null)
            {
                return HttpNotFound();
            }
            return View(wechatMaterial);
        }

        // POST: Wechat/Material/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            this.db.Entity<WechatMaterial>().Delete(id);
            return RedirectToAction("Index");
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Refresh()
        {
            if (ModelState.IsValid)
            {
                WechatManager wechatManager = new WechatManager(this.db, this.CurrentOfficialAccount);

                wechatManager.BatchGetMaterial(1, 1, WechatMaterialType.image);
                wechatManager.BatchGetMaterial(1, 1000, WechatMaterialType.news);
                wechatManager.BatchGetMaterial(1, 1, WechatMaterialType.video);
                wechatManager.BatchGetMaterial(1, 1000, WechatMaterialType.voice);
                wechatManager.DownloadOtherMaterial("sambJ-Im3jRWxxoRtqR78SLIb81JwG32HEV_mwzQSPw", "d:\test.jpg");
            }
            return RedirectToAction("Index");
        }
    }
}
