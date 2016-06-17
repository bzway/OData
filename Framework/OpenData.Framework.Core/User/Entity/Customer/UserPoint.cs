using OpenData.Data.Core;
using System;

namespace OpenData.Framework.Core.Entity
{

    public class UserPoint : BaseEntity
    {
        public string UserID { get; set; }
        public PointType Type { get; set; }
        public int Amount { get; set; }
        public string ExtensionID { get; set; }
        public string Source { get; set; }
        public DateTime ExpiringOn { get; set; }
        public string Remark { get; set; }
    }
  
}