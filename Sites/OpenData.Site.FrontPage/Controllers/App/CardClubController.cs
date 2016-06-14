using OpenData.Data.Core;
using System;
using System.Web.Mvc;
using OpenData.Globalization;
using OpenData.Utility;
using OpenData.Sites.FrontPage.Models;
using OpenData.Framework.Core;
using OpenData.Framework.Core.Entity;

namespace OpenData.Sites.FrontPage.Controllers.App
{
    public class CardClubController : BzwayController
    {
        [Authorize(Roles = "CardClubManager,CardClubStore")]
        public ActionResult Index()
        {
            return View();
        }

        [Authorize(Roles = "CardClubManager")]
        public ActionResult Member()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "CardClubManager")]
        public ActionResult Member(CardSearchViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var db = OpenDatabase.GetDatabase();
            var userCard = db.Entity<UserCard>().Query().Where(m => m.CardNumber, model.CardNumber.Trim(), CompareType.Equal).First();
            if (userCard == null)
            {
                this.ModelState.AddModelError("", "Card does not exist".Localize("CardClub"));
                return View();
            }
            var user = db.Entity<User>().Query().Where(m => m.Id, userCard.UserID, CompareType.Equal).First();
            if (user == null)
            {
                this.ModelState.AddModelError("", "User does not exist".Localize("CardClub"));
                return View();
            }
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var userEmail = db.Entity<UserEmail>().Query().Where(m => m.UserID, user.Id, CompareType.Equal).First();
            var userPhone = db.Entity<UserPhone>().Query().Where(m => m.UserID, user.Id, CompareType.Equal).First();
            CardSearchResultViewModel x = new CardSearchResultViewModel()
            {
                Name = user.Name,
                CardNumber = userCard.CardNumber,
                Birthday = user.Birthday,
                Gender = user.Gender,
                Email = userEmail == null ? "" : userEmail.Email,
                MobileNumber = userPhone == null ? "" : userPhone.PhoneNumber,
            };
            return View("MemberDetail", x);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "CardClubManager")]
        public ActionResult SaveMember(CardSearchResultViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var db = OpenDatabase.GetDatabase();
            var userCard = db.Entity<UserCard>().Query().Where(m => m.CardNumber, model.CardNumber.Trim(), CompareType.Equal).First();
            if (userCard == null)
            {
                this.ModelState.AddModelError("", "Card does not exist".Localize("CardClub"));
                return View();
            }
            var user = db.Entity<User>().Query().Where(m => m.Id, userCard.UserID, CompareType.Equal).First();
            if (user == null)
            {
                this.ModelState.AddModelError("", "User does not exist".Localize("CardClub"));
                return View();
            }
            if (RegexHelper.IsEmail(model.Email))
            {

                var userEmail = db.Entity<UserEmail>().Query().Where(m => m.Email, model.Email, CompareType.Equal).First();
                if (userEmail == null)
                {
                    userEmail = new UserEmail()
                    {
                        Email = model.Email,
                        UserID = user.Id,
                        ValidateCode = ValidateCodeGenerator.CreateRandomCode(6),
                        IsConfirmed = false,
                        ValidateTime = DateTime.UtcNow,
                    };
                    db.Entity<UserEmail>().Insert(userEmail);
                }
            }
            if (RegexHelper.IsMobileNumber(model.MobileNumber))
            {
                var userPhone = db.Entity<UserPhone>().Query().Where(m => m.UserID, model.MobileNumber, CompareType.Equal).First();
                if (userPhone == null)
                {

                    userPhone = new UserPhone()
                    {
                        IsConfirmed = false,
                        PhoneNumber = model.MobileNumber,
                        Type = PhoneType.MobilePhone,
                        UserID = user.Id,
                        ValidateCode = ValidateCodeGenerator.CreateRandomCode(6),
                        ValidateTime = DateTime.UtcNow,
                    };
                    db.Entity<UserPhone>().Insert(userPhone);
                }
            }
            user.Name = model.Name;
            user.Gender = model.Gender;
            user.Birthday = model.Birthday;
            db.Entity<User>().Update(user);
            return View("MemberDetail", model);
        }

        [Authorize(Roles = "CardClubManager")]
        public ActionResult Store()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "CardClubManager")]
        public ActionResult Store(CardSearchViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var db = OpenDatabase.GetDatabase();
            var userCard = db.Entity<UserCard>().Query().Where(m => m.CardNumber, model.CardNumber.Trim(), CompareType.Equal).First();
            if (userCard == null)
            {
                this.ModelState.AddModelError("", "Card does not exist".Localize("CardClub"));
                return View();
            }
            var user = db.Entity<User>().Query().Where(m => m.Id, userCard.UserID, CompareType.Equal).First();
            if (user == null)
            {
                this.ModelState.AddModelError("", "User does not exist".Localize("CardClub"));
                return View();
            }
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            CardSearchResultViewModel x = new CardSearchResultViewModel()
            {
                Name = user.Name,
                Birthday = user.Birthday,
                Gender = user.Gender,
                Email = db.Entity<UserEmail>().Query().Where(m => m.UserID, user.Id, CompareType.Equal).First().Email,
                MobileNumber = db.Entity<UserPhone>().Query().Where(m => m.UserID, user.Id, CompareType.Equal).First().PhoneNumber,
            };
            return View("StoreDetail", x);

        }

        [Authorize(Roles = "CardClubStore")]
        public ActionResult Card()
        {
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "CardClubStore")]
        public ActionResult Card(CardSearchViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var db = OpenDatabase.GetDatabase();
            var userCard = db.Entity<UserCard>().Query().Where(m => m.CardNumber, model.CardNumber.Trim(), CompareType.Equal).First();
            if (userCard == null)
            {
                this.ModelState.AddModelError("", "Card does not exist".Localize("CardClub"));
                return View();
            }
            var user = db.Entity<User>().Query().Where(m => m.Id, userCard.UserID, CompareType.Equal).First();
            if (user == null)
            {
                this.ModelState.AddModelError("", "User does not exist".Localize("CardClub"));
                return View();
            }
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            CardSearchResultViewModel x = new CardSearchResultViewModel()
            {
                Name = user.Name,
                Birthday = user.Birthday,
                Gender = user.Gender,
                Email = db.Entity<UserEmail>().Query().Where(m => m.UserID, user.Id, CompareType.Equal).First().Email,
                MobileNumber = db.Entity<UserPhone>().Query().Where(m => m.UserID, user.Id, CompareType.Equal).First().PhoneNumber,
            };
            return View("StoreDetail", x);

        }
    }
}