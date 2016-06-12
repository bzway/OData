using System.Web.Mvc;

namespace OpenData.Framework.WebApp.Controllers
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