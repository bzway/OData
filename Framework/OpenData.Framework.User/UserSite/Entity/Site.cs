using OpenData.Data.Core;
using System.ComponentModel.DataAnnotations;

namespace OpenData.Framework.Entity
{
    public partial class Site : BaseEntity
    {
        #region Base
        [Display(Name = "Name", ResourceType = typeof(ViewModelResource))]
        public string Name { get; set; }
        [Display(Name = "Caption", ResourceType = typeof(ViewModelResource))]
        public string Caption { get; set; }

        [Display(Name = "LogoUrl", ResourceType = typeof(ViewModelResource))]
        public string LogoUrl { get; set; }

        [Display(Name = "FaviconUrl", ResourceType = typeof(ViewModelResource))]
        public string FaviconUrl { get; set; }

        [Display(Name = "Description", ResourceType = typeof(ViewModelResource))]
        public string Description { get; set; }

        [Display(Name = "Domains", ResourceType = typeof(ViewModelResource))]
        public string Domains { get; set; }

        [Display(Name = "TimeZone", ResourceType = typeof(ViewModelResource))]
        public string TimeZone { get; set; }
        #endregion

        #region ShowSetting

        [Display(Name = "IsShowLogo", ResourceType = typeof(ViewModelResource))]
        public bool IsShowLogo { get; set; }

        [Display(Name = "IsShowMenu", ResourceType = typeof(ViewModelResource))]
        public bool IsShowMenu { get; set; }

        [Display(Name = "IsShowSideBar", ResourceType = typeof(ViewModelResource))]
        public bool IsShowSideBar { get; set; }

        #endregion

        #region Database
        [Display(Name = "ProviderName", ResourceType = typeof(ViewModelResource))]
        public string ProviderName { get; set; }
        [Display(Name = "DatabaseName", ResourceType = typeof(ViewModelResource))]
        public string DatabaseName { get; set; }
        [Display(Name = "ConnectionString", ResourceType = typeof(ViewModelResource))]
        public string ConnectionString { get; set; }
        public string AppSecret { get; set; }
        #endregion
    }
}