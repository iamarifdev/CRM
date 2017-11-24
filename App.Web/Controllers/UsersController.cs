using System;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.SqlClient;
using System.Linq;
using System.Transactions;
using System.Web.Mvc;
using App.Entity.Models;
using App.Web.Context;
using App.Web.Helper;
using App.Web.Models;
using EntityFramework.Extensions;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using WebGrease.Css.Extensions;

namespace App.Web.Controllers
{
    [CrmAuthorize(Roles = "Admin")]
    [CrmPermission]
    public class UsersController : Controller
    {
        #region Private Zone
        private readonly CrmDbContext _db;
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        #endregion
        public UsersController()
        {
            _db = new CrmDbContext();
            _context = new ApplicationDbContext();
            _userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(_context));
        }
        // GET: Users
        public ActionResult Index()
        {
            return View();
        }

        // GET: Users/Details/5
        public ActionResult Details(int? id)
        {
            try
            {
                if (id == null)
                {
                    TempData["Toastr"] = Toastr.BadRequest;
                    return RedirectToAction("Index");
                }

                var user = _db.Users.Find(id);
                if (user != null) return View(user);

                TempData["Toastr"] = Toastr.HttpNotFound;
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                TempData["Toastr"] = Toastr.DbError(ex.Message);
                return RedirectToAction("Index");
            }
        }

