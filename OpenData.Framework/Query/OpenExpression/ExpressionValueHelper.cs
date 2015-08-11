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

namespace OpenData.Framework.Query.OpenExpressions
{
    public static class OpenExpressionValueHelper
    {
        public static object Escape(object value)
        {
            //if (value is DateTime)
            //{
            //    return TimeZoneHelper.ConvertToUtcTime((DateTime)value);
            //}
            //else
            //{
            return value;
            //}
        }
    }
}
