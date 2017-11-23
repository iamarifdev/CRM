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
    [CrmPermission]
    public class DepositsController : Controller
    {
        #region Private Zone

        private readonly CrmDbContext _db;

        #endregion

        public DepositsController()
        {
            _db = new CrmDbContext();
        }
        // GET: Deposits
        public ActionResult Index()
        {
            return View();
        }

        // GET: Deposits/Details/5
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

        // GET: Deposits/Create
        public ActionResult Create()
        {
            ViewBag.Accounts = new SelectList(_db.BankAccounts.ToList(), "Id", "AccountName");
            ViewBag.PayerTypes = Common.ToSelectList<PayerType>();
            ViewBag.PaymentMethods = new SelectList(_db.PaymentMethods.ToList(), "Id", "MethodName");
            return View();
        }

        // POST: Deposits/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,AccountId,Date,PayerType,Amount,MethodId,PayerId,Description")] DepositViewModel deposit)
        {
            using (var dbTransaction = _db.Database.BeginTransaction())
            {
                try
                {
                    if (!ModelState.IsValid) return View(deposit);
                    ModelState.Clear();

                    var transaction = new TransactionsInfo();
                    transaction.TransactionId = string.Format("DI-{0:000000}", _db.TransactionsInfos.Count(x => x.TransactionType == TransactionType.Deposit || x.TransactionType == TransactionType.Transfer) + 1);
                    transaction.TransactionType = TransactionType.Deposit;
                    transaction.Date = deposit.Date;
                    transaction.AccountTo = deposit.AccountId;
                    transaction.PayerType = deposit.PayerType;
                    transaction.PayerId = deposit.PayerId;
                    transaction.Amount = deposit.Amount;
                    transaction.MethodId = deposit.MethodId;
                    transaction.Description = deposit.Description;

                    _db.TransactionsInfos.Add(transaction);
                    if (_db.SaveChanges() > 0) _db.UpdateBalance(_db.BankAccounts.Find(deposit.AccountId), (double) deposit.Amount);

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
                    ViewBag.PayerTypes = Common.ToSelectList<PayerType>();
                    ViewBag.PaymentMethods = new SelectList(_db.PaymentMethods.ToList(), "Id", "MethodName");
                }
            }
        }

        // GET: Deposits/Edit/5
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
                var deposit = new DepositViewModel();
                deposit.Id = transactionsInfo.Id;
                // ReSharper disable once PossibleInvalidOperationException
                deposit.Date = (DateTime)transactionsInfo.Date;
                // ReSharper disable once PossibleInvalidOperationException
                deposit.AccountId = (int)transactionsInfo.AccountTo;
                // ReSharper disable once PossibleInvalidOperationException
                deposit.PayerType = (PayerType)transactionsInfo.PayerType;
                deposit.PayerId = transactionsInfo.PayerId;
                deposit.Amount = transactionsInfo.Amount;
                deposit.MethodId = transactionsInfo.MethodId;
                deposit.Description = transactionsInfo.Description;

                ViewBag.Accounts = new SelectList(_db.BankAccounts.ToList(), "Id", "AccountName",deposit.AccountId);
                ViewBag.PayerTypes = Common.ToSelectList<PayerType>(deposit.PayerType);
                ViewBag.PaymentMethods = new SelectList(_db.PaymentMethods.ToList(), "Id", "MethodName",deposit.MethodId);

                return View(deposit);
            }
            catch (Exception ex)
            {
                TempData["Toastr"] = Toastr.DbError(ex.Message);
                return RedirectToAction("Index");
            }
        }

        // POST: Deposits/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,AccountId,Date,PayerType,Amount,MethodId,PayerId,Description")] DepositViewModel deposit, int? id)
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

                    if (!ModelState.IsValid) return View(deposit);
                    var count = _db.TransactionsInfos
                        .Where(x => x.Id == id)
                        .Update(u => new TransactionsInfo
                        {
                            Date = deposit.Date,
                            AccountTo = deposit.AccountId,
                            PayerType = deposit.PayerType,
                            PayerId = deposit.PayerId,
                            Amount = deposit.Amount,
                            MethodId = deposit.MethodId,
                            Description = deposit.Description
                        });
                    if (count > 0) _db.UpdateBalance(_db.BankAccounts.Find(deposit.AccountId), (double)deposit.Amount);
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
                    ViewBag.Accounts = new SelectList(_db.BankAccounts.ToList(), "Id", "AccountName", deposit.AccountId);
                    ViewBag.PayerTypes = Common.ToSelectList<PayerType>(deposit.PayerType);
                    ViewBag.PaymentMethods = new SelectList(_db.PaymentMethods.ToList(), "Id", "MethodName", deposit.MethodId);
                }
            }
        }

        // POST: Deposits/Delete/5
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
                    var transaction = _db.TransactionsInfos.FirstOrDefault(x => x.Id==id && x.TransactionType == TransactionType.Deposit);
                    if (transaction == null)
                    {
                        TempData["Toastr"] = Toastr.HttpNotFound;
                        return RedirectToAction("Index");
                    }
                    var amount = transaction.Amount;
                    var account = _db.BankAccounts.Find(transaction.AccountTo);
                    _db.TransactionsInfos.Remove(transaction);
                    if (_db.SaveChanges() > 0) _db.UpdateBalance(account, (double) amount, BalanceMode.Decrement);
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

        [HttpPost]
        public ActionResult GetAdditionalPayerTypeFields(PayerType? payerType, int? id = null)
        {
            var model = new DepositViewModel();
            if (id != null)
            {
                var transaction = _db.TransactionsInfos.Find(id);
                if (transaction != null)
                {
                    model.Id = transaction.Id;
                    // ReSharper disable once PossibleInvalidOperationException
                    model.AccountId = (int)transaction.AccountTo;
                    // ReSharper disable once PossibleInvalidOperationException
                    model.Date = (DateTime)transaction.Date;
                    // ReSharper disable once PossibleInvalidOperationException
                    model.PayerType = (PayerType)transaction.PayerType;
                    model.PayerId = transaction.PayerId;
                    model.Amount = transaction.Amount;
                    model.MethodId = transaction.MethodId;
                    model.Description = transaction.Description;
                }
            }

            if (payerType == null) return Json(new { }, JsonRequestBehavior.AllowGet);
            switch (payerType)
            {
                case PayerType.Agent:
                    return PartialView("_AdditionalAgentField", model);
                case PayerType.Client:
                    return PartialView("_AdditionalClientFileld", model);
                default:
                    return Json(new { }, JsonRequestBehavior.AllowGet);
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