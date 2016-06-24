using OpenData.Framework.Core;
using System.Web.Mvc;
using System.Web;
using System.Web.Mvc.Html;
using System.Text;
using OpenData.Framework.Common;
using OpenData.Data.Core;
using OpenData.Framework.Core.Entity;
using Autofac;
using OpenData.Common.AppEngine;
using System.Web.Routing;

namespace OpenData.Sites.FrontPage.Controllers
{
    public class FrontPageController : BzwayController
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public ActionResult Index(string PageUrl)
        {
            if (ApplicationEngine.Current[this.Site.GetSite().Name].IsRegisteredWithName<IController>(PageUrl))
            {
                var controller = ApplicationEngine.Current[this.Site.GetSite().Name].ResolveNamed<IController>(PageUrl);
                RequestContext requestContext = this.ControllerContext.RequestContext;
                controller.Execute(requestContext);
                return null;
            }

            //get webpage according to PageUrl which is a friendly name of the web page created by user.
            var page = this.Site.GetSitePage(PageUrl);
            if (page == null)
            {
                return HttpNotFound();
            }
            //get page view according to file extension which can identify the view enginer.
            FrontViewResult view = new FrontViewResult(this.ControllerContext, page.FileExtension, page.VirtualPath, page.MasterVirtualPath);
            view.ViewBag.Page = page;
            view.ViewBag.SiteManager = this.Site;
            view.ViewBag.UserManager = this.User;
            return view;
        }
    }
}