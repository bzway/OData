//using Bzway.Business.Model;
//using Bzway.Data;
//using Bzway.Data.Mongo;
//using Bzway.Data.Query.OpenExpressions;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Net;
//using System.Web;
//using System.Web.Mvc;
//using Bzway.Globalization;
//using System.Data;
//using System.IO;
//using Bzway.Utility;
//using Bzway.Web.Mvc;
//using Bzway.Business.Service;

//namespace Bzway.WebSite.WebApp.Controllers
//{
//    public class CardController : BzwayController
//    {
//        CardService cardService = ApplicationEngine.Current.Resolve<CardService>();

//        public ActionResult Index()
//        {
//            return View();
//        }
//        [HttpPost]
//        [ValidateAntiForgeryToken]
//        public ActionResult Index(CardSearchViewModel model)
//        {
//            if (!ModelState.IsValid)
//            {
//                return View(model);
//            }
//            this.cardService.SearchCard(model);

//            return View(model);
//        }



//        // GET: Tests/Details/5
//        public ActionResult Details(string id)
//        {
//            if (string.IsNullOrEmpty(id))
//            {
//                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
//            }
//            var model = cardService.FindoCardByID(id);
//            if (model == null)
//            {
//                return HttpNotFound();
//            }
//            return View(model);
//        }

//        // GET: Tests/Create
//        public ActionResult Create()
//        {
//            return View();
//        }

//        // POST: Tests/Create
//        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
//        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
//        [HttpPost]
//        [ValidateAntiForgeryToken]
//        public ActionResult Create(ViewUserCard model)
//        {
//            if (!ModelState.IsValid)
//            {
//                return View(model);
//            }
//            cardService.CreateUserCard(model);
//            return View("Close");
//        }

//        // GET: Tests/Edit/5
//        public ActionResult Edit(string id)
//        {
//            if (string.IsNullOrEmpty(id))
//            {
//                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
//            }
//            var model = cardService.FindoCardByID(id);
//            if (model == null)
//            {
//                return HttpNotFound();
//            }
//            return View(model);
//        }

//        // POST: Tests/Edit/5
//        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
//        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
//        [HttpPost]
//        [ValidateAntiForgeryToken]
//        public ActionResult Edit(ViewUserCard model, string id)
//        {
//            if (!ModelState.IsValid)
//            {
//                return View(model);
//            }
//            if (string.IsNullOrEmpty(id))
//            {
//                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
//            }
//            cardService.UpdateCard(model);
//            return View("Close");
//        }

//        // GET: Tests/Delete/5
//        public ActionResult Delete(string id)
//        {
//            if (string.IsNullOrEmpty(id))
//            {
//                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
//            }
//            var model = cardService.FindoCardByID(id);
//            if (model == null)
//            {
//                return HttpNotFound();
//            }

//            return View(model);

//        }

//        // POST: Tests/Delete/5
//        [HttpPost, ActionName("Delete")]
//        [ValidateAntiForgeryToken]
//        public ActionResult DeleteConfirmed(string id)
//        {
//            if (string.IsNullOrEmpty(id))
//            {
//                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
//            }
//            cardService.DeleteCard(id);
//            return View("Close");
//        }



//        // GET: Tests/Create
//        public ActionResult Import()
//        {
//            return View();
//        }

//        // POST: Tests/Create
//        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
//        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
//        [HttpPost]
//        [ValidateAntiForgeryToken]
//        public ActionResult Import(SearchViewModel model)
//        {
//            if (!ModelState.IsValid)
//            {
//                return View(model);
//            }
//            if (this.Request.Files.Count == 0)
//            {
//                this.ModelState.AddModelError("", "No file uploaded".Localize());
//                return View(model);
//            }

//            var file = this.Request.Files[0];
//            if (file == null || file.ContentLength == 0)
//            {
//                this.ModelState.AddModelError("", "No file uploaded".Localize());
//                return View(model);
//            }


//            string originalName = file.FileName;
//            string fileExtension = Path.GetExtension(originalName).ToLower();

//            if (fileExtension != ".xls" && fileExtension != ".xlsx" && fileExtension != ".csv")
//            {
//                this.ModelState.AddModelError("", "File Format is not supported".Localize());
//                return View(model);
//            }
//            //限制上传大小为13M
//            int size = 20 * 1024 * 1024;
//            if (file.ContentLength > size)
//            {
//                this.ModelState.AddModelError("", "File is too big to support".Localize());
//                return View(model);
//            }

//            string dir = AppDomain.CurrentDomain.BaseDirectory + "UpLoad\\";//上传文件目录
//            if (!Directory.Exists(dir))
//            {
//                Directory.CreateDirectory(dir);
//            }
//            string fileName = String.Concat(dir, DateTime.Now.ToString("yyyyMMddHHmmssffff"), fileExtension);
//            file.SaveAs(fileName);//保存上传文件
//            DataSet ds;
//            if (fileExtension == ".xls" || fileExtension == ".xlsx")
//            {
//                ds = ExcelHelper.GetDataFromExcel(fileName);
//            }
//            else
//            {
//                ds = new DataSet();
//                ds.Tables.Add(CSVHelper.Import(fileName, true, ','));
//            }

//            if (ds == null || ds.Tables.Count == 0)
//            {
//                this.ModelState.AddModelError("", "No data in the file".Localize());
//                return View(model);
//            }

//            cardService.Import(ds);

//            return View("Close");
//        }


//        protected override void Dispose(bool disposing)
//        {
//            if (disposing)
//            {
                
//            }
//            base.Dispose(disposing);
//        }
//    }
//}
