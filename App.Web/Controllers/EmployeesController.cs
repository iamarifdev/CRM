using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using App.Entity.Models;
using App.Web.Context;
using App.Web.Helper;

namespace App.Web.Controllers
{
    [CrmAuthorize(Roles = "Admin")]
    [CrmPermission]
    public class EmployeesController : Controller
    {
        #region Private Zone
        private readonly CrmDbContext _db;
        #endregion
        public EmployeesController()
        {
            _db = new CrmDbContext();
        }
        // GET: Employees
        public ActionResult Index()
        {
            return View();
        }

        // GET: Employees/Create
        public ActionResult Create()
        {
            try
            {
                ViewBag.Designations = new SelectList(_db.EmployeeDesignations, "Id", "DesignationTitleEn");
                ViewBag.BloodGroups = Common.ToSelectList<BloodGroup>();
                ViewBag.Levels = Common.ToSelectList<EmployeeLevel>();
                return View();
            }
            catch (Exception ex)
            {
                TempData["Toastr"] = Toastr.DbError(ex.Message);
                return RedirectToAction("Index");
            }
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