        // GET: Users/Create
        [HttpGet]
        public ActionResult Create()
        {
            try
            {
                ViewBag.Employees = new SelectList(_db.EmployeeBasicInfos.Where(x => x.Status == Status.Active), "Id", "EmployeeName");
                ViewBag.Branches = new SelectList(_db.BranchInfos.Where(x => x.Status == Status.Active), "Id", "BranchName");
                ViewBag.Groups = new SelectList(_db.Groups.Where(x=>x.Name != "Admin"), "Id", "Name");
                ViewBag.StatusList = Common.ToSelectList<Status>();
                ViewBag.UserLevels = Common.ToSelectList<UserLevel>();
                return View();
            }
            catch (Exception ex)
            {
                TempData["Toastr"] = Toastr.DbError(ex.Message);
                return RedirectToAction("Index");
            }

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,UserName,Password,ConfirmPassword,Email,Status,EmployeeId,BranchId,Level,GroupId")]UserViewModel userView)
        {
            using (var transaction = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                try
                {
                    if (!ModelState.IsValid)
                    {
                        transaction.Dispose();
                        return View(userView);
                    }

                    var user = new User();
                    user.Uid = string.Format("UI-{0:000000}", _db.Users.Count() + 1);
                    user.CreatedOn = DateTime.Now;
                    user.IpAddress = "127.0.0.1";
                    user.UserName = userView.UserName;
                    user.Email = userView.Email;
                    user.Status = userView.Status;
                    user.EmployeeId = userView.EmployeeId;
                    user.BranchId = userView.BranchId;
                    user.Level = userView.Level;
                    user.GroupId = userView.GroupId;
                    
                    ModelState.Clear();
                    if (TryValidateModel(user))
                    {
                        var appliationUser = new ApplicationUser { Email = userView.Email, UserName = userView.UserName };
                        var ack = _userManager.Create(appliationUser, userView.Password);
                        if (!ack.Succeeded)
                        {
                            Transaction.Current.Rollback();
                            return View(userView);
                        }
                        _userManager.AddToRole(appliationUser.Id, "Admin");
                        _db.Users.Add(user);
                        _db.SaveChanges();

                        transaction.Complete();
                        TempData["Toastr"] = Toastr.Added;

                        return RedirectToAction("Index");
                    }
                    Transaction.Current.Rollback();
                    return View(userView);
                }
                catch (Exception ex)
                {
                    Transaction.Current.Rollback();
                    TempData["Toastr"] = Toastr.DbError(ex.Message);
                    return RedirectToAction("Index");
                }
                finally
                {
                    ViewBag.Employees = new SelectList(_db.EmployeeBasicInfos.Where(x => x.Status == Status.Active), "Id", "EmployeeName");
                    ViewBag.Branches = new SelectList(_db.BranchInfos.Where(x => x.Status == Status.Active), "Id", "BranchName");
                    ViewBag.Groups = new SelectList(_db.Groups, "Id", "Name");
                    ViewBag.StatusList = Common.ToSelectList<Status>();
                    ViewBag.UserLevels = Common.ToSelectList<UserLevel>();
                }
            }

        }

        // GET: Users/Edit/5
        public ActionResult Edit(int? id)
        {
            try
            {
                if (id == null)
                {
                    TempData["Toastr"] = Toastr.BadRequest;
                    return RedirectToAction("Index");
                }
                var user = _db.Users.Find(id);
                if (user == null)
                {
                    TempData["Toastr"] = Toastr.HttpNotFound;
                    return RedirectToAction("Index");
                }
                ViewBag.Employees = new SelectList(_db.EmployeeBasicInfos.Where(x => x.Status == Status.Active), "Id", "EmployeeName",user.EmployeeId);
                ViewBag.Branches = new SelectList(_db.BranchInfos.Where(x => x.Status == Status.Active), "Id", "BranchName",user.BranchId);
                var isUserAdmin = _db.Users.Any(x => x.Id == id && x.UserName.ToLower() == "admin");
                ViewBag.Groups = isUserAdmin ? new SelectList(_db.Groups, "Id", "Name", user.GroupId) : new SelectList(_db.Groups.Where(x=>x.Name.ToLower() != "admin"), "Id", "Name",user.GroupId);
                ViewBag.StatusList = Common.ToSelectList<Status>(user.Status);
                ViewBag.UserLevels = Common.ToSelectList<UserLevel>(user.Level);
                ViewBag.IsUserIsAdmin = isUserAdmin;
                var userView = new UserViewModel
                {
                    Id = user.Id,
                    UserName = user.UserName,
                    Status = user.Status,
                    BranchId = user.BranchId ?? 0,
                    Email = user.Email,
                    EmployeeId = user.EmployeeId ?? 0,
                    GroupId = user.GroupId,
                    Level = user.Level
                };
                return View(userView);
            }
            catch (Exception ex)
            {
                TempData["Toastr"] = Toastr.DbError(ex.Message);
                return RedirectToAction("Index");
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,UserName,Password,ConfirmPassword,Email,Status,EmployeeId,BranchId,Level,GroupId")] UserViewModel userView, int? id)
        {
            using (var transaction = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                try
                {
                    if (id == null)
                    {
                        TempData["Toastr"] = Toastr.HttpNotFound;
                        return RedirectToAction("Index");
                    }
                    if (!ModelState.IsValid)
                    {
                        transaction.Dispose();
                        ViewBag.Employees = new SelectList(_db.EmployeeBasicInfos.Where(x => x.Status == Status.Active), "Id", "EmployeeName", userView.EmployeeId);
                        ViewBag.Branches = new SelectList(_db.BranchInfos.Where(x => x.Status == Status.Active), "Id", "BranchName", userView.BranchId);
                        var isUserAdmin = _db.Users.Any(x => x.Id == id && x.UserName.ToLower() == "admin");
                        ViewBag.Groups = isUserAdmin ? new SelectList(_db.Groups, "Id", "Name", userView.GroupId) : new SelectList(_db.Groups.Where(x => x.Name.ToLower() != "admin"), "Id", "Name", userView.GroupId);
                        ViewBag.StatusList = Common.ToSelectList<Status>(userView.Status);
                        ViewBag.UserLevels = Common.ToSelectList<UserLevel>(userView.Level);
                        ViewBag.IsUserIsAdmin = isUserAdmin;
                        return View(userView);
                    }
                    if (!_db.Users.Any(x => x.Id == id))
                    {
                        TempData["Toastr"] = Toastr.HttpNotFound;
                        return RedirectToAction("Index");
                    }
                    // ReSharper disable once PossibleNullReferenceException
                    var appUser = _userManager.FindByName(_db.Users.Find(id).UserName);
                    if (appUser.UserName.ToLower() == "admin")
                    {
                        _db.Users
                            .Where(x => x.Id == id)
                            .Update(u => new User
                            {
                                EmployeeId = userView.EmployeeId,
                                Level = userView.Level,
                                Email = userView.Email,
                                BranchId = userView.BranchId
                            });
                        appUser.Email = userView.Email;

                    }
                    else
                    {
                        _db.Users
                            .Where(x => x.Id == id)
                            .Update(u => new User
                            {
                                EmployeeId = userView.EmployeeId,
                                BranchId = userView.BranchId,
                                GroupId = userView.GroupId,
                                UserName = userView.UserName,
                                Status = userView.Status,
                                Level = userView.Level,
                                Email = userView.Email
                            });
                        appUser.UserName = appUser.UserName;
                        appUser.Email = userView.Email;
                    }
                    appUser.PasswordHash = Common.HashPassword(userView.Password);
                    _context.Entry(appUser).State = EntityState.Modified;
                    _context.SaveChanges();

                    transaction.Complete();

                    TempData["Toastr"] = Toastr.Updated;
                    return RedirectToAction("Index");
                }
                catch (Exception ex)
                {
                    Transaction.Current.Rollback();
                    TempData["Toastr"] = Toastr.DbError(ex.Message);
                    return RedirectToAction("Index");
                }
            }
        }

        // POST: Country/Delete/5
        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int? id)
        {
            using (var transaction = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                try
                {
                    if (id == null)
                    {
                        TempData["Toastr"] = Toastr.BadRequest;
                        return RedirectToAction("Index");
                    }
                    var user = _db.Users.Find(id);
                    if (user == null)
                    {
                        TempData["Toastr"] = Toastr.HttpNotFound;
                        return RedirectToAction("Index");
                    }
                    if (user.UserName.ToLower() == "admin")
                    {
                        TempData["Toastr"] = Toastr.CustomError("The User admin cannot be deleted.");
                        return RedirectToAction("Index");
                    }

                    var applicationUser = _userManager.FindByName(user.UserName);
                    var logins = applicationUser.Logins;
                    var rolesForUser = _userManager.GetRoles(applicationUser.Id);

                    logins.ForEach(login =>
                        _userManager.RemoveLogin(login.UserId,
                            new UserLoginInfo(login.LoginProvider, login.ProviderKey)));
                    if (rolesForUser.Any())
                        rolesForUser.ForEach(role => _userManager.RemoveFromRole(applicationUser.Id, role));
                    _userManager.Delete(applicationUser);

                    _db.Users.Remove(user);
                    _db.SaveChanges();

                    transaction.Complete();

                    TempData["Toastr"] = Toastr.Deleted;
                    return RedirectToAction("Index");
                }
                catch (DbUpdateException ex)
                {
                    var sqlException = ex.GetBaseException() as SqlException;
                    if (sqlException == null || sqlException.Errors.Count <= 0) throw;
                    switch (sqlException.Errors[0].Number)
                    {
                        case 547: // Foreign Key violation
                            Transaction.Current.Rollback();
                            TempData["Toastr"] = Toastr.CustomError("The user cannot be deleted, because it is in use.");
                            return RedirectToAction("Index");
                        default:
                            throw;
                    }
                }
                catch (Exception ex)
                {
                    Transaction.Current.Rollback();
                    TempData["Toastr"] = Toastr.DbError(ex.Message);
                    return RedirectToAction("Index");
                }
            }
        }

        public bool IsAdminUser()
        {
            if (!User.Identity.IsAuthenticated) return false;
            var user = User.Identity;
            var context = new ApplicationDbContext();
            var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));
            var s = userManager.GetRoles(user.GetUserId());
            return s[0] == "Admin";
        }

