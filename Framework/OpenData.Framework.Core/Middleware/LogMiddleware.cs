using Microsoft.Owin;
using Owin;
using System.Threading.Tasks;
using System;
using System.Collections.Generic;
using System.Web;

using System.Diagnostics;

namespace OpenData.Framework.Common
{
    using AppFunc = Func<IDictionary<string, object>, Task>;

    public class LogMiddleware
    {

        private readonly AppFunc next;

        public LogMiddleware(AppFunc next)
        {
            this.next = next;
        }

        public async Task Invoke(IDictionary<string, object> env)
        {

            OwinResponse response = new OwinResponse(env);

#if TRACE
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            response.Write("FrontPageMiddleware Start");
#endif
            foreach (var item in env.Keys)
            {
                response.Write(string.Format("{0}:{1}<br/>", item, env[item]));
            }
            await next(env);

#if TRACE

            stopwatch.Stop();
            response.Write(string.Format("FrontPageMiddleware, {0}ms.</br>", stopwatch.ElapsedMilliseconds));
#endif

        }
    }
}