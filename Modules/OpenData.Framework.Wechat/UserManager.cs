using Microsoft.Owin;
using OpenData.Framework.Entity;
using OpenData.Globalization;
using OpenData.Message;
using OpenData.Utility;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using Microsoft.Owin.Extensions;

namespace OpenData.Framework.Service
{
    public class UserIdentity
    {
        public string ID { get; set; }
        public string Name { get; set; }
        public string Roles { get; set; }
        public int In { get; set; }
        public LockType Locked { get; set; }
        public string Token { get; set; }
        public string EncryptAES(string input)
        {
            var r = DateTime.Now.Ticks % 30;
            return input.Insert((int)r, this.ID);
        }

        public string DecryptAES(string input)
        {
            return input.Replace(this.ID, "");
        }
    }
    public enum LockType
    {
        None = 0,
        MobilePhone = 1,
        Email = 2,
        Information = 3,
    }
    public enum LoginStatus
    {
        // Summary:
        //     Sign in was successful
        Success = 0,
        //
        // Summary:
        //     User is locked out
        LockedOut = 1,
        //
        // Summary:
        //     Sign in requires addition verification (i.e. two factor)
        RequiresVerification = 2,
        //
        // Summary:
        //     Sign in failed
        Failure = 3,
        RegisterByEmail,
        EmailNoComfirmed,
        RegisterByPhoneNumber,
        PhoneNumberNoComfirmed,
        ProfileNoComfirmed,

    }
    public class UserManager
    {
        #region ctor
        static log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        static readonly string SessionKey = "Bzway.Session";
        static readonly string PasswordKey = "BzwayIdentity";
        static readonly DateTime BaseDate = new DateTime(2010, 4, 22);
        readonly IOwinContext context;
        IUserService userService = ApplicationEngine.Current.Resolve<IUserService>();
        ISMSService smsService = ApplicationEngine.Current.Resolve<ISMSService>();
        ISMTPService smtpService = ApplicationEngine.Current.Resolve<ISMTPService>();
        public UserManager(HttpContextBase context)
        {
            this.context = context.GetOwinContext();
        }
        #endregion
        #region PasswordSignIn
        public Task<LoginStatus> PasswordSignInAsync(string loginName, string password, bool rememberMe = true)
        {
            Task<LoginStatus> task = new Task<LoginStatus>(() => { return this.PasswordSignIn(loginName, password); });
            task.Start();
            return task;
        }

        public LoginStatus PasswordSignIn(string loginName, string password, bool rememberMe = true)
        {
            if (RegexHelper.IsEmail(loginName))
            {
                var userEmail = userService.VerifyEmail(loginName);
                if (userEmail == null)
                {
                    userEmail = userService.RegisterByEmail(loginName, password);
                    SendEDMValidationCodeAsync(userEmail);
                    return LoginStatus.RegisterByEmail;
                }
                var user = userService.FindUserByID(userEmail.UserID);
                if (user == null)
                {
                    return LoginStatus.Failure;
                }
                if (!string.Equals(Cryptor.EncryptMD5(password), user.Password))
                {
                    return LoginStatus.Failure;
                }
                if (!userEmail.IsConfirmed)
                {
                    this.SetAuthCookies(user, LockType.Email, rememberMe);
                    return LoginStatus.EmailNoComfirmed;
                }
                this.SetAuthCookies(user, LockType.None, rememberMe);
                return LoginStatus.Success;
            }
            if (RegexHelper.IsMobileNumber(loginName))
            {
                var phoneNumber = loginName;
                var userPhone = userService.VerifyPhone(phoneNumber);
                if (userPhone == null)
                {
                    userPhone = userService.RegisterByPhoneNumber(phoneNumber, password);
                    SendSMSValidationCodeAsync(userPhone);
                    return LoginStatus.RegisterByPhoneNumber;
                }
                var user = userService.FindUserByID(userPhone.ValidateCode);
                if (user == null)
                {
                    return LoginStatus.Failure;
                }
                if (!string.Equals(Cryptor.EncryptMD5(password), user.Password))
                {
                    return LoginStatus.Failure;
                }
                if (!userPhone.IsConfirmed)
                {
                    SetAuthCookies(user, LockType.MobilePhone, rememberMe);
                    return LoginStatus.PhoneNumberNoComfirmed;
                }
                SetAuthCookies(user, LockType.None, rememberMe);
                return LoginStatus.Success;
            }
            var userName = loginName;
            var verifiedUser = userService.VerifyUser(userName, password);
            if (verifiedUser == null)
            {
                return LoginStatus.Failure;
            }
            if (!verifiedUser.IsConfirmed)
            {
                return LoginStatus.ProfileNoComfirmed;
            }
            if (verifiedUser.IsLocked)
            {
                return LoginStatus.LockedOut;
            }
            SetAuthCookies(verifiedUser, LockType.Information, rememberMe);
            return LoginStatus.Success;
        }
        #endregion
        public void RemoveAuthCookies()
        {
            //myCookie.Expires = DateTime.Now.AddDays(-1d);
            //httpContext.Response.Cookies.Add(myCookie);
            //var httpContext = HttpContext.Current;
            //HttpCookie myCookie = httpContext.Request.Cookies["OpenID"];
            //if (myCookie != null)
            //{
            //    myCookie.Expires = DateTime.Now.AddDays(-1d);
            //    httpContext.Response.Cookies.Add(myCookie);
            //}

            //httpContext.Response.Cookies.Clear();
            //httpContext.Request.Cookies.Clear();
            //this.context.Server.Abandon();
            this.context.Response.Cookies.Delete(SessionKey, new CookieOptions() { Expires = DateTime.UtcNow.AddDays(-1) });
            //this.context.Response.Cookies.Delete(SessionKey);

        }
        private void SetAuthCookies(User user, LockType lockType, bool rememberMe)
        {
            var expringTime = DateTime.UtcNow.AddMonths(rememberMe ? 1 : 0);
            var userIdentity = new UserIdentity()
            {
                ID = user.Id,
                Name = user.NickName,
                Roles = user.Roles,
                Locked = lockType,
                In = (expringTime - BaseDate).Minutes,
            };

            var token = Cryptor.EncryptAES(SerializationHelper.SerializeObjectToJson(userIdentity), PasswordKey);
            userIdentity.Token = token;
            this.context.Set<UserIdentity>(SessionKey, userIdentity);
            this.context.Response.Cookies.Append(SessionKey, token, new CookieOptions { Expires = expringTime });
        }

