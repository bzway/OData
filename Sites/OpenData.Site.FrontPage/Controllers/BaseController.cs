using OpenData.Framework.Core;
using OpenData.Framework.WebApp.Models;
using System.Collections.Generic;

namespace OpenData.Framework.WebApp.Controllers
{
    public class BaseController : BzwayController
    {
        SiteManager siteManager;
        public SiteManager SiteManager
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

    }
}