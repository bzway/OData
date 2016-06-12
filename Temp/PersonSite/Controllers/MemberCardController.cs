//using Bzway.Business.Model;
//using Bzway.Data;
//using Bzway.Data.Mongo;
//using Bzway.Data.Query.OpenExpressions;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Net;
//using System.Web;
//using System.Web.Mvc;
//using Bzway.Globalization;
//using Bzway.Web.Mvc;
//using Bzway.Web.Mvc.Paging;
//using Bzway.Business.Service;

//namespace Bzway.WebSite.WebApp.Controllers
//{
//    public class MemberCardController : BzwayController
//    {
//        UserService UserService = ApplicationEngine.Current.Resolve<UserService>();

//        public ActionResult Index(int? pageIndex, int? pageSize)
//        {
//            pageIndex = pageIndex ?? 1;
//            pageSize = pageSize ?? 10;
//            SearchViewModel model = new SearchViewModel();
//            using (var db = Database.Defualt())
//            {
//                model.SearchResult = db.Entity("User").Query().ToPageList(pageIndex.Value, pageSize.Value);
//                return View(model);
//            }
//        }
//        [HttpPost]
//        [ValidateAntiForgeryToken]
//        public ActionResult Index(MemberSearchViewModel model)
//        {
//            if (!ModelState.IsValid)
//            {
//                return View(model);
//            }

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
//    }
//}