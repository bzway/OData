using OpenData.Site.Entity;
using OpenData.Site.Core;
using OpenData.Caching;
using OpenData.Data;
using OpenData.Utility;
using System;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using System.Web.Mvc;
using OpenData.Site.FrontPage.Models;

namespace OpenData.Site.FrontPage.Areas.Users.Controllers
{
    public class AuthorizeController : BaseUserController
    {
        public ActionResult Login(string state, string returnUrl)
        {
            if (string.IsNullOrEmpty(returnUrl))
            {
                returnUrl = this.Request.Headers["Referer"];
            }
            if (string.IsNullOrEmpty(returnUrl))
            {
                returnUrl = "/";
            }
            if (returnUrl.Contains("?"))
            {
                returnUrl = string.Format("{0}&state={1}", returnUrl, state);
            }
            else
            {
                returnUrl = string.Format("{0}?state={1}", returnUrl, state);
            }

            ViewBag.ReturnUrl = returnUrl;
            LoginViewModel model = new LoginViewModel() { RememberMe = true };
            return View(model);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Login(LoginViewModel model, string returnUrl)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var result = await this.UserManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe);
            switch (result)
            {
                case LoginStatus.Success:
                    var code = Guid.NewGuid().ToString("N");
                    returnUrl = string.Format("{0}&code={1}", returnUrl, code);
                    ApplicationEngine.Current.Resolve<ICacheManager>().Set(code, this.UserManager.GetCurrentUser().Token, 60 * 10);
                    return Redirect(returnUrl);
                case LoginStatus.LockedOut:
                    return View("Lockout");
                case LoginStatus.RegisterByEmail:
                    return RedirectToAction("SendEmailCode", new { ReturnUrl = returnUrl, RememberMe = model.RememberMe });
                case LoginStatus.EmailNoComfirmed:
                    return RedirectToAction("SendEmailCode", new { ReturnUrl = returnUrl, RememberMe = model.RememberMe });
                case LoginStatus.RegisterByPhoneNumber:
                case LoginStatus.PhoneNumberNoComfirmed:
                    return RedirectToAction("SendPhoneCode", new { ReturnUrl = returnUrl, RememberMe = model.RememberMe });
                case LoginStatus.Failure:
                default:
                    ModelState.AddModelError("", "Invalid login attempt.");
                    return View(model);
            }
        }

        
        public ActionResult SendEmailCode(string returnUrl, bool? rememberMe)
        {
            rememberMe = rememberMe ?? true;
            this.UserManager.SendEDMValidationCodeAsync(null, returnUrl);
            return RedirectToAction("VerifyCode", new { ReturnUrl = returnUrl, RememberMe = rememberMe.Value, Provider = "email" });
        }
       
