using System;
using System.Linq;
using System.Web.Mvc;
using App.Entity.Models;
using App.Web.Context;
using App.Web.Helper;
using App.Web.Models;

namespace App.Web.Controllers
{
    [Authorize(Roles = "Admin")]
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
                finally
                {
                    var accounts = new SelectList(_db.BankAccounts.ToList(), "Id", "AccountName");
                    ViewBag.FromAccounts = accounts;
                    ViewBag.ToAccounts = accounts;
                    ViewBag.PaymentMethods = new SelectList(_db.PaymentMethods.ToList(), "Id", "MethodName");
                }
            }
        }
    }
}