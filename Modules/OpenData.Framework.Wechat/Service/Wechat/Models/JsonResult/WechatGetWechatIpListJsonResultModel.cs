using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OpenData.Framework.Service.Wechat.Models
{
    public class WechatGetWechatIpListJsonResultModel : WechatJsonResultModel
    {
        public List<string> ip_list { get; set; }
    }

}
