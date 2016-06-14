using OpenData.Data;
using System;

namespace OpenData.Site.Entity
{
    public class UserPhone : BaseEntity
    {
        public string UserID { get; set; }
        public PhoneType Type { get; set; }
        public string PhoneNumber { get; set; }
        public string ValidateCode { get; set; }
        public DateTime ValidateTime { get; set; }
        public bool IsConfirmed { get; set; }
    }
    
}