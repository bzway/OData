using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OpenData.Framework.Service.Wechat.Models
{
    public class WechatCreateUserGroupJsonResultModel : WechatJsonResultModel
    {
        public string id { get; set; }

        public string name { get; set; }
    }
}