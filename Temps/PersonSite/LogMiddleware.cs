using Microsoft.Owin;
using Owin;
using System.Threading.Tasks;
using System;
using System.Collections.Generic;



namespace OpenData.WebSite.WebApp
{
    using AppFunc = Func<IDictionary<string, object>, Task>;
    using OpenData.Web.Mvc;
    using System.Web.Routing;
    using System.Web;
    using System.Collections.Specialized;
    using System.Web.Mvc;
    using OpenData.WebSite.WebApp.Controllers;

    public class LogMiddleware
    {

        private readonly AppFunc next;

        public LogMiddleware(AppFunc next)
        {
            this.next = next;
        }

        public async Task Invoke(IDictionary<string, object> env)
        {
            if (env.ContainsKey("_culture"))
            {
                var aaa = "";
            }
            Console.WriteLine("LogMiddleware Start.");
            await next(env);
            Console.WriteLine("LogMiddleware End.");
        }
    }

    public class FrontPageMiddleware : OwinMiddleware
    {

        public FrontPageMiddleware(OwinMiddleware next)
            : base(next)
        {
        }

        public async override Task Invoke(IOwinContext context)
        {
            if (context.Request.Uri.ToString().Contains("wechat"))
            {
                FrontPageController controller = new FrontPageController(context);
                controller.Index();
                return;
            }

            await Next.Invoke(context);
            context.Response.Write("FrontPageMiddleware End.");
        }

    }

    public class XRequest : HttpRequestBase
    {
        public XRequest()
        {

        }
    }
    public class XResponse : HttpResponseBase
    {

    }
    public class XContext : HttpContextBase
    {
    }
}