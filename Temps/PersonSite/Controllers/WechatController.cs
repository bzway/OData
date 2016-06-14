using System;
using System.Collections.Generic;
using System.Web.Mvc; 
using System.Linq;
using OpenData.Script;
using OpenData.Data;
using OpenData.Data.Mongo; 
using OpenData.Business.Model;

namespace OpenData.WebSite.WebApp.Controllers
{
    public class WechatController : WechatBaseController
    {

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
<content></content>
<MsgId></MsgId>
</xml>",
                result = ""
            };

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