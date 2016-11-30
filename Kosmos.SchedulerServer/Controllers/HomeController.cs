using System.Web.Mvc;

namespace Kosmos.DownloaderServer.Controllers {
    public class HomeController : Controller {
        
        public ActionResult Index() {
            ViewBag.Title = "Home Page";

            return View();
        }
    }
}
