using OpenData.Site.Entity;
using OpenData.Data;
using System;
using System.Collections.Generic;

namespace OpenData.Site.Core
{
    /// <summary>
    /// GrantRequest service
    /// </summary>
    public partial class SiteService : ISiteService
    {
        #region
        static log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        IDatabase db = OpenDatabase.GetDatabase();
        public SiteService()
        {

        }
        #endregion

        public Site FindSiteByDomain(string domain)
        {
            return db.Entity<Entity.Site>().Query().Where(m => m.Domains, domain, CompareType.Equal).First();
        }
        public IEnumerable<Entity.Site> FindSiteByUserID(string userID)
        {
            List<Entity.Site> list = new List<Entity.Site>();
            foreach (var item in db.Entity<UserSite>().Query().Where(m => m.UserID, userID, CompareType.Equal).ToList())
            {
                list.AddRange(db.Entity<Site>().Query().Where(m => m.Id, item.SiteID, CompareType.Equal).ToList());
            }
            return list;
        }

        public void CreateSite(Entity.Site site, string UserID)
        {
            if (string.IsNullOrEmpty(site.Id))
            {
                site.Id = Guid.NewGuid().ToString("N");
            }
            this.db.Entity<Entity.Site>().Insert(site);
            this.db.Entity<UserSite>().Insert(new UserSite()
            {
            });
        }

        public Site FindSiteByID(string id)
        {
            return this.db.Entity<Entity.Site>().Query().Where(m => m.Id, id, CompareType.Equal).First();
        }
        public Site FindSiteByName(string siteName)
        {
            return this.db.Entity<Entity.Site>().Query().Where(m => m.Name, siteName, CompareType.Equal).First();
        }
        public void CreateOrUpdateSite(Entity.Site site, string userID)
        {
            if (!string.IsNullOrEmpty(site.Id))
            {
                this.db.Entity<Entity.Site>().Update(site);
                return;
            }
            site.Id = Guid.NewGuid().ToString("N");
            this.db.Entity<Entity.Site>().Insert(site);
            this.db.Entity<UserSite>().Insert(new UserSite() { UserID = userID, SiteID = site.Id });
        }

        public void DeleteSiteByID(string siteID)
        {
            this.db.Entity<Entity.Site>().Delete(siteID);
            var userSite = this.db.Entity<UserSite>().Query()
                .Where(m => m.SiteID, siteID, CompareType.Equal)
                .First();
            this.db.Entity<UserSite>().Delete(userSite);
        }
    }
}