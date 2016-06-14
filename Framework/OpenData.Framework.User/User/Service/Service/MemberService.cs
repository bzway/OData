using OpenData.Framework.Core.Entity;
using OpenData.Data.Core;
using OpenData.Utility;
using System;
using System.Data;
using System.Text;
using OpenData.Framework.Core.Entity;

namespace OpenData.Framework.Core
{

    public class MemberService : OpenData.Framework.Core.IMemberService
    {
        #region ctor
        static log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        readonly static string LocalMemberTableName = "Member";
        IUserService userService = ApplicationEngine.Current.Resolve<IUserService>();
        //readonly IOwinContext context;
        public MemberService(/*IOwinContext context*/)
        {
            //this.context = context;
        }
        #endregion
        public void CreateAccount(string email, string phoneNumber, DynamicEntity data)
        {

            var userEmail = userService.VerifyEmail(email);
            var userPhone = userService.VerifyPhone(phoneNumber);
            dynamic enity = data;
            if (userEmail == null && userPhone == null)
            {
                #region Create One
                var userID = Guid.NewGuid().ToString("N");
                using (var db = OpenDatabase.GetDatabase())
                {

                    var user =
                        //new ViewUser()
                        new User()
                    {
                        UserName = email,
                        Province = enity.Province,
                        NickName = enity.NickName,
                        Country = enity.Country,
                        Name = enity.Name,
                        Birthday = enity.Birthday,
                        City = enity.City,
                        Distinct = enity.Distinct,
                        Gender = enity.Gender,
                        Grade = GradeType.Crystal,
                        IsLocked = false,
                        IsLunarBirthday = false,
                        LockedTime = null,
                        IsConfirmed = false,
                        Password = null,
                        Roles = string.Empty,
                    };
                    db.Entity<User>().Insert(user);
                    userEmail =
                        new UserEmail()
                    //new ViewUserEmail()
                    {
                        ValidateTime = DateTime.UtcNow,
                        UserID = userID,
                        Email = email,
                        IsConfirmed = false,
                        ValidateCode = ValidateCodeGenerator.CreateRandomCode(6),
                    };
                    db.Entity<UserEmail>().Insert(userEmail);
                    userPhone =                        new UserPhone()
                    {
                        UserID = userID,
                        IsConfirmed = false,
                        PhoneNumber = phoneNumber,
                        Type = PhoneType.MobilePhone,
                        ValidateCode = ValidateCodeGenerator.CreateRandomCode(6),
                    };
                    db.Entity<UserPhone>().Insert(userPhone);
                    ApplicationEngine.Current.Resolve<OpenData.Message.ISMTPService>().SendMail("", email, "", "");
                    ApplicationEngine.Current.Resolve<OpenData.Message.ISMSService>().Send("", phoneNumber, "", "");
                }
                #endregion
                using (var db = OpenDatabase.GetDatabase("", "test"))
                {
                    data["ID"] = userID;
                    db.DynamicEntity(db["Member"]).Insert(data);
                }
                return;
            }

            if (userEmail != null && userPhone != null && string.Equals(userPhone.UserID, userEmail.UserID))
            {
                #region Create One
                using (var db = OpenDatabase.GetDatabase("", "test"))
                {
                    data["ID"] = userEmail.UserID;
                    db.DynamicEntity(db["Member"]).Insert(data);
                }
                return;

                #endregion
            }
        }

        //public MemberSearchViewModel SearchMember(MemberSearchViewModel model)
        //{
        //    model.PageIndex = model.PageIndex ?? 1;
        //    model.PageSize = model.PageSize ?? 10;
        //    using (var db = DefaultDatabase.GetDatabase())
        //    {
        //        var query = db.Entity<User>().Query();

        //        if (!string.IsNullOrEmpty(model.Name))
        //        {
        //            query = query.Where(m => m.Name, model.Name, CompareType.Like);
        //        }
        //        if (model.Gender.HasValue)
        //        {
        //            query = query.Where(m => m.Gender, model.Gender, CompareType.Equal);
        //        }

