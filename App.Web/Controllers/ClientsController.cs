using System;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using App.Entity.Models;
using App.Web.Context;
using App.Web.Helper;
using EntityState = System.Data.Entity.EntityState;

namespace App.Web.Controllers
{
    [Authorize(Roles = "Admin")]
    public class ClientsController : Controller
    {

        #region Private Zone
        private readonly CrmDbContext _db;
        #endregion

        public ClientsController()
        {
            _db = new CrmDbContext();
        }
        // GET: Clients
        public ActionResult Index()
        {
            return View();
        }

        // GET: Clients/Details/5
        //public ActionResult Details(int? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    ClientInfo clientInfo = _db.ClientInfos.Find(id);
        //    if (clientInfo == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(clientInfo);
        //}

        // GET: Clients/Create
        public ActionResult Create()
        {
            ViewBag.BranchList = new SelectList(_db.BranchInfos, "Id", "BranchName");
            ViewBag.ReferralTypes = Common.ToSelectList<ReferralsType>();
            ViewBag.IsRequireSupplier = Common.ToSelectList<RequireSuppiler>(RequireSuppiler.No);
            ViewBag.ServiceList = new SelectList(_db.ServiceInfos.OrderBy(x=>x.ServiceName), "Id", "ServiceName");
            return View();
        }

        // POST: Clients/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,CustomerId,BranchId,Sn,ReferralType,AgentId,SupplierId,FirstName,LastName,ContactNo,Referral,ReferralContactNo,ServiceId,AirLineId,OldFlightDate,ChangeFlightDate,AirLinePnr,GdsPnr,NewFlightDate,CollageName,CourseName,EmailAddress,ServiceCharge,Cost,Profit,Discount,ServedBy,DoneBy,WorkingStatus,DeliveryStatus,InfoStatus,Remark,Status,DelStatus,EntryBy,EntryDate,VenueFromId,VenueToId,SmsNo,CountryId,Finger,Manpower,TicketIssue,FlightStatus")] ClientInfo clientInfo)
        {
            if (ModelState.IsValid)
            {
                _db.ClientInfos.Add(clientInfo);
                _db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(clientInfo);
        }

        // GET: Clients/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ClientInfo clientInfo = _db.ClientInfos.Find(id);
            if (clientInfo == null)
            {
                return HttpNotFound();
            }
            ViewBag.AgentId = new SelectList(_db.AgentInfos, "Id", "AgentId", clientInfo.AgentId);
            ViewBag.AirLineId = new SelectList(_db.AirLineInfos, "Id", "AirLineId", clientInfo.AirLineId);
            ViewBag.CountryId = new SelectList(_db.CountryLists, "CountryId", "CountryCode", clientInfo.CountryId);
            ViewBag.VenueFromId = new SelectList(_db.SectorInfos, "Id", "SectorId", clientInfo.VenueFromId);
            ViewBag.VenueToId = new SelectList(_db.SectorInfos, "Id", "SectorId", clientInfo.VenueToId);
            ViewBag.ServiceId = new SelectList(_db.ServiceInfos, "Id", "ServiceId", clientInfo.ServiceId);
            ViewBag.SupplierId = new SelectList(_db.SuppliersInfos, "Id", "SupplierId", clientInfo.SupplierId);
            ViewBag.DoneBy = new SelectList(_db.Users, "Id", "Uid", clientInfo.DoneBy);
            ViewBag.EntryBy = new SelectList(_db.Users, "Id", "Uid", clientInfo.EntryBy);
            ViewBag.ServedBy = new SelectList(_db.Users, "Id", "Uid", clientInfo.ServedBy);
            return View(clientInfo);
        }

