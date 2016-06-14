using OpenData.Data.Core;
using OpenData.Framework.Core;
using OpenData.Sites.FrontPage.Controllers;

namespace OpenData.Sites.FrontPage.Areas.Wechats.Controllers
{
    public class BaseWechatManageController : BzwayController
    {
        public IDatabase db
        {
            get
            {
                return this.SiteManager.GetSiteDataBase();
            }
        }

        public string CurrentOfficialAccount
        {
            get
            {
                if (this.Session["OfficialAccount"] == null)
                {
                    return string.Empty;
                }
                return this.Session["OfficialAccount"].ToString();
            }
            set
            {
                this.Session["OfficialAccount"] = value;
            }
        }
    }
}