        [HttpPost]
        public JsonResult IsEmailAvailable(string email, int? id)
        {
            try
            {
                var flag = true;
                //create mode
                if (id == null)
                {
                   flag = !_db.Users.Any(x => x.Email == email);
                    if (flag) flag = !_db.AgentInfos.Any(x => x.Email == email);
                }
                // edit mode
                else
                {
                    var user = _db.Users.Find(id);
                    if (user == null) return Json(false, JsonRequestBehavior.AllowGet);
                    if (user.Email != email)
                    {
                        flag = !_db.Users.Any(x => x.Email == email);
                        if (flag) flag = !_db.AgentInfos.Any(x => x.Email == email);
                    }
                }
                return Json(flag, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return Json(false, JsonRequestBehavior.AllowGet);
            }
        }


        [HttpPost]
        public JsonResult IsUserAvailable(string userName, int? id)
        {
            try
            {
                var flag = true;
                var applicationUser = _userManager.FindByName(userName);
                //create mode
                if (id == null) flag = applicationUser == null;
                // edit mode
                else
                {
                    var user = _db.Users.Find(id);
                    if (user == null) return Json(false, JsonRequestBehavior.AllowGet);
                    if (user.UserName != userName) flag = applicationUser == null;
                }
                return Json(flag, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return Json(false, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public JsonResult UpdateUserStatus(int? userId)
        {
            try
            {
                if (userId == null) return Json(new {Flag = false, Msg = "Invalid Data Submitted."});

                var user = _db.Users.Find(userId);
                if (user == null) return Json(new { Flag = false, Msg = "User not found." });
                if (user.UserName == "admin") return Json(new { Flag = false, Msg = "User admin cannot be Deactiveed." });
                using (var dbTransaction = _db.Database.BeginTransaction())
                {
                    try
                    {
                        if (user.Status == Status.Inactive) _db.Users.Where(x=>x.Id == userId).Update(u => new User { Status = Status.Active });
                        else _db.Users.Where(x => x.Id == userId).Update(u => new User { Status = Status.Inactive });
                        dbTransaction.Commit();
                        return Json(new
                        {
                            Flag = true, 
                            Msg = user.Status == Status.Active ? "User Succesfully Deactived." : "User Succesfully Actived."
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