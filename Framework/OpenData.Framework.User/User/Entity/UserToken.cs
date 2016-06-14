using OpenData.Data;
using System;

namespace OpenData.Site.Entity
{
    public class UserToken : BaseEntity
    {
        public string UserID { get; set; }
        public DateTime ExpringTime { get; set; }
        public string AccessToken { get; set; }
    }
}