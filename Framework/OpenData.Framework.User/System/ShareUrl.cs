using OpenData.Data;
namespace OpenData.Site.Entity
{
    public class ShareUrl : BaseEntity
    {
        /// <summary>
        /// 分享编号
        /// </summary>
        public string TrackId { get; set; }
        /// <summary>
        /// 原始链接
        /// </summary>
        public string OrginalUrl { get; set; }
        /// <summary>
        /// 分享链接
        /// </summary>
        public string Url { get; set; }
        /// <summary>
        /// 分享描述
        /// </summary>
        public string Descripion { get; set; }
        /// <summary>
        /// 原始分享编号
        /// </summary>
        public string SourceId { get; set; }

        public string SharerId { get; set; }
        /// <summary>
        /// 分享类型
        /// </summary>
        public string Type { get; set; }

        /// <summary>
        /// 浏览器
        /// </summary>
        public string UserAgent { get; set; }

        /// <summary>
        /// IP地址
        /// </summary>
        public string IPAddress { get; set; }






    }
}
