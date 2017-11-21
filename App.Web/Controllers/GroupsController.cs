using System.Web.Mvc;
using App.Web.Context;

namespace App.Web.Controllers
{
    public class GroupsController : Controller
    {
        #region private zone
        private readonly CrmDbContext _db;
        #endregion
        public GroupsController()
        {
            _db = new CrmDbContext();
        }
        // GET: Groups
        public ActionResult Index()
        {
            return View();
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