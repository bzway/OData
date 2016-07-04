using Autofac;
using Microsoft.Owin;
using OpenData.Framework.Common;
using OpenData.Framework.Core;
using Owin;
using System.Web.Mvc;
using System;
using System.Collections.Generic;
using System.Web;
using System.Reflection;
using OpenData.Common.AppEngine;

[assembly: OwinStartup(typeof(OpenData.Sites.FrontPage.Startup))]

namespace OpenData.Sites.FrontPage
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            //app.Use<LogMiddleware>();
            //app.Use<UserIdentityMiddleware>();
            //app.Use<UserSiteMiddleware>();
            //app.Use<FrontPageMiddleware>();
            ConfigureAuth(app);
            //DependencyResolver.SetResolver(new MvcDependencyResolver(DependencyResolver.Current));
        }
    }
}