using Kosmos.SchedulerServer.DbContext;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reactive.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;


namespace Kosmos.SchedulerServer
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            GlobalConfiguration.Configure(WebApiConfig.Register);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            IocConfig.RegisterDependencies();

            try
            {
                using (var dbContext = new AppDbContext())
                {
                    var urls = dbContext.Urls.ToList();
                    UrlCache.Init(urls);
                }
            }
            catch (Exception)
            {
            }
        }

        void Application_End(object sender, EventArgs e)
        {

            using (var dbContext = new AppDbContext())
            {
                UrlCache.CacheToDb(dbContext);
            }
        }
    }
}
