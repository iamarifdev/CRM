using System.Web.Mvc;
using App.Web.Context;
using App.Web.Helper;

namespace App.Web.Controllers
{
    [CrmAuthorize]
    [CrmPermission]
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
            return View();
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