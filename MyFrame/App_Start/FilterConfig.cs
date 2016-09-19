using MyFrame.Filter;
using System.Web;
using System.Web.Mvc;

namespace MyFrame
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
            filters.Add(new PermissionFilterAttribute());
        }
    }
}
