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
using System.Collections.Specialized;

namespace OpenData.Extensions
{
    public static class HttpExtensions
    {
        static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public static T Get<T>(this System.Web.HttpSessionStateBase session, string key = "")
        {
            try
            {
                if (string.IsNullOrEmpty(key))
                {
                    key = typeof(T).Name;
                }
                var t = session[key];
                if (t == null)
                {
                    return default(T);
                }
                return (T)t;
            }
            catch (Exception ex)
            {
                log.Error("Get Session", ex);
                return default(T);
            }
        }
        public static bool Set<T>(this System.Web.HttpSessionStateBase session, T t, string key = "")
        {
            try
            {
                if (string.IsNullOrEmpty(key))
                {
                    key = typeof(T).Name;
                }
                session[key] = t;
                return true;
            }
            catch (Exception ex)
            {
                log.Error("Set Session", ex);
                return false;
            }
        }
    }
}
