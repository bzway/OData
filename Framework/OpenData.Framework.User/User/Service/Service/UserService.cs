using OpenData.Framework.Core.Entity;
using OpenData.Data.Core;
using OpenData.Utility;
using System;
using System.Collections.Generic;
using OpenData.Framework.Core.Entity;

namespace OpenData.Framework.Core
{
    public class UserService : IUserService
    {
        #region ctor
        static log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        IDatabase db = OpenDatabase.GetDatabase();
        #endregion

        public UserEmail VerifyEmail(string email)
        {
            return db.Entity<UserEmail>().Query().Where(m => m.Email, email, CompareType.Equal).First();
        }

        public User FindUserByID(string userID)
        {
            using (var db = OpenDatabase.GetDatabase())
            {
                return db.Entity<User>().Query().Where(m => m.Id, userID, CompareType.Equal).First();
            }
        }

        public IEnumerable<UserEmail> FindUserEmailsByUserID(string userID)
        {
            using (var db = OpenDatabase.GetDatabase())
            {
                return db.Entity<UserEmail>().Query().Where(m => m.UserID, userID, CompareType.Equal).ToList();
            }
        }

        public UserEmail FindUserEmailByValidationCode(string code)
        {
            using (var db = OpenDatabase.GetDatabase())
            {
                return db.Entity<UserEmail>().Query().Where(m => m.ValidateCode, code, CompareType.Equal)
                    //.Where(m => m.ValidateTime, DateTime.UtcNow.AddDays(-1), CompareType.GreaterThanOrEqual)
                                     .First();
            }
        }

        public UserPhone FindUserPhoneByValidationCode(string code)
        {
            using (var db = OpenDatabase.GetDatabase())
            {
                return db.Entity<UserPhone>().Query().Where(m => m.ValidateCode, code, CompareType.Equal)
                                     .Where(m => m.ValidateTime, DateTime.UtcNow.AddDays(-1), CompareType.GreaterThanOrEqual).First();
            }
        }

        public IEnumerable<UserPhone> FindUserPhonesByUserID(string userID)
        {
            using (var db = OpenDatabase.GetDatabase())
            {
                return db.Entity<UserPhone>().Query().Where(m => m.UserID, userID, CompareType.Equal).ToList();
            }
        }

        public UserPhone VerifyPhone(string phoneNumber)
        {
            using (var db = OpenDatabase.GetDatabase())
            {
                return db.Entity<UserPhone>().Query().Where(m => m.PhoneNumber, phoneNumber, CompareType.Equal).First();
            }
        }
        public User VerifyUser(string userName, string password)
        {
            password = Cryptor.EncryptMD5(password);
            using (var db = OpenDatabase.GetDatabase())
            {
                return db.Entity<User>().Query().Where(m => m.UserName, userName, CompareType.Equal)
                    .Where(m => m.Password, password, CompareType.Equal).First();
            }
        }


        public UserPhone RegisterByPhoneNumber(string phoneNumber, string password)
        {
            var userId = Guid.NewGuid().ToString("N");
            var user = new User()
            {
                Id = userId,
                Province = string.Empty,
                Birthday = string.Empty,
                City = string.Empty,
                Country = string.Empty,
                Distinct = string.Empty,
                Name = phoneNumber,
                NickName = phoneNumber,
                Gender = GenderType.Unkonw,
                IsConfirmed = false,
                Grade = GradeType.Crystal,
                IsLocked = false,
                IsLunarBirthday = false,
                LockedTime = null,
                UserName = phoneNumber,
                Password = Cryptor.EncryptMD5(password),
                Roles = string.Empty,

            };
            var userPhone = new UserPhone()
            {
                ValidateTime = DateTime.UtcNow,
                UserID = userId,
                PhoneNumber = phoneNumber,
                Type = PhoneType.MobilePhone,
                IsConfirmed = false,
                ValidateCode = ValidateCodeGenerator.CreateRandomCode(6),
            };

            using (var db = OpenDatabase.GetDatabase())
            {
                db.Entity<User>().Insert(user);
                db.Entity<UserPhone>().Insert(userPhone);
            }
            return userPhone;
        }

        public virtual UserEmail RegisterByEmail(string email, string password)
        {
            var userId = Guid.NewGuid().ToString("N");
            var user = new User()
            {
                Id = userId,
                Province = string.Empty,
                Birthday = string.Empty,
                City = string.Empty,
                Country = string.Empty,
                Distinct = string.Empty,
                Name = email,
                NickName = email,
                Gender = GenderType.Unkonw,
                IsConfirmed = false,
                Grade = GradeType.Crystal,
                IsLocked = false,
                IsLunarBirthday = false,
                LockedTime = null,
                UserName = email,
                Password = Cryptor.EncryptMD5(password),
                Roles = string.Empty,
            };
            var userEmail = new UserEmail()
            {
                ValidateTime = DateTime.UtcNow,
                UserID = userId,
                Email = email,
                IsConfirmed = false,
                ValidateCode = ValidateCodeGenerator.CreateRandomCode(6),
            };
            using (var db = OpenDatabase.GetDatabase())
            {
                db.Entity<User>().Insert(user);
                db.Entity<UserEmail>().Insert(userEmail);
            }
            return userEmail;
        }

        public void ConfirmUserEmail(UserEmail userEmail)
        {
            using (var db = OpenDatabase.GetDatabase())
            {
                db.Entity<UserEmail>().Update(userEmail);
            }
        }

        public  void ConfirmUserPhone(UserPhone userPhone)
        {
            using (var db = OpenDatabase.GetDatabase())
            {
                db.Entity<UserPhone>().Update(userPhone);
            }
        }

    }
}