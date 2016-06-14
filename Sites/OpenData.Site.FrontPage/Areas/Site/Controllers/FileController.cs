using System;
using System.Collections.Generic;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.IO;
using OpenData.Data.Core;
using OpenData.Framework.Core.Entity;

namespace OpenData.Sites.FrontPage.Areas.Sites.Controllers
{
    public class FileController : BaseSiteController
    {
        #region FileUpload
        public ActionResult Upload()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Upload(string id)
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

        #endregion
        public ActionResult Index()
        {
            //HtmlAgilityPack.HtmlWeb web = new HtmlAgilityPack.HtmlWeb();
            //HtmlAgilityPack.HtmlDocument doc = web.Load("https://mp.weixin.qq.com/s?__biz=MjM5OTM5NTg3Mg==&mid=402940395&idx=1&sn=a4b7341dd84ebb518f7bd86981631389&scene=1&srcid=0322EL9CIX0H2xEO7yINnquU&pass_ticket=2KHrBnIH7o4rSeAEFGXwfBo5DvD1mgywXFrTKi7LuE72hVbr5FB0Fw%2FdOhJNMdaJ#rd");
            //var contentNode = doc.GetElementbyId("page-content");
            //var titleNode = doc.GetElementbyId("activity-name");
            //var textNode = doc.GetElementbyId("js_content");
            //return Content(textNode.InnerHtml);
            var list = this.SiteManager.GetSiteDataBase().Entity<SitePage>().Query().ToList();

            return View(list);
        }

        // GET: Site/Page/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SitePage sitePage = this.SiteManager.GetSiteDataBase().Entity<SitePage>().Query()
                .Where(mbox => mbox.Id, id, CompareType.Equal).First();
            if (sitePage == null)
            {
                return HttpNotFound();
            }
            return View(sitePage);
        }

        // GET: Site/Page/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Site/Page/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,SiteID,FileExtension,Layout,VirtualPath,ExternalUrl,LinkTarget,MasterVirtualPath,PageUrl,PageType,OutputCache,Duration,Published,ContentTitle,Searchable,EnableTheming,EnableScript,Author,Keywords,Description,Customs,HtmlTitle,Canonical,ShowInNavigation,DisplayText,SortBy,ShowInCrumb,AllowedRole,DeniedRole,CreatedOn,UpdatedOn,CreatedBy,UpdatedBy,Status")] SitePage sitePage)
        {
            if (ModelState.IsValid)
            {
                this.SiteManager.GetSiteDataBase().Entity<SitePage>().Insert(sitePage);
                return RedirectToAction("Index");
            }

            return View(sitePage);
        }

        // GET: Site/Page/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SitePage sitePage = this.SiteManager.GetSiteDataBase().Entity<SitePage>().Query().Where(mbox => mbox.Id, id, CompareType.Equal).First();
            if (sitePage == null)
            {
                return HttpNotFound();
            }
            return View(sitePage);
        }

        // POST: Site/Page/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,SiteID,FileExtension,Layout,VirtualPath,ExternalUrl,LinkTarget,MasterVirtualPath,PageUrl,PageType,OutputCache,Duration,Published,ContentTitle,Searchable,EnableTheming,EnableScript,Author,Keywords,Description,Customs,HtmlTitle,Canonical,ShowInNavigation,DisplayText,SortBy,ShowInCrumb,AllowedRole,DeniedRole,CreatedOn,UpdatedOn,CreatedBy,UpdatedBy,Status")] SitePage sitePage)
        {
            if (ModelState.IsValid)
            {
                this.SiteManager.GetSiteDataBase().Entity<SitePage>().Insert(sitePage);
                return RedirectToAction("Index");
            }
            return View(sitePage);
        }



        // POST: Site/Page/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            this.SiteManager.GetSiteDataBase().Entity<SitePage>().Delete(id);
            return RedirectToAction("Index");
        }
    }
}