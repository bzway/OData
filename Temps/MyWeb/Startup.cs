using Microsoft.Owin;
using Owin;
using System.Threading.Tasks;
using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Mvc;
using OpenData.Common;
using System.Web.Routing;

[assembly: OwinStartupAttribute(typeof(OpenData.MyWeb.Startup))]
namespace OpenData.MyWeb
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
