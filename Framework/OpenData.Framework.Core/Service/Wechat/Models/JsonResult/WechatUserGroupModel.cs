using System.Collections.Generic;

namespace OpenData.Framework.Core.Wechat.Models
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