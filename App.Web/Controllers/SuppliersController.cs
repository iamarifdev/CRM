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
    public class SuppliersController : Controller
    {
        #region Private Zone
        private readonly CrmDbContext _db;
        #endregion

        public SuppliersController()
        {
            _db = new CrmDbContext();
        }

        // GET: Suppliers
        public ActionResult Index()
        {
            return View();
        }

        //// GET: Suppliers/Details/5
        //public ActionResult Details(int? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    SuppliersInfo suppliersInfo = _db.SuppliersInfos.Find(id);
        //    if (suppliersInfo == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(suppliersInfo);
        //}

        // GET: Suppliers/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Suppliers/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,SupplierName,SupplierEmail,SupplierPhone,SupplierAddress,SupplierMobileNo")] SuppliersInfo supplier)
        {
            using (var dbTransaction = _db.Database.BeginTransaction())
            {
                try
                {
                    ModelState.Clear();
                    supplier.SupplierId = string.Format("SI-{0:000000}", _db.SuppliersInfos.Count() + 1);
                    supplier.EntryBy = _db.Users.First(x => x.UserName == User.Identity.Name).Id;
                    supplier.EntryDate = DateTime.Now;
                    TryValidateModel(supplier);
                    if (ModelState.IsValid)
                    {
                        _db.SuppliersInfos.Add(supplier);
                        _db.SaveChanges();

                        dbTransaction.Commit();
                        TempData["Toastr"] = Toastr.Added;

                        return RedirectToAction("Index");
                    }
                    dbTransaction.Rollback();
                    return View(supplier);
                }
                catch (Exception ex)
                {
                    dbTransaction.Rollback();
                    TempData["Toastr"] = Toastr.DbError(ex.Message);
                    return RedirectToAction("Index");
                }

            }

        }

        // GET: Suppliers/Edit/5
        public ActionResult Edit(int? id)
        {
            try
            {
                if (id == null)
                {
                    TempData["Toastr"] = Toastr.BadRequest;
                    return RedirectToAction("Index");
                }

                var supplier = _db.SuppliersInfos.Find(id);
                if (supplier != null) return View(supplier);

                TempData["Toastr"] = Toastr.HttpNotFound;
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                TempData["Toastr"] = Toastr.DbError(ex.Message);
                return RedirectToAction("Index");
            }
        }

        // POST: Suppliers/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,SupplierName,SupplierEmail,SupplierPhone,SupplierAddress,SupplierMobileNo")] SuppliersInfo supplier, int? id)
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
                    if (_db.SuppliersInfos.Count(x => x.Id == id) < 1)
                    {
                        TempData["Toastr"] = Toastr.HttpNotFound;
                        return RedirectToAction("Index");
                    }
                    var suppliersInfo = _db.SuppliersInfos.Single(x => x.Id == id);
                    if (suppliersInfo == null)
                    {
                        TempData["Toastr"] = Toastr.HttpNotFound;
                        return RedirectToAction("Index");
                    }

                    ModelState.Clear();
                    supplier.SupplierId = suppliersInfo.SupplierId;
                    supplier.EntryBy = suppliersInfo.EntryBy;
                    supplier.EntryDate = suppliersInfo.EntryDate;
                    supplier.DelStatus = suppliersInfo.DelStatus;

                    TryValidateModel(supplier);

                    if (!ModelState.IsValid) return View(supplier);

                    _db.SuppliersInfos
                        .Where(x => x.Id == id)
                        .Update(u => new SuppliersInfo
                        {
                            SupplierName = supplier.SupplierName,
                            SupplierEmail = supplier.SupplierEmail,
                            SupplierPhone = supplier.SupplierPhone,
                            SupplierAddress = supplier.SupplierAddress,
                            SupplierMobileNo = supplier.SupplierMobileNo
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

        //// GET: Suppliers/Delete/5
        //public ActionResult Delete(int? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    SuppliersInfo suppliersInfo = _db.SuppliersInfos.Find(id);
        //    if (suppliersInfo == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(suppliersInfo);
        //}

        // POST: Suppliers/Delete/5
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
                    var supplier = _db.SuppliersInfos.Find(id);
                    if (supplier == null)
                    {
                        TempData["Toastr"] = Toastr.HttpNotFound;
                        return RedirectToAction("Index");
                    }
                    _db.SuppliersInfos.Remove(supplier);
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
