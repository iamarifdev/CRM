using System;
using System.Web.Mvc;
using App.Entity.Models;
using App.Web.Context;
using App.Web.Helper;

namespace App.Web.Controllers
{
    [Authorize(Roles = "Admin")]
    public class TransactionsController : Controller
    {
        #region Private Zone

        private readonly CrmDbContext _db;

        #endregion

        public TransactionsController()
        {
            _db = new CrmDbContext();
        }

        // GET: Transactions
        public ActionResult Index()
        {
            return View();
        }

        // GET: Transactions/Details/5
        [HttpGet]
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                TempData["Toastr"] = Toastr.BadRequest;
                return RedirectToAction("Index");
            }
            var transaction = _db.TransactionsInfos.Find(id);

            if (transaction == null)
            {
                TempData["Toastr"] = Toastr.HttpNotFound;
                return RedirectToAction("Index");
            }
            switch (transaction.TransactionType)
            {
                case TransactionType.Deposit:
                    return RedirectToAction("Details", "Deposits", new { id = (int)id });
                case TransactionType.Expense:
                    return RedirectToAction("Details", "Expenses", new { id = (int)id });
                case TransactionType.Transfer:
                    return RedirectToAction("Details", "Transfers", new { id = (int)id });
                default:
                    TempData["Toastr"] = Toastr.HttpNotFound;
                    return RedirectToAction("Index");
            }
        }

        // GET: Transactions/Details/5
        [HttpGet]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                TempData["Toastr"] = Toastr.BadRequest;
                return RedirectToAction("Index");
            }
            var transaction = _db.TransactionsInfos.Find(id);

            if (transaction == null)
            {
                TempData["Toastr"] = Toastr.HttpNotFound;
                return RedirectToAction("Index");
            }
            switch (transaction.TransactionType)
            {
                case TransactionType.Deposit:
                    return RedirectToAction("Edit", "Deposits", new { id = (int)id });
                case TransactionType.Expense:
                    return RedirectToAction("Edit", "Expenses", new { id = (int)id });
                case TransactionType.Transfer:
                    return RedirectToAction("Edit", "Transfers", new { id = (int)id });
                default:
                    TempData["Toastr"] = Toastr.HttpNotFound;
                    return RedirectToAction("Index");
            }
        }

        // POST: Transactions/Delete/5
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
                    var transaction = _db.TransactionsInfos.Find(id);
                    if (transaction == null)
                    {
                        TempData["Toastr"] = Toastr.HttpNotFound;
                        return RedirectToAction("Index");
                    }
                    _db.TransactionsInfos.Remove(transaction);
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