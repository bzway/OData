using OpenData.Site.Entity;
using OpenData.Site.Core;
using OpenData.Site.FrontPage.Controllers;
using OpenData.Site.FrontPage.Models;
using System;
using System.Security.Cryptography;
using System.Text;
using System.Web.Mvc;

namespace OpenData.Site.FrontPage.Areas.Api.Controllers
{
    public class PublicController : BaseController
    {
        WechatManager wechatManager;
        public WechatManager WechatManager
        {

            get
            {
                if (this.wechatManager == null)
                {
                    var id = this.RouteData.Values["id"].ToString();
                    if (id == null)
                    {
                        return null;
                    }
                    this.wechatManager = this.HttpContext.GetWechatManager(id);
                }
                return this.wechatManager;
            }
        }

        // GET: Api/Public/Track
        public ActionResult Track(string id)
        {
            var userAgent = this.Request.UserAgent;
            var userLanguages = string.Join(";", this.Request.UserLanguages);
            var ipAddress = this.Request.UserHostAddress;
            var urlReferer = this.Request.Headers["Referer"];
            var wechatOfficialAccount = this.SiteManager.GetSiteDataBase().Entity<WechatOfficialAccount>().Query().Where(m => m.Id == id).First();
            if (wechatOfficialAccount == null)
            {

                return View(new TrackModel()
                {
                    appId = string.Empty,
                    nonceStr = string.Empty,
                    signature = string.Empty,
                    timeStamp = 0,
                    trackId = string.Empty,
                    useWechat = false,
                });
            }

            var jsapi_ticket = this.WechatManager.TryGetJsApiTicket();
            var noncestr = Guid.NewGuid().ToString("N");
            var timestamp = DateTime.Now.Ticks;
            var url = this.Request.Url.ToString();
            StringBuilder sb = new StringBuilder();
            sb.Append("jsapi_ticket=");
            sb.Append(jsapi_ticket);
            sb.Append("&noncestr=");
            sb.Append(noncestr);
            sb.Append("&timestamp=");
            sb.Append(timestamp);
            sb.Append("&url=");
            sb.Append(url);
            var sha1 = SHA1.Create();
            var sha1Arr = sha1.ComputeHash(Encoding.UTF8.GetBytes(sb.ToString()));
            StringBuilder enText = new StringBuilder();
            foreach (var b in sha1Arr)
            {
                enText.AppendFormat("{0:x2}", b);
            }
            var signature = enText.ToString();
            TrackModel model = new TrackModel()
            {
                appId = wechatOfficialAccount.AppID,
                nonceStr = noncestr,
                signature = signature,
                timeStamp = timestamp,
                trackId = Guid.NewGuid().ToString("N"),
                useWechat = false,
            };
            return View(model);
        }
        public ActionResult TrackShare(string sender, string content, string remark, string callback)
        {
            var result = new { sender = sender, content = content, remark = remark };
            if (string.IsNullOrEmpty(callback))
            {
                return Json(result);
            }
            else
            {
                return Content(string.Format("{0}({1})", callback, result.ToString()));
            }
        }
    }
}