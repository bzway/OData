using System.Collections.Generic;

namespace OpenData.Site.Core.Wechat.Models
{
    public class WechatGetWechatIpListJsonResultModel : WechatJsonResultModel
    {
        public List<string> ip_list { get; set; }
    }

}
