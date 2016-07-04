
using Autofac;
using Microsoft.Owin;
using OpenData.Common.AppEngine;
using OpenData.Common.Caching;
using OpenData.Data.Core;
using OpenData.Framework.Core.Entity;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OpenData.Framework.Core
{
    /// <summary>
    /// GrantRequest service
    /// </summary>
    public partial class SiteManager
    {

        #region ctor
        static log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        ISiteService siteService = ApplicationEngine.Current.Default.Resolve<ISiteService>();

        IOwinContext context;
        Site site;
        IDatabase db;

        public SiteManager(string siteID)
        {
            var site = this.siteService.FindSiteByID(siteID);
            if (site != null)
            {
                this.db = OpenDatabase.GetDatabase(this.site.ProviderName, this.site.ConnectionString, this.site.DatabaseName);
            }
        }
        public SiteManager(IOwinContext context)
        {
            this.context = context;
        }
        public SiteManager(HttpContextBase context)
        {
            this.context = context.GetOwinContext();
        }
        #endregion

        public Site GetSite()
        {
            if (this.site == null)
            { this.site = this.siteService.FindSiteByDomain(this.context.Request.Host.Value); }
            if (site == null)
            {
                site = GetDefaultSite();
            }
            return this.site;
        }

        public Site GetDefaultSite()
        {
            return new Site() { };
        }
        public IDatabase GetSiteDataBase()
        {
            if (this.db == null)
            {
                this.db = OpenDatabase.GetDatabase(this.GetSite().ProviderName, this.GetSite().ConnectionString, this.GetSite().DatabaseName);
            }
            return this.db;
        }
        public SitePage GetSitePage(string PageUrl)
        {
            ICacheManager cache = ApplicationEngine.Current.Default.Resolve<ICacheManager>();
            var pageList = cache.Get<List<SitePage>>("SitePage", () =>
            {
                List<SitePage> list = new List<SitePage>();
                foreach (var item in this.GetSiteDataBase().Entity<SitePage>().Query().ToList())
                {
                    list.Add(item);
                }
                return list;
            });
            
            var page = pageList.Where(m => m.PageUrl == PageUrl).FirstOrDefault();
            if (page == null)
            {
                return new SitePage()
                {
                    Name = "~/Views/Home/NotFound.cshtml",
                    FileExtension = ".cshtml",
                    MasterVirtualPath = "~/Views/Shared/_Layout.cshtml",
                };
            }

            return page;
        } 
        public IMemberService GetMemberService()
        {
            return new MemberService();
        }
    }
}
