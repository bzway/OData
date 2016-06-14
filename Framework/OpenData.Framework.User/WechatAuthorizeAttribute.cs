using System;
using System.Web.Mvc;
using System.Web;
using OpenData.Extensions;
using OpenData.Framework.Core.Entity;
using OpenData.Utility;

namespace OpenData.Framework.Core
{
    public class WechatAuthorizeAttribute : FilterAttribute, IAuthorizationFilter
    {
        private static readonly string WechatAuthorizeUrl = "https://open.weixin.qq.com/connect/oauth2/authorize?appid={0}&redirect_uri={1}&response_type=code&scope={2}&state={3}#wechat_redirect";
        private static readonly string WechatAppID = ConfigSetting.Get("WechatAppID");
        public enum WechatAuthorizeType
        {
            BaseInfo,
            UserInfo,
            FanInfo,
            MemberInfo,
        }
        public WechatAuthorizeType AuthorizeType { get; set; }

        public string RedirectUrl { get; set; }
        public WechatAuthorizeAttribute()
        {
            this.Order = 0;
        }

        public void OnAuthorization(AuthorizationContext filterContext)
        {
            var httpContext = filterContext.HttpContext;
            if (string.IsNullOrEmpty(this.RedirectUrl))
            {
                this.RedirectUrl = httpContext.Request.RawUrl;
            }
            var user = httpContext.Session.Get<WechatUser>();
            if (user == null)
            {
                GoAuthorizeUrl(httpContext);
                return;
            }
            switch (this.AuthorizeType)
            {
                case WechatAuthorizeType.BaseInfo:
                    break;
                case WechatAuthorizeType.UserInfo:
                    if (string.IsNullOrEmpty(user.NickName) && string.IsNullOrEmpty(user.HeadImageUrl))
                    {
                        GoAuthorizeUrl(httpContext);
                    }
                    break;
                case WechatAuthorizeType.FanInfo:
                    break;
                case WechatAuthorizeType.MemberInfo:
                    break;
                default:
                    break;
            }
        }

        void GoAuthorizeUrl(HttpContextBase httpContext)
        {
            var state = Guid.NewGuid().ToString("N");

            if (this.AuthorizeType == WechatAuthorizeType.BaseInfo)
            {
                this.RedirectUrl = string.Format(WechatAuthorizeUrl, WechatAppID, HttpUtility.UrlEncode(this.RedirectUrl), "snsapi_base", state);
            }
            else
            {
                this.RedirectUrl = string.Format(WechatAuthorizeUrl, WechatAppID, HttpUtility.UrlEncode(this.RedirectUrl), "snsapi_userinfo", state);
            }
            httpContext.Session.Set<string>(state, "ValidateCode");
            httpContext.Response.Redirect(this.RedirectUrl);
        }
    }
}