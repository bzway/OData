using System;
using System.Collections.Generic;
using System.Web.Mvc;
using OpenData.Site.Entity;
using OpenData.Script;
using OpenData.Site.FrontPage.Models;
using OpenData.Site.Core.Wechat.Models;
using OpenData.Site.FrontPage.Controllers;
using HtmlAgilityPack;
using System.Threading.Tasks;

namespace OpenData.Site.FrontPage.Areas.Wechats.Controllers
{
    public class ReceiveController : WechatBaseController
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public ActionResult Message(string id, string signature, string echostr, string timestamp, string nonce)
        {
            log.InfoFormat("{0}{1}{2}{3}{4}", id, signature, echostr, timestamp, nonce);

            Task<ActionResult> task = new Task<ActionResult>(() => { return base.Index(id, signature, echostr, timestamp, nonce); });
            var taskList = new Task[] { task };

            if (Task.WaitAll(taskList, 4900))
            {
                return task.GetAwaiter().GetResult();
            }
            else
            {
                log.Info("Time Out");
                return Content("");
            }
        }

        public override ActionResult ProcessText(string Content, string MsgId)
        {
            this.SiteManager.GetSiteDataBase().Entity<WechatUserInteraction>().Insert(new WechatUserInteraction()
            {
                //UpdatedOn = null,
                //CreatedBy = this.OpenID,
                //CreatedOn = null,
                //UpdatedBy = this.OpenID,
                //Status = 0,
                OfficialAccount = this.WechatManager.CurrentWechat.Id,
                OpenId = this.OpenID,
                Type = InteractionType.ReceiveTextMessage,
                Content = Content,
                MsgId = MsgId,
            });
            if (Content.StartsWith("http://mp.weixin.qq.com", StringComparison.OrdinalIgnoreCase) || Content.StartsWith("https://mp.weixin.qq.com", StringComparison.OrdinalIgnoreCase))
            {
                try
                {
                    HtmlWeb web = new HtmlWeb();
                    HtmlDocument doc = web.Load(Content);
                    var titleNode = doc.GetElementbyId("activity-name");
                    var contentNode = doc.GetElementbyId("js_content");
                    WechatNewsMaterial newsMaterial = new WechatNewsMaterial()
                    {
                        //CreatedBy = null,
                        //CreatedOn = null,
                        //UpdatedBy = null,
                        //UpdatedOn = null,
                        //Id = null,
                        //Status = null,
                        Author = "朱明武",
                        Content = contentNode.OuterHtml,
                        ContentSourceUrl = "http://www.bzway.com",
                        Digest = titleNode.InnerText,
                        OfficialAccount = this.WechatManager.CurrentWechat.Id,
                        IsReleased = false,
                        SortBy = 0,
                        ShowCoverPicture = false,
                        LastUpdateTime = DateTime.Now,
                        Title = titleNode.InnerText,
                        Url = Content,
                        ThumbMediaId = ""
                    };
                    //this.SiteManager.GetSiteDataBase().Entity<WechatNewsMaterial>().Insert(newsMaterial);
                    ResponseNewsMessage message = new ResponseNewsMessage()
                    {
                        CreateTime = DateTime.Now,
                        FromUserName = this.WechatManager.CurrentWechat.OpenId,
                        ToUserName = this.OpenID,
                        Articles = new List<Article>(),
                    };
                    message.Articles.Add(new Article()
                    {
                        Title = titleNode.InnerText,
                        Description = contentNode.InnerText.Substring(0, 100),
                        PicUrl = "http://mmbiz.qpic.cn/mmbiz/vIibpX7BPGhfGBC3854ZL6prrFyUtuCaGMq3ecgmYy6T0J89eyxqTz4LeQlHGY7AQnIKd5uxsP7cP6IdSJReIKQ/640?wx_fmt=jpeg&tp=webp&wxfrom=5&wx_lazy=1",
                        Url = "http://www.bzway.com"
                    });
                    return this.Content(message.ToXMLString());
                }
                catch (Exception ex)
                {

                    log.Error("", ex);
                    return this.Content(new ResponseTextMessage()
                    {
                        Content = ex.Message,
                        CreateTime = DateTime.Now,
                        FromUserName = this.WechatManager.CurrentWechat.OpenId,
                        ToUserName = this.OpenID,
                    }.ToXMLString());
                }
            }
            return base.ProcessText(Content, MsgId);
        }
        public override ActionResult ProcessScan(string scanKey, string Ticket)
        {
            this.SiteManager.GetSiteDataBase().Entity<WechatUserInteraction>().Insert(new WechatUserInteraction()
            {
                //UpdatedOn = null,
                //CreatedBy = this.OpenID,
                //CreatedOn = null,
                //UpdatedBy = this.OpenID,
                //Status = 0,
                OfficialAccount = this.WechatManager.CurrentWechat.Id,
                OpenId = this.OpenID,
                Type = InteractionType.ReceiveScan,
                Content = scanKey,
                MsgId = Ticket,
            });
            return base.ProcessScan(scanKey, Ticket);
        }
        public override ActionResult ProcessScanSubscribe(string scanKey, string Ticket)
        {
            this.SiteManager.GetSiteDataBase().Entity<WechatUserInteraction>().Insert(new WechatUserInteraction()
            {
                //UpdatedOn = null,
                //CreatedBy = this.OpenID,
                //CreatedOn = null,
                //UpdatedBy = this.OpenID,
                //Status = 0,
                OfficialAccount = this.WechatManager.CurrentWechat.Id,
                OpenId = this.OpenID,
                Type = InteractionType.ReceiveScanSubscribe,
                Content = scanKey,
                Remark = Ticket,
                MsgId = null,
            });
            return base.ProcessScanSubscribe(scanKey, Ticket);
        }
        public override ActionResult ProcessSubscribe()
        {
            this.SiteManager.GetSiteDataBase().Entity<WechatUserInteraction>().Insert(new WechatUserInteraction()
            {
                //UpdatedOn = null,
                //CreatedBy = this.OpenID,
                //CreatedOn = null,
                //UpdatedBy = this.OpenID,
                //Status = 0,
                OfficialAccount = this.WechatManager.CurrentWechat.Id,
                OpenId = this.OpenID,
                Type = InteractionType.ReceiveSubscribe,
                Content = null,
                MsgId = null,
            });
            return base.ProcessSubscribe();
        }

