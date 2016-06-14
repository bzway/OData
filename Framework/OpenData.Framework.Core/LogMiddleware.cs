using Microsoft.Owin;
using Owin;
using System.Threading.Tasks;
using System;
using System.Collections.Generic;
using System.Web;



namespace OpenData.Site
{
    using AppFunc = Func<IDictionary<string, object>, Task>;
    public class LogOwinMiddleware : OwinMiddleware
    {

        public LogOwinMiddleware(OwinMiddleware next)
            : base(next)
        {
        }

        public async override Task Invoke(IOwinContext context)
        {
            var sessionId = context.Request.Cookies["_session"];
            await Next.Invoke(context);
        }
    }



    

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
                //var aaa = "";
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
            //context.Response.Write("FrontPageMiddleware start.");
            //var site = context.GetSiteManager().GetSite();
            
            //if (context.Request.Uri.ToString().Contains("wechat"))
            //{
            //    HomeController controller = new HomeController();
            //    controller.Index();
            //    return;
            //}
            await Next.Invoke(context);
            //context.Response.Write("FrontPageMiddleware End.");
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