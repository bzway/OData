using Microsoft.Owin;
using Owin;
using System.Threading.Tasks;
using System;
using System.Collections.Generic;
using System.Web.Mvc;
using System.Web;
using OpenData.Framework.Service;
using System.Net;
using OpenData.Framework.Entity;
using System.Linq;

namespace OpenData.Framework
{
    public class AuthorizeAttribute : FilterAttribute, IAuthorizationFilter
    {
        /// <summary>
        /// 认证失败后的链接
        /// </summary>
        public string AuthorizeUrl { get; set; }

        /// <summary>
        /// 判断用户的信息是否完整
        /// </summary>
        public HasInfo HasInfo { get; set; }
        /// <summary>
        /// 判断用户是否拥有角色
        /// </summary>
        public string Roles { get; set; }
        /// <summary>
        /// 判断用户是否可以访问当前页面
        /// </summary>
        public bool DynamicRole { get; set; }
        public AuthorizeAttribute()
        {
            base.Order = 0;
        }
        bool hasRols(UserIdentity user)
        {
            foreach (var item in this.Roles.Split(',', '|', ';'))
            {
                if (user.Roles.Contains(item, StringComparison.CurrentCultureIgnoreCase))
                {
                    return true;
                }
            }
            return false;
        }
        public void OnAuthorization(AuthorizationContext filterContext)
        {
            var httpContext = filterContext.HttpContext;
            var userManager = httpContext.GetUserManager();
            var user = userManager.GetCurrentUser();
            if (user == null)
            {
                httpContext.Response.Redirect("/User/Authorize/Login/?returnUrl=" + WebUtility.UrlEncode(httpContext.Request.RawUrl));
                return;
            }
            if (!string.IsNullOrEmpty(this.Roles))
            {
                if (!hasRols(user))
                {
                    httpContext.Response.Redirect("/User/Authorize/Login/?returnUrl=" + WebUtility.UrlEncode(httpContext.Request.RawUrl));
                    return;
                }
            }
            switch (this.HasInfo)
            {
                case HasInfo.None:

                    break;
                case HasInfo.HasEmail:
                    UserService userService = new UserService();
                    var userEmail = userService.FindUserEmailsByUserID(user.ID).ToList().Where(m => m.IsConfirmed).FirstOrDefault();
                    if (userEmail == null)
                    {
                        httpContext.Response.Redirect("/User/Authorize/Login/?returnUrl=" + WebUtility.UrlEncode(httpContext.Request.RawUrl));
                    }
                    break;
                case HasInfo.HasMobileNumber:

                    break;
                case HasInfo.HasBaseInfo:
                    break;
                case HasInfo.HasAllInfo:
                    break;
                default:
                    break;
            }
        }
    }

    [Flags]
    public enum HasInfo
    {
        None,
        HasEmail,
        HasMobileNumber,
        HasBaseInfo,
        HasAllInfo,
    }
}