using System.Collections.Generic;

namespace OpenData.Framework.Core.Wechat.Models
{
    public class WechatGetWechatIpListJsonResultModel : WechatJsonResultModel
    {
        public List<string> ip_list { get; set; }
    }

}
