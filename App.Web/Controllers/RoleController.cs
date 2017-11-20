using System.Linq;
using System.Web.Mvc;
using App.Web.Context;
using App.Web.Helper;
using App.Web.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace App.Web.Controllers
{
    [CrmAuthorize]
    public class RoleController : Controller
    {
        private readonly ApplicationDbContext _context;
        public RoleController()
        {
            _context = new ApplicationDbContext();
        }
        // GET: Role
        public ActionResult Index()
        {
            if (User.Identity.IsAuthenticated)
            {


                if (!IsAdminUser())
                {
                    return RedirectToAction("Index", "Home");
                }
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }

            var roles = _context.Roles.ToList();
            return View(roles);
        }
        public bool IsAdminUser()
        {
            if (!User.Identity.IsAuthenticated) return false;
            var user = User.Identity;
            var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(_context));
            var s = userManager.GetRoles(user.GetUserId());
            return s[0] == "Admin";
        }

        public ActionResult Create()
        {
            if (User.Identity.IsAuthenticated)
            {
                if (!IsAdminUser())
                {
                    return RedirectToAction("Index", "Home");
                }
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }

            var role = new IdentityRole();
            return View(role);
        }
    }


}