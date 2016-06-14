using OpenData.Data;
using System;

namespace OpenData.Site.Entity
{
    public class UserEmail : BaseEntity
    {
        public string UserID { get; set; }
        public string Email { get; set; }
        public string ValidateCode { get; set; }
        public DateTime ValidateTime { get; set; }
        public bool IsConfirmed { get; set; }
    }
}