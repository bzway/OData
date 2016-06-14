using OpenData.Data;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using OpenData.Framework.WebApp.Models;

namespace OpenData.Framework.WebApp.Controllers.App
{
    public class SchemaController : BaseController
    {
        public ActionResult Index(string siteName)
        {
            if (string.IsNullOrEmpty(siteName))
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            using (var db = this.SiteManager.GetSiteDataBase())
            {
                var list = db.ToList();
                return View(list);
            }
        }
        public ActionResult Column(string siteName, string schemaName)
        {
            if (string.IsNullOrEmpty(siteName) || string.IsNullOrEmpty(schemaName))
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            using (var db = this.SiteManager.GetSiteDataBase())
            {
                var schema = db[schemaName];
                return View(schema.AllColumns.OrderBy(m => m.Name).ToList());
            }
        }

        public ActionResult Details(string siteName, string schemaName, string columnName)
        {
            if (string.IsNullOrEmpty(schemaName) || string.IsNullOrEmpty(schemaName) || string.IsNullOrEmpty(columnName))
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            using (var db = this.SiteManager.GetSiteDataBase())
            {
                var model = db[schemaName].AllColumns.Where(m => m.Name == columnName).FirstOrDefault();
                if (model == null)
                {
                    return HttpNotFound();
                }
                return View(model);
            }
        }

        // GET: DataSchema/Create
        public ActionResult CreateSchema(string siteName)
        {
            if (string.IsNullOrEmpty(siteName))
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateSchema(SchemaViewModel model, string siteName)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            using (var db = this.SiteManager.GetSiteDataBase())
            {
                Schema schema = Schema.EntitySchema(model.Name, model.Name);
                db.RefreshSchema(schema);
                return RedirectToAction("Column", "Schema", new { schemaName = model.Name, siteName = siteName });
            }
        }


        // GET: DataSchema/Create
        public ActionResult Create(string siteName, string schemaName)
        {
            if (string.IsNullOrEmpty(schemaName) || string.IsNullOrEmpty(schemaName))
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            return View();
        }

        // POST: DataSchema/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Column column, string siteName, string schemaName)
        {
            if (ModelState.IsValid)
            {
                using (var db = this.SiteManager.GetSiteDataBase())
                {

                    db[schemaName].AddColumn(column);
                    db.RefreshSchema(db[schemaName]);
                    return RedirectToAction("Index", new { siteName = siteName, schemaName = schemaName });
                }
            }

            return View(column);
        }

        // GET: DataSchema/Edit/5
        public ActionResult Edit(string siteName, string schemaName, string columnName)
        {
            if (string.IsNullOrEmpty(siteName) || string.IsNullOrEmpty(schemaName) || string.IsNullOrEmpty(columnName))
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            using (var db = this.SiteManager.GetSiteDataBase())
            {
                var model = db[schemaName].AllColumns.Where(m => m.Name == columnName).FirstOrDefault();
                if (model == null)
                {
                    return HttpNotFound();
                }
                return View(model);
            }
        }

        // POST: DataSchema/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Column column, string siteName, string schemaName, string columnName)
        {
            if (!ModelState.IsValid)
            {
                return View(column);
            }
            using (var db = this.SiteManager.GetSiteDataBase())
            {
                db[schemaName].AddColumn(column);
                db.RefreshSchema(db[schemaName]);
                return RedirectToAction("Index", new { siteName = siteName, schemaName = schemaName });
            }
        }

        // GET: DataSchema/Delete/5
        public ActionResult Delete(string siteName, string schemaName, string columnName)
        {
            if (string.IsNullOrEmpty(siteName) || string.IsNullOrEmpty(schemaName) || string.IsNullOrEmpty(columnName))
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            using (var db = this.SiteManager.GetSiteDataBase())
            {
                var model = db[schemaName].AllColumns.Where(m => m.Name == columnName).FirstOrDefault();
                if (model == null)
                {
                    return HttpNotFound();
                }
                return View(model);
            }
        }

        // POST: DataSchema/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string siteName, string schemaName, string columnName)
        {
            using (var db = this.SiteManager.GetSiteDataBase())
            {
                var column = db[schemaName].AllColumns.Where(m => m.Name == columnName).FirstOrDefault();
                if (column != null)
                {
                    db[schemaName].RemoveColumn(column);
                    db.RefreshSchema(db[schemaName]);
                }
                return RedirectToAction("Index", new { siteName = siteName, schemaName = schemaName });
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {

            }
            base.Dispose(disposing);
        }
    }
}
