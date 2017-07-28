using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace App.Web.Controllers
{
    public class GeneralSettingsController : Controller
    {
        // GET: GeneralSettings
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Add(FormCollection collection)
        {

            return RedirectToAction("Index", "GeneralSettings");
        }
    }
}