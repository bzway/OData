using System;
using System.Globalization;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Microsoft.Owin.Security;
using OpenData.MyWeb.Models;
using System.Web.Security;

namespace OpenData.MyWeb.Controllers
{
    [Authorize]
    public class AccountController : Controller
    {
        [AllowAnonymous]
        public ActionResult Login(string returnUrl, string message)
        {
            ModelState.AddModelError("", message);
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult Login(LoginViewModel model, string returnUrl)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            using (var db = OpenData.Data.DefaultDatabase.GetDatabase())
            {
                var user = db.Entity<MyUser>().Query().Where(m => m.UserName, model.Email).Where(m => m.Password, model.Password).First();
                if (user == null)
                {
                    ModelState.AddModelError("", "用户名或密码错误请重新再试！");
                    return View(model);
                }
                FormsAuthentication.SetAuthCookie(model.Email, model.RememberMe);
                if (user.IsAdmin)
                {
                    this.Session["UserName"] = "管理员";
                }
                else
                {
                    this.Session["UserName"] = "操作员";
                }

                return RedirectToLocal(returnUrl);
            }
        }



        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LogOff()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Index", "Home");
        }


        private ActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            return RedirectToAction("Index", "Home");
        }


    }
}