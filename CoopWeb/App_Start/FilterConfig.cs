using Cooperative.Filters;
using System.Web;
using System.Web.Mvc;

namespace Cooperative
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }
    }
}
