using System;
using System.Globalization;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using OpenData.Business.Service;
using OpenData.Web.Mvc;
using OpenData.Caching;
using System.ComponentModel.DataAnnotations;
using OpenData.Business.Model;

namespace OpenData.WebSite.WebApp.Controllers
{

    public class UserController : BzwayController
    {
        public UserManager UserManager
        {
            get
            {
                return this.HttpContext.GetOwinContext().GetUserManager();
            }
        }

        public ActionResult Index()
        {
            var model = new MemberProfileViewModel()
            {
                Birthday = DateTime.Now.ToShortDateString(),
            };
            return View(model);
        }

        #region Login


        public ActionResult Login(string state, string returnUrl)
        {
            if (string.IsNullOrEmpty(returnUrl))
            {
                returnUrl = this.Request.Headers["Referer"];
            }
            ViewBag.ReturnUrl = returnUrl;
            LoginViewModel model = new LoginViewModel() { RememberMe = true };
            return View(model);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Login(LoginViewModel model, string state, string returnUrl)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var result = await this.UserManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe);
            switch (result)
            {
                case LoginStatus.Success:
                    if (!string.IsNullOrEmpty(state))
                    {
                        var code = Guid.NewGuid().ToString("N");
                        returnUrl = string.Format("{0}&state={1}&code={2}&ErrorCode={3}&ErrorMessage={4}", returnUrl, state, code, "", "");
                        ApplicationEngine.Current.Resolve<ICacheManager>().Set(code, this.UserManager.GetCurrentUser().Token, 60 * 10);
                    }

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
        public ActionResult Authorize(string appid, string scope, string state, string type, string returnUrl)
        {
            //return Redirect(string.Format("{0}&state={1}&Code={2}&ErrorCode={3}&ErrorMessage={4}", returnUrl, state, string.Empty, (int)ErrorCode.Success, ErrorCode.Success));
            //account/authorize?appid=wx8aea35acb51b7625&returnUrl=http%3a%2f%2fnestlemguat.accuat.com%2fTestOpenID.aspx&response_type=code&scope=snsapi_base&state=123

            if (string.IsNullOrEmpty(appid))
            {
                return Redirect(string.Format("{0}&state={1}&code={2}&ErrorCode={3}&ErrorMessage={4}", returnUrl, state, string.Empty, (int)ErrorCode.Success, ErrorCode.Success));
            }
            if (string.IsNullOrEmpty(returnUrl))
            {
                return Redirect(string.Format("{0}&state={1}&code={2}&ErrorCode={3}&ErrorMessage={4}", returnUrl, state, string.Empty, (int)ErrorCode.Success, ErrorCode.Success));
            }
            if (string.IsNullOrEmpty(state))
            {
                return Redirect(string.Format("{0}&state={1}&code={2}&ErrorCode={3}&ErrorMessage={4}", returnUrl, state, string.Empty, (int)ErrorCode.Success, ErrorCode.Success));
            }
            return RedirectToAction("Login", new { ReturnUrl = returnUrl, State = state });
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
        #endregion

        [BzwayAuthorize(WithLock = true)]
        public ActionResult SendEmailCode(string returnUrl, bool? rememberMe)
        {
            rememberMe = rememberMe ?? true;
            this.UserManager.SendEDMValidationCodeAsync(null, returnUrl);
            return RedirectToAction("VerifyCode", new { ReturnUrl = returnUrl, RememberMe = rememberMe.Value, Provider = "email" });
        }
        [BzwayAuthorize(WithLock = true)]
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

        //
        // POST: /Account/VerifyCode
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
            // If a user enters incorrect codes for a specified amount of time then the user account 
            // will be locked out for a specified amount of time. 
            // You can configure the account lockout settings in IdentityConfig
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

        ////
        //// GET: /Account/Register
        //[AllowAnonymous]
        //public ActionResult Register()
        //{
        //    return View();
        //}

        ////
        //// POST: /Account/Register
        //[HttpPost]
        //[AllowAnonymous]
        //[ValidateAntiForgeryToken]
        //public async Task<ActionResult> Register(RegisterViewModel model)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        var user = new ApplicationUser { UserName = model.Email, Email = model.Email };
        //        var result = await UserManager.CreateAsync(user, model.Password);
        //        if (result.Succeeded)
        //        {
        //            await UserManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);

        //            return RedirectToAction("Index", "Home");
        //        }
        //        AddErrors(result);
        //    }

        //    // If we got this far, something failed, redisplay form
        //    return View(model);
        //}

        ////
        //// GET: /Account/ConfirmEmail
        //[AllowAnonymous]
        //public async Task<ActionResult> ConfirmEmail(string userId, string code)
        //{
        //    if (userId == null || code == null)
        //    {
        //        return View("Error");
        //    }
        //    var result = await UserManager.ConfirmEmailAsync(userId, code);
        //    return View(result.Succeeded ? "ConfirmEmail" : "Error");
        //}

        ////
        //// GET: /Account/ForgotPassword
        //[AllowAnonymous]
        //public ActionResult ForgotPassword()
        //{
        //    return View();
        //}

        ////
        //// POST: /Account/ForgotPassword
        //[HttpPost]
        //[AllowAnonymous]
        //[ValidateAntiForgeryToken]
        //public async Task<ActionResult> ForgotPassword(ForgotPasswordViewModel model)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        var user = await UserManager.FindByNameAsync(model.Email);
        //        if (user == null || !(await UserManager.IsEmailConfirmedAsync(user.Id)))
        //        {
        //            // Don't reveal that the user does not exist or is not confirmed
        //            return View("ForgotPasswordConfirmation");
        //        }

        //        // For more information on how to enable account confirmation and password reset please visit http://go.microsoft.com/fwlink/?LinkID=320771
        //        // Send an email with this link
        //        // string code = await UserManager.GeneratePasswordResetTokenAsync(user.Id);
        //        // var callbackUrl = Url.Action("ResetPassword", "Account", new { userId = user.Id, code = code }, protocol: Request.Url.Scheme);		
        //        // await UserManager.SendEmailAsync(user.Id, "Reset Password", "Please reset your password by clicking <a href=\"" + callbackUrl + "\">here</a>");
        //        // return RedirectToAction("ForgotPasswordConfirmation", "Account");
        //    }

        //    // If we got this far, something failed, redisplay form
        //    return View(model);
        //}

        ////
        //// GET: /Account/ForgotPasswordConfirmation
        //[AllowAnonymous]
        //public ActionResult ForgotPasswordConfirmation()
        //{
        //    return View();
        //}

        ////
        //// GET: /Account/ResetPassword
        //[AllowAnonymous]
        //public ActionResult ResetPassword(string code)
        //{
        //    return code == null ? View("Error") : View();
        //}

        ////
        //// POST: /Account/ResetPassword
        //[HttpPost]
        //[AllowAnonymous]
        //[ValidateAntiForgeryToken]
        //public async Task<ActionResult> ResetPassword(ResetPasswordViewModel model)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return View(model);
        //    }
        //    var user = await UserManager.FindByNameAsync(model.Email);
        //    if (user == null)
        //    {
        //        // Don't reveal that the user does not exist
        //        return RedirectToAction("ResetPasswordConfirmation", "Account");
        //    }
        //    var result = await UserManager.ResetPasswordAsync(user.Id, model.Code, model.Password);
        //    if (result.Succeeded)
        //    {
        //        return RedirectToAction("ResetPasswordConfirmation", "Account");
        //    }
        //    AddErrors(result);
        //    return View();
        //}

        ////
        //// GET: /Account/ResetPasswordConfirmation
        //[AllowAnonymous]
        //public ActionResult ResetPasswordConfirmation()
        //{
        //    return View();
        //}

        ////
        //// POST: /Account/ExternalLogin
        //[HttpPost]
        //[AllowAnonymous]
        //[ValidateAntiForgeryToken]
        //public ActionResult ExternalLogin(string provider, string returnUrl)
        //{
        //    // Request a redirect to the external login provider
        //    return new ChallengeResult(provider, Url.Action("ExternalLoginCallback", "Account", new { ReturnUrl = returnUrl }));
        //}


        ////
        //// GET: /Account/ExternalLoginCallback
        //[AllowAnonymous]
        //public async Task<ActionResult> ExternalLoginCallback(string returnUrl)
        //{
        //    var loginInfo = await AuthenticationManager.GetExternalLoginInfoAsync();
        //    if (loginInfo == null)
        //    {
        //        return RedirectToAction("Login");
        //    }

        //    // Sign in the user with this external login provider if the user already has a login
        //    var result = await UserManager.ExternalSignInAsync(loginInfo, isPersistent: false);
        //    switch (result)
        //    {
        //        case SignInStatus.Success:
        //            return RedirectToLocal(returnUrl);
        //        case SignInStatus.LockedOut:
        //            return View("Lockout");
        //        case SignInStatus.RequiresVerification:
        //            return RedirectToAction("SendCode", new { ReturnUrl = returnUrl, RememberMe = false });
        //        case SignInStatus.Failure:
        //        default:
        //            // If the user does not have an account, then prompt the user to create an account
        //            ViewBag.ReturnUrl = returnUrl;
        //            ViewBag.LoginProvider = loginInfo.Login.LoginProvider;
        //            return View("ExternalLoginConfirmation", new ExternalLoginConfirmationViewModel { Email = loginInfo.Email });
        //    }
        //}

        ////
        //// POST: /Account/ExternalLoginConfirmation
        //[HttpPost]
        //[AllowAnonymous]
        //[ValidateAntiForgeryToken]
        //public async Task<ActionResult> ExternalLoginConfirmation(ExternalLoginConfirmationViewModel model, string returnUrl)
        //{
        //    if (User.Identity.IsAuthenticated)
        //    {
        //        return RedirectToAction("Index", "Manage");
        //    }

        //    if (ModelState.IsValid)
        //    {
        //        // Get the information about the user from the external login provider
        //        var info = await AuthenticationManager.GetExternalLoginInfoAsync();
        //        if (info == null)
        //        {
        //            return View("ExternalLoginFailure");
        //        }
        //        var user = new ApplicationUser { UserName = model.Email, Email = model.Email };
        //        var result = await UserManager.CreateAsync(user);
        //        if (result.Succeeded)
        //        {
        //            result = await UserManager.AddLoginAsync(user.Id, info.Login);
        //            if (result.Succeeded)
        //            {
        //                await UserManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
        //                return RedirectToLocal(returnUrl);
        //            }
        //        }
        //        AddErrors(result);
        //    }

        //    ViewBag.ReturnUrl = returnUrl;
        //    return View(model);
        //}


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

        ////
        //// GET: /Account/ExternalLoginFailure
        //[AllowAnonymous]
        //public ActionResult ExternalLoginFailure()
        //{
        //    return View();
        //}

        //protected override void Dispose(bool disposing)
        //{
        //              base.Dispose(disposing);
        //}

        #region Helpers
        // Used for XSRF protection when adding external logins
        private const string XsrfKey = "XsrfId";

        //private IAuthenticationManager AuthenticationManager
        //{
        //    get
        //    {
        //        return HttpContext.GetOwinContext().Authentication;
        //    }
        //}

        //private void AddErrors(IdentityResult result)
        //{
        //    foreach (var error in result.Errors)
        //    {
        //        ModelState.AddModelError("", error);
        //    }
        //}


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