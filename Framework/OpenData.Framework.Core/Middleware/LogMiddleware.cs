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
            await next(env);
        }
    }
}