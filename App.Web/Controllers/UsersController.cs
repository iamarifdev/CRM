using System.Web.Mvc;
using App.Web.Context;
using App.Web.Helper;
using App.Web.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace App.Web.Controllers
{
    [CrmAuthorize(Roles = "Admin")]
    public class UsersController : Controller
    {
        #region Private Zone
        private readonly CrmDbContext _db;
        #endregion
        public UsersController()
        {
            _db = new CrmDbContext();
        }
        // GET: Users
        public ActionResult Index()
        {
            return View();   
        }

        public bool IsAdminUser()
        {
            if (!User.Identity.IsAuthenticated) return false;
            var user = User.Identity;
            var context = new ApplicationDbContext();
            var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));
            var s = userManager.GetRoles(user.GetUserId());
            return s[0] == "Admin";
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