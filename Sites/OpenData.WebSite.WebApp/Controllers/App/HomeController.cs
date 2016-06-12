using System;
using System.Collections.Generic;
using System.Web.Mvc;
using System.Web;
using System.IO;
using OpenData.Framework.Core;
using OpenData.Script;
using OpenData.Framework.WebApp.Models;

namespace OpenData.Framework.WebApp.Controllers.App
{
    public class HomeController : BzwayController
    {
        public SiteManager SiteManager
        {
            get
            {
                return this.HttpContext.GetSiteManager();
            }
        }

        public ActionResult Index()
        {
            HtmlAgilityPack.HtmlWeb web = new HtmlAgilityPack.HtmlWeb();
            HtmlAgilityPack.HtmlDocument doc = web.Load("https://mp.weixin.qq.com/s?__biz=MjM5OTM5NTg3Mg==&mid=402940395&idx=1&sn=a4b7341dd84ebb518f7bd86981631389&scene=1&srcid=0322EL9CIX0H2xEO7yINnquU&pass_ticket=2KHrBnIH7o4rSeAEFGXwfBo5DvD1mgywXFrTKi7LuE72hVbr5FB0Fw%2FdOhJNMdaJ#rd");
            var contentNode = doc.GetElementbyId("page-content");
            var titleNode = doc.GetElementbyId("activity-name");
            var textNode = doc.GetElementbyId("js_content");
           
            return Content(textNode.InnerHtml);
        }
        public ActionResult Frame(string url)
        {
            return Content(string.Format(@"<iframe src='{0}' style='width: 100%; height:99%; border: none; ' frameborder='0' scrolling='auto'></iframe>", url));
        }
        public void Modules()
        {
            HttpApplication httpApps = ControllerContext.HttpContext.ApplicationInstance;
            // 获取所有 http module
            HttpModuleCollection httpModules = httpApps.Modules;

            Response.Write(string.Format("一共有{0}个 HttpModule</br>", httpModules.Count.ToString()));
            foreach (string activeModule in httpModules.AllKeys)
            {
                Response.Write(activeModule + "</br>");
            }
        }

        public ActionResult upload()
        {



            var r = new List<ViewDataUploadFilesResult>();

            foreach (string file in Request.Files)
            {
                var statuses = new List<ViewDataUploadFilesResult>();
                var headers = Request.Headers;

                if (string.IsNullOrEmpty(headers["X-File-Name"]))
                {
                    UploadWholeFile(Request, statuses);
                }
                else
                {
                    UploadPartialFile(headers["X-File-Name"], Request, statuses);
                }

                JsonResult result = Json(statuses);
                result.ContentType = "text/plain";

                return result;
            }

            return Json(r);
        }

        [HttpGet]
        public void Delete(string id)
        {
            var filename = id;
            var filePath = Server.MapPath("~/upload/" + filename);

            if (System.IO.File.Exists(filePath))
            {
                System.IO.File.Delete(filePath);
            }
        }

        //DONT USE THIS IF YOU NEED TO ALLOW LARGE FILES UPLOADS
        [HttpGet]
        public void Download(string id)
        {
            var filename = id;
            var filePath = Server.MapPath("~/upload/" + filename);

            var context = HttpContext;

            if (System.IO.File.Exists(filePath))
            {
                context.Response.AddHeader("Content-Disposition", "attachment; filename=\"" + filename + "\"");
                context.Response.ContentType = "application/octet-stream";
                context.Response.ClearContent();
                context.Response.WriteFile(filePath);
            }
            else
            {
                context.Response.StatusCode = 404;
            }
        }

        private string EncodeFile(string fileName)
        {
            return Convert.ToBase64String(System.IO.File.ReadAllBytes(fileName));
        }

        //DONT USE THIS IF YOU NEED TO ALLOW LARGE FILES UPLOADS
        //Credit to i-e-b and his ASP.Net uploader for the bulk of the upload helper methods - https://github.com/i-e-b/jQueryFileUpload.Net
        private void UploadPartialFile(string fileName, HttpRequestBase request, List<ViewDataUploadFilesResult> statuses)
        {
            if (request.Files.Count != 1) throw new HttpRequestValidationException("Attempt to upload chunked file containing more than one fragment per request");
            var file = request.Files[0];
            var inputStream = file.InputStream;

            var fullName = Server.MapPath("~/upload/" + fileName);


            using (var fs = new FileStream(fullName, FileMode.Append, FileAccess.Write))
            {
                var buffer = new byte[1024];

                var l = inputStream.Read(buffer, 0, 1024);
                while (l > 0)
                {
                    fs.Write(buffer, 0, l);
                    l = inputStream.Read(buffer, 0, 1024);
                }
                fs.Flush();
            }
            statuses.Add(new ViewDataUploadFilesResult()
            {
                name = fileName,
                size = file.ContentLength,
                type = file.ContentType,
                url = "/Home/Download/" + fileName,
                delete_url = "/Home/Delete/" + fileName,
                thumbnail_url = @"data:image/png;base64," + EncodeFile(fullName),
                delete_type = "GET",
            });
        }

        //DONT USE THIS IF YOU NEED TO ALLOW LARGE FILES UPLOADS
        //Credit to i-e-b and his ASP.Net uploader for the bulk of the upload helper methods - https://github.com/i-e-b/jQueryFileUpload.Net
        private void UploadWholeFile(HttpRequestBase request, List<ViewDataUploadFilesResult> statuses)
        {
            for (int i = 0; i < request.Files.Count; i++)
            {
                var file = request.Files[i];

                var fullPath = Server.MapPath("~/upload/" + file.FileName);

                file.SaveAs(fullPath);

                statuses.Add(new ViewDataUploadFilesResult()
                {
                    name = file.FileName,
                    size = file.ContentLength,
                    type = file.ContentType,
                    url = "/Home/Download/" + file.FileName,
                    delete_url = "/Home/Delete/" + file.FileName,
                    thumbnail_url = @"data:image/png;base64," + EncodeFile(fullPath),
                    delete_type = "GET",
                });
            }
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
        public ActionResult Lang(string id, string url)
        {
            // Validate input
            id = CultureHelper.GetImplementedCulture(id);
            // Save culture in a cookie
            HttpCookie cookie = Request.Cookies["_culture"];
            if (cookie != null)
            {
                cookie.Value = id;   // update cookie value
            }
            else
            {
                cookie = new HttpCookie("_culture");
                cookie.Value = id;
                cookie.Expires = DateTime.Now.AddYears(1);
            }
            this.HttpContext.GetOwinContext().Response.Cookies.Append("_culture", id, new Microsoft.Owin.CookieOptions { Expires = DateTime.Now.AddYears(1) });
            //Response.Cookies.Add(cookie);
            url = this.Request.Headers["Referer"];
            if (string.IsNullOrEmpty(url))
            {
                return RedirectToAction("Index");
            }
            else
            {
                return Redirect(url);
            }
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

    public class ViewDataUploadFilesResult
    {
        public string name { get; set; }
        public int size { get; set; }
        public string type { get; set; }
        public string url { get; set; }
        public string delete_url { get; set; }
        public string thumbnail_url { get; set; }
        public string delete_type { get; set; }
    }

}