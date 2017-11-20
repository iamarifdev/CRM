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
    [CrmAuthorize(Roles = "Admin")]
    public class ExpensesController : Controller
    {
        #region Private Zone

        private readonly CrmDbContext _db;

        #endregion

        public ExpensesController()
        {
            _db = new CrmDbContext();
        }
        // GET: Expenses
        public ActionResult Index()
        {
            return View();
        }

        // GET: Expenses/Details/5
        [HttpGet]
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                TempData["Toastr"] = Toastr.BadRequest;
                return RedirectToAction("Index");
            }
            var transaction = _db.TransactionsInfos.Find(id);

            if (transaction != null) return View(transaction);

            TempData["Toastr"] = Toastr.HttpNotFound;
            return RedirectToAction("Index");
        }

        // GET: Expenses/Create
        public ActionResult Create()
        {
            ViewBag.Accounts = new SelectList(_db.BankAccounts.ToList(), "Id", "AccountName");
            ViewBag.PaymentMethods = new SelectList(_db.PaymentMethods.ToList(), "Id", "MethodName");
            return View();
        }

        // POST: Expenses/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,AccountId,Date,Amount,MethodId,Description")] ExpenseViewModel expense)
        {
            using (var dbTransaction = _db.Database.BeginTransaction())
            {
                try
                {
                    if (!ModelState.IsValid) return View(expense);
                    ModelState.Clear();

                    var transaction = new TransactionsInfo();
                    transaction.TransactionId = string.Format("EI-{0:000000}", _db.TransactionsInfos.Count(x => x.TransactionType == TransactionType.Expense) + 1);
                    transaction.TransactionType = TransactionType.Expense;
                    transaction.Date = expense.Date;
                    transaction.AccountFrom = expense.AccountId;
                    transaction.Amount = expense.Amount;
                    transaction.MethodId = expense.MethodId;
                    transaction.Description = expense.Description;

                    _db.TransactionsInfos.Add(transaction);
                    if (_db.SaveChanges() > 0) _db.UpdateBalance(_db.BankAccounts.Find(expense.AccountId), (double)expense.Amount, BalanceMode.Decrement);

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
                finally
                {
                    ViewBag.Accounts = new SelectList(_db.BankAccounts.ToList(), "Id", "AccountName");
                    ViewBag.PaymentMethods = new SelectList(_db.PaymentMethods.ToList(), "Id", "MethodName");
                }
            }
        }

        // GET: Expenses/Edit/5
        [HttpGet]
        public ActionResult Edit(int? id)
        {
            try
            {
                if (id == null)
                {
                    TempData["Toastr"] = Toastr.BadRequest;
                    return RedirectToAction("Index");
                }
                var transactionsInfo = _db.TransactionsInfos.Find(id);
                if (transactionsInfo == null)
                {
                    TempData["Toastr"] = Toastr.HttpNotFound;
                    return RedirectToAction("Index");
                }
                var expense = new ExpenseViewModel();
                expense.Id = transactionsInfo.Id;
                // ReSharper disable once PossibleInvalidOperationException
                expense.Date = (DateTime)transactionsInfo.Date;
                // ReSharper disable once PossibleInvalidOperationException
                expense.AccountId = (int)transactionsInfo.AccountFrom;
                expense.Amount = transactionsInfo.Amount;
                expense.MethodId = transactionsInfo.MethodId;
                expense.Description = transactionsInfo.Description;

                ViewBag.Accounts = new SelectList(_db.BankAccounts.ToList(), "Id", "AccountName", expense.AccountId);
                ViewBag.PaymentMethods = new SelectList(_db.PaymentMethods.ToList(), "Id", "MethodName", expense.MethodId);

                return View(expense);
            }
            catch (Exception ex)
            {
                TempData["Toastr"] = Toastr.DbError(ex.Message);
                return RedirectToAction("Index");
            }
        }

        // POST: Expenses/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,AccountId,Date,Amount,MethodId,Description")] ExpenseViewModel expense, int? id)
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
                    if (!_db.TransactionsInfos.Any(x => x.Id == id))
                    {
                        TempData["Toastr"] = Toastr.HttpNotFound;
                        return RedirectToAction("Index");
                    }
                    if (!ModelState.IsValid) return View(expense);

                    var count = _db.TransactionsInfos
                        .Where(x => x.Id == id)
                        .Update(u => new TransactionsInfo
                        {
                            Date = expense.Date,
                            AccountFrom = expense.AccountId,
                            Amount = expense.Amount,
                            MethodId = expense.MethodId,
                            Description = expense.Description
                        });
                    if (count > 0) _db.UpdateBalance(_db.BankAccounts.Find(expense.AccountId), (double)expense.Amount, BalanceMode.Decrement);

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
                    ViewBag.Accounts = new SelectList(_db.BankAccounts.ToList(), "Id", "AccountName", expense.AccountId);
                    ViewBag.PaymentMethods = new SelectList(_db.PaymentMethods.ToList(), "Id", "MethodName", expense.MethodId);
                }
            }
        }

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
                    var transaction = _db.TransactionsInfos.FirstOrDefault(x => x.Id == id && x.TransactionType == TransactionType.Expense);
                    if (transaction == null)
                    {
                        TempData["Toastr"] = Toastr.HttpNotFound;
                        return RedirectToAction("Index");
                    }
                    var amount = transaction.Amount;
                    var account = _db.BankAccounts.Find(transaction.AccountFrom);

                    _db.TransactionsInfos.Remove(transaction);
                    if (_db.SaveChanges() > 0) _db.UpdateBalance(account, (double)amount);

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