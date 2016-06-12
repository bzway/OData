using System.Web.Mvc;

namespace OpenData.Framework.WebApp
{
    public class FilterConfig
    {
          public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }
    }
}
