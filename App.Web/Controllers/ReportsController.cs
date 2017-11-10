using System.Linq;
using System.Web.Mvc;
using App.Web.Context;

namespace App.Web.Controllers
{
    [Authorize(Roles = "Admin")]
    public class ReportsController : Controller
    {
        #region Private Zone

        private readonly CrmDbContext _db;

        #endregion

        public ReportsController()
        {
            _db = new CrmDbContext();
        }
        // GET: ClientInfoReport
        public ActionResult ClientInfoReport()
        {
            ViewBag.Clients = new SelectList(_db.ClientInfos.ToList(), "Id", "FirstName");
            ViewBag.Branches = new SelectList(_db.BranchInfos.ToList(), "Id", "BranchName");
            ViewBag.Users = new SelectList(_db.Users.ToList(), "Id", "UserName");
            ViewBag.Services = new SelectList(_db.ServiceInfos.ToList(), "Id", "ServiceName");

            return View();
        }
    }
}