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

        public SiteManager SiteManager
        {
            get
            {
                return this.HttpContext.GetSiteManager();
            }
        }

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

//namespace OpenData.Sites.FrontPage
//{
//    public static class FrontPageHtmlHelper
//    {
//        /// <summary>
//        /// 得到当前页面对象
//        /// </summary>
//        /// <param name="helper"></param>
//        /// <returns></returns>
//        public static SitePage FrontPage(this HtmlHelper helper)
//        {
//            return helper.ViewBag.Page;
//        }
//        public static HtmlString RenderBody(this HtmlHelper helper)
//        {
//            return new HtmlString(helper.FrontPage().Content);
//        }

//        public static HtmlString RenderBlocks(this HtmlHelper helper, string blockName)
//        {
//            SiteManager siteManager = helper.ViewBag.SiteManager;
//            using (var db = siteManager.GetSiteDataBase())
//            {
//                StringBuilder sb = new StringBuilder();
//                foreach (var block in db.Entity<PageBlock>().Query().Where(m => m.Name, blockName, CompareType.Equal)
//                    .Where(m => m.PageId, helper.FrontPage().Id, CompareType.Equal).OrderBy(m => m.OrderBy).ToList())
//                {
//                    var item = db.Entity<PageView>().Query().Where(m => m.Id, block.ViewId, CompareType.Equal).First();
//                    if (item == null)
//                    {
//                        continue;
//                    }
//                    switch (item.Type)
//                    {
//                        case BlockType.StaticHtml:
//                            sb.Append(helper.Partial(item.Path));
//                            break;
//                        case BlockType.CreateView:
//                            sb.Append(helper.Partial(item.Path, null));
//                            break;
//                        case BlockType.DeleteView:
//                        case BlockType.UpdateView:
//                            var id = helper.ViewContext.HttpContext.Request.QueryString["id"];
//                            var model = db.DynamicEntity(db[item.EntityName]).Query().Where("Id", id, CompareType.Equal).First();
//                            sb.Append(helper.Partial(item.Path, model));
//                            break;
//                        case BlockType.QueryView:
//                            sb.Append(helper.Partial(item.Path));
//                            break;
//                        default:
//                            break;
//                    }
//                    sb.Append(item.ToString());
//                }
//                return new HtmlString(sb.ToString());
//            }

//        }
//    }
//}