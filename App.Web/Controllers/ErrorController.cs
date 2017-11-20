using System;
using System.Web.Mvc;
using System.Web.Routing;

namespace App.Web.Controllers
{
    public class ErrorController : Controller
    {
        // GET: Error
        public ActionResult Index()
        {
            return RedirectToRoute(new RouteValueDictionary
            {
                {"action", "Unknown"},
                {"controller", "Errror"}
            });
        }

        public ActionResult Unauthorized()
        {
            return View();
        }

        public ActionResult BadRequest()
        {
            return View();
        }

        public ActionResult Forbidden()
        {
            return View();
        }

        public ActionResult NotFound()
        {
            return View();
        }
        public ActionResult InternalServerError()
        {
            return View();
        }
        public ActionResult Unknown()
        {
            return View();
        }
    }
}