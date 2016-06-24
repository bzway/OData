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
            response.Write("<!--\r\n");
#endif
            foreach (var item in env.Keys)
            {
                response.Write(string.Format("{0}:{1}\r\n", item, env[item]));
            }
            response.Write("\r\n-->");
            await next(env);

#if TRACE

            stopwatch.Stop();
            response.Write(string.Format("<!--Exec in {0}ms.-->", stopwatch.ElapsedMilliseconds));
#endif

        }
    }
}