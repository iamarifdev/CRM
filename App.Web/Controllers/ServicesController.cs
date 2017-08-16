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
    public class ServicesController : Controller
    {

        #region Private Zone
        private readonly CrmDbContext _db;
        #endregion

        public ServicesController()
        {
            _db = new CrmDbContext();
        }

        // GET: Services
        public ActionResult Index()
        {
            return View();
        }

        //// GET: Services/Details/5
        //public ActionResult Details(int? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    ServiceInfo serviceInfo = _db.ServiceInfos.Find(id);
        //    if (serviceInfo == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(serviceInfo);
        //}

        // GET: Services/Create
        public ActionResult Create()
        {
            ViewBag.Status = new SelectList(Common.StatusList, "Value", "Text");
            return View();
        }

        // POST: Services/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,ServiceName,Description,Status")] ServiceInfo serviceInfo)
        {
            using (var dbTransaction = _db.Database.BeginTransaction())
            {
                try
                {
                    ModelState.Clear();
                    serviceInfo.ServiceId = string.Format("DI-{0:000000}", _db.ServiceInfos.Count() + 1);
                    serviceInfo.EntryBy = _db.Users.First(x => x.UserName == User.Identity.Name).Id;
                    serviceInfo.EntryDate = DateTime.Now;
                    TryValidateModel(serviceInfo);
                    if (ModelState.IsValid)
                    {
                        _db.ServiceInfos.Add(serviceInfo);
                        _db.SaveChanges();

                        dbTransaction.Commit();
                        TempData["Toastr"] = Toastr.Added;

                        return RedirectToAction("Index");
                    }
                    dbTransaction.Rollback();
                    return View(serviceInfo);
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

        // GET: Services/Edit/5
        public ActionResult Edit(int? id)
        {
            try
            {
                if (id == null)
                {
                    TempData["Toastr"] = Toastr.BadRequest;
                    return RedirectToAction("Index");
                }
                var service = _db.ServiceInfos.Find(id);
                if (service == null)
                {
                    TempData["Toastr"] = Toastr.HttpNotFound;
                    return RedirectToAction("Index");
                }
                ViewBag.StatusList = new SelectList(Common.StatusList, "Value", "Text", service.Status);

                return View(service);
            }
            catch (Exception ex)
            {
                TempData["Toastr"] = Toastr.DbError(ex.Message);
                return RedirectToAction("Index");
            }
        }

        // POST: Services/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,ServiceName,Description,Status")] ServiceInfo service, int? id)
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
                    if (_db.ServiceInfos.Count(x => x.Id == id) < 1)
                    {
                        TempData["Toastr"] = Toastr.HttpNotFound;
                        return RedirectToAction("Index");
                    }
                    var serviceInfo = _db.ServiceInfos.Single(x => x.Id == id);
                    if (serviceInfo == null)
                    {
                        TempData["Toastr"] = Toastr.HttpNotFound;
                        return RedirectToAction("Index");
                    }

                    ModelState.Clear();
                    service.ServiceId = serviceInfo.ServiceId;
                    service.EntryBy = serviceInfo.EntryBy;
                    service.EntryDate = serviceInfo.EntryDate;
                    service.DelStatus = serviceInfo.DelStatus;

                    TryValidateModel(service);

                    if (!ModelState.IsValid) return View(service);

                    _db.ServiceInfos
                        .Where(x => x.Id == id)
                        .Update(u => new ServiceInfo
                        {
                            ServiceName = service.ServiceName,
                            Description = service.Description,
                            Status = service.Status
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
                    ViewBag.StatusList = new SelectList(Common.StatusList, "Value", "Text", service.Status);
                }
            }
        }

        //// GET: Services/Delete/5
        //public ActionResult Delete(int? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    ServiceInfo serviceInfo = _db.ServiceInfos.Find(id);
        //    if (serviceInfo == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(serviceInfo);
        //}

        // POST: Services/Delete/5
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
                    var service = _db.ServiceInfos.Find(id);
                    if (service == null)
                    {
                        TempData["Toastr"] = Toastr.HttpNotFound;
                        return RedirectToAction("Index");
                    }
                    _db.ServiceInfos.Remove(service);
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
