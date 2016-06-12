#region License
// 
// Copyright (c) 2013, Bzway team
// 
// Licensed under the BSD License
// See the file LICENSE.txt for details.
// 
#endregion
using OpenData.AppEngine.Dependency.InRequestScope;
using Microsoft.Web.Infrastructure.DynamicModuleHelper;
using Ninject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
[assembly: System.Web.PreApplicationStartMethod(typeof(OpenData.AppEngine.Dependency.NinjectWebCommonStartupTask), "Start")]
namespace OpenData.AppEngine.Dependency
{
    public class NinjectWebCommonStartupTask
    {
        /// <summary>
        /// Starts the application
        /// </summary>
        public static void Start()
        {
            DynamicModuleUtility.RegisterModule(typeof(OnePerRequestHttpModule));
        }
    }
}
