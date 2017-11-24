using System;
using System.Linq;
using System.Web.Mvc;
using App.Entity.Models;
using App.Web.Context;
using App.Web.Helper;
using EntityFramework.Extensions;

namespace App.Web.Controllers
{

    [CrmAuthorize(Roles = "Admin")]
    [CrmPermission]
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

        // GET: Groups/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Groups/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Group group)
        {
            using (var dbTransaction = _db.Database.BeginTransaction())
            {
                try
                {
                    if (!ModelState.IsValid) return View(group);
                    if (_db.Groups.Any(x => x.Name == group.Name))
                    {
                        ModelState.AddModelError("Name", @"Group Name already exist, try another.");
                        return View(group);
                    }
                    _db.Groups.Add(group);
                    _db.SaveChanges();
                    dbTransaction.Commit();

                    TempData["Toastr"] = Toastr.Added;
                    return RedirectToAction("Index");
                }
                catch (Exception ex)
                {
                    dbTransaction.Rollback();
                    TempData["Toastr"] = Toastr.DbError(ex.Message);
                    return RedirectToAction("Index");
                }
            }
        }

        // GET: Groups/Edit/5
        public ActionResult Edit(int? id)
        {
            try
            {
                if (id == null)
                {
                    TempData["Toastr"] = Toastr.BadRequest;
                    return RedirectToAction("Index");
                }
                var group = _db.Groups.Find(id);
                ViewBag.IsGroupIsAdmin = _db.Groups.Any(x => x.Id == id && x.Name.ToLower() == "admin");
                if (group != null) return View(group);

                TempData["Toastr"] = Toastr.HttpNotFound;
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                TempData["Toastr"] = Toastr.DbError(ex.Message);
                return RedirectToAction("Index");
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Group model, int? id)
        {
            using (var dbTransaction = _db.Database.BeginTransaction())
            {
                try
                {
                    if (id == null)
                    {
                        TempData["Toastr"] = Toastr.HttpNotFound;
                        return RedirectToAction("Index");
                    }
                    if (!ModelState.IsValid)
                    {
                        dbTransaction.Dispose();
                        ViewBag.IsGroupIsAdmin = _db.Groups.Any(x => x.Id == id && x.Name.ToLower() == "admin");
                        return View(model);
                    }
                    if (!_db.Groups.Any(x => x.Id == id))
                    {
                        TempData["Toastr"] = Toastr.HttpNotFound;
                        return RedirectToAction("Index");
                    }
                    var groupName = _db.Groups.Where(x => x.Id == id).Select(x => x.Name).First();
                    _db.Groups
                        .Where(x => x.Id == id)
                        .Update(u => new Group
                        {
                            Name = groupName.ToLower() == "admin" ? "Admin" : model.Name,
                            Description = model.Description,
                            Account = model.Account,
                            Billing = model.Billing,
                            Crm = model.Crm,
                            Hrm = groupName.ToLower() == "admin",
                            Report = model.Report,
                            Setup = model.Setup
                        });
                    dbTransaction.Commit();

                    TempData["Toastr"] = Toastr.Updated;
                    return RedirectToAction("Index");
                }
                catch (Exception ex)
                {
                    dbTransaction.Rollback();
                    TempData["Toastr"] = Toastr.DbError(ex.Message);
                    return RedirectToAction("Index");
                }
            }
        }

        [HttpPost]
        public JsonResult IsGroupAvailable(string name, int? id)
        {
            try
            {
                var flag = true;
                //create mode
                if (id == null)
                {
                    flag = !_db.Groups.Any(x => x.Name == name);
                }
                // edit mode
                else
                {
                    var group = _db.Groups.Find(id);
                    if (group == null) return Json(false, JsonRequestBehavior.AllowGet);
                    if (!string.Equals(group.Name, name, StringComparison.CurrentCultureIgnoreCase)) flag = !_db.Groups.Any(x => x.Name.ToLower() == name.ToLower());
                }
                return Json(flag, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return Json(false, JsonRequestBehavior.AllowGet);
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