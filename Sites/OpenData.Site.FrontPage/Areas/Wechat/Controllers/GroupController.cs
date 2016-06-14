using OpenData.Site.Entity;
using OpenData.Data;
using System;
using System.Net;
using System.Web.Mvc;

namespace OpenData.Site.FrontPage.Areas.Wechats.Controllers
{
    public class GroupController : BaseWechatManageController
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
            var list = this.db.Entity<WechatUserGroup>().Query()
                .Where(m => m.OfficialAccount, this.CurrentOfficialAccount, CompareType.Equal)
                .ToList();
            return View(list);
        }


        public ActionResult Details(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            if (string.IsNullOrEmpty(this.CurrentOfficialAccount))
            {
                return RedirectToAction("Index");
            }
            string account = this.CurrentOfficialAccount;
            WechatUserGroup model = db.Entity<WechatUserGroup>().Query()
                .Where(m => m.OfficialAccount, account, CompareType.Equal)
                .Where(m => m.Id, id, CompareType.Equal)
                .First();
            if (model == null)
            {
                return HttpNotFound();
            }
            return View(model);
        }

        public ActionResult Create()
        {
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(WechatUserGroup model)
        {
            if (string.IsNullOrEmpty(this.CurrentOfficialAccount))
            {
                return RedirectToAction("Index");
            }
            if (ModelState.IsValid)
            {
                var uuid = Guid.NewGuid().ToString("N");
                model.Id = uuid;
                model.OfficialAccount = this.CurrentOfficialAccount;
                this.db.Entity<WechatUserGroup>().Insert(model);
                return RedirectToAction("Index");
            }
            return View(model);
        }


        public ActionResult Edit(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            if (string.IsNullOrEmpty(this.CurrentOfficialAccount))
            {
                return RedirectToAction("Index");
            }
            var model = this.db.Entity<WechatUserGroup>().Query()
                .Where(m => m.OfficialAccount, this.CurrentOfficialAccount, CompareType.Equal)
                .Where(m => m.Id, id, CompareType.Equal)
                .First();
            if (model == null)
            {
                return HttpNotFound();
            }
            return View(model);
        }

        // POST: WechatSettings/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(WechatUserGroup model)
        {
            if (ModelState.IsValid)
            {
                this.db.Entity<WechatUserGroup>().Update(model);
                return RedirectToAction("Index");
            }
            return View(model);
        }

        public ActionResult Delete(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var model = this.db.Entity<WechatUserGroup>().Query()
                .Where(m => m.OfficialAccount, this.CurrentOfficialAccount, CompareType.Equal)
                .Where(m => m.Id, id, CompareType.Equal)
                .First();
            if (model == null)
            {
                return HttpNotFound();
            }
            return View(model);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            this.db.Entity<WechatUserGroup>().Delete(id);
            return RedirectToAction("Index");
        }
    }
}