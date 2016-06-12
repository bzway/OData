using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Hosting;
using System.Web.UI.WebControls;
using System.Web.Mvc;
using System.Security.Principal;
using System.Web.Security;

namespace OpenData.Utility
{
    public static class Extensions
    {
        public static bool IsNullOrDefault<T>(this T? value) where T : struct
        {
            return default(T).Equals(value.GetValueOrDefault());
        }
        public static string Value(this Enum value)
        {
            return value.ToString();
        }
        public static string Format(this string value, object data)
        {
            return Regex.Replace(value, @"@([\w\d]+)", match =>
            {
                var paraName = match.Groups[1].Value;
                var proper = data.GetType().GetProperty(paraName);
                return proper.GetValue(data).ToString();
            });
        }

        public static IEnumerable<SelectListItem> ToListItem(this Enum value)
        {
            var type = value.GetType();
            List<SelectListItem> li = new List<SelectListItem>();
            foreach (int s in Enum.GetValues(type))
            {
                li.Add(
                    new SelectListItem()
                    {
                        Value = s.ToString(),
                        Text = Enum.GetName(type, s)
                    }
                );
            }
            return li;
        }
    }
}