using OpenData.Framework;
using System;
using System.Collections.Generic;
using System.Web.Mvc;
using OpenData.WebApp.Models;
using System.Linq;

namespace OpenData.WebApp.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            //List<OpenEntity> list = new List<OpenEntity>();
            //list.Add(new OpenEntity() { UUID = "test" });
            //list.Add(new OpenEntity() { UUID = "abcd" });
            //var o = list.Where(m => m.Entity.UUID == "test").FirstOrDefault();
            using (var db = new OpenDatabase("test"))
            {
                //var list = db.Entity("").Query().Where(;

            }
            return View();
        }



        public ActionResult Execute(ExecuteModel model)
        {
            if (string.IsNullOrEmpty(model.Js))
            {
                model = new ExecuteModel();
            }
            else
            {
                model.Output = OpenData.Framework.Script.JavascriptExecuter.Exec(model.Js, model.Input);
            }
            return View(model);
        }




        public ActionResult About()
        {
            MyApplicationDbContext db = new MyApplicationDbContext();
            var e = new { id = 1, name = "test" };


            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}