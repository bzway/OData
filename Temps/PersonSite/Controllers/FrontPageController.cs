using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc; 
using OpenData.Web.Mvc;
using Microsoft.Owin;

namespace OpenData.WebSite.WebApp.Controllers
{
    public class FrontPageController
    {
        IOwinContext context;
        public FrontPageController(IOwinContext context)
        {
            this.context = context;
        }
        // GET: Tests
        public void Index()
        {
            this.context.Response.Write("test");
        }
    }
}