using System;
using System.Data;
using System.IO;
using System.Net;
using System.Web.Mvc;
using OpenData.Site.Core;
using OpenData.Site.FrontPage.Models;
using OpenData.Site.Entity;
using OpenData.Data.Core;
using OpenData.Utility;
using OpenData.Globalization;

namespace OpenData.Site.FrontPage.Controllers.App
{
    public class MemberController : BaseController
    {

        [Authorize(Roles = "Site")]
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Site")]
        public ActionResult Index(MemberSearchViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            //memberService.SearchMember(model);
            return View(model);
        }
        public ActionResult Create()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(OpenData.Site.Entity.User model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            using (var db = OpenDatabase.GetDatabase())
            {
                db.Entity<User>().Insert(model);
                return View("Close");
            }
        }

        public ActionResult Edit(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            using (var db = OpenDatabase.GetDatabase())
            {
                var model = db.Entity<User>().Query().Where(m => m.Id, this.UserManager.GetCurrentUser().DecryptAES(id), CompareType.Equal).First();
                if (model == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }

                return View(model);
            }
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(OpenData.Site.Entity.User model, string id)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            if (string.IsNullOrEmpty(id))
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            using (var db = OpenDatabase.GetDatabase())
            {
                var entity = db.Entity<User>().Query().Where(m => m.Id, id, CompareType.Equal).First();
                if (entity == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                entity.Name = model.Name;

                entity.Gender = model.Gender;
                entity.Grade = model.Grade;

                entity.Birthday = model.Birthday;
                entity.IsLunarBirthday = model.IsLunarBirthday;

                entity.IsConfirmed = model.IsConfirmed;
                entity.IsLocked = model.IsLocked;

                entity.LockedTime = model.LockedTime;

                entity.Roles = model.Roles;


                entity.Country = model.Country;
                entity.Province = model.Province;
                entity.City = model.City;
                entity.Distinct = model.Distinct;

                db.Entity<User>().Update(entity);
                return View("Close");
            }
        }

        public ActionResult Details(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            using (var db = OpenDatabase.GetDatabase())
            {
                var model = db.Entity<User>().Query().Where(m => m.Id, id, CompareType.Equal).First();
                if (model == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }

                return View(model);
            }
        }

        // GET: Tests/Delete/5
        public ActionResult Delete(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            using (var db = OpenDatabase.GetDatabase())
            {
                var model = db.Entity<User>().Query().Where(m => m.Id, id, CompareType.Equal).First();
                if (model == null)
                {
                    return HttpNotFound();
                }

                return View(model);
            }
        }

        // POST: Tests/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            using (var db = OpenDatabase.GetDatabase())
            {
                db.Entity<User>().Delete(id);
                return View("Close");
            }
        }

        public ActionResult Bind(string id)
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Bind(string id, SearchViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            using (var db = OpenDatabase.GetDatabase())
            {
                var userCard = db.Entity<UserCard>().Query().Where(m => m.CardNumber, model.Name, CompareType.Equal)
                    .Where(m => m.IsUsed, false, CompareType.Equal)
                    //.Where(m => m.Status, EntityStatus.Initial, CompareType.Equal)
                    .First();
                if (userCard == null)
                {
                    ModelState.AddModelError("", "Card is not Existed".Localize());
                    return View(model);
                }
                var user = db.Entity<User>().Query().Where(m => m.Id, id, CompareType.Equal).First();
                if (user == null)
                {
                    return View("Error");
                }
                userCard.UserID = id;
                userCard.IsUsed = true;
                //userCard.Status = EntityStatus.Confirmed;
                db.Entity<UserCard>().Update(userCard);

                if (user.Grade < userCard.CardGrade)
                {
                    user.Grade = userCard.CardGrade;
                    db.Entity<User>().Update(user);
                }
                return View("Close");
            }
        }

        [Authorize(Roles = "Site")]
        public ActionResult Import()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Site")]
        public ActionResult Import(ImportFileViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            if (this.Request.Files.Count == 0)
            {
                this.ModelState.AddModelError("", "No file uploaded".Localize(this.SiteManager.GetSite().Name));
                return View(model);
            }
            var file = this.Request.Files[0];
            if (file == null || file.ContentLength == 0)
            {
                this.ModelState.AddModelError("", "No file uploaded".Localize(this.SiteManager.GetSite().Name));
                return View(model);
            }
            string originalName = file.FileName;
            string fileExtension = Path.GetExtension(originalName).ToLower();

            if (fileExtension != ".xls" && fileExtension != ".xlsx" && fileExtension != ".csv")
            {
                this.ModelState.AddModelError("", "File Format is not supported".Localize(this.SiteManager.GetSite().Name));
                return View(model);
            }
            //限制上传大小为20M
            int size = 20 * 1024 * 1024;
            if (file.ContentLength > size)
            {
                this.ModelState.AddModelError("", "File is too big to support".Localize(this.SiteManager.GetSite().Name));
                return View(model);
            }

            string dir = AppDomain.CurrentDomain.BaseDirectory + "UpLoad\\" + this.SiteManager.GetSite().Name + "\\";//上传文件目录
            if (!Directory.Exists(dir))
            {
                Directory.CreateDirectory(dir);
            }
            string fileName = String.Concat(dir, DateTime.Now.ToString("yyyyMMddHHmmssffff"), fileExtension);
            file.SaveAs(fileName);//保存上传文件
            DataSet ds;
            if (fileExtension == ".xls" || fileExtension == ".xlsx")
            {
                ds = ExcelHelper.GetDataFromExcel(fileName);
            }
            else
            {
                ds = new DataSet();
                ds.Tables.Add(CSVHelper.Import(fileName, true, ','));
            }
            if (ds == null || ds.Tables.Count == 0)
            {
                this.ModelState.AddModelError("", "No data in the file".Localize(this.SiteManager.GetSite().Name));
                return View(model);
            }
            this.SiteManager.GetMemberService().Import(ds, this.SiteManager.GetSite());
            return View("Close");
        }
    }
}