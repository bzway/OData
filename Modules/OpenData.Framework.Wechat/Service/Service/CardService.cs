using OpenData.Framework.Entity;
using OpenData.Common;
using OpenData.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace OpenData.Framework.Service
{
    public class CardService
    {
        #region ctor
        static log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        IDatabase db = OpenDatabase.GetDatabase();
        public CardService()
        {

        }
        #endregion


        //public void SearchCard(CardSearchViewModel model)
        //{
        //    model.PageIndex = model.PageIndex ?? 1;
        //    model.PageSize = model.PageSize ?? 10;
        //    var query = db.Entity<UserCard>().Query();
        //    if (model.CardGrade != null)
        //    {
        //        query = query.Where(m => m.CardGrade, model.CardGrade, CompareType.Equal);
        //    }
        //    if (!string.IsNullOrEmpty(model.CardNumber))
        //    {
        //        query = query.Where(m => m.CardNumber, model.CardNumber, CompareType.Like);
        //    }
        //    if (model.IsUsed != null)
        //    {
        //        query = query.Where(m => m.IsUsed, model.IsUsed, CompareType.Equal);
        //    }
        //    var searchedList = query.ToPageList(model.PageIndex.Value, model.PageSize.Value);

        //    var list = new List<ViewUserCard>();
        //    foreach (var item in searchedList)
        //    {
        //        list.Add(new ViewUserCard()
        //        {
        //            CardGrade = item.CardGrade,
        //            CardName = item.CardName,
        //            CardNumber = item.CardNumber,
        //            IsUsed = item.IsUsed,
        //            UserID = item.UserID,
        //            ValidateCode = item.ValidateCode,
        //        });
        //    }
        //    model.SearchResult = new PagedList<ViewUserCard>(list, searchedList.CurrentPageIndex, searchedList.PageSize);
        //}

        public UserCard FindoCardByID(string id)
        {
            return db.Entity<UserCard>().Query().Where(m => m.UUID, id, CompareType.Equal).First();
        }
        public void CreateUserCard(UserCard entity)
        {
            entity.Status = 0;
            db.Entity<UserCard>().Insert(entity);
        }

        public void UpdateCard(UserCard model)
        {
            var entity = db.Entity<UserCard>().Query().Where(m => m.UUID, model.UUID, CompareType.Equal).First();
            if (entity == null)
            {
                return;
            }
            entity.CardGrade = model.CardGrade;
            entity.CardName = model.CardName;
            entity.CardNumber = model.CardNumber;
            entity.IsUsed = model.IsUsed;
            entity.ValidateCode = model.ValidateCode;
            db.Entity<UserCard>().Update(entity);
        }

        public void DeleteCard(string id)
        {
            var entity = db.Entity<UserCard>().Query().Where(m => m.UUID, id, CompareType.Equal).First();
            if (entity == null)
            {
                return;
            }
            db.Entity<UserCard>().Delete(entity);
        }

        public void Import(DataSet ds)
        {
            foreach (DataTable dt in ds.Tables)
            {
                foreach (DataRow dr in dt.Rows)
                {
                    GradeType CardGrade = GradeType.Crystal;
                    if (dr["CardGrade"] != null)
                    {
                        CardGrade = (GradeType)int.Parse(dr["CardGrade"].ToString());
                    }
                    string UserID = Guid.NewGuid().ToString("N");
                    db.Entity<User>().Insert(new User() { Id = UserID });
                    string ValidateCode = string.Empty;
                    if (dr["ValidateCode"] != null)
                    {
                        ValidateCode = dr["ValidateCode"].ToString();
                    }
                    bool IsUsed = false;
                    if (dr["IsUsed"] != null)
                    {
                        bool.TryParse(dr["IsUsed"].ToString(), out IsUsed);
                    }
                    string CardNumber = string.Empty;
                    if (dr["CardNumber"] != null)
                    {
                        CardNumber = dr["CardNumber"].ToString();

                    }
                    string CardName = string.Empty;
                    if (dr["CardName"] != null)
                    {
                        CardName = dr["CardName"].ToString();
                    }

                    var userCard = new UserCard()
                    {
                        CardGrade = CardGrade,
                        UserID = UserID,
                        ValidateCode = ValidateCode,
                        IsUsed = IsUsed,
                        CardNumber = CardNumber,
                        CardName = CardName,
                    };

                    userCard.Status = 0;
                    db.Entity<UserCard>().Insert(userCard);
                }
            }
        }
    }
}