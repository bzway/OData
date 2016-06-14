using System.ComponentModel.DataAnnotations;
using OpenData.Framework.Entity;

namespace OpenData.Framework.WebApp.Models
{
    public class UserSiteViewModel
    {
        public string ID { get; set; }
        [Display(Name = "Name", ResourceType = typeof(ViewModelResource))]
        public string Name { get; set; }
        [Display(Name = "ProviderName", ResourceType = typeof(ViewModelResource))]
        public string ProviderName { get; set; }
        [Display(Name = "DatabaseName", ResourceType = typeof(ViewModelResource))]
        public string DatabaseName { get; set; }
        [Display(Name = "ConnectionString", ResourceType = typeof(ViewModelResource))]
        public string ConnectionString { get; set; }

        public string Domains { get; set; }
    }
}
