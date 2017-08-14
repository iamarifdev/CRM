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
    public class PaymentMethodsController : Controller
    {
        #region Private Zone
        private readonly CrmDbContext _db;
        #endregion

        public PaymentMethodsController()
        {
            _db = new CrmDbContext();
        }

        // GET: PaymentMethods
        public ActionResult Index()
        {
            return View();
        }

        //// GET: PaymentMethods/Details/5
        //public ActionResult Details(int? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    PaymentMethod paymentMethod = _db.PaymentMethods.Find(id);
        //    if (paymentMethod == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(paymentMethod);
        //}

        // GET: PaymentMethods/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: PaymentMethods/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,MethodName")] PaymentMethod paymentMethod)
        {
            using (var dbTransaction = _db.Database.BeginTransaction())
            {
                try
                {
                    ModelState.Clear();
                    paymentMethod.MethodId = string.Format("PI-{0:000000}", _db.PaymentMethods.Count() + 1);
                    paymentMethod.EntryBy = _db.Users.First(x => x.Username == User.Identity.Name).Id;
                    paymentMethod.EntryDate = DateTime.Now;
                    TryValidateModel(paymentMethod);
                    if (ModelState.IsValid)
                    {
                        _db.PaymentMethods.Add(paymentMethod);
                        _db.SaveChanges();

                        dbTransaction.Commit();
                        TempData["Toastr"] = Toastr.Added;

                        return RedirectToAction("Index");
                    }
                    dbTransaction.Rollback();
                    return View(paymentMethod);
                }
                catch (Exception ex)
                {
                    dbTransaction.Rollback();
                    TempData["Toastr"] = Toastr.DbError(ex.Message);
                    return RedirectToAction("Index");
                }
            }
        }

        // GET: PaymentMethods/Edit/5
        public ActionResult Edit(int? id)
        {
            try
            {
                if (id == null)
                {
                    TempData["Toastr"] = Toastr.BadRequest;
                    return RedirectToAction("Index");
                }
                var paymentMethod = _db.PaymentMethods.Find(id);
                if (paymentMethod != null) return View(paymentMethod);

                TempData["Toastr"] = Toastr.HttpNotFound;
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                TempData["Toastr"] = Toastr.DbError(ex.Message);
                return RedirectToAction("Index");
            }
        }

        // POST: PaymentMethods/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,MethodName")] PaymentMethod paymentMethod, int? id)
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
                    if (_db.PaymentMethods.Count(x => x.Id == id) < 1)
                    {
                        TempData["Toastr"] = Toastr.HttpNotFound;
                        return RedirectToAction("Index");
                    }
                    var method = _db.PaymentMethods.Single(x => x.Id == id);
                    if (method == null)
                    {
                        TempData["Toastr"] = Toastr.HttpNotFound;
                        return RedirectToAction("Index");
                    }

                    ModelState.Clear();
                    paymentMethod.MethodId = method.MethodId;
                    paymentMethod.CurrentValue = method.CurrentValue;
                    paymentMethod.EntryBy = method.EntryBy;
                    paymentMethod.EntryDate = method.EntryDate;
                    paymentMethod.DelStatus = method.DelStatus;

                    TryValidateModel(paymentMethod);

                    if (!ModelState.IsValid) return View(paymentMethod);

                    _db.PaymentMethods
                        .Where(x => x.Id == id)
                        .Update(u => new PaymentMethod { MethodName = paymentMethod.MethodName });
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

        //// GET: PaymentMethods/Delete/5
        //public ActionResult Delete(int? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    PaymentMethod paymentMethod = _db.PaymentMethods.Find(id);
        //    if (paymentMethod == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(paymentMethod);
        //}

        // POST: PaymentMethods/Delete/5
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
                    var method = _db.PaymentMethods.Find(id);
                    if (method == null)
                    {
                        TempData["Toastr"] = Toastr.HttpNotFound;
                        return RedirectToAction("Index");
                    }
                    _db.PaymentMethods.Remove(method);
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
