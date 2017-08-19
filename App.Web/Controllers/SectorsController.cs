using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Transactions;
using System.Web;
using System.Web.Mvc;
using App.Entity.Models;
using App.Web.Context;
using App.Web.Helper;
using EntityFramework.Extensions;
using ExcelDataReader;

namespace App.Web.Controllers
{
    [Authorize(Roles = "Admin")]
    public class SectorsController : Controller
    {
        #region Private Zone

        private readonly CrmDbContext _db;
        private readonly List<string> _allowedUploadFile = new List<string> { ".xls", ".csv", ".xlsx" };

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

        //// GET: Sectors/Details/5
        //public ActionResult Details(int? id)
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

        // GET: Sectors/Create
        public ActionResult Create()
        {
            ViewBag.Status = Common.ToSelectList<Status>();
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
                    sector.EntryBy = _db.Users.First(x => x.UserName == User.Identity.Name).Id;
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

        public ActionResult BatchUpload(HttpPostedFileBase sectorFile)
        {
            using (var scope = new TransactionScope())
            {
                try
                {
                    if (sectorFile == null || sectorFile.ContentLength <= 0)
                    {
                        TempData["Toastr"] = Toastr.CustomError("Invalid File!", "File is empty or corrupted.");
                        return RedirectToAction("Index");
                    }
                    // 1048567 bytes = 1 MegaBytes
                    if (sectorFile.FileName == string.Empty || sectorFile.ContentLength > 1048576)
                    {
                        TempData["Toastr"] = Toastr.CustomError("Large File!", "File cannot be more than 1 MegaByte.");
                        return RedirectToAction("Index");
                    }
                    var extension = Path.GetExtension(sectorFile.FileName);
                    // ReSharper disable once InvertIf
                    if (extension == null || _allowedUploadFile.IndexOf(extension) == -1)
                    {
                        TempData["Toastr"] = Toastr.CustomError("Invalid File!", "Unsupported file, only .xls, .xlsx, .csv file are allowed.");
                        return RedirectToAction("Index");
                    }

                    // File reading begin with following format
                    // +--------------+--------------+--------+
                    // | Airport Name | Airport Code | Status |
                    // | xcxcxcxcxcxc | codecxcxcxcc |   0/1  |

                    using (var stream = sectorFile.InputStream)
                    {
                        IExcelDataReader reader = null;
                        switch (extension)
                        {
                            case ".xls":
                                reader = ExcelReaderFactory.CreateBinaryReader(stream);
                                break;
                            case ".xlsx":
                                reader = ExcelReaderFactory.CreateOpenXmlReader(stream);
                                break;
                        }
                        var affectedRows = 0;
                        var count = 0;
                        while (reader != null && reader.Read())
                        {
                            if (count == 0)
                            {
                                count = 1; continue;
                            }

                            var sector = new SectorInfo
                            {
                                SectorId = string.Format("BI-{0:000000}", _db.SectorInfos.Count() + 1),
                                SectorName = reader.GetString(0).ToUpper().Trim(),
                                SectorCode = reader.GetString(1).ToUpper().Trim(),
                                Status = reader.GetString(2) == "Active" ? Status.Active : Status.Inactive,
                                EntryBy = _db.Users.First(x => x.UserName == User.Identity.Name).Id,
                                EntryDate = DateTime.Now
                            };

                            if (_db.SectorInfos.Any(x => x.SectorName == sector.SectorName || x.SectorCode==sector.SectorCode)) continue;

                            _db.SectorInfos.Add(sector);
                            affectedRows += _db.SaveChanges();
                        }
                        scope.Complete();
                        TempData["Toastr"] = Toastr.CustomSuccess("Sector file uploaded successfully.");
                        return RedirectToAction("Index");
                    }

                }
                catch (Exception ex)
                {
                    Transaction.Current.Rollback();
                    TempData["Toastr"] = Toastr.CustomError("Exception!", ex.Message);
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
