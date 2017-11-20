using System;
using System.Linq;
using System.Web.Mvc;
using App.Entity.Models;
using App.Web.Context;
using App.Web.Helper;
using EntityFramework.Extensions;

namespace App.Web.Controllers
{
    public class CountryController : Controller
    {
        #region Private Zone
        private readonly CrmDbContext _db;
        #endregion

        public CountryController()
        {
            _db = new CrmDbContext();
        }
        // GET: Country
        public ActionResult Index()
        {
            return View();
        }

        // GET: Country/Details/5
        public ActionResult Details(int? id)
        {
            try
            {
                if (id == null)
                {
                    TempData["Toastr"] = Toastr.BadRequest;
                    return RedirectToAction("Index");
                }
                var country = _db.Countries.Find(id);
                if (country != null) return View(country);

                TempData["Toastr"] = Toastr.HttpNotFound;
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                TempData["Toastr"] = Toastr.DbError(ex.Message);
                return RedirectToAction("Index");
            }

        }

        // GET: Country/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Country/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "CountryName,CountryCode,DelStatus")] Country country)
        {
            using (var dbTransaction = _db.Database.BeginTransaction())
            {
                try
                {
                    ModelState.Clear();
                    country.EntryBy = _db.Users.First(x => x.UserName == User.Identity.Name).Id;
                    country.EntryDate = DateTime.Now;
                    TryValidateModel(country);
                    if (ModelState.IsValid)
                    {
                        _db.Countries.Add(country);
                        _db.SaveChanges();

                        dbTransaction.Commit();
                        TempData["Toastr"] = Toastr.Added;

                        return RedirectToAction("Index");
                    }
                    dbTransaction.Rollback();
                    return View(country);
                }
                catch (Exception ex)
                {
                    dbTransaction.Rollback();
                    TempData["Toastr"] = Toastr.DbError(ex.Message);
                    return RedirectToAction("Index");
                }
                finally
                {
                    ViewBag.Status = Common.ToSelectList<Status>();
                }

            }
        }

        // GET: Country/Edit/5
        public ActionResult Edit(int? countryId)
        {
            try
            {
                if (countryId == null)
                {
                    TempData["Toastr"] = Toastr.BadRequest;
                    return RedirectToAction("Index");
                }
                var country = _db.Countries.Find(countryId);
                if (country != null) return View(country);

                TempData["Toastr"] = Toastr.HttpNotFound;
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                TempData["Toastr"] = Toastr.DbError(ex.Message);
                return RedirectToAction("Index");
            }
        }

        // POST: Country/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "CountryId,CountryName,CountryCode,DelStatus")] Country country, int? countryId)
        {
            using (var dbTransaction = _db.Database.BeginTransaction())
            {
                try
                {
                    if (countryId == null)
                    {
                        TempData["Toastr"] = Toastr.HttpNotFound;
                        return RedirectToAction("Index");
                    }
                    if (!_db.Countries.Any(x=>x.CountryId == countryId))
                    {
                        TempData["Toastr"] = Toastr.HttpNotFound;
                        return RedirectToAction("Index");
                    }
                    var data = _db.Countries.Single(x => x.CountryId == countryId);

                    ModelState.Clear();

                    country.DelStatus = data.DelStatus;
                    country.EntryBy = data.EntryBy;
                    country.EntryDate = data.EntryDate;

                    TryValidateModel(country);
                    if (!ModelState.IsValid) return View(country);

                    _db.Countries
                        .Where(x => x.CountryId == countryId)
                        .Update( u => new Country {
                            CountryName = country.CountryName,
                            CountryCode = country.CountryCode
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

        //// GET: Country/Delete/5
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

        // POST: Country/Delete/5
        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int? countryId)
        {
            using (var dbTransaction = _db.Database.BeginTransaction())
            {
                try
                {
                    if (countryId == null)
                    {
                        TempData["Toastr"] = Toastr.BadRequest;
                        return RedirectToAction("Index");
                    }
                    if (!_db.Countries.Any(x=>x.CountryId == countryId))
                    {
                        TempData["Toastr"] = Toastr.HttpNotFound;
                        return RedirectToAction("Index");
                    }

                    _db.Countries
                        .Where(x => x.CountryId == countryId)
                        .Update(u => new Country
                        {
                            DelStatus = true
                        });
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