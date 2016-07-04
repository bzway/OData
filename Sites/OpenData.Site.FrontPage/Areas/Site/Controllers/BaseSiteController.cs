using OpenData.Framework.Core;
using OpenData.Sites.FrontPage.Controllers;
using OpenData.Data.Core;
using OpenData.Common.AppEngine;
using Autofac;

namespace OpenData.Sites.FrontPage.Areas.Sites.Controllers
{
    [BzwayAuthorize(Roles = "Site")]
    public class BaseSiteController : BzwayController
    {
        public IDatabase db
        {
            get
            {
                return this.Site.GetSiteDataBase();
            }
        }
        public string CurrentSite
        {
            get
            {
                if (this.Session["CurrentSite"] == null)
                {
                    this.RedirectToLocal("/User");
                    return string.Empty;
                }
                return this.Session["CurrentSite"].ToString();
            }
            set
            {
                this.Session["CurrentSite"] = value;
            }
        }

        ISiteService siteService;
        public ISiteService SiteService
        {
            get
            {
                if (siteService == null)
                {
                    siteService = ApplicationEngine.Current.Default.Resolve<ISiteService>();
                }
                return siteService;
            }
        }

    }
}