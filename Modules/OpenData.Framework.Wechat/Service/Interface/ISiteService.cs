using OpenData.Framework.Entity;

using OpenData.Data;
using System;
using System.Collections.Generic;
namespace OpenData.Framework.Service
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
