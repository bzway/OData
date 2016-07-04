using OpenData.Framework.Core;
using OpenData.Data.Core;
using OpenData.Common.AppEngine;
using Autofac;

namespace OpenData.Module.Website.Controllers
{
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
                    this.Redirect("/User");
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