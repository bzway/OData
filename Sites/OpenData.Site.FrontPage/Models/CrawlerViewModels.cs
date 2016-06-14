using System.ComponentModel.DataAnnotations;

namespace OpenData.Site.FrontPage.Models
{

    public class CrawlerViewModels
    {
        [Display(Name="Url")]
        public string Url { get; set; }
        [Display(Name = "是否是单页")]

        public bool IsSinglePage { get; set; }
        [Display(Name = "是否使用布局")]
        public bool UseLayout { get; set; }
        [Display(Name = "抓取深度")]
        [UIHint("test")]
        public int CrawlerLevel { get; set; }
        [Display(Name = "是否下载资源文件")]
        public bool DownloadResource { get; set; }

    }
}
