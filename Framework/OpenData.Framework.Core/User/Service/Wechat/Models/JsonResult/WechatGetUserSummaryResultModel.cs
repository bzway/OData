using OpenData.Framework.Core.Entity;
using System;
using System.Collections.Generic;

namespace OpenData.Framework.Core.Wechat.Models
{
    public class WechatGetUserSummaryResultModel : WechatJsonResultModel
    {
        public List<UserSummary> list { get; set; }
        public class UserSummary
        {
            public DateTime ref_date { get; set; }
            public WechatUserSource user_source { get; set; }
            public int new_user { get; set; }
            public int cancel_user { get; set; }

        }
    }
}