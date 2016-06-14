using OpenData.Data.Core;
using System;

namespace OpenData.Framework.Entity
{

    public class WechatUserCumulate : BaseEntity
    {

        public string OfficialAccount { get; set; }

        public DateTime RefDateTime { get; set; }
        /// <summary>
        /// 关注总人数
        /// </summary>
        public int CountOfFollower { get; set; }
    }
}