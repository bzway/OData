//using Bzway.Business.Model;
//using Bzway.Business.Service;
//using Bzway.Data;
//using Bzway.Data.Mongo;
//using Bzway.Globalization;
//using Bzway.Utility;
//using Bzway.Web.Mvc;
//using System;
//using System.Data;
//using System.IO;
//using System.Net;
//using System.Web.Mvc;
//using Bzway.Business.Service;
//using System.Web;
//namespace Bzway.WebSite.WebApp.Controllers
//{
//    public class MemberController : BzwayController
//    {
//        MemberService memberService
//        {
//            get
//            {
//                return ApplicationEngine.Current.Resolve<MemberService>();
//            }
//        }

//        SiteManager SiteManager
//        {
//            get
//            {
//                return this.HttpContext.GetOwinContext().GetSiteManager();
//            }
//        }
//        UserManager userManager
//        {
//            get
//            {
//                return this.HttpContext.GetOwinContext().GetUserManager();
//            }
//        }
//        [BzwayAuthorize(Roles = "Site")]
//        public ActionResult Index()
//        {
//            return View();
//        }

//        [HttpPost]
//        [ValidateAntiForgeryToken]
//        [BzwayAuthorize(Roles = "Site")]
//        public ActionResult Index(MemberSearchViewModel model)
//        {
//            if (!ModelState.IsValid)
//            {
//                return View(model);
//            }
//            memberService.SearchMember(model);
//            return View(model);
//        }
//        public ActionResult Create()
//        {
//            return View();
//        }
//        [HttpPost]
//        [ValidateAntiForgeryToken]
//        public ActionResult Create(ViewUser model)
//        {
//            if (!ModelState.IsValid)
//            {
//                return View(model);
//            }
//            using (var db = Database.Defualt())
//            {
//                db.Entity<User>().Insert(model.GetEntity<User>());
//                return View("Close");
//            }
//        }

//        public ActionResult Edit(string id)
//        {
//            if (string.IsNullOrEmpty(id))
//            {
//                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
//            }

//            using (var db = Database.Defualt())
//            {
//                var entity = db.Entity<User>().Query().Where(m => m.UUID, this.userManager.GetCurrentUser().DecryptAES(id), CompareType.Equal).First();
//                if (entity == null)
//                {
//                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
//                }
//                var model = new ViewUser()
//                {
//                    Name = entity.Name,
//                    Gender = entity.Gender,
//                    Grade = entity.Grade,
//                    Birthday = entity.Birthday,
//                    IsLunarBirthday = entity.IsLunarBirthday,
//                    IsConfirmed = entity.IsConfirmed,
//                    IsLocked = entity.IsLocked,
//                    LockedTime = entity.LockedTime,
//                    Country = entity.Country,
//                    Province = entity.Province,
//                    City = entity.City,
//                    Roles = entity.Roles,
//                    NickName = entity.NickName,
//                    OpenID = entity.OpenID,
//                    UserName = entity.UserName,
//                    Distinct = entity.Distinct,

//                };
//                return View(model);
//            }
//        }
//        [HttpPost]
//        [ValidateAntiForgeryToken]
//        public ActionResult Edit(ViewUser model, string id)
//        {
//            if (!ModelState.IsValid)
//            {
//                return View(model);
//            }
//            if (string.IsNullOrEmpty(id))
//            {
//                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
//            }

//            using (var db = Database.Defualt())
//            {
//                var entity = db.Entity<User>().Query().Where(m => m.UUID, id, CompareType.Equal).First();
//                if (entity == null)
//                {
//                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
//                }
//                entity.Name = model.Name;

//                entity.Gender = model.Gender;
//                entity.Grade = model.Grade;

//                entity.Birthday = model.Birthday;
//                entity.IsLunarBirthday = model.IsLunarBirthday;

//                entity.IsConfirmed = model.IsConfirmed;
//                entity.IsLocked = model.IsLocked;

//                entity.LockedTime = model.LockedTime;

//                entity.Roles = model.Roles;


//                entity.Country = model.Country;
//                entity.Province = model.Province;
//                entity.City = model.City;
//                entity.Distinct = model.Distinct;

//                db.Entity<User>().Update(entity);
//                return View("Close");
//            }
//        }

//        public ActionResult Details(string id)
//        {
//            if (string.IsNullOrEmpty(id))
//            {
//                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
//            }

//            using (var db = Database.Defualt())
//            {
//                var entity = db.Entity<User>().Query().Where(m => m.UUID, id, CompareType.Equal).First();
//                if (entity == null)
//                {
//                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
//                }
//                var model = new ViewUser()
//                {
//                    Name = entity.Name,
//                    Gender = entity.Gender,
//                    Grade = entity.Grade,
//                    Birthday = entity.Birthday,
//                    IsLunarBirthday = entity.IsLunarBirthday,
//                    IsConfirmed = entity.IsConfirmed,
//                    IsLocked = entity.IsLocked,
//                    LockedTime = entity.LockedTime,
//                    Country = entity.Country,
//                    Province = entity.Province,
//                    City = entity.City,
//                    Distinct = entity.Distinct,
//                };
//                return View(model);
//            }
//        }

