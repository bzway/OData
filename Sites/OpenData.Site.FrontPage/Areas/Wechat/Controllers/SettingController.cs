using OpenData.Site.Entity;
using OpenData.Site.Core;
using OpenData.Data.Core;
using System;
using System.Net;
using System.Web.Mvc;
using OpenData.Globalization;

namespace OpenData.Site.FrontPage.Areas.Wechats.Controllers
{
    public class SettingController : BaseWechatManageController
    {

        private void Any(string id)
        {
            var wechat = this.db.Entity<WechatOfficialAccount>().Query()
                .Where(m => m.Id, id, CompareType.Equal).First();
            if (wechat == null)
            {
                return;
            }
            WechatManager mgt = new WechatManager(this.db, id);
            mgt.GetUserCumulate(DateTime.Now.AddDays(-2), DateTime.Now.AddDays(-1));
            var list = mgt.GetWechatIpList();
            ////同步分组
            //foreach (var item in mgt.GetGroupList())
            //{
            //    item.OfficialAccount = id;
            //    this.db.Entity<WechatUserGroup>().Insert(item);
            //}
            ////同步粉丝
            //foreach (var item in mgt.GetUserList())
            //{
            //    var user = mgt.GetUserInfo(item);
            //    user.OfficialAccount = id;
            //    this.db.Entity<WechatUser>().Insert(user);
            //}
            ////同步二维码
            //for (int i = 0; i < 100000; i++)
            //{
            //    var sceneString = Guid.NewGuid().ToString("N");
            //    var qrCode = mgt.CreateLimitSceneQRCode(sceneString);
            //    this.db.Entity<WechatQRCode>().Insert(qrCode);
            //}
            //同步素材
            mgt.GetMenu();

        }

        #region Setting

        // GET: Admin/Default
        public ActionResult Index(string id)
        {
            if (!string.IsNullOrEmpty(id))
            {
                this.CurrentOfficialAccount = id;
            }
            ViewBag.CurrentOfficialAccount = this.CurrentOfficialAccount;
            if (string.IsNullOrEmpty(this.CurrentOfficialAccount))
            {
                this.Danger("Please select on wechat official account".Localize(), true);
            }

            return View(db.Entity<WechatOfficialAccount>().Query().ToPageList(0, 12));
        }

        // GET: WechatSettings/Details/5
        public ActionResult Details(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            WechatOfficialAccount wechatSetting = db.Entity<WechatOfficialAccount>().Query().Where(m => m.Id, id, CompareType.Equal).First();
            if (wechatSetting == null)
            {
                return HttpNotFound();
            }
            return View(wechatSetting);
        }

        // GET: WechatSettings/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: WechatSettings/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(WechatOfficialAccount wechatSetting)
        {
            if (ModelState.IsValid)
            {
                var id = Guid.NewGuid().ToString("N");
                wechatSetting.Id = id;
                this.db.Entity<WechatOfficialAccount>().Insert(wechatSetting);
                this.Any(id);
                return RedirectToAction("Index");
            }

            return View(wechatSetting);
        }

        // GET: WechatSettings/Edit/5
        public ActionResult Edit(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            WechatOfficialAccount wechatSetting = this.db.Entity<WechatOfficialAccount>().Query()
                .Where(m => m.Id, id, CompareType.Equal).First();
            if (wechatSetting == null)
            {
                return HttpNotFound();
            }
            return View(wechatSetting);
        }

        // POST: WechatSettings/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(WechatOfficialAccount wechatSetting)
        {
            if (ModelState.IsValid)
            {
                this.db.Entity<WechatOfficialAccount>().Update(wechatSetting);
                return RedirectToAction("Index");
            }
            return View(wechatSetting);
        }

        // GET: WechatSettings/Delete/5
        public ActionResult Delete(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            WechatOfficialAccount wechatSetting = this.db.Entity<WechatOfficialAccount>().Query().Where(m => m.Id, id, CompareType.Equal).First();
            if (wechatSetting == null)
            {
                return HttpNotFound();
            }
            return View(wechatSetting);
        }

        // POST: WechatSettings/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            this.db.Entity<WechatOfficialAccount>().Delete(id);
            return RedirectToAction("Index");
        }
        #endregion

    }
}