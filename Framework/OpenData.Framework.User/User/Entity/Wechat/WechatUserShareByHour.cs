using OpenData.Data.Core;
using System;

namespace OpenData.Framework.Core.Entity
{

  
    public class WechatUserShareByHour : BaseEntity
    {

        public string OfficialAccount { get; set; }
        public DateTime RefDateTime { get; set; }
        public WechatUserShareScene Scene { get; set; }
        public int PageShareUser { get; set; }
        public int PageShareCount { get; set; }
    }
}