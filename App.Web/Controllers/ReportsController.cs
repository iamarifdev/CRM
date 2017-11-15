using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Core.Objects;
using System.Linq;
using System.Web.Mvc;
using App.Entity.Models;
using App.Web.Context;
using App.Web.Helper;
using App.Web.Models;
using Microsoft.Ajax.Utilities;

namespace App.Web.Controllers
{
    [Authorize(Roles = "Admin")]
    public class ReportsController : Controller
    {
        #region Private Zone

        private readonly CrmDbContext _db;

        #endregion

        public ReportsController()
        {
            _db = new CrmDbContext();
        }
        // GET: ClientInfoReport
        [HttpGet]
        public ActionResult ClientInfoReport()
        {
            try
            {
                ViewBag.Clients = new SelectList(_db.ClientInfos.ToList(), "Id", "FirstName");
                ViewBag.Branches = new SelectList(_db.BranchInfos.ToList(), "Id", "BranchName");
                ViewBag.Users = new SelectList(_db.Users.ToList(), "Id", "UserName");
                ViewBag.Services = new SelectList(_db.ServiceInfos.ToList(), "Id", "ServiceName");
                ViewBag.Airlines = new SelectList(_db.AirLineInfos.ToList(), "Id", "AirLineName");
                ViewBag.Agents = new SelectList(_db.AgentInfos.ToList(), "Id", "AgentName");
                ViewBag.WorkingStatusList = Common.ToSelectList<WorkingStatus>();
                ViewBag.InfoStatusList = Common.ToSelectList<InformationUpdate>();
                ViewBag.DeliveryStatusList = Common.ToSelectList<DeliveryStatus>();
                return View();
            }
            catch (Exception ex)
            {
                TempData["Toastr"] = Toastr.DbError(ex.Message);
                return RedirectToAction("Index", "Home");
            }
        }

        // POST: ClientInfoReport
        [HttpPost]
        public ActionResult ClientInfoReport(ClientInfoViewModel clientInfo)
        {
            try
            {
                if (!ModelState.IsValid) return View(clientInfo);

                var query = _db.ClientInfos.Include(x => x.ServiceInfo).Include(x => x.AgentInfo).Include(x => x.UserServedBy).Where(x => x.Id == clientInfo.Id);
                if (clientInfo.BranchId != null) query = query.Where(x => x.BranchId == clientInfo.BranchId);
                if (clientInfo.ServedBy != null) query = query.Where(x => x.ServedBy == clientInfo.ServedBy);
                if (clientInfo.ServiceId != null) query = query.Where(x => x.ServiceId == clientInfo.ServiceId);
                if (clientInfo.AirLineId != null) query = query.Where(x => x.AirLineId == clientInfo.AirLineId);
                if (clientInfo.AgentId != null) query = query.Where(x => x.AgentId == clientInfo.AgentId);
                if (clientInfo.WorkingStatus != null) query = query.Where(x => x.WorkingStatus == clientInfo.WorkingStatus);
                if (clientInfo.InfoStatus != null) query = query.Where(x => x.InfoStatus == clientInfo.InfoStatus);
                if (clientInfo.DeliveryStatus != null) query = query.Where(x => x.DeliveryStatus == clientInfo.DeliveryStatus);

                if (clientInfo.FromDate != null && clientInfo.ToDate != null) query = query.Where(x => x.EntryDate >= clientInfo.FromDate && x.EntryDate <= clientInfo.ToDate);
                else if (clientInfo.FromDate != null && clientInfo.ToDate == null) query = query.Where(x => x.EntryDate >= clientInfo.FromDate);
                else if (clientInfo.FromDate == null && clientInfo.ToDate != null) query = query.Where(x => x.EntryDate <= clientInfo.ToDate);

                var data = query.ToList();
                var totalServiceCharge = data.Sum(x => x.ServiceCharge);
                var totalCost = data.Sum(x => x.Cost);
                var totalProfit = data.Sum(x => x.Profit);
                var clientReports = data.Select(x => new
                {
                    EntryDate = string.Format("{0:dd/MM/yyyy}", x.EntryDate),
                    CID = x.CustomerId,
                    PaxName = x.LastName != null ? string.Format("{0} {1}", x.FirstName, x.LastName) : x.FirstName,
                    Referral = x.Referral ?? "",
                    Service = x.ServiceInfo != null ? x.ServiceInfo.ServiceName : "",
                    Airline = x.AirLineInfo != null ? x.AirLineInfo.AirLineName : "",
                    AirLinePnr = x.AirLinePnr ?? "",
                    x.ServiceCharge,
                    x.Cost,
                    x.Profit,
                    ServedBy = x.UserServedBy.UserName,
                    WorkingStatus = Common.GetDescription(x.WorkingStatus),
                    InfoStatus = Common.GetDescription(x.InfoStatus),
                    DeliveryStatus = Common.GetDescription(x.DeliveryStatus)
                }).ToList();
                return Json(new { Flag = true, ClientReports = clientReports, ServiceCharge = totalServiceCharge, Cost = totalCost, Profit = totalProfit });

            }
            catch (Exception ex)
            {
                return Json(new { Flag = false, Msg = ex.Message });
            }

        }

