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
    public class TransfersController : Controller
    {

        #region Private Zone

        private readonly CrmDbContext _db;

        #endregion

        public TransfersController()
        {
            _db = new CrmDbContext();
        }

        // GET: Transfers
        public ActionResult Index()
        {
            return View();
        }

        // GET: Transfers/Details/5
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

        // GET: Transfers/Create
        public ActionResult Create()
        {
            var accounts = new SelectList(_db.BankAccounts.ToList(), "Id", "AccountName");
            ViewBag.FromAccounts = accounts;
            ViewBag.ToAccounts= accounts;
            ViewBag.PaymentMethods = new SelectList(_db.PaymentMethods.ToList(), "Id", "MethodName");
            return View();
        }

        // POST: Transfers/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,AccountFrom,AccountTo,Date,Amount,MethodId,Description")] TransferViewModel transfer)
        {
            using (var dbTransaction = _db.Database.BeginTransaction())
            {
                try
                {
                    if (!ModelState.IsValid) return View(transfer);
                    ModelState.Clear();

                    var transaction = new TransactionsInfo();
                    transaction.TransactionId = string.Format("DI-{0:000000}", _db.TransactionsInfos.Count(x => x.TransactionType == TransactionType.Deposit || x.TransactionType == TransactionType.Transfer) + 1);
                    transaction.TransactionType = TransactionType.Transfer;
                    transaction.Date = transfer.Date;
                    transaction.AccountTo = transfer.AccountTo;
                    transaction.AccountFrom = transfer.AccountFrom;
                    transaction.Amount = transfer.Amount;
                    transaction.MethodId = transfer.MethodId;
                    transaction.Description = transfer.Description;

                    _db.TransactionsInfos.Add(transaction);
                    if (_db.SaveChanges() > 0)
                    {
                        _db.UpdateBalance(_db.BankAccounts.Find(transfer.AccountFrom), (double)transfer.Amount, BalanceMode.Decrement);
                        _db.UpdateBalance(_db.BankAccounts.Find(transfer.AccountTo), (double) transfer.Amount);
                    }

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
                    var accounts = new SelectList(_db.BankAccounts.ToList(), "Id", "AccountName");
                    ViewBag.FromAccounts = accounts;
                    ViewBag.ToAccounts = accounts;
                    ViewBag.PaymentMethods = new SelectList(_db.PaymentMethods.ToList(), "Id", "MethodName");
                }
            }
        }

        // GET: Transfers/Edit/5
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
                if (transactionsInfo.TransactionType != TransactionType.Transfer)
                {
                    TempData["Toastr"] = Toastr.HttpNotFound;
                    return RedirectToAction("Index");
                }
                var transfer = new TransferViewModel();
                transfer.Id = transactionsInfo.Id;
                // ReSharper disable once PossibleInvalidOperationException
                transfer.Date = (DateTime)transactionsInfo.Date;
                // ReSharper disable once PossibleInvalidOperationException
                transfer.AccountFrom = (int)transactionsInfo.AccountFrom;
                // ReSharper disable once PossibleInvalidOperationException
                transfer.AccountTo = (int)transactionsInfo.AccountTo;
                transfer.Amount = transactionsInfo.Amount;
                transfer.MethodId = transactionsInfo.MethodId;
                transfer.Description = transactionsInfo.Description;

                ViewBag.FromAccounts = new SelectList(_db.BankAccounts.ToList(), "Id", "AccountName", transfer.AccountFrom);
                ViewBag.ToAccounts = new SelectList(_db.BankAccounts.ToList(), "Id", "AccountName", transfer.AccountTo);
                ViewBag.PaymentMethods = new SelectList(_db.PaymentMethods.ToList(), "Id", "MethodName", transfer.MethodId);

                return View(transfer);
            }
            catch (Exception ex)
            {
                TempData["Toastr"] = Toastr.DbError(ex.Message);
                return RedirectToAction("Index");
            }
        }

        // POST: Transfers/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,AccountFrom,AccountTo,Date,Amount,MethodId,Description")] TransferViewModel transfer, int? id)
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
                    if (!_db.TransactionsInfos.Any(x => x.Id == id && x.TransactionType == TransactionType.Transfer))
                    {
                        TempData["Toastr"] = Toastr.HttpNotFound;
                        return RedirectToAction("Index");
                    }
                    if (!ModelState.IsValid) return View(transfer);
                    var count = _db.TransactionsInfos
                        .Where(x => x.Id == id)
                        .Update(u => new TransactionsInfo
                        {
                            Date = transfer.Date,
                            AccountTo = transfer.AccountTo,
                            AccountFrom = transfer.AccountFrom,
                            Amount = transfer.Amount,
                            MethodId = transfer.MethodId,
                            Description = transfer.Description
                        });
                    if (count > 0)
                    {
                        _db.UpdateBalance(_db.BankAccounts.Find(transfer.AccountFrom), (double)transfer.Amount, BalanceMode.Decrement);
                        _db.UpdateBalance(_db.BankAccounts.Find(transfer.AccountTo), (double)transfer.Amount);
                    }
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
                    ViewBag.FromAccounts = new SelectList(_db.BankAccounts.ToList(), "Id", "AccountName", transfer.AccountFrom);
                    ViewBag.ToAccounts = new SelectList(_db.BankAccounts.ToList(), "Id", "AccountName", transfer.AccountTo);
                    ViewBag.PaymentMethods = new SelectList(_db.PaymentMethods.ToList(), "Id", "MethodName", transfer.MethodId);
                }
            }
        }

        // POST: Transfers/Delete/5
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
                    var transaction = _db.TransactionsInfos.FirstOrDefault(x => x.Id == id && x.TransactionType == TransactionType.Transfer);
                    if (transaction == null)
                    {
                        TempData["Toastr"] = Toastr.HttpNotFound;
                        return RedirectToAction("Index");
                    }
                    var amount = transaction.Amount;
                    var accountFrom = _db.BankAccounts.Find(transaction.AccountFrom);
                    var accountTo = _db.BankAccounts.Find(transaction.AccountTo);
                    _db.TransactionsInfos.Remove(transaction);
                    if (_db.SaveChanges() > 0)
                    {
                        _db.UpdateBalance(accountFrom, (double)amount);
                        _db.UpdateBalance(accountTo, (double)amount, BalanceMode.Decrement);
                    }
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