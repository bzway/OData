using OpenData.Business.Service;
using OpenData.Data;
using OpenData.RuleEngine;
using OpenData.Web.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OpenData.WebSite.WebApp.Controllers
{

    public class AdminController : AdminBaseController
    {
        public ActionResult Index()
        {
           
            return View();
        }
    }
}