#region License
// 
// Copyright (c) 2013, Bzway team
// 
// Licensed under the BSD License
// See the file LICENSE.txt for details.
// 
#endregion

using Autofac;
using Microsoft.Owin;
using OpenData.Common.AppEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.Web.Routing;

namespace OpenData.Framework.Core
{

    public class FrontPageMiddleware : OwinMiddleware
    {
        public FrontPageMiddleware(OwinMiddleware next)
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