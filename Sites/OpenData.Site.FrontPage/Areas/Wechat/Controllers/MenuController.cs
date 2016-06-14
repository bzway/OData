using System.Linq;
using System.Net;
using System.Web.Mvc;
using OpenData.Site.Entity;
using OpenData.Data.Core;
using OpenData.Site.Core;

namespace OpenData.Site.FrontPage.Areas.Wechats.Controllers
{
    public class MenuController : BaseWechatManageController
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
            var list = this.db.Entity<WechatMenu>().Query()
                .Where(m => m.OfficialAccount, this.CurrentOfficialAccount, CompareType.Equal)
                .ToList();
            return View(list);
        }

        // GET: Wechat/Menu/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            WechatMenu wechatMenu = this.db.Entity<WechatMenu>().Query()
                .Where(m => m.Id, id, CompareType.Equal).First();
            if (wechatMenu == null)
            {
                return HttpNotFound();
            }
            return View(wechatMenu);
        }

        // GET: Wechat/Menu/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Wechat/Menu/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,OfficialAccount,ParentId,Type,Name,Key,MediaId,Url,CreatedOn,UpdatedOn,CreatedBy,UpdatedBy,Status")] WechatMenu wechatMenu)
        {
            if (ModelState.IsValid)
            {
                this.db.Entity<WechatMenu>().Insert(wechatMenu);
                return RedirectToAction("Index");
            }

            return View(wechatMenu);
        }

        // GET: Wechat/Menu/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            WechatMenu wechatMenu = this.db.Entity<WechatMenu>().Query()
                .Where(m => m.Id, id, CompareType.Equal)
                .First();
            if (wechatMenu == null)
            {
                return HttpNotFound();
            }
            return View(wechatMenu);
        }

        // POST: Wechat/Menu/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,OfficialAccount,ParentId,Type,Name,Key,MediaId,Url,CreatedOn,UpdatedOn,CreatedBy,UpdatedBy,Status")] WechatMenu wechatMenu)
        {
            if (ModelState.IsValid)
            {
                this.db.Entity<WechatMenu>().Update(wechatMenu);
                return RedirectToAction("Index");
            }
            return View(wechatMenu);
        }

        // GET: Wechat/Menu/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            WechatMenu wechatMenu = this.db.Entity<WechatMenu>().Query()
                .Where(m => m.Id, id, CompareType.Equal)
                .First();
            if (wechatMenu == null)
            {
                return HttpNotFound();
            }
            return View(wechatMenu);
        }

        // POST: Wechat/Menu/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            this.db.Entity<WechatMenu>().Delete(id);
            return RedirectToAction("Index");
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Refresh()
        {
            if (ModelState.IsValid)
            {
                WechatManager wechatManager = new WechatManager(this.db, this.CurrentOfficialAccount);

                wechatManager.GetMenu();

                return RedirectToAction("Index");
            }
            return RedirectToAction("Index");
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Publish()
        {
            if (ModelState.IsValid)
            {
                WechatManager wechatManager = new WechatManager(this.db, this.CurrentOfficialAccount);
                var list = this.db.Entity<WechatMenu>().Query()
               .Where(m => m.OfficialAccount, this.CurrentOfficialAccount, CompareType.Equal)
               .ToList().ToList();
                wechatManager.CreateMenu(list);

                return RedirectToAction("Index");
            }
            return RedirectToAction("Index");
        }
    }
}