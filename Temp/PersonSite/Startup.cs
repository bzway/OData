using Microsoft.Owin;
using Owin;
using System.Threading.Tasks;
using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Mvc;
using OpenData.Web.Mvc;
using System.Web.Routing;

[assembly: OwinStartupAttribute(typeof(OpenData.WebSite.WebApp.Startup))]
namespace OpenData.WebSite.WebApp
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            //app.Use<FrontPageMiddleware>();
            //ConfigureAuth(app);
        }
    }


}
