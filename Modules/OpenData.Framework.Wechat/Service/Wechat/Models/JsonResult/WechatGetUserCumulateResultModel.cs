using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OpenData.Framework.Service.Wechat.Models
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