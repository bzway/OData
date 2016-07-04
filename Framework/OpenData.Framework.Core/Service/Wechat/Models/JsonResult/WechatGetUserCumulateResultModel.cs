using System;
using System.Collections.Generic;

namespace OpenData.Framework.Core.Wechat.Models
{
    public class WechatGetUserCumulateResultModel : WechatJsonResultModel
    {
        public class UserCumulate
        {
            public DateTime ref_date { get; set; }
            public int cumulate_user { get; set; }
        }
        public List<UserCumulate> list { get; set; }
    }
}