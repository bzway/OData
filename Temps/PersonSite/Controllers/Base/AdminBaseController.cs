using OpenData.Business.Service;
using OpenData.Web.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OpenData.WebSite.WebApp.Controllers
{
    [BzwayAuthorize]
    public class AdminBaseController : BzwayController
    {
        public UserManager UserManager
        {
            get
            {
                return this.HttpContext.GetOwinContext().GetUserManager();
            }
        }
        public SiteManager SiteManager
        {
            get
            {
               return this.HttpContext.GetOwinContext().GetSiteManager();
            }
        }
    }
}