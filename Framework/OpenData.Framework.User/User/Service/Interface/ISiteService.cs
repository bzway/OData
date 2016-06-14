using OpenData.Site.Entity;
using System.Collections.Generic;
namespace OpenData.Site.Core
{
    public interface ISiteService
    {
        Site FindSiteByDomain(string domain);
        Site FindSiteByID(string siteID);
        Site FindSiteByName(string siteName);
        IEnumerable<Entity.Site> FindSiteByUserID(string userID);
        void CreateOrUpdateSite(Entity.Site site, string userID);

        void DeleteSiteByID(string siteID);
    }
}
