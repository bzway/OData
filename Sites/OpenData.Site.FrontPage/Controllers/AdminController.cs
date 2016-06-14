using System.Web.Mvc;

namespace OpenData.Sites.FrontPage.Controllers
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