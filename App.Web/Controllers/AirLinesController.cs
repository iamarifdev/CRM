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
    public class AirLinesController : Controller
    {
        #region Private Zone

        private readonly CrmDbContext _db;

        #endregion

        public AirLinesController()
        {
            _db = new CrmDbContext();
        }

        // GET: AirLines
        public ActionResult Index()
        {
            return View();
        }

        // GET: AirLines/Details/5
        public ActionResult Details(int? id)
        {
            try
            {
                if (id == null)
                {
                    TempData["Toastr"] = Toastr.BadRequest;
                    return RedirectToAction("Index");
                }
                var airLineInfo = _db.AirLineInfos.Find(id);
                if (airLineInfo != null) return View(airLineInfo);
                TempData["Toastr"] = Toastr.HttpNotFound;
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                TempData["Toastr"] = Toastr.DbError(ex.Message);
                return RedirectToAction("Index");
            }
        }

        // GET: AirLines/Create
        public ActionResult Create()
        {
            ViewBag.StatusList = Common.ToSelectList<Status>();
            return View();
        }

        // POST: AirLines/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,AirLineName,Description,Status")] AirLineInfo airLine)
        {
            using (var dbTransaction = _db.Database.BeginTransaction())
            {
                try
                {
                    ModelState.Clear();
                    airLine.AirLineId = string.Format("BI-{0:000000}", _db.AirLineInfos.Count() + 1);
                    airLine.EntryBy = _db.Users.First(x => x.UserName == User.Identity.Name).Id;
                    airLine.EntryDate = DateTime.Now;
                    TryValidateModel(airLine);
                    if (ModelState.IsValid)
                    {
                        _db.AirLineInfos.Add(airLine);
                        _db.SaveChanges();

                        dbTransaction.Commit();
                        TempData["Toastr"] = Toastr.Added;

                        return RedirectToAction("Index");
                    }
                    dbTransaction.Rollback();
                    return View(airLine);
                }
                catch (Exception ex)
                {
                    dbTransaction.Rollback();
                    TempData["Toastr"] = Toastr.DbError(ex.Message);
                    return RedirectToAction("Index");
                }
                finally
                {
                    ViewBag.StatusList = Common.ToSelectList<Status>();
                }

            }

        }

        // GET: AirLines/Edit/5
        public ActionResult Edit(int? id)
        {
            try
            {
                if (id == null)
                {
                    TempData["Toastr"] = Toastr.BadRequest;
                    return RedirectToAction("Index");
                }
                var airLine = _db.AirLineInfos.Find(id);
                if (airLine == null)
                {
                    TempData["Toastr"] = Toastr.HttpNotFound;
                    return RedirectToAction("Index");
                }
                ViewBag.StatusList = Common.ToSelectList<Status>(airLine.Status);

                return View(airLine);
            }
            catch (Exception ex)
            {
                TempData["Toastr"] = Toastr.DbError(ex.Message);
                return RedirectToAction("Index");
            }
        }

        // POST: AirLines/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,AirLineName,Description,Status")] AirLineInfo airLine, int? id)
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
                    if (_db.AirLineInfos.Count(x => x.Id == id) < 1)
                    {
                        TempData["Toastr"] = Toastr.HttpNotFound;
                        return RedirectToAction("Index");
                    }
                    var airLineInfo = _db.AirLineInfos.Single(x => x.Id == id);
                    if (airLineInfo == null)
                    {
                        TempData["Toastr"] = Toastr.HttpNotFound;
                        return RedirectToAction("Index");
                    }

                    ModelState.Clear();
                    airLine.AirLineId = airLineInfo.AirLineId;
                    airLine.EntryBy = airLineInfo.EntryBy;
                    airLine.EntryDate = airLineInfo.EntryDate;
                    airLine.DelStatus = airLineInfo.DelStatus;

                    TryValidateModel(airLine);

                    if (!ModelState.IsValid) return View(airLine);

                    _db.AirLineInfos
                        .Where(x => x.Id == id)
                        .Update(u => new AirLineInfo
                        {
                            AirLineName = airLine.AirLineName,
                            Description = airLine.Description,
                            Status = airLine.Status
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
                    ViewBag.StatusList = Common.ToSelectList<Status>(airLine.Status);
                }
            }
        }

        //// GET: AirLines/Delete/5
        //public ActionResult Delete(int? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    AirLineInfo airLineInfo = _db.AirLineInfos.Find(id);
        //    if (airLineInfo == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(airLineInfo);
        //}

        // POST: AirLines/Delete/5
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
                    var airLine = _db.AirLineInfos.Find(id);
                    if (airLine == null)
                    {
                        TempData["Toastr"] = Toastr.HttpNotFound;
                        return RedirectToAction("Index");
                    }
                    _db.AirLineInfos.Remove(airLine);
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
