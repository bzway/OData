using OpenData.Data.Core;

namespace OpenData.Framework.Core.Entity
{
    public class UserCard : BaseEntity
    {
        public string UUID { get; set; }
        public string UserID { get; set; }
        public string CardNumber { get; set; }
        public GradeType CardGrade { get; set; }
        public string CardName { get; set; }
        public string ValidateCode { get; set; }
        public bool IsUsed { get; set; }
    }
  
}