        public ActionResult SendPhoneCode(string returnUrl, bool? rememberMe)
        {
            rememberMe = rememberMe ?? true;
            this.UserManager.SendSMSValidationCodeAsync(null, returnUrl);
            return RedirectToAction("VerifyCode", new { ReturnUrl = returnUrl, RememberMe = rememberMe.Value, Provider = "phone" });
        }
        public ActionResult VerifyCode(string returnUrl, string provider, bool rememberMe, string code)
        {
            return View(new VerifyCodeViewModel { RememberBrowser = rememberMe, Provider = provider, Code = code, ReturnUrl = returnUrl, RememberMe = rememberMe });
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> VerifyCode(VerifyCodeViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            // The following code protects for brute force attacks against the two factor codes. 
            // If a user enters incorrect codes for a specified amount of time then the user id 
            // will be locked out for a specified amount of time. 
            // You can configure the id lockout settings in IdentityConfig
            //var result = await UserManager.TwoFactorSignInAsync(model.Provider, model.Code, isPersistent: model.RememberMe, rememberBrowser: model.RememberBrowser);
            //switch (result)
            //{
            //    case SignInStatus.Success:
            //        return RedirectToLocal(model.ReturnUrl);
            //    case SignInStatus.LockedOut:
            //        return View("Lockout");
            //    case SignInStatus.Failure:
            //    default:
            //        ModelState.AddModelError("", "Invalid code.");
            //        return View(model);
            //}
            LoginStatus result = await this.UserManager.ValidateCodeAsync(model.Provider, model.Code, model.RememberMe);
            switch (result)
            {
                case LoginStatus.Success:
                    return Redirect(model.ReturnUrl);
                case LoginStatus.LockedOut:
                    return View("Lockout");
                case LoginStatus.RequiresVerification:
                    break;
                case LoginStatus.Failure:
                    return View("Failure");
                case LoginStatus.RegisterByEmail:
                    break;
                case LoginStatus.EmailNoComfirmed:
                    return View("SendEmailCode");
                case LoginStatus.RegisterByPhoneNumber:
                    break;
                case LoginStatus.PhoneNumberNoComfirmed:
                    return View("SendPhoneCode");
                case LoginStatus.ProfileNoComfirmed:
                    return View("Profile");
                default:
                    break;
            }
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LogOff(string returnUrl)
        {
            this.UserManager.RemoveAuthCookies();
            if (string.IsNullOrEmpty(returnUrl))
            {
                return RedirectToAction("Index", "Home");
            }
            return Redirect(returnUrl);
        }

        public ActionResult Index(string appid, string scope, string state, string type, string returnUrl)
        {
            if (string.IsNullOrEmpty(returnUrl))
            {
                returnUrl += "/User/";
            }
            if (this.UserManager.GetCurrentUser() == null)
            {
                if (returnUrl.Contains("?"))
                {
                    returnUrl = string.Format("{0}&state={1}", returnUrl, state);
                }
                else
                {
                    returnUrl = string.Format("{0}?state={1}", returnUrl, state);
                }
                returnUrl = string.Format("{0}&appid={1}&scope={2}&type={3}", returnUrl, appid, scope, type);
                return RedirectToAction("Login", new { ReturnUrl = returnUrl });
            }

            if (string.IsNullOrEmpty(appid))
            {
                return Redirect(string.Format("{0}&state={1}&code={2}&ErrorCode={3}&ErrorMessage={4}", returnUrl, state, string.Empty, (int)ErrorCode.Success, ErrorCode.Success));
            }
            if (string.IsNullOrEmpty(type))
            {
                type = "code";
            }
            if (string.IsNullOrEmpty(scope))
            {
                scope = "get_user_id";
            }
            var code = Guid.NewGuid().ToString("N");

            var accessToken = Guid.NewGuid().ToString("N");
            var openID = Cryptor.EncryptMD5(this.UserManager.GetCurrentUser().ID + appid);
            var expiredTime = DateTime.UtcNow.AddHours(2);
            SiteManager siteManager = new SiteManager(appid);
            var db = siteManager.GetSiteDataBase();
            var siteAuth = db.Entity<SiteAuth>().Query()
                .Where(m => m.AppID, appid, CompareType.Equal)
                .Where(m => m.UserID, this.UserManager.GetCurrentUser().ID, CompareType.Equal)
                .First();
            if (siteAuth != null)
            {
                db.Entity<SiteAuth>().Delete(siteAuth);

            }
            siteAuth = new SiteAuth()
            {
                Status = 0,
                ExpiredTime = expiredTime,
                CreatedOn = DateTime.UtcNow,
                UpdatedBy = this.UserManager.GetCurrentUser().ID,
                UpdatedOn = DateTime.UtcNow,
                AppID = appid,
                CreatedBy = this.UserManager.GetCurrentUser().ID,
                OpenID = openID,
                Scope = scope,
                UserID = this.UserManager.GetCurrentUser().ID,
                Id = code,
                AccessToken = accessToken,
            };
            db.Entity<SiteAuth>().Insert(siteAuth);
            ApplicationEngine.Current.Resolve<ICacheManager>().Set(code, siteAuth, 10 * 60);

            return Redirect(string.Format("{0}&state={1}&code={2}&ErrorCode={3}&ErrorMessage={4}", returnUrl, state, code, (int)ErrorCode.Success, ErrorCode.Success));
        }
        [HttpPost]

        public ActionResult AccessToken(string appid, string appsecret, string state, string code, string scope)
        {
            using (var db = OpenDatabase.GetDatabase())
            {
                var site = db.Entity<Entity.Site>().Query()
                    .Where(m => m.Id, appid, CompareType.Equal)
                    .Where(m => m.AppSecret, appsecret, CompareType.Equal).First();
                if (site == null)
                {
                    return Json(new
                    {
                        AccessToken = string.Empty,
                        ExpiredTime = string.Empty,
                        OpenID = string.Empty,
                        ErrorCode = "0001",
                        ErrorMessage = "error",
                    });
                }

            }
            var siteManager = new SiteManager(appid);
            var siteAuth = siteManager.GetSiteDataBase().Entity<SiteAuth>().Query()
                  .Where(m => m.Id, code, CompareType.Equal)
                  .Where(m => m.AppID, appid, CompareType.Equal)
                  .Where(m => m.ExpiredTime, DateTime.UtcNow, CompareType.GreaterThanOrEqual)
                  .First();
            if (siteAuth == null)
            {
                return Json(new
                {
                    AccessToken = string.Empty,
                    ExpiredTime = string.Empty,
                    OpenID = string.Empty,
                    ErrorCode = "0001",
                    ErrorMessage = "error",
                });
            }
            else
            {
                return Json(new
                {
                    AccessToken = siteAuth.AccessToken,
                    ExpiredTime = siteAuth.ExpiredTime,
                    OpenID = siteAuth.OpenID,
                    ErrorCode = string.Empty,
                    ErrorMessage = string.Empty,
                });
            }
        }
        public enum ErrorCode
        {
            [Display(Name = "请求成功")]
            Success = 0,
            [Display(Name = "请求成功")]
            AppIDWrong = 40001,
        }

        [HttpPost]
        public ActionResult Token(TokenViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return this.Json(new TokenJson() { ErrorCode = "", ErrorMessage = "" });
            }
            return this.Json(new TokenJson() { ErrorCode = "", ErrorMessage = "" });
        }
        public class TokenJson
        {
            public string AccessToken { get; set; }
            public string ExpiredTime { get; set; }
            public string OpenID { get; set; }
            public string ErrorCode { get; set; }
            public string ErrorMessage { get; set; }
        }

        #region Helpers
        // Used for XSRF protection when adding external logins
        private const string XsrfKey = "XsrfId";



        internal class ChallengeResult : HttpUnauthorizedResult
        {
            public ChallengeResult(string provider, string redirectUri)
                : this(provider, redirectUri, null)
            {
            }

            public ChallengeResult(string provider, string redirectUri, string userId)
            {
                LoginProvider = provider;
                RedirectUri = redirectUri;
                UserId = userId;
            }

            public string LoginProvider { get; set; }
            public string RedirectUri { get; set; }
            public string UserId { get; set; }

            public override void ExecuteResult(ControllerContext context)
            {
                //var properties = new AuthenticationProperties { RedirectUri = RedirectUri };
                //if (UserId != null)
                //{
                //    properties.Dictionary[XsrfKey] = UserId;
                //}
                //context.HttpContext.GetOwinContext().Authentication.Challenge(properties, LoginProvider);
            }
        }
        #endregion
    }
}