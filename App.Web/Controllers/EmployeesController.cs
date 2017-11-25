using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using App.Entity.Models;
using App.Web.Context;
using App.Web.Helper;
using EntityFramework.Extensions;

namespace App.Web.Controllers
{
    [CrmAuthorize(Roles = "Admin")]
    [CrmPermission]
    public class EmployeesController : Controller
    {
        #region Private Zone
        private readonly CrmDbContext _db;
        private readonly List<string> _allowedLogoFileTypes = new List<string> { ".png", ".jpg", ".jpeg", ".gif", ".bmp" };
        #endregion
        public EmployeesController()
        {
            _db = new CrmDbContext();
        }
        // GET: Employees
        public ActionResult Index()
        {
            return View();
        }

        // GET: Employees/Create
        public ActionResult Create()
        {
            try
            {
                ViewBag.Designations = new SelectList(_db.EmployeeDesignations, "Id", "DesignationTitleEn");
                ViewBag.BloodGroups = Common.ToSelectList<BloodGroup>();
                ViewBag.Levels = Common.ToSelectList<EmployeeLevel>();
                return View();
            }
            catch (Exception ex)
            {
                TempData["Toastr"] = Toastr.DbError(ex.Message);
                return RedirectToAction("Index");
            }
        }

        // POST: Employees/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(EmployeeBasicInfo employee)
        {
            using (var dbTransaction = _db.Database.BeginTransaction())
            {
                const string basePath = @"/Images/Employees/Profile/";
                var filePath = "";
                try
                {
                    ModelState.Clear();
                    employee.EmployeeId = string.Format("EI-{0:000000}", _db.EmployeeBasicInfos.Count() + 1);
                    employee.EntryBy = _db.Users.First(x => x.UserName == User.Identity.Name).Id;
                    employee.EntryDate = DateTime.Now;
                    employee.Status = Status.Active;
                    
                    var serverPath = Server.MapPath("~" + basePath);
                    var profilePhoto = Request.Files["ImageUrl"];
                    // ReSharper disable once PossibleNullReferenceException
                    if (profilePhoto.ContentLength > 0)
                    {
                        // 1048567 bytes = 1 MegaByte
                        if (profilePhoto.FileName == string.Empty || profilePhoto.ContentLength > 2097152)
                        {
                            ModelState.AddModelError("ImageUrl", @"Max file size: 2 MB!");
                            return View(employee);
                        }
                        var extension = Path.GetExtension(profilePhoto.FileName);
                        if (extension == null)
                        {
                            ModelState.AddModelError("ImageUrl", @"Its not a valid file.");
                            return View(employee);
                        }
                        extension = extension.ToLower();
                        if (_allowedLogoFileTypes.IndexOf(extension) == -1)
                        {
                            ModelState.AddModelError("ImageUrl",
                                @"Only .jpg, .png, .jpeg, .gif, .bmp file types are allowed.");
                            return View(employee);
                        }
                        var image = Image.FromStream(profilePhoto.InputStream);
                        if (image.Width != image.Height)
                        {
                            ModelState.AddModelError("ImageUrl", @"Image Should be square size.");
                            return View(employee);
                        }
                        var generatedFileName = string.Format("{0}_{1}", employee.EmployeeId, profilePhoto.FileName);
                        filePath = Path.Combine(serverPath, generatedFileName);
                        if (!Directory.Exists(serverPath)) Directory.CreateDirectory(serverPath);
                        profilePhoto.SaveAs(filePath);
                        employee.ImageUrl = string.Format("{0}{1}", basePath, generatedFileName);
                    }

                    TryValidateModel(employee);
                    if (ModelState.IsValid)
                    {
                        _db.EmployeeBasicInfos.Add(employee);
                        _db.SaveChanges();

                        dbTransaction.Commit();
                        TempData["Toastr"] = Toastr.Added;
                        return RedirectToAction("Index");
                    }
                    dbTransaction.Rollback();
                    if(System.IO.File.Exists(filePath)) System.IO.File.Delete(filePath);
                    return View(employee);
                }
                catch (Exception ex)
                {
                    dbTransaction.Rollback();
                    if (System.IO.File.Exists(filePath)) System.IO.File.Delete(filePath);
                    TempData["Toastr"] = Toastr.DbError(ex.Message);
                    return RedirectToAction("Index");
                }
                finally
                {
                    ViewBag.Designations = new SelectList(_db.EmployeeDesignations, "Id", "DesignationTitleEn");
                    ViewBag.BloodGroups = Common.ToSelectList<BloodGroup>();
                    ViewBag.Levels = Common.ToSelectList<EmployeeLevel>();
                }
            }
        }

