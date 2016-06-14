using OpenData.Framework.Core;
using System.Web.Mvc;
using System.Web;
using System.Web.Mvc.Html;
using System.Text;
using OpenData.Framework.Common;
using OpenData.Data.Core;
using OpenData.Framework.Core.Entity;

namespace OpenData.Sites.FrontPage.Controllers
{
    public class FrontPageController : BzwayController
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public ActionResult Index(string PageUrl)
        {
            //get webpage according to PageUrl which is a friendly name of the web page created by user.
            var page = this.SiteManager.GetSitePage(PageUrl);
            if (page == null)
            {
                return HttpNotFound();
            }
            //get page view according to file extension which can identify the view enginer.
            FrontViewResult view = new FrontViewResult(this.ControllerContext, page.FileExtension, page.VirtualPath, page.MasterVirtualPath);
            view.ViewBag.Page = page;
            view.ViewBag.SiteManager = this.SiteManager;
            view.ViewBag.UserManager = this.UserManager;
            return view;
        }
    }
}