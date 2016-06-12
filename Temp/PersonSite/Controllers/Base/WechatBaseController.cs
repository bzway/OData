using OpenData.Business.Model;
using OpenData.Business.Service;
using OpenData.Business.Service.Wechat;
using OpenData.Business.Service.Wechat.Models;
using OpenData.Web.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Xml;

namespace OpenData.WebSite.WebApp.Controllers
{
    public class WechatAuthorizeAttribute : AuthorizeAttribute
    {
        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            if (httpContext.Session["WechatID"] == null)
            {
                var CurrentWechat = httpContext.GetOwinContext().GetWechatManager();
                httpContext.Response.Redirect(CurrentWechat.Authorize(httpContext.Request.Url.Host + "/Service/Auth?return_url=" + httpContext.Request.Url, "code"));
                return false;
            }
            return base.AuthorizeCore(httpContext);
        }
    }
    public class WechatBaseController : Controller
    {
        WechatManager CurrentWechat
        {
            get
            {
                return this.HttpContext.GetOwinContext().GetWechatManager();
            }
        }
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public virtual ActionResult Auth(string code, string state, string return_url)
        {

            this.Session["WechatID"] = CurrentWechat.GetOpenID(code);
            return Redirect(return_url);
        }

        protected override IAsyncResult BeginExecuteCore(AsyncCallback callback, object state)
        {
            return base.BeginExecuteCore(callback, state);
        }

        public virtual ActionResult Index(string id)
        {
            var token = id;
            var signature = this.Request.QueryString["signature"].ToString();
            var timestamp = this.Request.QueryString["timestamp"].ToString();
            var nonce = this.Request.QueryString["nonce"].ToString();


            if (!CheckSignature(token, signature, timestamp, nonce))
            {
                log.FatalFormat("CheckSignature Failed from {0}", DateTime.Now);
                this.Response.End();
                return null;
            }
            if (string.Equals(this.Request.HttpMethod, "Post", StringComparison.CurrentCultureIgnoreCase))
            {
                Stream s = this.Request.InputStream;
                byte[] b = new byte[s.Length];
                s.Read(b, 0, (int)s.Length);
                var postString = Encoding.UTF8.GetString(b);
                this.RequestXMLMessage = new XmlDocument();
                RequestXMLMessage.LoadXml(postString);
                OpenID = RequestXMLMessage.GetElementsByTagName("FromUserName")[0].InnerText;
                OriginalID = RequestXMLMessage.GetElementsByTagName("ToUserName")[0].InnerText;
                MessageType = RequestXMLMessage.GetElementsByTagName("MsgType")[0].InnerText;
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
                                return ProcessDefault();
                        }

                    default:
                        return ProcessDefault();
                }
            }
            else
            {
                var echostr = this.Request.QueryString["echostr"].ToString();
                return Content(echostr);
            }
        }

        #region Publice Virtual
        public virtual ActionResult ProcessDefault()
        {
            return null;
        }
        public virtual ActionResult ProcessText(string Content, string MsgId)
        {
            CurrentWechat.SendTemplateMessageAsync(this.OpenID, "BCgvoANrefdscqOeuNRN55z4E8uFIxFcbeGEGuSiX-I", "http://www.bzway.com", "SendTextMessage");
            ReplyTextMessage model = new ReplyTextMessage()
            {
                FromUserName = this.OriginalID,
                ToUserName = this.OpenID,
                Content = Content,
                CreateTime = DateTime.Now
            };
            var res = model.ToString();

            return this.Content(res);
            //            return View("ReplyTextMessage", model);
        }
        public virtual ActionResult ProcessImage(string PicUrl, string MediaId, string MsgId)
        {
            ReplyNewsMessage model = new ReplyNewsMessage()
            {
                CreateTime = DateTime.Now,
                FromUserName = this.OriginalID,
                ToUserName = this.OpenID,
                Articles = new List<Article>(),
            };
            model.Articles.Add(new Article() { Description = "", Title = "test", PicUrl = PicUrl, Url = "http://www.bzway.com" });
            var replyMessage = model.ToString();
            CurrentWechat.SendTemplateMessageAsync(this.OpenID, "BCgvoANrefdscqOeuNRN55z4E8uFIxFcbeGEGuSiX-I", "http://www.bzway.com", "SendTextMessage");
            return this.Content(replyMessage);
        }
        public virtual ActionResult ProcessVoice(string Format, string MediaId, string MsgId)
        {
            return this.Content("test");
        }
        public virtual ActionResult ProcessVideo(string ThumbMediaId, string MediaId, string MsgId)
        {
            return this.Content("test");
        }
        public virtual ActionResult ProcessShortVideo(string ThumbMediaId, string MediaId, string MsgId)
        {
            return this.Content("test");
        }
        public virtual ActionResult ProcessLocation(string Location_X, string Location_Y, string Scale, string Label, string MsgId)
        {
            return this.Content("test");
        }
        public virtual ActionResult ProcessLink(string Title, string Description, string Url, string MsgId)
        {
            return this.Content("test");
        }
        public virtual ActionResult ProcessSubscribe()
        {
            return this.Content("test");
        }
        public virtual ActionResult ProcessUnSubscribe()
        {
            return this.Content("test");
        }
        public virtual ActionResult ProcessScan(string scanKey, string Ticket)
        {
            return this.Content("test");
        }
        public virtual ActionResult ProcessScanSubscribe(string scanKey, string Ticket)
        {
            return this.Content("test");
        }
        public virtual ActionResult ProcessReportLocation(string Latitude, string Longitude, string Precision)
        {
            return this.Content("test");
        }
        public virtual ActionResult ProcessMenuClick(string EventKey)
        {
            return this.Content("test");
        }
        public virtual ActionResult ProcessMenuView(string URL)
        {
            return this.Content("test");
        }

        #endregion

        #region Properties
        public string OpenID { get; set; }
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
