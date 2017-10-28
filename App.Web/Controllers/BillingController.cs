using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using App.Entity.Models;
using App.Web.Context;
using App.Web.Models;
using Microsoft.AspNet.Identity;

namespace App.Web.Controllers
{
    [Authorize(Roles = "Admin")]
    public class BillingController : Controller
    {
        #region Private Zone

        private readonly CrmDbContext _db;

        #endregion

        public BillingController()
        {
            _db = new CrmDbContext();
        }

        [HttpGet]
        public ActionResult ClientPayment()
        {
            ViewBag.BranchList = new SelectList(_db.BranchInfos.ToList(), "Id", "BranchName");
            ViewBag.ClientList = new SelectList(new List<ClientInfo>());
            return View();
        }

        [HttpPost]
        public ActionResult ClientPayment(ClientPaymentViewModel model)
        {
            try
            {
                if (!ModelState.IsValid) return View(model);


            }
            catch (Exception)
            {
                throw;
            }
            return Json("", JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetClientsBranchWise(int? branchId)
        {
            try
            {
                if (branchId == null) return Json(new { Flag = false, Msg = "Bad request" }, JsonRequestBehavior.AllowGet);
                if (!_db.ClientInfos.Any(x => x.BranchId == branchId)) return Json(new { Flag = false, Msg = "No clients are available." }, JsonRequestBehavior.AllowGet);
                var clients = new SelectList(_db.ClientInfos.Where(x => x.BranchId == branchId).ToList(), "Id", "FirstName");
                return Json(new { Flag = true, Clients = clients }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { Flag = false, Msg = ex.Message }, JsonRequestBehavior.AllowGet);
            }

        }

        public JsonResult GetServiceChargeInfo(int? customerId)
        {
            try
            {
                if (customerId == null) return Json(new { Flag = false, Msg = "Bad request" }, JsonRequestBehavior.AllowGet);
                var totalServiceCharge = _db.ClientInfos.Where(x => x.Id == (int)customerId).Sum(x => x.ServiceCharge) ?? 0;
                var due = totalServiceCharge;
                var totalPaid = 0.00;
                // ReSharper disable once InvertIf
                if (_db.CustomerPayments.Any(x => x.CustomerId == customerId))
                {
                    totalPaid = _db.CustomerPayments.Where(x => x.CustomerId == customerId).Sum(x => x.PaymentAmount);
                    due = totalServiceCharge - totalPaid;
                }
                var data = new { Flag = true, TotalPaid = totalPaid, TotalServiceCharge = totalServiceCharge, Due = due, IsDueExist = due > 0.00 };
                return Json(data, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { Flag = false, Msg = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpGet]
        public ActionResult GetPaymentFields()
        {
            try
            {
                return PartialView("_ClientPaymentAdditional", new ClientPaymentViewModel());
            }
            catch (Exception ex)
            {
                return Json(null, JsonRequestBehavior.AllowGet);
            }
            
        }
    }
}