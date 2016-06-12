using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OpenData.Framework.Service.Wechat.Models
{
    public class WechatGetGroupModel : WechatJsonResultModel
    {
        public List<WechatUserGroup> groups { get; set; }

        public class WechatUserGroup
        {
            public string id { get; set; }
            public string name { get; set; }
            public int count { get; set; }
        }
    }
}