#region License
// 
// Copyright (c) 2013, Bzway team
// 
// Licensed under the BSD License
// See the file LICENSE.txt for details.
// 
#endregion
using Ninject.Activation;
using Ninject.Syntax;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenData.AppEngine.Dependency.InRequestScope
{
    public static class RequestScopeExtensionMethod
    {
        // Methods
        private static object GetScope(IContext ctx)
        {
            return ((object)System.Web.HttpContext.Current ?? (object)System.Threading.Thread.CurrentThread);
        }

        public static IBindingNamedWithOrOnSyntax<T> InRequestScope<T>(this IBindingInSyntax<T> syntax)
        {
            return syntax.InScope(new Func<IContext, object>(RequestScopeExtensionMethod.GetScope));
        }
    }


}
