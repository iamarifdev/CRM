using System;
using System.Web.Mvc;
using App.Web.Context;
using App.Web.Helper;
using App.Web.Models;

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
                if (Session.Get<AppData>("AppData") != null) return View();
                
                Session.RemoveAll();
                Session.Abandon();
                return RedirectToAction("Login", "Account");
            }
            catch (Exception)
            {
                Session.RemoveAll();
                Session.Abandon();
                return RedirectToAction("Login", "Account");
            }
            
        }

        [ChildActionOnly]
        public ActionResult DisplayMainInfo()
        {
            return PartialView("DisplayMainInfo");
        }
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}