        public override ActionResult ProcessImage(string PicUrl, string MediaId, string MsgId)
        {
            this.SiteManager.GetSiteDataBase().Entity<WechatUserInteraction>().Insert(new WechatUserInteraction()
            {
                //UpdatedOn = null,
                //CreatedBy = this.OpenID,
                //CreatedOn = null,
                //UpdatedBy = this.OpenID,
                //Status = 0,
                OfficialAccount = this.WechatManager.CurrentWechat.Id,
                OpenId = this.OpenID,
                Type = InteractionType.ReceiveImageMessage,
                Content = PicUrl,
                Remark = MediaId,
                MsgId = MsgId,
            });
            return base.ProcessImage(PicUrl, MediaId, MsgId);
        }
        public ActionResult Fake()
        {
            FakeViewModel model = new FakeViewModel()
            {
                token = "010c7906e302be7c0b6b5b1c5645d7833d5d97b6",
                signature = "1d8dd5ac39f61666cc53c87f99230c2baf31ea9c",
                timestamp = "timestamp",
                nonce = "nonce",
                data = @"<xml>
<FromUserName></FromUserName>
<ToUserName></ToUserName>
<MsgType></MsgType>
<Content></Content>
<MsgId></MsgId>
</xml>",
                result = ""
            };

            return View(model);
        }
        [WechatAuthorizeAttribute]
        public ActionResult Debug()
        {
            FakeViewModel model = new FakeViewModel()
            {
                token = this.WechatManager.TryGetAccessToken(false),
                signature = "1d8dd5ac39f61666cc53c87f99230c2baf31ea9c",
                nonce = "https://api.weixin.qq.com/cgi-bin/material/batchget_material",
            };

            return View(model);
        }
        [HttpPost]
        public ActionResult Debug(FakeViewModel model)
        {
            model.result = this.WechatManager.HttpPost(model.nonce + "?access_token=" + model.token, model.data);
            return View(model);
        }




        public ActionResult Execute(ExecuteModel model)
        {
            if (string.IsNullOrEmpty(model.Js))
            {
                model = new ExecuteModel();
            }
            else
            {
                model.Output = JavascriptExecuter.Exec(model.Js, model.Input);
            }
            return View(model);
        }


        public ActionResult About()
        {
            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}