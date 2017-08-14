using System;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using App.Entity.Models;
using App.Web.Context;
using App.Web.Helper;
using EntityFramework.Extensions;

namespace App.Web.Controllers
{
    [Authorize(Roles = "Admin")]
    public class DesignationsController : Controller
    {
        #region Private Zone

        private readonly CrmDbContext _db;

        #endregion

        public DesignationsController()
        {
            _db = new CrmDbContext();
        }

        // GET: Designations
        public ActionResult Index()
        {
            return View();
        }

        // GET: Designations/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var employeeDesignation = _db.EmployeeDesignations.Find(id);
            if (employeeDesignation == null)
            {
                return HttpNotFound();
            }
            return View(employeeDesignation);
        }

        // GET: Designations/Create
        public ActionResult Create()
        {
            ViewBag.StatusList = new SelectList(Common.StatusList, "Value", "Text");
            return View();
        }

        // POST: Designations/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(
            [Bind(Include = "Id,DesignationTitleEn,DesignationTitleBn,DesignationDepertment,Status")] EmployeeDesignation designation)
        {
            using (var dbTransaction = _db.Database.BeginTransaction())
            {
                try
                {
                    ModelState.Clear();
                    designation.DesignationId = string.Format("ED-{0:000000}", _db.EmployeeDesignations.Count() + 1);
                    designation.DelStatus = false;
                    designation.EntryDate = DateTime.Now;
                    designation.EntryBy = _db.Users.First(x => x.Username == User.Identity.Name).Id;
                    TryValidateModel(designation);

                    if (ModelState.IsValid)
                    {
                        _db.EmployeeDesignations.Add(designation);
                        _db.SaveChanges();
                        dbTransaction.Commit();
                        TempData["Toastr"] = Toastr.Added;
                        return RedirectToAction("Index");
                    }

                    dbTransaction.Rollback();
                    return View(designation);
                }
                catch (Exception ex)
                {
                    dbTransaction.Rollback();
                    TempData["Toastr"] = Toastr.DbError(ex.Message);
                    return RedirectToAction("Index");
                }
                finally
                {
                    ViewBag.StatusList = new SelectList(Common.StatusList, "Value", "Text");
                }
            }

        }

        // GET: Designations/Edit/5
        public ActionResult Edit(int? id)
        {
            try
            {
                if (id == null)
                {
                    TempData["Toastr"] = Toastr.BadRequest;
                    return RedirectToAction("Index");
                }
                var designation = _db.EmployeeDesignations.Find(id);
                if (designation == null)
                {
                    TempData["Toastr"] = Toastr.HttpNotFound;
                    return RedirectToAction("Index");
                }
                ViewBag.StatusList = new SelectList(Common.StatusList, "Value", "Text", designation.Status);
                return View(designation);
            }
            catch (Exception ex)
            {
                TempData["Toastr"] = Toastr.DbError(ex.Message);
                return RedirectToAction("Index");
            }
        }

        // POST: Designations/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,DesignationTitleEn,DesignationTitleBn,DesignationDepertment,Status")] EmployeeDesignation designation, int? id)
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
                    if (_db.EmployeeDesignations.Count(x => x.Id == id) < 1)
                    {
                        TempData["Toastr"] = Toastr.HttpNotFound;
                        return RedirectToAction("Index");
                    }
                    var empDesignation = _db.EmployeeDesignations.Single(x => x.Id == id);
                    if (empDesignation == null)
                    {
                        TempData["Toastr"] = Toastr.HttpNotFound;
                        return RedirectToAction("Index");
                    }

                    ModelState.Clear();
                    designation.DesignationId = empDesignation.DesignationId;
                    designation.EntryBy = empDesignation.EntryBy;
                    designation.EntryDate = empDesignation.EntryDate;
                    designation.DelStatus = empDesignation.DelStatus;
                    TryValidateModel(designation);

                    if (!ModelState.IsValid) return View(designation);

                    _db.EmployeeDesignations
                        .Where(x => x.Id == id)
                        .Update( u => new EmployeeDesignation {
                            DesignationTitleEn = designation.DesignationTitleEn,
                            DesignationTitleBn = designation.DesignationTitleBn,
                            Status = designation.Status
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
                finally
                {
                    ViewBag.StatusList = new SelectList(Common.StatusList, "Value", "Text", designation.Status);
                }
            }
        }

        // GET: Designations/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            EmployeeDesignation employeeDesignation = _db.EmployeeDesignations.Find(id);
            if (employeeDesignation == null)
            {
                return HttpNotFound();
            }
            return View(employeeDesignation);
        }

        // POST: Designations/Delete/5
        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int? id)
        {
            using (var dbTransaction = _db.Database.BeginTransaction())
            {
                try
                {
                    if (id == null)
                    {
                        TempData["Toastr"] = Toastr.BadRequest;
                        return RedirectToAction("Index");
                    }
                    var designation = _db.EmployeeDesignations.Find(id);
                    if (designation == null)
                    {
                        TempData["Toastr"] = Toastr.HttpNotFound;
                        return RedirectToAction("Index");
                    }
                    _db.EmployeeDesignations.Remove(designation);
                    _db.SaveChanges();
                    dbTransaction.Commit();
                    TempData["Toastr"] = Toastr.Deleted;
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
