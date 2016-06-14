using OpenData.Site.Core;
using System;
using System.Web;
using System.Web.Mvc;

namespace OpenData.Site.FrontPage.Areas.Users.Controllers
{

    public class HomeController : BaseUserController
    {
        [Authorize]
        public ActionResult Index()
        {
            var userId = this.UserManager.GetCurrentUser().ID;
            var list = this.SiteService.FindSiteByUserID(userId);
            return View(list);
        }

        public ActionResult Langauge(string id, string url)
        {
            // Validate input
            id = CultureHelper.GetImplementedCulture(id);
            // Save culture in a cookie
            HttpCookie cookie = Request.Cookies["_culture"];
            if (cookie != null)
            {
                cookie.Value = id;   // update cookie value
            }
            else
            {
                cookie = new HttpCookie("_culture");
                cookie.Value = id;
                cookie.Expires = DateTime.Now.AddYears(1);
            }
            this.HttpContext.GetOwinContext().Response.Cookies.Append("_culture", id, new Microsoft.Owin.CookieOptions { Expires = DateTime.Now.AddYears(1) });
            //Response.Cookies.Add(cookie);
            if (string.IsNullOrEmpty(url))
            {
                url = this.Request.Headers["Referer"];
            }
            if (string.IsNullOrEmpty(url))
            {
                return RedirectToAction("Index");
            }
            else
            {
                return Redirect(url);
            }
        }
    }
}