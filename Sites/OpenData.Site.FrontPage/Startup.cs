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

[assembly: OwinStartup(typeof(OpenData.Sites.FrontPage.Startup))]

namespace OpenData.Sites.FrontPage
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            app.Use<LogMiddleware>();
            //app.Use<UserIdentityMiddleware>();
            //app.Use<SiteMiddleware>();
            app.Use<FrontPageMiddleware>();
            ConfigureAuth(app);
            DependencyResolver.SetResolver(new MyR(DependencyResolver.Current));
        }
    }

    public class MyR : IDependencyResolver
    {
        IDependencyResolver _this;
        public MyR(IDependencyResolver r)
        {
            this._this = r;
        }
        object IDependencyResolver.GetService(Type serviceType)
        {
            var i = this._this.GetService(serviceType);
            if (i == null)
            {
                HttpContext.Current.Response.Write(string.Format("{0}:{1}<BR />", i, serviceType));
            }
            else
            {
                HttpContext.Current.Response.Write(string.Format("{0}:{1}<BR />", i.GetType(), serviceType));
            }
            return i;
        }

        IEnumerable<object> IDependencyResolver.GetServices(Type serviceType)
        {
            var i = this._this.GetServices(serviceType);
            foreach (var item in i)
            {
                HttpContext.Current.Response.Write(string.Format("{0}:{1}<BR />", item.GetType(), serviceType));
            }
            return i;
        }
    }
}