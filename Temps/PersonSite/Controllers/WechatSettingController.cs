using OpenData.Business.Model;
using OpenData.Data;
using OpenData.Data.Mongo;
using OpenData.Data.Query.OpenExpressions;
using OpenData.Web.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OpenData.WebSite.WebApp.Controllers
{
    public class WechatSettingController : BzwayController
    {
        // GET: Admin/Default
        public ActionResult Index()
        {
            return View();
        }
        //public ActionResult Setting()
        //{
        //    using (var db = DatabaseProvider.Defualt.GetMongoDatabase(""))
        //    {
        //        var model = db.Entity<object>().Query().Where(m => m.IsServiceAccount, true, CompareType.Equal).ToList();
        //        return View(model);
        //    }
        //}

        public ActionResult Menu()
        {
            return View();
        }

    }
}