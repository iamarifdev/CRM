using System;
using System.Linq;
using System.Web.Mvc;
using App.Entity.Models;
using App.Web.Context;
using App.Web.Helper;
using EntityFramework.Extensions;

namespace App.Web.Controllers
{
    [Authorize(Roles = "Admin")]
    public class BranchController : Controller
    {
        #region Private Zone
        private readonly CrmDbContext _db;
        #endregion
        
        public BranchController()
        {
            _db = new CrmDbContext();
        }
        // GET: Branch
        public ActionResult Index()
        {
            return View();
        }

        //// GET: Branch/Details/5
        //public ActionResult Details(int? id)
        //{
        //    try
        //    {
        //        if (id == null)
        //        {
        //            return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //        }
        //        var branchInfo = _db.BranchInfos.Find(id);
        //        if (branchInfo != null) return View(branchInfo);

        //        TempData["Toastr"] = Toastr.HttpNotFound;
        //        return RedirectToAction("Index");
        //    }
        //    catch (Exception ex)
        //    {
        //        TempData["Toastr"] = Toastr.DbError(ex.Message);
        //        return RedirectToAction("Index");
        //    }

        //}

        // GET: Branch/Create
        public ActionResult Create()
        {
            ViewBag.StatusList = new SelectList(Common.StatusList, "Value", "Text");
            return View();
        }

        // POST: Branch/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "BranchName,BranchCode,Status")] BranchInfo branchInfo)
        {
            using (var dbTransaction = _db.Database.BeginTransaction())
            {
                try
                {
                    ModelState.Clear();
                    branchInfo.BranchId = string.Format("BI-{0:000000}", _db.BranchInfos.Count() + 1);
                    branchInfo.EntryBy = _db.Users.First(x => x.UserName == User.Identity.Name).Id;
                    branchInfo.EntryDate = DateTime.Now;
                    TryValidateModel(branchInfo);
                    if (ModelState.IsValid)
                    {
                        _db.BranchInfos.Add(branchInfo);
                        _db.SaveChanges();

                        dbTransaction.Commit();
                        TempData["Toastr"] = Toastr.Added;

                        return RedirectToAction("Index");
                    }
                    dbTransaction.Rollback();
                    return View(branchInfo);
                }
                catch (Exception ex)
                {
                    dbTransaction.Rollback();
                    TempData["Toastr"] = Toastr.DbError(ex.Message);
                    return RedirectToAction("Index");
                }
                finally
                {
                    ViewBag.Status = new SelectList(Common.StatusList, "Value", "Text");
                }

            }
        }

        // GET: Branch/Edit/5
        public ActionResult Edit(int? id)
        {
            try
            {
                if (id == null)
                {
                    TempData["Toastr"] = Toastr.BadRequest;
                    return RedirectToAction("Index");
                }
                var branchInfo = _db.BranchInfos.Find(id);
                if (branchInfo == null)
                {
                    TempData["Toastr"] = Toastr.HttpNotFound;
                    return RedirectToAction("Index");
                }
                ViewBag.StatusList = new SelectList(Common.StatusList, "Value", "Text", branchInfo.Status);

                return View(branchInfo);
            }
            catch (Exception ex)
            {
                TempData["Toastr"] = Toastr.DbError(ex.Message);
                return RedirectToAction("Index");
            }
        }

        // POST: Branch/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,BranchName,BranchCode,Status")] BranchInfo branchInfo, int? id)
        {
            using (var dbTransaction = _db.Database.BeginTransaction())
            {
                try
                {
                    if (id == null) return HttpNotFound();
                    if (_db.BranchInfos.Count(x => x.Id == id) < 1) return HttpNotFound();
                    var branch = _db.BranchInfos.Single(x => x.Id == id);

                    ModelState.Clear();

                    branchInfo.BranchId = branch.BranchId;
                    branchInfo.DelStatus = branch.DelStatus;
                    branchInfo.EntryBy = branch.EntryBy;
                    branchInfo.EntryDate = branch.EntryDate;

                    TryValidateModel(branchInfo);
                    if (!ModelState.IsValid) return View(branchInfo);

                    _db.BranchInfos
                        .Where(x => x.Id == id)
                        .Update( u => new BranchInfo {
                            BranchName = branchInfo.BranchName,
                            BranchCode = branchInfo.BranchCode,
                            Status = branchInfo.Status
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
                    ViewBag.StatusList = new SelectList(Common.StatusList, "Value", "Text", branchInfo.Status);
                }
            }
        }

        //// GET: Branch/Delete/5
        //public ActionResult Delete(int? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    BranchInfo branchInfo = _db.BranchInfos.Find(id);
        //    if (branchInfo == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(branchInfo);
        //}

        // POST: Branch/Delete/5
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
                    var branchInfo = _db.BranchInfos.Find(id);
                    if (branchInfo == null)
                    {
                        TempData["Toastr"] = Toastr.HttpNotFound;
                        return RedirectToAction("Index");
                    }
                    _db.BranchInfos.Remove(branchInfo);
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
