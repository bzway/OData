using OpenData.Framework.Core;
using OpenData.Sites.FrontPage.Controllers;
using OpenData.Data.Core;

namespace OpenData.Sites.FrontPage.Areas.Users.Controllers
{
    public class BaseUserController : BaseController
    {
        public IDatabase db
        {
            get
            {
                return this.SiteManager.GetSiteDataBase();
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
                    siteService = ApplicationEngine.Current.Resolve<ISiteService>();
                }
                return siteService;
            }
        }

    }
}