using OpenData.Data;

namespace OpenData.Site.Entity
{
    public class UserSite : BaseEntity
    {
        public string UserID { get; set; }
        public string SiteID { get; set; }
        public bool IsAdmin { get; set; }
    }
}