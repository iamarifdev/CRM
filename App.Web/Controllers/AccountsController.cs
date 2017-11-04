using System;
using System.Linq;
using System.Web.Mvc;
using App.Entity.Models;
using App.Web.Context;
using App.Web.Helper;
using App.Web.Models;
using EntityFramework.Extensions;

namespace App.Web.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AccountsController : Controller
    {
        #region Private Zone

        private readonly CrmDbContext _db;

        #endregion

        public AccountsController()
        {
            _db = new CrmDbContext();
        }
        // GET: Accounts
        public ActionResult Index()
        {
            return View();
        }

        // GET: Accounts/Details/5
        public ActionResult Details(int? id)
        {
            try
            {
                if (id == null)
                {
                    TempData["Toastr"] = Toastr.BadRequest;
                    return RedirectToAction("Index");
                }
                var bankAccount = _db.BankAccounts.Find(id);
                if (bankAccount != null) return View(bankAccount);
                TempData["Toastr"] = Toastr.HttpNotFound;
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                TempData["Toastr"] = Toastr.DbError(ex.Message);
                return RedirectToAction("Index");
            }
        }

        // GET: Accounts/Create
        public ActionResult Create()
        {
            ViewBag.StatusList = Common.ToSelectList<Status>();
            return View();
        }

        // POST: Accounts/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,AccountName,AccountNumber,BankName,BranchName,Balance,Status")] BankAccount bankAccount)
        {
            using (var dbTransaction = _db.Database.BeginTransaction())
            {
                try
                {
                    ModelState.Clear();
                    bankAccount.AccountId = string.Format("PI-{0:000000}", _db.BankAccounts.Count() + 1);
                    bankAccount.EntryBy = _db.Users.First(x => x.UserName == User.Identity.Name).Id;
                    bankAccount.EntryDate = DateTime.Now;
                    TryValidateModel(bankAccount);
                    if (ModelState.IsValid)
                    {
                        _db.BankAccounts.Add(bankAccount);
                        _db.SaveChanges();

                        dbTransaction.Commit();
                        TempData["Toastr"] = Toastr.Added;

                        return RedirectToAction("Index");
                    }
                    dbTransaction.Rollback();
                    return View(bankAccount);
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

        // GET: Accounts/Edit/5
        public ActionResult Edit(int? id)
        {
            try
            {
                if (id == null)
                {
                    TempData["Toastr"] = Toastr.BadRequest;
                    return RedirectToAction("Index");
                }
                var bankAccount = _db.BankAccounts.Find(id);
                if (bankAccount == null)
                {
                    TempData["Toastr"] = Toastr.HttpNotFound;
                    return RedirectToAction("Index");
                }
                ViewBag.StatusList = Common.ToSelectList<Status>(bankAccount.Status);
                return View(bankAccount);
            }
            catch (Exception ex)
            {
                TempData["Toastr"] = Toastr.DbError(ex.Message);
                return RedirectToAction("Index");
            }
        }

        // POST: Accounts/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,AccountName,AccountNumber,BankName,BranchName,Balance,Status")] BankAccount bankAccount, int? id)
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
                    if (_db.BankAccounts.Count(x => x.Id == id) < 1)
                    {
                        TempData["Toastr"] = Toastr.HttpNotFound;
                        return RedirectToAction("Index");
                    }
                    var bankAccountInfo = _db.BankAccounts.Single(x => x.Id == id);
                    if (bankAccountInfo == null)
                    {
                        TempData["Toastr"] = Toastr.HttpNotFound;
                        return RedirectToAction("Index");
                    }

                    ModelState.Clear();
                    bankAccount.AccountId = bankAccountInfo.AccountId;
                    bankAccount.EntryBy = bankAccountInfo.EntryBy;
                    bankAccount.EntryDate = bankAccountInfo.EntryDate;
                    bankAccount.DelStatus = bankAccountInfo.DelStatus;

                    TryValidateModel(bankAccount);

                    if (!ModelState.IsValid) return View(bankAccount);

                    _db.BankAccounts
                        .Where(x => x.Id == id)
                        .Update(u => new BankAccount
                        {
                            AccountName = bankAccount.AccountName,
                            AccountNumber = bankAccount.AccountNumber,
                            BankName = bankAccount.BankName,
                            BranchName = bankAccount.BranchName,
                            Balance = bankAccount.Balance,
                            Status = bankAccount.Status
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
                    ViewBag.StatusList = Common.ToSelectList<Status>(bankAccount.Status);
                }
            }
        }

        //// GET: Accounts/Delete/5
        //public ActionResult Delete(int id)
        //{
        //    return View();
        //}

        // POST: Accounts/Delete/5
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
                    var bankAccount = _db.BankAccounts.Find(id);
                    if (bankAccount == null)
                    {
                        TempData["Toastr"] = Toastr.HttpNotFound;
                        return RedirectToAction("Index");
                    }
                    _db.BankAccounts.Remove(bankAccount);
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

        [HttpGet]
        public ActionResult BalanceSheet()
        {
            try
            {
                var data = _db.BankAccounts.AsNoTracking().AsQueryable();
                ViewBag.Accounts = data.Select(x => new AccountViewModel { Account = x.AccountName, Balance = x.Balance }).ToList();
                ViewBag.TotalBalance = data.Sum(x => x.Balance);
                return View();
            }
            catch (Exception ex)
            {
                TempData["Toastr"] = Toastr.CustomError(ex.Message);
                return RedirectToAction("Index");
            }
            
        }

        [HttpPost]
        public JsonResult IsExpenseBalanceAvailable(double amount, int? accountId)
        {
            try
            {
                if (accountId == null) return Json(false, JsonRequestBehavior.AllowGet);
                var flag = _db.BankAccounts.Any(x => x.Id == accountId && x.Balance >= amount);
                return Json(flag, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(false, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public JsonResult IsTransferBalanceAvailable(double amount, int? accountFrom)
        {
            try
            {
                if (accountFrom == null) return Json(false, JsonRequestBehavior.AllowGet);
                var flag = _db.BankAccounts.Any(x => x.Id == accountFrom && x.Balance >= amount);
                return Json(flag, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(false, JsonRequestBehavior.AllowGet);
            }
        }


    }
}
