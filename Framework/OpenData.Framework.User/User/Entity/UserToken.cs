using OpenData.Data.Core;
using System;

namespace OpenData.Framework.Entity
{
    public class UserToken : BaseEntity
    {
        public string UserID { get; set; }
        public DateTime ExpringTime { get; set; }
        public string AccessToken { get; set; }
    }
}