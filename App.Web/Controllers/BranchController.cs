using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using App.Entity.Models;
using App.Web.Context;
using EntityFramework.Extensions;

namespace App.Web.Controllers
{
    [Authorize]
    public class BranchController : Controller
    {
        #region Private Zone
        private readonly CrmDbContext _db;
        private readonly List<SelectListItem> _aiStatus = Enum.GetValues(typeof(Status)).Cast<Status>().Select(v => new
                SelectListItem { Text = v.ToString(), Value = ((int)v).ToString()}).ToList();
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

        // GET: Branch/Details/5
        public ActionResult Details(int? id)
        {
            try
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                var branchInfo = _db.BranchInfos.Find(id);
                if (branchInfo != null) return View(branchInfo);

                TempData["Toastr"] = "toastr.error('Information not found!', ' Error!');";
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                TempData["Toastr"] = string.Format("toastr.error('{0}', ' Database Error!');", ex.Message);
                return RedirectToAction("Index");
            }

        }

        // GET: Branch/Create
        public ActionResult Create()
        {
            ViewBag.Status = new SelectList(Enum.GetValues(typeof(Status)).Cast<Status>().Select(v => new
                SelectListItem { Text = v.ToString(), Value = ((int)v).ToString() }).ToList(), "Value", "Text");
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
                    branchInfo.EntryBy = _db.Users.First(x => x.Username == User.Identity.Name).Id;
                    branchInfo.EntryDate = DateTime.Now;
                    TryValidateModel(branchInfo);
                    if (ModelState.IsValid)
                    {
                        _db.BranchInfos.Add(branchInfo);
                        _db.SaveChanges();

                        dbTransaction.Commit();

                        return RedirectToAction("Index");
                    }
                    dbTransaction.Rollback();
                    return View(branchInfo);
                }
                catch (Exception ex)
                {
                    dbTransaction.Rollback();
                    throw ex;
                }
                finally
                {
                    ViewBag.Status = new SelectList(Enum.GetValues(typeof(Status)).Cast<Status>().Select(v => new
                        SelectListItem { Text = v.ToString(), Value = ((int)v).ToString() }).ToList(), "Value", "Text");
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
                    TempData["Toastr"] = "toastr.error('Bad request!', 'Error!');";
                    return RedirectToAction("Index");
                }
                var branchInfo = _db.BranchInfos.Find(id);
                if (branchInfo == null)
                {
                    TempData["Toastr"] = "toastr.error('Information not found!', 'Error!');";
                    return RedirectToAction("Index");
                }
                ViewBag.Statuses = new SelectList(_aiStatus, "Value", "Text", branchInfo.Status);

                return View(branchInfo);
            }
            catch (Exception ex)
            {
                TempData["Toastr"] = string.Format("toastr.error('{0}', ' Database Error!');", ex.Message);
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
                       .Where(x => x.Id==id)
                       .Update(u => new BranchInfo { BranchName = branchInfo.BranchName, BranchCode = branchInfo.BranchCode, Status = branchInfo.Status});
                    dbTransaction.Commit();

                    TempData["Toastr"] = "toastr.success('Information successfully updated!', ' Success!');";
                    return RedirectToAction("Index");
                }
                catch (Exception ex)
                {
                    dbTransaction.Rollback();
                    TempData["Toastr"] = string.Format("toastr.error('{0}', ' Database Error!');", ex.Message);
                    return RedirectToAction("Index");
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
                        TempData["Toastr"] = "toastr.error('Bad request!', 'Error!');";
                        return RedirectToAction("Index");
                    }
                    var branchInfo = _db.BranchInfos.Find(id);
                    if (branchInfo == null)
                    {
                        TempData["Toastr"] = "toastr.error('Information not found!', 'Error!');";
                        return RedirectToAction("Index");
                    }
                    _db.BranchInfos.Remove(branchInfo);
                    _db.SaveChanges();
                    dbTransaction.Commit();
                    TempData["Toastr"] = "toastr.success('Information deleted successfully!', 'Success!');";
                    return RedirectToAction("Index");
                }
                catch (Exception ex)
                {
                    dbTransaction.Rollback();
                    TempData["Toastr"] = string.Format("toastr.error('{0}', ' Database Error!');", ex.Message);
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