        //        if (!string.IsNullOrEmpty(model.Email))
        //        {
        //            foreach (var item in db.Entity<UserEmail>().Query("Id").Where(m => m.Email, model.Email, CompareType.Like).ToList())
        //            {
        //                query = query.Or(db.Entity<User>().Query().Where(m => m.Id, item.UserID, CompareType.Equal));
        //            }
        //        }
        //        if (!string.IsNullOrEmpty(model.CardNumber))
        //        {
        //            foreach (var item in db.Entity<UserCard>().Query("Id").Where(m => m.CardNumber, model.CardNumber, CompareType.Like).ToList())
        //            {
        //                query = query.Or(db.Entity<User>().Query().Where(m => m.Id, item.UserID, CompareType.Equal));
        //            }
        //        }
        //        if (!string.IsNullOrEmpty(model.MobileNumber))
        //        {
        //            foreach (var item in db.Entity<UserPhone>().Query("Id").Where(m => m.PhoneNumber, model.MobileNumber, CompareType.Like).Where(m => m.Type, PhoneType.MobilePhone, CompareType.Equal).ToList())
        //            {
        //                query = query.Or(db.Entity<User>().Query().Where(m => m.Id, item.UserID, CompareType.Equal));
        //            }
        //        }

        //        var list = new List<MemberProfileViewModel>();
        //        var searchedList = query.ToPageList(model.PageIndex.Value, model.PageSize.Value);
        //        foreach (var item in searchedList)
        //        {
        //            var userEmail = db.Entity<UserEmail>().Query().Where(m => m.UserID, item.Id, CompareType.Equal).First();
        //            var userPhone = db.Entity<UserPhone>().Query().Where(m => m.UserID, item.Id, CompareType.Equal)
        //                .Where(m => m.Type, PhoneType.MobilePhone, CompareType.Equal)
        //                .First();
        //            list.Add(new MemberProfileViewModel()
        //            {
        //                Birthday = item.Birthday,
        //                //Id = this.context.GetUserManager().GetCurrentUser().EncryptAES(item.Id),
        //                City = item.City,
        //                Country = item.Country,
        //                Distinct = item.Distinct,
        //                Email = userEmail == null ? string.Empty : userEmail.Email,
        //                Gender = item.Gender,
        //                Grade = item.Grade,
        //                IsConfirmed = item.IsConfirmed,
        //                IsLocked = item.IsLocked,
        //                IsLunarBirthday = item.IsLunarBirthday,
        //                LockedTime = item.LockedTime,
        //                Name = item.Name,
        //                NickName = item.NickName,
        //                PhoneNumber = userPhone == null ? string.Empty : userPhone.PhoneNumber,
        //                Province = item.Province,
        //                Roles = item.Roles,
        //                UserName = item.UserName,
        //            });
        //        }
        //        model.SearchResult = new PagedList<MemberProfileViewModel>(list, searchedList.CurrentPageIndex, searchedList.PageSize);
        //    }
        //    return model;
        //}

        public void Import(DataSet ds, Site site)
        {
            foreach (DataTable dt in ds.Tables)
            {
                if (!string.IsNullOrEmpty(Check(dt, site)))
                {
                    return;
                }
                foreach (DataRow dr in dt.Rows)
                {
                    GradeType CardGrade = GradeType.Crystal;
                    if (dr["CardGrade"] != null)
                    {
                        CardGrade = (GradeType)int.Parse(dr["CardGrade"].ToString());
                    }
                    string email = string.Empty;
                    if (dr["Email"] != null)
                    {
                        email = dr["Email"].ToString();
                    }
                    string phoneNumber = string.Empty;
                    if (dr["Mobile"] != null)
                    {
                        email = dr["Mobile"].ToString();
                    }

                    CreateAccount(email, phoneNumber, new DynamicEntity());
                    var localdb = OpenDatabase.GetDatabase();
                    localdb.DynamicEntity(localdb[LocalMemberTableName]).Insert(null);
                }
            }
        }

        private string Check(DataTable dt, Site site)
        {
            var errorMessage = new StringBuilder();
            //判断用户数据是否要求
            using (var db = OpenDatabase.GetDatabase(site.ProviderName, site.ConnectionString, site.DatabaseName))
            {
                var list = db[LocalMemberTableName].AllColumns;
                foreach (var item in list)
                {
                    if (!item.AllowNull && !dt.Columns.Contains(item.Name))
                    {
                        errorMessage.AppendLine(item.Name + " 不能为空！");
                    }
                }
                return errorMessage.ToString();
            }

        }
    }
}