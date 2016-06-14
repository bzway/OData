using System.Net;
using System.Web.Mvc;
using OpenData.Framework.Entity;
using OpenData.Data;
using OpenData.Framework.Core;

namespace OpenData.Framework.WebApp.Areas.Wechats.Controllers
{
    public class UserController : BaseWechatManageController
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
            var list = this.db.Entity<WechatUser>().Query()
                .Where(m => m.OfficialAccount, this.CurrentOfficialAccount, CompareType.Equal)
                .ToPageList(this.pageIndex, this.pageSize);
            return View(list);
        }

        // GET: Wechat/WechatUser/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            WechatUser wechatUser = this.db.Entity<WechatUser>().Query()
                .Where(m => m.Id, id, CompareType.Equal)
                .First();
            if (wechatUser == null)
            {
                return HttpNotFound();
            }
            return View(wechatUser);
        }

        // GET: Wechat/WechatUser/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Wechat/WechatUser/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,OfficialAccount,OpenId,NickName,Sex,Province,City,Country,HeadImageUrl,Privilege,UnionId,GroupId,CreatedOn,UpdatedOn,CreatedBy,UpdatedBy,Status")] WechatUser wechatUser)
        {
            if (ModelState.IsValid)
            {
                this.db.Entity<WechatUser>().Insert(wechatUser);

                return RedirectToAction("Index");
            }

            return View(wechatUser);
        }

        // GET: Wechat/WechatUser/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            WechatUser wechatUser = this.db.Entity<WechatUser>().Query().Where(m => m.Id, id, CompareType.Equal).First();
            if (wechatUser == null)
            {
                return HttpNotFound();
            }
            return View(wechatUser);
        }

        // POST: Wechat/WechatUser/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,OfficialAccount,OpenId,NickName,Sex,Province,City,Country,HeadImageUrl,Privilege,UnionId,GroupId,CreatedOn,UpdatedOn,CreatedBy,UpdatedBy,Status")] WechatUser wechatUser)
        {
            if (ModelState.IsValid)
            {
                this.db.Entity<WechatUser>().Update(wechatUser);
                return RedirectToAction("Index");
            }
            return View(wechatUser);
        }

        // GET: Wechat/WechatUser/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            WechatUser wechatUser = this.db.Entity<WechatUser>().Query().Where(m => m.Id, id, CompareType.Equal).First();
            if (wechatUser == null)
            {
                return HttpNotFound();
            }
            return View(wechatUser);
        }

        // POST: Wechat/WechatUser/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            this.db.Entity<WechatUser>().Delete(id);
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

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Refresh()
        {
            if (ModelState.IsValid)
            {
                WechatManager wechatManager = new WechatManager(this.db, this.CurrentOfficialAccount);

                foreach (var openId in wechatManager.GetUserList())
                {
                    var wechatUser = wechatManager.GetUserInfo(openId);
                    var user = this.db.Entity<WechatUser>().Query()
                        .Where(m => m.OfficialAccount, this.CurrentOfficialAccount, CompareType.Equal)
                        .Where(m => m.OpenId, openId, CompareType.Equal)
                        .First();
                    if (user == null)
                    {
                        this.db.Entity<WechatUser>().Insert(wechatUser);
                    }
                    else
                    {
                        user.City = wechatUser.City;
                        user.Country = wechatUser.Country;
                        user.GroupId = wechatUser.GroupId;
                        user.HeadImageUrl = wechatUser.HeadImageUrl;
                        user.NickName = wechatUser.NickName;
                        user.OfficialAccount = wechatUser.OfficialAccount;
                        user.OpenId = wechatUser.OpenId;
                        user.Privilege = wechatUser.Privilege;
                        user.Province = wechatUser.Province;
                        user.Sex = wechatUser.Sex;
                        user.UnionId = wechatUser.UnionId;
                        user.Status = 1;
                        this.db.Entity<WechatUser>().Update(user);
                    }
                }
                return RedirectToAction("Index");
            }
            return RedirectToAction("Index");
        }
    }
}
