
using OpenData.Data.Core;
using OpenData.Framework.Core.Entity;
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
        ISiteService siteService = ApplicationEngine.Current.Resolve<ISiteService>();

        readonly Site site;
        readonly IDatabase db;

        public SiteManager(string siteID)
        {
            var site = this.siteService.FindSiteByID(siteID);
            if (site != null)
            {
                this.db = OpenDatabase.GetDatabase(this.site.ProviderName, this.site.ConnectionString, this.site.DatabaseName);
            }
        }
        public SiteManager(HttpContextBase context)
        {
            this.site = this.siteService.FindSiteByDomain(context.GetOwinContext().Request.Host.Value);

            if (this.site == null && context.Session["CurrentSite"] != null)
            {
                this.site = siteService.FindSiteByID(context.Session["CurrentSite"].ToString());
            }

            if (this.site == null)
            {
                this.site = siteService.FindSiteByName(context.Request.QueryString["siteName"]);
            }

            if (this.site != null)
            {
                this.db = OpenDatabase.GetDatabase(this.site.ProviderName, this.site.ConnectionString, this.site.DatabaseName);
            }
        }
        #endregion

        public Site GetSite()
        {
            return this.site;
        }
        public IDatabase GetSiteDataBase()
        {
            return this.db;
        }
        public SitePage GetSitePage(string PageUrl)
        {
            if (this.db == null)
            {
                return new SitePage()
                {
                    VirtualPath = "~/Views/Home/NotFound.cshtml",
                    FileExtension = ".cshtml",
                    MasterVirtualPath = "~/Views/Shared/_Layout.cshtml",
                };
            }
            if (string.IsNullOrWhiteSpace(PageUrl))
            {
                var page = this.db.Entity<SitePage>().Query().Where(m => m.PageUrl, null, CompareType.Equal).First();
                if (page == null)
                {
                    return new SitePage()
                    {
                        VirtualPath = "~/Views/Home/NotFound.cshtml",
                        FileExtension = ".cshtml",
                        MasterVirtualPath = "~/Views/Shared/_Layout.cshtml",
                    };
                }

                return page;
            }
            else
            {
                var page = this.db.Entity<SitePage>().Query().Where(m => m.PageUrl, PageUrl, CompareType.Equal).First();

                if (page == null)
                {
                    return new SitePage()
                    {
                        VirtualPath = "~/Views/Home/NotFound.cshtml",
                        FileExtension = ".cshtml",
                        MasterVirtualPath = "~/Views/Shared/_Layout.cshtml",
                    };
                }

                return page;
            }
        }

        public IMemberService GetMemberService()
        {
            return new MemberService();
        }
    }
}
