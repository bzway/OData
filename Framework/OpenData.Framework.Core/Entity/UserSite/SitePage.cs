using OpenData.Data.Core;
using System.ComponentModel.DataAnnotations;

namespace OpenData.Framework.Core.Entity
{
    public enum TargetType
    {
        _self = 0,
        _blank,
        _parent,
        _top,
    }
    public class SitePage : BaseEntity
    {
        /// <summary>
        /// 没有域名的相对地址
        /// </summary>
        public string PageUrl { get; set; }
        /// <summary>
        /// 目前只支持 .cshtml
        /// </summary>
        public string FileExtension { get; set; }
        /// <summary>
        /// 虚拟路径 for Virtual Path
        /// </summary>
        public string Name { get; set; }
        [Display(Name = "模板", Description = "相对路径")]
        /// <summary>
        /// 模板
        /// </summary>
        public string MasterVirtualPath { get; set; }

        public string Title { get; set; }
        public string Content { get; set; }
        public TargetType LinkTarget { get; set; }

        #region PageAttribute
        /// <summary>
        /// 是否使用输入缓存
        /// </summary>
        public bool OutputCache { get; set; }
        /// <summary>
        /// 输出缓存时间 单位s
        /// </summary>
        public int Duration { get; set; }

        public bool Published { get; set; }
        #endregion

        #region HtmlMeta
        public bool EnableTheming { get; set; }
        public bool EnableScript { get; set; }
        public string HtmlTitle { get; set; }
        public string Author { get; set; }
        public string Keywords { get; set; }
        public string Description { get; set; }
        public string Customs { get; set; }
        public string Canonical { get; set; }
        #endregion

        #region Navigation
        public bool ShowInNavigation { get; set; }
        public bool ShowInCrumb { get; set; }
        public string DisplayText { get; set; }
        public int SortBy { get; set; }

        public string NavigationId { get; set; }
        #endregion

        #region Permission
        public string AllowedRole { get; set; }
        public string DeniedRole { get; set; }
        #endregion

    }
}