        public Task<bool> SendEDMValidationCodeAsync(UserEmail email, string returnUrl = "")
        {

            Task<bool> task = new Task<bool>(() => { return this.SendEDMValidationCode(email, returnUrl); });
            task.Start();
            return task;
        }
        public bool SendEDMValidationCode(UserEmail email, string returnUrl = "")
        {
            if (email == null)
            {
                var userIdentiy = this.GetCurrentUser();
                if (userIdentiy == null)
                {
                    return false;
                }
                email = this.userService.FindUserEmailsByUserID(userIdentiy.ID).FirstOrDefault();
            }
            if (email == null)
            {
                return false;
            }
            string from = "上海比之为网络科技有限公司".Localize("phone");
            string to = email.Email;
            string subject = "Email Confirm".Localize("phone");
            string body = string.Format(@"亲爱的 {0},

您在{1}的账号已创建，你即将可以通过{0}登录{1}。

请点击后面的链接完成确认: {1}/account/validation?code={2}&returnUrl={3}

谢谢您的配合!
{4}
", email.Email, context.Request.Host, email.ValidateCode, System.Net.WebUtility.UrlEncode(returnUrl), from);
            smtpService.SendMail(from, to, subject, body);
            return true;
        }


        public Task<bool> SendSMSValidationCodeAsync(UserPhone phone, string returnUrl = "")
        {
            Task<bool> task = new Task<bool>(() => { return this.SendSMSValidationCode(phone, returnUrl); });
            task.Start();
            return task;
        }
        public bool SendSMSValidationCode(UserPhone phone, string returnUrl = "")
        {
            if (phone == null)
            {
                var userIdentiy = this.GetCurrentUser();
                if (userIdentiy == null)
                {
                    return false;
                }
                phone = this.userService.FindUserPhonesByUserID(userIdentiy.ID).FirstOrDefault();
            }
            if (phone == null)
            {
                return false;
            }
            string from = "上海比之为网络科技有限公司".Localize("phone");
            string to = phone.PhoneNumber;
            string subject = "Email Confirm".Localize("phone");
            string body = string.Format(@"亲爱的 {0},

您在{1}的账号已创建，你即将可以通过{0}登录{1}。

请点击后面的链接完成确认: {1}/account/validation?code={2}&returnUrl={3}

谢谢您的配合!
{4}
", phone.PhoneNumber, context.Request.Host, phone.ValidateCode, System.Net.WebUtility.UrlEncode(returnUrl), from);
            smtpService.SendMail(from, to, subject, body);
            return true;
        }

        public UserIdentity GetCurrentUser()
        {
            try
            {
                var userIdentity = this.context.Get<UserIdentity>(SessionKey);
                if (userIdentity != null)
                {
                    return userIdentity;
                }
                var data = this.context.Request.Cookies[SessionKey];
                if (string.IsNullOrEmpty(data))
                {
                    return null;
                }
                data = Cryptor.DecryptAES(data, PasswordKey);
                userIdentity = SerializationHelper.DeserializeObjectJson<UserIdentity>(data);
                if (userIdentity == null)
                {
                    return null;
                }
                userIdentity.Token = data;
                this.context.Set<UserIdentity>(SessionKey, userIdentity);
                return userIdentity;
            }
            catch (Exception ex)
            {
                log.Error("GetCurrentUser", ex);
                return null;
            }
        }

        public Task<LoginStatus> ValidateCodeAsync(string provider, string code, bool rememberMe)
        {
            Task<LoginStatus> task = new Task<LoginStatus>(() => { return this.ValidateCode(provider, code, rememberMe); });
            task.Start();
            return task;
        }

        public LoginStatus ValidateCode(string provider, string code, bool rememberMe)
        {
            if (string.Equals("email", provider, StringComparison.CurrentCultureIgnoreCase))
            {
                var userEmail = this.userService.FindUserEmailByValidationCode(code);
                if (userEmail == null)
                {
                    return LoginStatus.Failure;
                }

                var user = userService.FindUserByID(userEmail.UserID);
                if (user == null)
                {
                    return LoginStatus.Failure;
                }

                userEmail.IsConfirmed = true;
                //userEmail.ValidateCode = string.Empty;
                //userEmail.ValidateTime = DateTime.UtcNow;
                this.userService.ConfirmUserEmail(userEmail);
                SetAuthCookies(user, LockType.None, rememberMe);
                return LoginStatus.Success;
            }
            if (string.Equals("phone", provider, StringComparison.CurrentCultureIgnoreCase))
            {
                var userPhone = this.userService.FindUserPhoneByValidationCode(code);
                if (userPhone == null)
                {
                    return LoginStatus.Failure;
                }

                var user = userService.FindUserByID(userPhone.UserID);
                if (user == null)
                {
                    return LoginStatus.Failure;
                }

                userPhone.IsConfirmed = true;
                this.userService.ConfirmUserPhone(userPhone);
                SetAuthCookies(user, LockType.None, rememberMe);
                return LoginStatus.Success;
            }
            return LoginStatus.Failure;

        }

    }
}