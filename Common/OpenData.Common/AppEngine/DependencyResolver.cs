#region License
// 
// Copyright (c) 2013, Bzway team
// 
// Licensed under the BSD License
// See the file LICENSE.txt for details.
// 
#endregion 
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace OpenData.AppEngine
{
    public class DependencyResolver : IDependencyResolver
    {
        public object GetService(Type serviceType)
        {
            return ApplicationEngine.Current.Resolve(serviceType);
        }

        public IEnumerable<object> GetServices(Type serviceType)
        {
            return ApplicationEngine.Current.ResolveAll(serviceType);
        }
    }
}
