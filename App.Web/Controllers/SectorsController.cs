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
    public class SectorsController : Controller
    {
        #region Private Zone
        private readonly CrmDbContext _db;
        #endregion

        public SectorsController()
        {
            _db = new CrmDbContext();
        }

        // GET: Sectors
        public ActionResult Index()
        {
            return View();
        }

        // GET: Sectors/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SectorInfo sectorInfo = _db.SectorInfos.Find(id);
            if (sectorInfo == null)
            {
                return HttpNotFound();
            }
            return View(sectorInfo);
        }

        // GET: Sectors/Create
        public ActionResult Create()
        {
            ViewBag.Status = new SelectList(Common.StatusList, "Value", "Text");
            return View();
        }

        // POST: Sectors/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,SectorName,SectorCode,Status")] SectorInfo sector)
        {

            using (var dbTransaction = _db.Database.BeginTransaction())
            {
                try
                {
                    ModelState.Clear();
                    sector.SectorId = string.Format("BI-{0:000000}", _db.SectorInfos.Count() + 1);
                    sector.EntryBy = _db.Users.First(x => x.Username == User.Identity.Name).Id;
                    sector.EntryDate = DateTime.Now;
                    TryValidateModel(sector);
                    if (ModelState.IsValid)
                    {
                        _db.SectorInfos.Add(sector);
                        _db.SaveChanges();

                        dbTransaction.Commit();
                        TempData["Toastr"] = Toastr.Added;

                        return RedirectToAction("Index");
                    }
                    dbTransaction.Rollback();
                    return View(sector);
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

        // GET: Sectors/Edit/5
        public ActionResult Edit(int? id)
        {
            try
            {
                if (id == null)
                {
                    TempData["Toastr"] = Toastr.BadRequest;
                    return RedirectToAction("Index");
                }
                var sector = _db.SectorInfos.Find(id);
                if (sector == null)
                {
                    TempData["Toastr"] = Toastr.HttpNotFound;
                    return RedirectToAction("Index");
                }
                ViewBag.StatusList = new SelectList(Common.StatusList, "Value", "Text", sector.Status);

                return View(sector);
            }
            catch (Exception ex)
            {
                TempData["Toastr"] = Toastr.DbError(ex.Message);
                return RedirectToAction("Index");
            }
        }

        // POST: Sectors/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,SectorName,SectorCode,Status")] SectorInfo sector, int? id)
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
                    if (_db.SectorInfos.Count(x => x.Id == id) < 1)
                    {
                        TempData["Toastr"] = Toastr.HttpNotFound;
                        return RedirectToAction("Index");
                    }
                    var sectorInfo = _db.SectorInfos.Single(x => x.Id == id);
                    if (sectorInfo == null)
                    {
                        TempData["Toastr"] = Toastr.HttpNotFound;
                        return RedirectToAction("Index");
                    }

                    ModelState.Clear();
                    sector.SectorId = sectorInfo.SectorId;
                    sector.EntryBy = sectorInfo.EntryBy;
                    sector.EntryDate = sectorInfo.EntryDate;
                    sector.DelStatus = sectorInfo.DelStatus;

                    TryValidateModel(sector);

                    if (!ModelState.IsValid) return View(sector);

                    _db.SectorInfos
                        .Where(x => x.Id == id)
                        .Update(u => new SectorInfo
                        {
                            SectorName = sector.SectorName,
                            SectorCode = sector.SectorCode,
                            Status = sector.Status
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
                    ViewBag.StatusList = new SelectList(Common.StatusList, "Value", "Text", sector.Status);
                }
            }
        }

        //// GET: Sectors/Delete/5
        //public ActionResult Delete(int? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    SectorInfo sectorInfo = _db.SectorInfos.Find(id);
        //    if (sectorInfo == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(sectorInfo);
        //}

        // POST: Sectors/Delete/5
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
                    var sector = _db.SectorInfos.Find(id);
                    if (sector == null)
                    {
                        TempData["Toastr"] = Toastr.HttpNotFound;
                        return RedirectToAction("Index");
                    }
                    _db.SectorInfos.Remove(sector);
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
