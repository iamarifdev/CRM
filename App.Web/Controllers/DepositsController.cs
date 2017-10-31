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
                    ModelState.Clear();
                    //todo need to uncomment
                    //deposit.CustomerId = string.Format("{0}{1:000000}{2:MMyy}",
                    //    branch.BranchCode.ToUpper(),
                    //    _db.ClientInfos.Count() + 1,
                    //    DateTime.Now
                    //);
                    //deposit.EntryBy = _db.Users.First(x => x.UserName == User.Identity.Name).Id;
                    //deposit.ServedBy = _db.Users.First(x => x.UserName == User.Identity.Name).Id;
                    //deposit.WorkingStatus = WorkingStatus.Pending;
                    //deposit.Status = Status.Inactive;
                    //deposit.InfoStatus = InformationUpdate.NotUpdated;
                    //deposit.DeliveryStatus = DeliveryStatus.NotDelivery;
                    //deposit.SmsConfirmation = SmsConfirmation.NotSendingSms;
                    //deposit.EntryDate = DateTime.Now;
                    //use default branch as head if branch id not selected
                    //deposit.BranchId = branch.Id;
                    //TryValidateModel(deposit);

                    //if (!ModelState.IsValid) return View(deposit);
                    //_db.ClientInfos.Add(deposit);
                    //_db.SaveChanges();

                    //dbTransaction.Commit();
                    //TempData["Toastr"] = Toastr.Added;

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
                    ViewBag.BranchList = new SelectList(_db.BranchInfos, "Id", "BranchName");
                    ViewBag.ReferralTypes = Common.ToSelectList<ReferralsType>();
                    ViewBag.IsRequireSupplier = Common.ToSelectList<RequireSuppiler>(RequireSuppiler.No);
                    ViewBag.ServiceList = new SelectList(_db.ServiceInfos.OrderBy(x => x.ServiceName), "Id", "ServiceName");
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
                    model.PayerType = transaction.PayerType;
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
    }
}