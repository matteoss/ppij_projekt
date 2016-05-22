using System.Web;
using System.Web.Mvc;

namespace ppij_web_aplikacija
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }
    }
}
