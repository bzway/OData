using System;
using System.Web.Mvc;
using OpenData.Caching;
using System.Web;
using System.Threading;
using System.Collections.Generic;

namespace OpenData.Framework.Core
{
    [HandleError]
    public class BzwayController : Controller
    {
        static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        UserManager userManager;

        public new UserManager User
        {
            get
            {
                if (this.userManager == null)
                {
                    this.userManager = this.HttpContext.GetUserManager();
                }
                return this.userManager;
            }
        }

        protected override IAsyncResult BeginExecuteCore(AsyncCallback callback, object state)
        {
            string cultureName = null;

            // Attempt to read the culture cookie from Request
            HttpCookie cultureCookie = Request.Cookies["_culture"];
            if (cultureCookie != null)
            {
                cultureName = cultureCookie.Value;
            }
            else
            {
                cultureName = Request.UserLanguages != null && Request.UserLanguages.Length > 0 ? Request.UserLanguages[0] : null;
            }
            // Validate culture name
            cultureName = CultureHelper.GetImplementedCulture(cultureName); // This is safe

            // Modify current thread's cultures
            Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo(cultureName);
            Thread.CurrentThread.CurrentUICulture = Thread.CurrentThread.CurrentCulture;
            return base.BeginExecuteCore(callback, state);
        }
        protected ActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }

        public string VerificationCode
        {
            get
            {
                var code = this.Session["ValidateCode"];
                if (code == null)
                {
                    return string.Empty;
                }
                else
                {
                    return this.Session["ValidateCode"].ToString();
                }
            }
            set
            {
                this.Session["ValidateCode"] = value;
            }
        }

        public bool NeedVerificationCode(string key)
        {
            key += GetIP();
            var c = ApplicationEngine.Current.Resolve<ICacheManager>();
            var count = c.Get<int>(key);
            count++;
            c.Set(key, count, 1);
            if (count > 3)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public string GetIP()
        {
            if (string.IsNullOrEmpty(this.Request.ServerVariables["HTTP_X_FORWARDED_FOR"]))
            {
                return this.Request.UserHostAddress;
            }
            else
            {
                return this.Request.ServerVariables["HTTP_X_FORWARDED_FOR"];
            }
        }

        SiteManager siteManager;
        public SiteManager Site
        {
            get
            {
                if (siteManager == null)
                {
                    this.siteManager = this.HttpContext.GetSiteManager();
                }
                return this.siteManager;
            }
        }

        public int pageIndex
        {
            get
            {
                var queryValue = this.Request.QueryString["pageIndex"];
                int result;
                if (int.TryParse(queryValue, out result))
                {
                    return result;
                }
                var cookieValue = this.Request.Cookies.Get("pageIndex");
                if (cookieValue == null)
                {
                    return 1;
                }

                if (string.IsNullOrEmpty(cookieValue.Value))
                {
                    return 1;
                }
                if (int.TryParse(cookieValue.Value, out result))
                {
                    return result;
                }
                return 1;
            }
        }
        public int pageSize
        {
            get
            {
                var cookieValue = this.Request.Cookies.Get("pageSize");
                if (cookieValue == null)
                {
                    return 10;
                }

                if (string.IsNullOrEmpty(cookieValue.Value))
                {
                    return 10;
                }
                int result;
                if (int.TryParse(cookieValue.Value, out result))
                {
                    return result;
                }
                return 10;
            }
        }


        public void Success(string message, bool dismissable = false)
        {
            AddAlert(AlertStyles.Success, message, dismissable);
        }

        public void Information(string message, bool dismissable = false)
        {
            AddAlert(AlertStyles.Information, message, dismissable);
        }

        public void Warning(string message, bool dismissable = false)
        {
            AddAlert(AlertStyles.Warning, message, dismissable);
        }

        public void Danger(string message, bool dismissable = false)
        {
            AddAlert(AlertStyles.Danger, message, dismissable);
        }

        private void AddAlert(string alertStyle, string message, bool dismissable)
        {
            var alerts = TempData.ContainsKey(Alert.TempDataKey)
                ? (List<Alert>)TempData[Alert.TempDataKey]
                : new List<Alert>();

            alerts.Add(new Alert
            {
                AlertStyle = alertStyle,
                Message = message,
                Dismissable = dismissable
            });

            TempData[Alert.TempDataKey] = alerts;
        }

        #region 获取路径
        public string GetAppPath()
        {
            string path = GetAppPathWithOutSlash();
            if (path.Substring(path.Length - 1, 1) != "/") path += "/";
            return path;
        }

        public string GetAppPathWithOutSlash()
        {
            string path = "http://" + Request.Url.Host;
            int port = Request.Url.Port;
            if (port != 80) path += ":" + port.ToString();
            path += Request.ApplicationPath;
            return path;
        }
        #endregion
    }
    public static class AlertStyles
    {
        public const string Success = "success";
        public const string Information = "info";
        public const string Warning = "warning";
        public const string Danger = "danger";
    }
    public class Alert
    {
        public const string TempDataKey = "TempDataAlerts";

        public string AlertStyle { get; set; }
        public string Message { get; set; }
        public bool Dismissable { get; set; }
    }
}