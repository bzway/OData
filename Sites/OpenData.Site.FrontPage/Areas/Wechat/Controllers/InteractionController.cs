using System.Net;
using System.Web.Mvc;
using OpenData.Site.Entity;

namespace OpenData.Site.FrontPage.Areas.Wechats.Controllers
{
    public class InteractionController : BaseWechatManageController
    {

        // GET: Wechat/Interaction
        public ActionResult Index()
        {
            return View(db.Entity<WechatUserInteraction>().Query().ToList());
        }

        // GET: Wechat/Interaction/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            WechatUserInteraction wechatUserInteraction = db.Entity<WechatUserInteraction>().Query().Where(m => m.Id, id, Data.CompareType.Equal).First();
            if (wechatUserInteraction == null)
            {
                return HttpNotFound();
            }
            return View(wechatUserInteraction);
        }

        // GET: Wechat/Interaction/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Wechat/Interaction/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,OfficialAccount,OpenId,Type,MsgId,Content,Remark,CreatedOn,UpdatedOn,CreatedBy,UpdatedBy,Status")] WechatUserInteraction wechatUserInteraction)
        {
            if (ModelState.IsValid)
            {
                db.Entity<WechatUserInteraction>().Insert(wechatUserInteraction);
                return RedirectToAction("Index");
            }

            return View(wechatUserInteraction);
        }

        // GET: Wechat/Interaction/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            WechatUserInteraction wechatUserInteraction = db.Entity<WechatUserInteraction>().Query().Where(m => m.Id, id, Data.CompareType.Equal).First();
            if (wechatUserInteraction == null)
            {
                return HttpNotFound();
            }
            return View(wechatUserInteraction);
        }

        // POST: Wechat/Interaction/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,OfficialAccount,OpenId,Type,MsgId,Content,Remark,CreatedOn,UpdatedOn,CreatedBy,UpdatedBy,Status")] WechatUserInteraction wechatUserInteraction)
        {
            if (ModelState.IsValid)
            {
                db.Entity<WechatUserInteraction>().Update(wechatUserInteraction);
                return RedirectToAction("Index");
            }
            return View(wechatUserInteraction);
        }

        // GET: Wechat/Interaction/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            WechatUserInteraction wechatUserInteraction = db.Entity<WechatUserInteraction>().Query().Where(m => m.Id, id, Data.CompareType.Equal).First();
            if (wechatUserInteraction == null)
            {
                return HttpNotFound();
            }
            return View(wechatUserInteraction);
        }

        // POST: Wechat/Interaction/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            db.Entity<WechatUserInteraction>().Delete(id);
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