        // GET: ClientPaymentReport
        [HttpGet]
        public ActionResult ClientPaymentReport()
        {
            try
            {
                ViewBag.Branches = new SelectList(_db.BranchInfos.ToList(), "Id", "BranchName");
                return View();
            }
            catch (Exception ex)
            {
                TempData["Toastr"] = Toastr.DbError(ex.Message);
                return RedirectToAction("Index", "Home");
            }
        }

        // POST: ClientPaymentReport
        [HttpPost]
        public ActionResult ClientPaymentReport(ClientPaymentReportViewModel clientPayment)
        {
            try
            {
                if (!ModelState.IsValid) return Json(new { Flag = false, Msg = "Invalid Data." }, JsonRequestBehavior.AllowGet);
                var data = _db.CustomerPayments
                    .Where(x => x.BranchId == clientPayment.BranchId && x.CustomerId == clientPayment.CustomerId)
                    .Select(x => new { x.PaymentDate, x.PaymentAmount }).ToList();
                return Json(new
                {
                    Flag = true,
                    ClientPayments = data.OrderByDescending(x => x.PaymentDate)
                    .Select(x => new { PaymentDate = string.Format("{0:dd/MM/yyyy}", x.PaymentDate), x.PaymentAmount }).ToList()
                });
            }
            catch (Exception ex)
            {
                return Json(new { Flag = false, Msg = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        // GET: ClientDueReport
        [HttpGet]
        public ActionResult ClientDueReport()
        {
            return View();
        }

        // POST: ClientPaymentReport
        [HttpPost]
        public ActionResult ClientDueReport(ClientDueReportViewModel model)
        {
            try
            {
                if (!ModelState.IsValid) return Json(new { Flag = false, Msg = "Invalid Data." }, JsonRequestBehavior.AllowGet);
                var data = _db.ClientInfos.Include(x => x.ServiceInfo)
                    .Join(_db.CustomerPayments, x => x.Id, y => y.CustomerId, (x, y) => new { x, y })
                    .Where(w => DbFunctions.TruncateTime(w.x.EntryDate) >= model.FromDate.Date && DbFunctions.TruncateTime(w.x.EntryDate) <= model.ToDate.Date)
                    .GroupBy(g => new
                    {
                        g.x.EntryDate,
                        g.x.CustomerId,
                        g.x.FullName,
                        g.x.ContactNo,
                        g.x.Referral,
                        g.x.ServiceInfo.ServiceName,
                        g.x.ServiceCharge
                    })
                    .Select(s => new
                    {
                        s.Key.EntryDate,
                        s.Key.CustomerId,
                        s.Key.FullName,
                        s.Key.ContactNo,
                        s.Key.Referral,
                        s.Key.ServiceName,
                        s.Key.ServiceCharge,
                        Paid = s.Sum(x => x.y.PaymentAmount),
                        Due = s.Key.ServiceCharge - s.Sum(x => x.y.PaymentAmount)
                    }).ToList();
                var clientDues = data.OrderByDescending(x => x.EntryDate).Select(x => new
                {
                    Date = string.Format("{0:yyyy-MM-dd}", x.EntryDate),
                    CID = x.CustomerId,
                    Name = x.FullName,
                    Referral = x.Referral ?? "",
                    ContactNo = x.ContactNo ?? "",
                    Service = x.ServiceName,
                    x.ServiceCharge,
                    x.Paid,
                    x.Due
                }).ToList();
                return Json(new
                {
                    Flag = true,
                    ClientDues = clientDues,
                    ServiceCharge = data.Sum(x => x.ServiceCharge),
                    Paid = data.Sum(x => x.Paid),
                    Due = data.Sum(x => x.Due)
                });
            }
            catch (Exception ex)
            {
                return Json(new { Flag = false, Msg = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        // GET: AgentInfoReport
        [HttpGet]
        public ActionResult AgentInfoReport()
        {
            try
            {
                ViewBag.Agents = new SelectList(_db.AgentInfos.ToList(), "Id", "AgentName");
                return View();
            }
            catch (Exception ex)
            {
                TempData["Toastr"] = Toastr.DbError(ex.Message);
                return RedirectToAction("Index", "Home");
            }
            
        }

        // POST: AgentInfoReport
        [HttpPost]
        public ActionResult AgentInfoReport(int? agentId)
        {
            try
            {
                var query = _db.AgentInfos.Select(x=> new
                {
                    x.Id,
                    x.AgentId,
                    x.OfficeName,
                    x.AgentName,
                    ContactName = x.ContactName??"",
                    MobileNo = x.MobileNo ?? "",
                    x.Email,
                    x.UserName,
                    Channel = x.Channel ?? "System"
                }).OrderBy(x=>x.AgentId);
                var agentsReport = agentId != null ? query.Where(x => x.Id == agentId).ToList() : query.ToList();
                return Json(new {Flag = true, AgentsReports = agentsReport});
            }
            catch (Exception ex)
            {
                return Json(new { Flag = false, Msg = ex.Message }, JsonRequestBehavior.AllowGet);
            }

        }

        // GET: AgentPaymentReport
        [HttpGet]
        public ActionResult AgentPaymentReport()
        {
            try
            {
                ViewBag.Agents = new SelectList(_db.AgentInfos.ToList(), "Id", "AgentName");
                return View();
            }
            catch (Exception ex)
            {
                TempData["Toastr"] = Toastr.DbError(ex.Message);
                return RedirectToAction("Index", "Home");
            }

        }

        // POST: AgentPaymentReport
        [HttpPost]
        public ActionResult AgentPaymentReport(int? agentId)
        {
            try
            {
                var query = _db.CustomerPayments.Select(x => new
                {
                    x.PaymentDate,
                    x.PaymentAmount,
                    x.CustomerId,
                    x.Channel
                }).OrderByDescending(x => x.PaymentDate);
                var data = agentId != null 
                    ? query.Where(x => x.CustomerId == agentId && x.Channel == Channel.IsAgent).ToList()
                    : query.Where(x=> x.Channel == Channel.IsAgent).ToList();
                var serviceAmount = agentId != null 
                    ? _db.ClientInfos.Where(x=>x.AgentId == agentId).Sum(x=>x.ServiceCharge)
                    : _db.ClientInfos.Where(x => x.AgentId != null).Sum(x => x.ServiceCharge);
                return Json(new { Flag = true,
                    AgentsPayments = agentId != null ? data.Select(x => new { PaymentDate = string.Format("{0:yyyy-MM-dd}", x.PaymentDate),x.PaymentAmount }).ToList() : null,
                    TotalPaidAmount = data.Sum(x=>x.PaymentAmount),
                    TotalDueAmount = serviceAmount - data.Sum(x=>x.PaymentAmount),
                    TotalServiceAmount = serviceAmount
                });
            }
            catch (Exception ex)
            {
                return Json(new { Flag = false, Msg = ex.Message }, JsonRequestBehavior.AllowGet);
            }

        }

        public JsonResult GetClientsByBranchId(int? branchId)
        {
            try
            {
                if (branchId == null) return Json(null, JsonRequestBehavior.AllowGet);
                var clients = new SelectList(_db.ClientInfos.Where(x => x.BranchId == branchId).ToList(), "Id", "FirstName");
                return Json(new { Clients = clients }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(null, JsonRequestBehavior.AllowGet);
            }
        }
    }
}