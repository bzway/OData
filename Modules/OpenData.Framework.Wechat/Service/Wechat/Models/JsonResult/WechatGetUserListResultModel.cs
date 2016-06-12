using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OpenData.Framework.Service.Wechat.Models
{
    
    public class WechatGetUserListResultModel : WechatJsonResultModel
    {
        public int total { get; set; }
        public int count { get; set; }
        public Data data { get; set; }
        public string next_openid { get; set; }
        public class Data
        {
            public List<string> openid { get; set; }
        }
    }
}