        // GET: Employess/Edit/5
        public ActionResult Edit(int? id)
        {
            try
            {
                if (id == null)
                {
                    TempData["Toastr"] = Toastr.BadRequest;
                    return RedirectToAction("Index");
                }
                var employee = _db.EmployeeBasicInfos.Find(id);
                if (employee == null)
                {
                    TempData["Toastr"] = Toastr.HttpNotFound;
                    return RedirectToAction("Index");
                }
                var imageToShow = !string.IsNullOrWhiteSpace(employee.ImageUrl)? employee.ImageUrl : "/Content/template/img/avatars/default.png";
                ViewBag.Designations = new SelectList(_db.EmployeeDesignations, "Id", "DesignationTitleEn",employee.EmployeeDesignationId);
                ViewBag.BloodGroups = Common.ToSelectList<BloodGroup>(employee.BloodGroup);
                ViewBag.Levels = Common.ToSelectList<EmployeeLevel>(employee.UserLevel);
                ViewBag.Image = System.IO.File.Exists(Server.MapPath("~" + imageToShow)) ? imageToShow : "";
                return View(employee);
            }
            catch (Exception ex)
            {
                TempData["Toastr"] = Toastr.DbError(ex.Message);
                return RedirectToAction("Index");
            }
        }

        // POST: Employees/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(EmployeeBasicInfo model, int? id)
        {
            using (var dbTransaction = _db.Database.BeginTransaction())
            {
                const string basePath = @"/Images/Employees/Profile/";
                var filePath = "";
                try
                {
                    if (id == null)
                    {
                        TempData["Toastr"] = Toastr.BadRequest;
                        return RedirectToAction("Index");
                    }
                    var employee = _db.EmployeeBasicInfos.AsNoTracking().Single(x=>x.Id == id);
                    if (employee == null)
                    {
                        TempData["Toastr"] = Toastr.HttpNotFound;
                        return RedirectToAction("Index");
                    }
                    model.EmployeeId = employee.EmployeeId;
                    model.Status = employee.Status;
                    model.EntryBy = employee.EntryBy;
                    model.EntryDate = employee.EntryDate;
                    model.ImageUrl = employee.ImageUrl;
                    ModelState.Clear();

                    var serverPath = Server.MapPath("~" + basePath);
                    var profilePhoto = Request.Files["ImageUrl"];
                    // ReSharper disable once PossibleNullReferenceException
                    if (profilePhoto.ContentLength > 0)
                    {
                        // 1048567 bytes = 1 MegaByte
                        if (profilePhoto.FileName == string.Empty || profilePhoto.ContentLength > 2097152)
                        {
                            ModelState.AddModelError("ImageUrl", @"Max file size: 2 MB!");
                            return View(model);
                        }
                        var extension = Path.GetExtension(profilePhoto.FileName);
                        if (extension == null)
                        {
                            ModelState.AddModelError("ImageUrl", @"Its not a valid file.");
                            return View(model);
                        }
                        extension = extension.ToLower();
                        if (_allowedLogoFileTypes.IndexOf(extension) == -1)
                        {
                            ModelState.AddModelError("ImageUrl",
                                @"Only .jpg, .png, .jpeg, .gif, .bmp file types are allowed.");
                            return View(model);
                        }
                        var image = Image.FromStream(profilePhoto.InputStream);
                        if (image.Width != image.Height)
                        {
                            ModelState.AddModelError("ImageUrl", @"Image Should be square size.");
                            return View(model);
                        }
                        var generatedFileName = string.Format("{0}_{1}", model.EmployeeId, profilePhoto.FileName);
                        filePath = Path.Combine(serverPath, generatedFileName);
                        if (!Directory.Exists(serverPath)) Directory.CreateDirectory(serverPath);
                        profilePhoto.SaveAs(filePath);
                        model.ImageUrl = string.Format("{0}{1}", basePath, generatedFileName);
                    }

                    TryValidateModel(model);
                    if (ModelState.IsValid)
                    {
                        _db.Entry(model).State = EntityState.Modified;
                        _db.SaveChanges();
                        dbTransaction.Commit();
                        TempData["Toastr"] = Toastr.Updated;
                        return RedirectToAction("Index");
                    }
                    dbTransaction.Rollback();
                    if (System.IO.File.Exists(filePath)) System.IO.File.Delete(filePath);
                    return View(model);
                }
                catch (Exception ex)
                {
                    dbTransaction.Rollback();
                    if (System.IO.File.Exists(filePath)) System.IO.File.Delete(filePath);
                    TempData["Toastr"] = Toastr.DbError(ex.Message);
                    return RedirectToAction("Index");
                }
                finally
                {
                    ViewBag.Designations = new SelectList(_db.EmployeeDesignations, "Id", "DesignationTitleEn", model.EmployeeDesignationId);
                    ViewBag.BloodGroups = Common.ToSelectList<BloodGroup>(model.BloodGroup);
                    ViewBag.Levels = Common.ToSelectList<EmployeeLevel>(model.UserLevel);
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