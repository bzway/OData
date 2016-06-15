using Microsoft.Owin;
using System.Threading.Tasks;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace OpenData.Framework.Common
{
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
}