using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using App.Entity.Models;
using App.Web.Context;
using App.Web.Models;
using Microsoft.AspNet.Identity;

namespace App.Web.Controllers
{
    [Authorize(Roles = "Admin,Agent")]
    public class BillingController : Controller
    {
        #region Private Zone

        private readonly CrmDbContext _db;

        #endregion

        public BillingController()
        {
            _db = new CrmDbContext();
        }

        [Authorize(Roles = "Admin,Agent")]
        [HttpGet]
        public ActionResult ClientPayment()
        {
            if (User.IsInRole("Admin"))
            {
                ViewBag.BranchList = new SelectList(_db.BranchInfos.ToList(), "Id", "BranchName");            
                ViewBag.ClientList = new SelectList(new List<ClientInfo>());
            }
            else if (User.IsInRole("Agent"))
            {
                var agentId = _db.AgentInfos.First(x => x.UserName == User.Identity.Name).Id;
                ViewBag.ClientList = new SelectList(_db.ClientInfos.Where(x => x.AgentId == agentId).ToList(), "Id", "FirstName");
            }
            return View();
        }

        [Authorize(Roles = "Admin,Agent")]
        [HttpPost]
        public ActionResult ClientPayment(ClientPaymentViewModel payment)
        {
            try
            {
                if (User.IsInRole("Agent"))
                {
                    ModelState.Clear();
                    var branchId = _db.ClientInfos.Where(x => x.Id == payment.CustomerId).Select(x=>x.BranchId).FirstOrDefault() 
                        ?? _db.BranchInfos.Where(x => x.BranchName == "Main").Select(x => x.Id).FirstOrDefault();
                    payment.BranchId = branchId;

                    TryValidateModel(payment);
                }

                if (!ModelState.IsValid) return Json(new { Flag = false, Msg = "Invalid payment info." });
                if (payment.PaymentAmount > payment.DueAmount)
                    return Json(new { Flag = false, Msg = "Payment amount cannot be greater than Due amount."});
                if (payment.PaymentAmount > payment.ServiceAmount)
                    return Json(new { Flag = false, Msg = "Payment amount cannot be greater than Service amount." });
                
                using (var dbTransaction = _db.Database.BeginTransaction())
                {
                    try
                    {
                        var clientPayment = new CustomerPayment
                        {
                            BranchId = payment.BranchId,
                            CustomerId = payment.CustomerId,
                            PaymentDate = payment.PaymentDate,
                            PaymentAmount = payment.PaymentAmount,
                            EntryDate = DateTime.Now
                        };
                        if (User.IsInRole("Admin"))
                        {
                            clientPayment.UserType = UserType.IsAdmin;
                            clientPayment.EntryBy = _db.Users.First(x => x.UserName == User.Identity.Name).Id;
                        }
                        if (User.IsInRole("Agent"))
                        {
                            clientPayment.UserType = UserType.IsAgent;
                            clientPayment.EntryBy = _db.AgentInfos.First(x => x.UserName == User.Identity.Name).Id;
                        }
                        _db.CustomerPayments.Add(clientPayment);
                        _db.SaveChanges();
                        dbTransaction.Commit();
                        return Json(new { Flag = true, Msg = "Payment successfully added." });
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
                List<PaymentViewModel> payments = null;
                // ReSharper disable once InvertIf
                if (_db.CustomerPayments.Any(x => x.CustomerId == customerId))
                {
                    var query = _db.CustomerPayments.Where(x => x.CustomerId == customerId);
                    totalPaid = query.Sum(x => x.PaymentAmount);
                    due = totalServiceCharge - totalPaid;
                    payments = query.Select(x => new PaymentViewModel
                    {
                        PaymnentMadeBy = x.UserType==UserType.IsAgent 
                            ? _db.AgentInfos.Where(a=>a.Id==x.EntryBy).Select(s=>s.AgentName).FirstOrDefault()
                            : _db.Users.Where(u=>u.Id == x.EntryBy).Select(s=>s.UserName).FirstOrDefault(), 
                        PaymentDate= x.PaymentDate, PaymentAmount = x.PaymentAmount
                    }).ToList();
                }
                var data = new { Flag = true, TotalPaid = totalPaid, TotalServiceCharge = totalServiceCharge, Due = due, IsDueExist = due > 0.00,
                                 Payments = payments != null 
                                 ? payments.Select(x => new { x.PaymnentMadeBy, PaymentDate = string.Format("{0:dd/MM/yyyy}", x.PaymentDate), x.PaymentAmount })
                                 : null
                };
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