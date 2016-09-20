using frame0.Filter;
using System.Web;
using System.Web.Mvc;

namespace frame0
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
            //filters.Add(new PermissionFilterAttribute());
           // filters.Add(new ExceptionHandleAttribute());
        }
    }
}
