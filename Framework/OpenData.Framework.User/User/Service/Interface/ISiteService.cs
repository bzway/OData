using OpenData.Framework.Core.Entity;
using OpenData.Framework.Core.Entity;
using System.Collections.Generic;
namespace OpenData.Framework.Core
{
    public interface ISiteService
    {
        Site FindSiteByDomain(string domain);
        Site FindSiteByID(string siteID);
        Site FindSiteByName(string siteName);
        IEnumerable<Site> FindSiteByUserID(string userID);
        void CreateOrUpdateSite(Site site, string userID);

        void DeleteSiteByID(string siteID);
    }
}
