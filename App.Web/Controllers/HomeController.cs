using System.Web.Mvc;

namespace App.Web.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        [ChildActionOnly]
        public ActionResult DisplayMainInfo()
        {
            return PartialView("DisplayMainInfo");
        }
    }
}