using System.Net;
using System.Web.Mvc;
using OpenData.Framework.Entity;
using OpenData.Data;
using OpenData.Framework.Core;

namespace OpenData.Framework.WebApp.Areas.Wechats.Controllers
{
    public class QRCodeController : BaseWechatManageController
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
            var list = this.db.Entity<WechatQRCode>().Query()
                .Where(m => m.OfficialAccount, this.CurrentOfficialAccount, CompareType.Equal)
                .ToList();
            return View(list);
        }

        // GET: Wechat/QRCode/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            WechatQRCode wechatQRCode = this.db.Entity<WechatQRCode>().Query()
                .Where(m => m.Id, id, CompareType.Equal)
                .First();
            if (wechatQRCode == null)
            {
                return HttpNotFound();
            }
            return View(wechatQRCode);
        }


        // GET: Wechat/QRCode/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            WechatQRCode wechatQRCode = this.db.Entity<WechatQRCode>().Query().Where(m => m.Id, id, CompareType.Equal).First();
            if (wechatQRCode == null)
            {
                return HttpNotFound();
            }
            return View(wechatQRCode);
        }

        // POST: Wechat/QRCode/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,OfficialAccount,Ticket,ExpiredTime,Url,Scene,Content,IsUsed,OwnerID,CreatedOn,UpdatedOn,CreatedBy,UpdatedBy,Status")] WechatQRCode wechatQRCode)
        {
            if (ModelState.IsValid)
            {
                this.db.Entity<WechatQRCode>().Update(wechatQRCode);
                return RedirectToAction("Index");
            }
            return View(wechatQRCode);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Refresh()
        {
            if (ModelState.IsValid)
            {
                WechatManager wechatManager = new WechatManager(this.db, this.CurrentOfficialAccount);
                var qrCode = wechatManager.CreateLimitSceneQRCode();
            }
            return RedirectToAction("Index");
        }
    }
}
