using System.Web.Mvc;

namespace OpenData.Site.FrontPage.Controllers
{
    [Authorize]
    public class AdminController : BaseController
    {
        public ActionResult Index()
        {

            return View();
        }
    }
}