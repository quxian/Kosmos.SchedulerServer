using System.Web;
using System.Web.Mvc;

namespace Kosmos.SchedulerServer {
    public class FilterConfig {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters) {
            filters.Add(new HandleErrorAttribute());
        }
    }
}
