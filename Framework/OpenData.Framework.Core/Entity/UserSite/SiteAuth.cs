using OpenData.Data.Core;
using System;

namespace OpenData.Framework.Core.Entity
{
    public partial class SiteAuth : BaseEntity
    {
        public string AppID { get; set; }
        public string UserID { get; set; }
        public string OpenID { get; set; }
        public string Scope { get; set; }
        public DateTime ExpiredTime { get; set; }

        public string AccessToken { get; set; }
    }
}