//        // GET: Tests/Delete/5
//        public ActionResult Delete(string id)
//        {
//            if (string.IsNullOrEmpty(id))
//            {
//                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
//            }
//            using (var db = Database.Defualt())
//            {
//                var entity = db.Entity<User>().Query().Where(m => m.UUID, id, CompareType.Equal).First();
//                if (entity == null)
//                {
//                    return HttpNotFound();
//                }
//                var model = new ViewUser()
//                {
//                    Birthday = entity.Birthday,
//                    City = entity.City,
//                    Country = entity.Country,
//                    Distinct = entity.Distinct,
//                    Gender = entity.Gender,
//                    Grade = entity.Grade,
//                    IsConfirmed = entity.IsConfirmed,
//                    IsLocked = entity.IsLocked,
//                    IsLunarBirthday = entity.IsLunarBirthday,
//                    LockedTime = entity.LockedTime,
//                    Name = entity.Name,
//                    NickName = entity.NickName,
//                    Password = entity.Password,
//                    UserName = entity.UserName,
//                    Province = entity.Province,
//                    Roles = entity.Roles,
//                };
//                return View(model);
//            }
//        }

//        // POST: Tests/Delete/5
//        [HttpPost, ActionName("Delete")]
//        [ValidateAntiForgeryToken]
//        public ActionResult DeleteConfirmed(string id)
//        {
//            using (var db = Database.Defualt())
//            {
//                db.Entity<User>().Delete(new EntityBase() { UUID = id });
//                return View("Close");
//            }
//        }

//        public ActionResult Bind(string id)
//        {
//            return View();
//        }
//        [HttpPost]
//        [ValidateAntiForgeryToken]
//        public ActionResult Bind(string id, SearchViewModel model)
//        {
//            if (!ModelState.IsValid)
//            {
//                return View(model);
//            }
//            using (var db = Database.Defualt())
//            {
//                var userCard = db.Entity<UserCard>().Query().Where(m => m.CardNumber, model.Name, CompareType.Equal)
//                    .Where(m => m.IsUsed, false, CompareType.Equal)
//                    .Where(m => m.Status, EntityStatus.Initial, CompareType.Equal)
//                    .First();
//                if (userCard == null)
//                {
//                    ModelState.AddModelError("", "Card is not Existed".Localize());
//                    return View(model);
//                }
//                var user = db.Entity<User>().Query().Where(m => m.UUID, id, CompareType.Equal).First();
//                if (user == null)
//                {
//                    return View("Error");
//                }
//                userCard.UserID = id;
//                userCard.IsUsed = true;
//                userCard.Status = EntityStatus.Confirmed;
//                db.Entity<UserCard>().Update(userCard);

//                if (user.Grade < userCard.CardGrade)
//                {
//                    user.Grade = userCard.CardGrade;
//                    db.Entity<User>().Update(user);
//                }
//                return View("Close");
//            }
//        }

//        [BzwayAuthorize(Roles = "Site")]
//        public ActionResult Import()
//        {
//            return View();
//        }

//        [HttpPost]
//        [ValidateAntiForgeryToken]
//        [BzwayAuthorize(Roles = "Site")]
//        public ActionResult Import(ImportFileViewModel model)
//        {
//            if (!ModelState.IsValid)
//            {
//                return View(model);
//            }
//            if (this.Request.Files.Count == 0)
//            {
//                this.ModelState.AddModelError("", "No file uploaded".Localize(this.SiteManager.GetCurrentSite().Name));
//                return View(model);
//            }
//            var file = this.Request.Files[0];
//            if (file == null || file.ContentLength == 0)
//            {
//                this.ModelState.AddModelError("", "No file uploaded".Localize(this.SiteManager.GetCurrentSite().Name));
//                return View(model);
//            }
//            string originalName = file.FileName;
//            string fileExtension = Path.GetExtension(originalName).ToLower();

//            if (fileExtension != ".xls" && fileExtension != ".xlsx" && fileExtension != ".csv")
//            {
//                this.ModelState.AddModelError("", "File Format is not supported".Localize(this.SiteManager.GetCurrentSite().Name));
//                return View(model);
//            }
//            //限制上传大小为20M
//            int size = 20 * 1024 * 1024;
//            if (file.ContentLength > size)
//            {
//                this.ModelState.AddModelError("", "File is too big to support".Localize(this.SiteManager.GetCurrentSite().Name));
//                return View(model);
//            }

//            string dir = AppDomain.CurrentDomain.BaseDirectory + "UpLoad\\" + this.SiteManager.GetCurrentSite().Name + "\\";//上传文件目录
//            if (!Directory.Exists(dir))
//            {
//                Directory.CreateDirectory(dir);
//            }
//            string fileName = String.Concat(dir, DateTime.Now.ToString("yyyyMMddHHmmssffff"), fileExtension);
//            file.SaveAs(fileName);//保存上传文件
//            DataSet ds;
//            if (fileExtension == ".xls" || fileExtension == ".xlsx")
//            {
//                ds = ExcelHelper.GetDataFromExcel(fileName);
//            }
//            else
//            {
//                ds = new DataSet();
//                ds.Tables.Add(CSVHelper.Import(fileName, true, ','));
//            }
//            if (ds == null || ds.Tables.Count == 0)
//            {
//                this.ModelState.AddModelError("", "No data in the file".Localize(this.SiteManager.GetCurrentSite().Name));
//                return View(model);
//            }
//            memberService.Import(ds, this.SiteManager.GetCurrentSite());
//            return View("Close");
//        }
//    }
//}