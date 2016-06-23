using OpenData.Framework.Core;
using OpenData.Common.Caching;
using System;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Xml;
using OpenData.Framework.Core.Entity;
using OpenData.Common.AppEngine;
using Autofac;

namespace OpenData.Sites.FrontPage.Controllers
{
    public class WechatAuthorizeAttribute : System.Web.Mvc.AuthorizeAttribute
    {
        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            if (WechatContext.CurrentUser == null)
            {
                var uuid = RouteTable.Routes["Wechat"].GetRouteData(httpContext).Values["id"];
                if (uuid == null)
                {
                    return false;
                }
                var wechatManager = httpContext.GetWechatManager(uuid.ToString());
                var state = DateTime.Now.Ticks.ToString();
                ApplicationEngine.Current.Default.Resolve<ICacheManager>().Set(state, 0, 10);
                httpContext.Response.Redirect(wechatManager.TryGetAuthorizeUrl(httpContext.Request.Url.Host + "/Wechat/Wechat/Auth?return_url=" + httpContext.Request.Url, state));
                return false;
            }
            return true;
        }
    }


    public class WechatContext
    {
        readonly string key;
        readonly ICacheManager cache;

        public static WechatUser CurrentUser
        {
            get
            {
                if (HttpContext.Current.Session["wechatUser"] == null)
                {
                    return null;
                }
                return (WechatUser)HttpContext.Current.Session["wechatUser"];
            }

            set
            {
                HttpContext.Current.Session.Add("wechatUser", value);
            }
        }
        public WechatContext(string openId)
        {
            this.key = openId;
            this.cache = ApplicationEngine.Current.Default.Resolve<ICacheManager>();
        }
        public T Get<T>()
        {
            var t = typeof(T).Name;
            return this.cache.Get<T>(this.key + t);
        }
        public void Set<T>(T data)
        {
            var t = typeof(T).Name;
            this.cache.Set(this.key + t, data, 6000);
        }
    }
    public class WechatBaseController : BzwayController
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
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public virtual ActionResult Auth(string code, string state, string return_url)
        {
            if (string.IsNullOrEmpty(state))
            {
                return HttpNotFound();
            }
            if (!ApplicationEngine.Current.Default.Resolve<ICacheManager>().IsSet(state))
            {
                return HttpNotFound();
            }
            var t = WechatManager.GetOpenID(code);
            WechatContext.CurrentUser = new WechatUser() { };
            return Redirect(return_url);
        }
        protected override IAsyncResult BeginExecuteCore(AsyncCallback callback, object state)
        {
            return base.BeginExecuteCore(callback, state);
        }
        public virtual ActionResult Index(string id, string signature, string echostr, string timestamp, string nonce)
        {
            try
            {
                if (string.IsNullOrEmpty(id) || string.IsNullOrEmpty(signature) || string.IsNullOrEmpty(timestamp) || string.IsNullOrEmpty(nonce))
                {
                    log.InfoFormat("parameter missed：id:{0},signature:{1},echostr:{2},timestamp:{3},nonce:{4}", id, signature, echostr, timestamp, nonce);
                    return Content("");
                }
                var db = this.Site.GetSiteDataBase();
                if (db == null)
                {
                    log.Info("database missed:" + this.HttpContext.GetOwinContext().Request.Host.Value);
                    return Content("");
                }

                var wechatSetting = this.WechatManager.CurrentWechat;

                if (wechatSetting == null)
                {
                    log.Info("wechat account missed");
                    return Content("");
                }

                var token = wechatSetting.Token;
                if (token == null)
                {
                    log.Info("wechat token missed");
                    return Content("");
                }

                if (!CheckSignature(token, signature, timestamp, nonce))
                {
                    log.InfoFormat("signature mispassed：{1},{2},{3},{4}@{0}", DateTime.Now, token, signature, timestamp, nonce);
                    return Content("");
                }
                if (string.Equals(this.Request.HttpMethod, "get", StringComparison.CurrentCultureIgnoreCase))
                {
                    log.InfoFormat("signature sucess:{0}", echostr);
                    return Content(echostr);
                }
                this.RequestXMLMessage = new XmlDocument();
                this.RequestXMLMessage.Load(this.Request.InputStream);
                //Stream stream = this.Request.InputStream;
                //byte[] b = new byte[stream.Length];
                //stream.Read(b, 0, (int)stream.Length);
                //var postString = Encoding.UTF8.GetString(b);
                //log.Info(postString);
                //this.RequestXMLMessage.LoadXml(postString);
                this.OpenID = RequestXMLMessage.GetElementsByTagName("FromUserName")[0].InnerText;
                this.OriginalID = RequestXMLMessage.GetElementsByTagName("ToUserName")[0].InnerText;
                this.MessageType = RequestXMLMessage.GetElementsByTagName("MsgType")[0].InnerText;

                switch (this.MessageType)
                {
                    case "text": //1 文本消息 
                        return ProcessText(this.RequestXMLMessage.GetElementsByTagName("Content")[0].InnerText,
                            this.RequestXMLMessage.GetElementsByTagName("MsgId")[0].InnerText);
                    case "image": //2 图片消息
                        return ProcessImage(this.RequestXMLMessage.GetElementsByTagName("PicUrl")[0].InnerText,
                            this.RequestXMLMessage.GetElementsByTagName("MediaId")[0].InnerText,
                            this.RequestXMLMessage.GetElementsByTagName("MsgId")[0].InnerText);
                    case "voice": //3 语音消息
                        return ProcessVoice(this.RequestXMLMessage.GetElementsByTagName("Format")[0].InnerText,
                            this.RequestXMLMessage.GetElementsByTagName("MediaId")[0].InnerText,
                            this.RequestXMLMessage.GetElementsByTagName("MsgId")[0].InnerText);
                    case "video"://4 视频消息
                        return ProcessVideo(this.RequestXMLMessage.GetElementsByTagName("ThumbMediaId")[0].InnerText,
                            this.RequestXMLMessage.GetElementsByTagName("MediaId")[0].InnerText,
                            this.RequestXMLMessage.GetElementsByTagName("MsgId")[0].InnerText);
                    case "shortvideo"://5 小视频消息
                        return ProcessShortVideo(this.RequestXMLMessage.GetElementsByTagName("ThumbMediaId")[0].InnerText,
                            this.RequestXMLMessage.GetElementsByTagName("MediaId")[0].InnerText,
                            this.RequestXMLMessage.GetElementsByTagName("MsgId")[0].InnerText);
                    case "location"://6 地理位置消息
                        return ProcessLocation(this.RequestXMLMessage.GetElementsByTagName("Location_X")[0].InnerText,
                            this.RequestXMLMessage.GetElementsByTagName("Location_Y")[0].InnerText,
                            this.RequestXMLMessage.GetElementsByTagName("Scale")[0].InnerText,
                            this.RequestXMLMessage.GetElementsByTagName("Label")[0].InnerText,
                            this.RequestXMLMessage.GetElementsByTagName("MsgId")[0].InnerText);
                    case "link"://7 链接消息
                        return ProcessLink(this.RequestXMLMessage.GetElementsByTagName("Title")[0].InnerText,
                            this.RequestXMLMessage.GetElementsByTagName("Description")[0].InnerText,
                            this.RequestXMLMessage.GetElementsByTagName("Url")[0].InnerText,
                            this.RequestXMLMessage.GetElementsByTagName("MsgId")[0].InnerText);
                    case "event"://事件
                        var eventType = this.RequestXMLMessage.GetElementsByTagName("Event")[0].InnerText;
                        switch (eventType)
                        {
                            case "subscribe"://1订阅
                                //EventKey  事件KEY值 qrscene_为前缀后面为二维码的参数值     
                                var eventKey = this.RequestXMLMessage.GetElementsByTagName("EventKey")[0].InnerText;
                                if (string.IsNullOrEmpty(eventKey))//2 扫描带参数二维码事件
                                {
                                    return ProcessScanSubscribe(eventKey.Remove(0, 8), this.RequestXMLMessage.GetElementsByTagName("Ticket")[0].InnerText);
                                }
                                return ProcessSubscribe();
                            case "unsubscribe"://2取消订阅
                                return ProcessUnSubscribe();
                            case "SCAN"://1订阅                         
                                return ProcessScan(this.RequestXMLMessage.GetElementsByTagName("EventKey")[0].InnerText, this.RequestXMLMessage.GetElementsByTagName("Ticket")[0].InnerText);
                            case "LOCATION"://3 上报地理位置事件
                                return ProcessReportLocation(this.RequestXMLMessage.GetElementsByTagName("Latitude")[0].InnerText,
                                        this.RequestXMLMessage.GetElementsByTagName("Longitude")[0].InnerText,
                                        this.RequestXMLMessage.GetElementsByTagName("Precision")[0].InnerText);
                            case "CLICK"://4 自定义菜单事件
                                return ProcessMenuClick(this.RequestXMLMessage.GetElementsByTagName("EventKey")[0].InnerText);
                            case "VIEW"://6 点击菜单跳转链接时的事件推送
                                return ProcessMenuView(this.RequestXMLMessage.GetElementsByTagName("EventKey")[0].InnerText);

                            default:
                                return ProcessDefault("event:" + eventType);
                        }

                    default:
                        return ProcessDefault("MessageType:" + this.MessageType);
                }
            }
            catch (Exception ex)
            {

                log.Error(ex);
                return Content(echostr);
            }

        }

        #region Public Virtual
        public virtual ActionResult ProcessDefault(string Event)
        {
            return this.Content(Event);
        }
        public virtual ActionResult ProcessText(string Content, string MsgId)
        {
            return this.Content("");
        }
        public virtual ActionResult ProcessImage(string PicUrl, string MediaId, string MsgId)
        {
            return this.Content("");
        }
        public virtual ActionResult ProcessVoice(string Format, string MediaId, string MsgId)
        {
            return this.Content("");
        }
        public virtual ActionResult ProcessVideo(string ThumbMediaId, string MediaId, string MsgId)
        {
            return this.Content("");
        }
        public virtual ActionResult ProcessShortVideo(string ThumbMediaId, string MediaId, string MsgId)
        {
            return this.Content("");
        }
        public virtual ActionResult ProcessLocation(string Location_X, string Location_Y, string Scale, string Label, string MsgId)
        {
            return this.Content("");
        }
        public virtual ActionResult ProcessLink(string Title, string Description, string Url, string MsgId)
        {
            return this.Content("");
        }
        public virtual ActionResult ProcessSubscribe()
        {
            return this.Content("");
        }
        public virtual ActionResult ProcessUnSubscribe()
        {
            return this.Content("");
        }
        public virtual ActionResult ProcessScan(string scanKey, string Ticket)
        {
            return this.Content("");
        }
        public virtual ActionResult ProcessScanSubscribe(string scanKey, string Ticket)
        {
            return this.Content("");
        }
        public virtual ActionResult ProcessReportLocation(string Latitude, string Longitude, string Precision)
        {
            return this.Content("");
        }
        public virtual ActionResult ProcessMenuClick(string EventKey)
        {
            return this.Content("");
        }
        public virtual ActionResult ProcessMenuView(string URL)
        {
            return this.Content("");
        }

        #endregion

        #region Properties
        public string OpenID { get; set; }
        /// <summary>
        /// 微信公众号的原始Id
        /// </summary>
        public string OriginalID { get; set; }

        public string MessageType { get; set; }

        private string requestUrl = "";
        public string RequestUrl
        {
            get { return requestUrl; }
            set
            {
                if (!string.IsNullOrEmpty(value))
                {
                    var queryQueryIndex = value.IndexOf("?");
                    if (queryQueryIndex > -1)
                    {
                        requestUrl = value.Substring(0, queryQueryIndex);
                    }
                    else
                    {
                        requestUrl = value;
                    }
                }
            }
        }

        WechatContext wechatContext;
        public WechatContext WechatContext
        {
            get
            {
                if (this.wechatContext == null)
                {
                    this.wechatContext = new WechatContext(this.OpenID);
                }
                return this.wechatContext;
            }
        }
        public XmlDocument RequestXMLMessage { get; set; }
        #endregion

        #region CheckSignature

        /// <summary>
        /// 验证微信签名
        /// </summary>
        /// * 将token、timestamp、nonce三个参数进行字典序排序
        /// * 将三个参数字符串拼接成一个字符串进行sha1加密
        /// * 开发者获得加密后的字符串可与signature对比，标识该请求来源于微信。
        /// <returns></returns>
        private bool CheckSignature(string token, string signature, string timestamp, string nonce)
        {
            var arr = new[] { token, timestamp, nonce }.OrderBy(z => z).ToArray();
            var arrString = string.Join("", arr);

            var sha1 = SHA1.Create();
            var sha1Arr = sha1.ComputeHash(Encoding.UTF8.GetBytes(arrString));
            StringBuilder enText = new StringBuilder();
            foreach (var b in sha1Arr)
            {
                enText.AppendFormat("{0:x2}", b);
            }
            var test = enText.ToString();
            return signature.Equals(enText.ToString());
        }


        #endregion


        #region Exception handle

        protected override void OnException(ExceptionContext filterContext)
        {

            base.OnException(filterContext);
        }


        protected override void OnResultExecuted(ResultExecutedContext filterContext)
        {
            base.OnResultExecuted(filterContext);

        }
        #endregion
    }
}