        // POST: Clients/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,CustomerId,BranchId,Sn,ReferralType,AgentId,SupplierId,FirstName,LastName,ContactNo,Referral,ReferralContactNo,ServiceId,AirLineId,OldFlightDate,ChangeFlightDate,AirLinePnr,GdsPnr,NewFlightDate,CollageName,CourseName,EmailAddress,ServiceCharge,Cost,Profit,Discount,ServedBy,DoneBy,WorkingStatus,DeliveryStatus,InfoStatus,Remark,Status,DelStatus,EntryBy,EntryDate,VenueFromId,VenueToId,SmsNo,CountryId,Finger,Manpower,TicketIssue,FlightStatus")] ClientInfo clientInfo)
        {
            if (ModelState.IsValid)
            {
                _db.Entry(clientInfo).State = EntityState.Modified;
                _db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.AgentId = new SelectList(_db.AgentInfos, "Id", "AgentId", clientInfo.AgentId);
            ViewBag.AirLineId = new SelectList(_db.AirLineInfos, "Id", "AirLineId", clientInfo.AirLineId);
            ViewBag.CountryId = new SelectList(_db.CountryLists, "CountryId", "CountryCode", clientInfo.CountryId);
            ViewBag.VenueFromId = new SelectList(_db.SectorInfos, "Id", "SectorId", clientInfo.VenueFromId);
            ViewBag.VenueToId = new SelectList(_db.SectorInfos, "Id", "SectorId", clientInfo.VenueToId);
            ViewBag.ServiceId = new SelectList(_db.ServiceInfos, "Id", "ServiceId", clientInfo.ServiceId);
            ViewBag.SupplierId = new SelectList(_db.SuppliersInfos, "Id", "SupplierId", clientInfo.SupplierId);
            ViewBag.DoneBy = new SelectList(_db.Users, "Id", "Uid", clientInfo.DoneBy);
            ViewBag.EntryBy = new SelectList(_db.Users, "Id", "Uid", clientInfo.EntryBy);
            ViewBag.ServedBy = new SelectList(_db.Users, "Id", "Uid", clientInfo.ServedBy);
            return View(clientInfo);
        }

        // GET: Clients/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ClientInfo clientInfo = _db.ClientInfos.Find(id);
            if (clientInfo == null)
            {
                return HttpNotFound();
            }
            return View(clientInfo);
        }

        // POST: Clients/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            ClientInfo clientInfo = _db.ClientInfos.Find(id);
            _db.ClientInfos.Remove(clientInfo);
            _db.SaveChanges();
            return RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult GetAdditionalReferralFields(ReferralsType? referralType)
        {
            switch (referralType)
            {
                case null:
                    return Json(new { }, JsonRequestBehavior.AllowGet);
                case ReferralsType.Referrals:
                    return Json(new { error = true, message = this.RenderRazorViewToString("_AdditionalReferralFields",new ClientInfo())});
                case ReferralsType.Office:
                    return Json(new { error = true, message = this.RenderRazorViewToString("_AdditionalOfficeFields", new ClientInfo())});
                case ReferralsType.Agent:
                    return Json(new { error = true, message = this.RenderRazorViewToString("_AdditionalAgentFields", new ClientInfo())});
                default:
                    return Json(new { }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public ActionResult GetAdditionalSupplierFields(RequireSuppiler? requireSupplier)
        {
            switch (requireSupplier)
            {
                case RequireSuppiler.No:
                    return Json(new { }, JsonRequestBehavior.AllowGet);
                case RequireSuppiler.Yes:
                    return Json(new { error = true, message = this.RenderRazorViewToString("_AdditionalSupplierFields", new ClientInfo()) });
                case null:
                    return Json(new { }, JsonRequestBehavior.AllowGet);
                default:
                    return Json(new { }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public ActionResult GetAdditionalServiceFields(int? id, string serviceName)
        {
            if (id == null || string.IsNullOrWhiteSpace(serviceName)) return Json(new { }, JsonRequestBehavior.AllowGet);
            if (!_db.ServiceInfos.Any(x => x.Id == id && x.ServiceName == serviceName)) return Json(new { }, JsonRequestBehavior.AllowGet);

            switch (serviceName.ToUpper())
            {
                case "VISA CHECK":
                    return Json(new { error = true, message = this.RenderRazorViewToString("_AdditionalCountryFilelds", new ClientInfo()) });
                case "E-MAIL":
                    return Json(new { error = true, message = this.RenderRazorViewToString("_AdditionalEmailFilelds", new ClientInfo()) });
                case "STUDENT VISA": 
                    return Json(new { error = true, message = this.RenderRazorViewToString("_AdditionalStudentVisaFilelds", new ClientInfo()) });
                case "TOURIST VISA":
                    return Json(new { error = true, message = this.RenderRazorViewToString("_AdditionalCountryFilelds", new ClientInfo()) });
                case "TKT+MP":
                    return Json(new { error = true, message = this.RenderRazorViewToString("_AdditionalTktMp_NewTicketFields", new ClientInfo()) });
                case "NEW TICKET":
                    return Json(new { error = true, message = this.RenderRazorViewToString("_AdditionalTktMp_NewTicketFields", new ClientInfo()) });
                case "RE-CONFIRM":
                    return Json(new { error = true, message = this.RenderRazorViewToString("_AdditionalReConfirmFields", new ClientInfo()) });
                case "DATE CHANGE":
                    return Json(new { error = true, message = this.RenderRazorViewToString("_AdditionalDateChangeFields", new ClientInfo()) });
                case "CONFIRM":
                    return Json(new { error = true, message = this.RenderRazorViewToString("_AdditionalConfirmFields", new ClientInfo()) });
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
