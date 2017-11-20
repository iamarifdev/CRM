using System;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;
using App.Entity.Models;
using App.Web.Context;
using App.Web.Helper;

namespace App.Web.Controllers
{
    [CrmAuthorize]
    public class HomeController : Controller
    {
        #region Private Zone
        private readonly CrmDbContext _db;
        #endregion
        public HomeController()
        {
            _db = new CrmDbContext();
        }
        public ActionResult Index()
        {
            try
            {
                return View();
            }
            catch (Exception ex)
            {
                TempData["Toastr"] = Toastr.DbError(ex.Message);
                return RedirectToAction("Index", "Home");
            }
            
        }

        [ChildActionOnly]
        public ActionResult DisplayMainInfo()
        {
            return PartialView("DisplayMainInfo");
        }
    }
}