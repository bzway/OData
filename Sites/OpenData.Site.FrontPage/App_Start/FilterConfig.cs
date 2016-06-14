using System.Web.Mvc;

namespace OpenData.Sites.FrontPage
{
    public class FilterConfig
    {
          public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }
    }
}
