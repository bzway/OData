#region License
// 
// Copyright (c) 2013, Bzway team
// 
// Licensed under the BSD License
// See the file LICENSE.txt for details.
// 
#endregion 
using OpenData.AppEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OpenData.Web.Common.Persistence.Non_Relational
{
    public static class IPersistableExtensions
    {
        public static T AsActual<T>(this T persistable)
           where T : IPersistable
        {
            if (persistable == null)
            {
                return persistable;
            }
            if (persistable.IsDummy)
            {
                var provider = ApplicationEngine.Current.Resolve<IProvider<T>>();
                return provider.Get(persistable);

            }
            return persistable;
        }
    }
}
