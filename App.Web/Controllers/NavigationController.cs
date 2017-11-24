using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
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
    [CrmAuthorize]
    [CrmPermission]
    public class NavigationController : Controller
    {
        #region Private Zone

        private readonly CrmDbContext _db;
        private readonly List<string> _allowedUploadFile = new List<string> { ".xls", ".csv", ".xlsx" };

        #endregion

        public NavigationController()
        {
            _db = new CrmDbContext();
        }
        // GET: Navigation
        public ActionResult Index()
        {
            return View();
        }


        // GET: Navigation/Create
        [HttpGet]
        public ActionResult Create()
        {
            try
            {
                ViewBag.Modules = Common.ToSelectList<Module>();
                ViewBag.StatusList = Common.ToSelectList<Status>();
                return View();
            }
            catch (Exception ex)
            {
                TempData["Toastr"] = Toastr.CustomError("Unknown Error.",ex.Message);
                return RedirectToAction("Index");
            }
            
        }

        // POST: Navigation/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Menu menu)
        {
            using (var dbTransaction = _db.Database.BeginTransaction())
            {
                try
                {
                    if (!ModelState.IsValid)
                    {
                        dbTransaction.Dispose();
                        ViewBag.Modules = Common.ToSelectList<Module>();
                        ViewBag.StatusList = Common.ToSelectList<Status>();
                        return View(menu);
                    }
                    _db.Menus.Add(menu);
                    _db.SaveChanges();
                    dbTransaction.Commit();

                    TempData["Toastr"] = Toastr.Added;
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

        // GET: Navigation/Edit/5
        public ActionResult Edit(int? id)
        {
            try
            {
                if (id == null)
                {
                    TempData["Toastr"] = Toastr.BadRequest;
                    return RedirectToAction("Index");
                }
                var menu = _db.Menus.Find(id);
                if (menu == null)
                {
                    TempData["Toastr"] = Toastr.HttpNotFound;
                    return RedirectToAction("Index");
                }
                ViewBag.Modules = Common.ToSelectList<Module>(menu.ModuleName);
                ViewBag.StatusList = Common.ToSelectList<Status>(menu.Status);

                return View(menu);
            }
            catch (Exception ex)
            {
                TempData["Toastr"] = Toastr.DbError(ex.Message);
                return RedirectToAction("Index");
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Menu menu, int? menuId)
        {
            using (var dbTransaction = _db.Database.BeginTransaction())
            {
                try
                {
                    if (!ModelState.IsValid)
                    {
                        dbTransaction.Dispose();
                        ViewBag.Modules = Common.ToSelectList<Module>(menu.ModuleName);
                        ViewBag.StatusList = Common.ToSelectList<Status>(menu.Status);
                        return View(menu);
                    }
                    if (menuId == null)
                    {
                        TempData["Toastr"] = Toastr.BadRequest;
                        return RedirectToAction("Index");
                    }
                    if (!_db.BranchInfos.Any(x => x.Id == menuId))
                    {
                        TempData["Toastr"] = Toastr.HttpNotFound;
                        return RedirectToAction("Index");
                    }

                    _db.Menus
                        .Where(x => x.MenuId == menuId)
                        .Update(u => new Menu
                        {
                            ModuleName = menu.ModuleName,
                            ControllerName = menu.ControllerName,
                            ActionName = menu.ActionName,
                            Status = menu.Status
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

        [HttpPost]
        public JsonResult UpdateMenuStatus(int? menuId)
        {
            try
            {
                if (menuId == null) return Json(new { Flag = false, Msg = "Invalid Data Submitted." });

                var menu = _db.Menus.Find(menuId);
                if (menu == null) return Json(new { Flag = false, Msg = "Menu not found." });
                using (var dbTransaction = _db.Database.BeginTransaction())
                {
                    try
                    {
                        if (menu.Status == Status.Inactive) _db.Menus.Where(x => x.MenuId == menuId).Update(u => new Menu { Status = Status.Active });
                        else _db.Menus.Where(x => x.MenuId == menuId).Update(u => new Menu { Status = Status.Inactive });
                        dbTransaction.Commit();
                        return Json(new
                        {
                            Flag = true,
                            Msg = menu.Status == Status.Active ? "Menu Succesfully Deactived." : "Menu Succesfully Actived."
                        });
                    }
                    catch (Exception ex)
                    {
                        dbTransaction.Rollback();
                        return Json(new { Flag = false, Msg = ex.Message });
                    }
                }
            }
            catch (Exception ex)
            {
                return Json(new { Flag = false, Msg = ex.Message });
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult BatchInsert(HttpPostedFileBase navigationFile)
        {
            using (var scope = new TransactionScope())
            {
                try
                {
                    var affectedRows = 0;
                    var menus = new List<Menu>();

                    if (navigationFile == null || navigationFile.ContentLength <= 0)
                    {
                        TempData["Toastr"] = Toastr.CustomError("Invalid File!", "File is empty or corrupted.");
                        return RedirectToAction("Index");
                    }
                    // 1048567 bytes = 1 MegaByte
                    if (navigationFile.FileName == string.Empty || navigationFile.ContentLength > 1048576)
                    {
                        TempData["Toastr"] = Toastr.CustomError("Large File!", "File cannot be more than 1 MegaByte.");
                        return RedirectToAction("Index");
                    }
                    var extension = Path.GetExtension(navigationFile.FileName);
                    // ReSharper disable once InvertIf
                    if (extension == null || _allowedUploadFile.IndexOf(extension) == -1)
                    {
                        TempData["Toastr"] = Toastr.CustomError("Invalid File!", "Unsupported file, only .xls, .xlsx, .csv file are allowed.");
                        return RedirectToAction("Index");
                    }

                    // File reading begin with following format
                    // +-------------+-----------------+-------------+-------+
                    // | Module Name | Controller Name | Action Name |Status |
                    // | 1/2/3/4/5/6 | Clients         | Index       |0/1    |

                    if (extension == ".csv")
                    {
                        using (var reader = new BinaryReader(navigationFile.InputStream))
                        {
                            var binData = reader.ReadBytes(navigationFile.ContentLength);
                            var result = System.Text.Encoding.UTF8.GetString(binData);
                            var rows = result.Split('\n');
                            var rowNumber = 0;
                            foreach (var row in rows)
                            {
                                if (rowNumber < 1) { rowNumber++; continue; }
                                if (string.IsNullOrWhiteSpace(row.Trim())) continue;
                                var cells = row.Trim().Replace("\r", "").Split(',');
                                var menu = new Menu
                                {
                                    ModuleName = (Module)Convert.ToInt32(cells[0].Trim()),
                                    ControllerName = cells[1].ToLower().Trim(),
                                    ActionName = cells[2].ToLower().Trim(),
                                    Status = (Status)Convert.ToInt32(cells[3].Trim())
                                };
                                if (_db.Menus.Any(x => x.ModuleName == menu.ModuleName && x.ControllerName == menu.ControllerName && x.ActionName == menu.ActionName)) continue;
                                menus.Add(menu);
                            }
                        }
                    }
                    else
                    {
                        using (var stream = navigationFile.InputStream)
                        {
                            IExcelDataReader reader;
                            switch (extension)
                            {
                                case ".xls":
                                    reader = ExcelReaderFactory.CreateBinaryReader(stream);
                                    break;
                                case ".xlsx":
                                    reader = ExcelReaderFactory.CreateOpenXmlReader(stream);
                                    break;
                                default:
                                    reader = ExcelReaderFactory.CreateOpenXmlReader(stream);
                                    break;
                            }

                            var isHeading = true;
                            while (reader != null && reader.Read())
                            {
                                //skip heading from excel file
                                if (isHeading)
                                {
                                    isHeading = false; continue;
                                }

                                var menu = new Menu
                                {
                                    ModuleName = (Module)Convert.ToInt32(reader.GetDouble(0)),
                                    ControllerName = reader.GetString(1).ToLower().Trim(),
                                    ActionName = reader.GetString(2).ToLower().Trim(),
                                    Status = (Status)Convert.ToInt32(reader.GetDouble(3))
                                };

                                if (_db.Menus.Any(x => x.ModuleName == menu.ModuleName && x.ControllerName == menu.ControllerName && x.ActionName == menu.ActionName)) continue;
                                menus.Add(menu);
                            }
                        }
                    }

                    foreach (var menu in menus)
                    {
                        _db.Menus.Add(menu);
                        affectedRows += _db.SaveChanges();
                        //Sending Progress using SignalR
                        Common.SendProgress("Uploading..", affectedRows, menus.Count);
                    }
                    scope.Complete();

                    Thread.Sleep(1000);

                    TempData["Toastr"] = Toastr.CustomSuccess(string.Format("Navigation file uploaded successfully. {0} items added.", affectedRows));
                    return RedirectToAction("Index");

                }
                catch (Exception ex)
                {
                    Transaction.Current.Rollback();
                    TempData["Toastr"] = Toastr.CustomError("Exception!", ex.Message);
                    return RedirectToAction("Index");
                }
            }
        }


        // POST: Navigation/Delete/5
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
                    var menu = _db.Menus.Find(id);
                    if (menu == null)
                    {
                        TempData["Toastr"] = Toastr.HttpNotFound;
                        return RedirectToAction("Index");
                    }
                    _db.Menus.Remove(menu);
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
    }

}