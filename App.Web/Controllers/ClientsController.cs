using System;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using App.Entity.Models;
using App.Web.Context;
using App.Web.Helper;
using EntityFramework.Extensions;
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
            ViewBag.ServiceList = new SelectList(_db.ServiceInfos.OrderBy(x => x.ServiceName), "Id", "ServiceName");
            return View();
        }

        // POST: Clients/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,BranchId,ReferralType,AgentId,SupplierId,FirstName,LastName,ContactNo,Referral,ReferralContactNo,ServiceId,AirLineId,OldFlightDate,ChangeFlightDate,AirLinePnr,GdsPnr,NewFlightDate,CollageName,CourseName,EmailAddress,ServiceCharge,Cost,Profit,Discount,DoneBy,WorkingStatus,DeliveryStatus,InfoStatus,Remark,VenueFromId,VenueToId,SmsNo,CountryId")] ClientInfo client)
        {
            using (var dbTransaction = _db.Database.BeginTransaction())
            {
                try
                {
                    ModelState.Clear();
                    client.CustomerId = string.Format("{0}{1:000000}{2:MMyy}",
                        _db.BranchInfos.First(x => x.Id == client.BranchId).BranchCode.ToUpper(),
                        _db.ClientInfos.Count() + 1,
                        DateTime.Now
                    );
                    client.EntryBy = _db.Users.First(x => x.UserName == User.Identity.Name).Id;
                    client.ServedBy = _db.Users.First(x => x.UserName == User.Identity.Name).Id;
                    client.WorkingStatus =WorkingStatus.Pending;
                    client.Status = Status.Inactive;
                    client.InfoStatus = InformationUpdate.NotUpdated;
                    client.DeliveryStatus = DeliveryStatus.NotDelivery;
                    client.SmsConfirmation = SmsConfirmation.NotSendingSms;
                    client.EntryDate = DateTime.Now;
                    TryValidateModel(client);

                    if (!ModelState.IsValid) return View(client);
                    _db.ClientInfos.Add(client);
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
                    ViewBag.BranchList = new SelectList(_db.BranchInfos, "Id", "BranchName");
                    ViewBag.ReferralTypes = Common.ToSelectList<ReferralsType>();
                    ViewBag.IsRequireSupplier = Common.ToSelectList<RequireSuppiler>(RequireSuppiler.No);
                    ViewBag.ServiceList = new SelectList(_db.ServiceInfos.OrderBy(x => x.ServiceName), "Id", "ServiceName");
                }
            }
        }

        // GET: Clients/Edit/5
        public ActionResult Edit(int? id)
        {
            try
            {
                if (id == null)
                {
                    TempData["Toastr"] = Toastr.BadRequest;
                    return RedirectToAction("Index");
                }
                var client = _db.ClientInfos.Find(id);
                if (client == null)
                {
                    TempData["Toastr"] = Toastr.HttpNotFound;
                    return RedirectToAction("Index");
                }
                ViewBag.StatusList = Common.ToSelectList<Status>();
                ViewBag.BranchList = new SelectList(_db.BranchInfos, "Id", "BranchName", client.BranchId);
                ViewBag.ReferralTypes = Common.ToSelectList<ReferralsType>(client.ReferralType);
                ViewBag.IsRequireSupplier = Common.ToSelectList<RequireSuppiler>(client.SupplierId == null ? RequireSuppiler.No : RequireSuppiler.Yes);
                ViewBag.ServiceList = new SelectList(_db.ServiceInfos.OrderBy(x => x.ServiceName), "Id", "ServiceName", client.ServiceId);
                ViewBag.WorkingStatusList = Common.ToSelectList<WorkingStatus>(client.WorkingStatus);
                ViewBag.SmsConfirmationList = Common.ToSelectList<SmsConfirmation>(client.SmsConfirmation);
                ViewBag.InfoStatusList = Common.ToSelectList<InformationUpdate>(client.InfoStatus);
                ViewBag.DeliveryStatusList = Common.ToSelectList<DeliveryStatus>(client.DeliveryStatus);
                ViewBag.StatusList = Common.ToSelectList<Status>(client.Status);
                return View(client);
            }
            catch (Exception ex)
            {
                TempData["Toastr"] = Toastr.DbError(ex.Message);
                return RedirectToAction("Index");
            }
        }

        // POST: Clients/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include="Id,BranchId,ReferralType,AgentId,SupplierId,FirstName,LastName,ContactNo,Referral,ReferralContactNo,ServiceId,AirLineId,OldFlightDate,ChangeFlightDate,AirLinePnr,GdsPnr,NewFlightDate,CollageName,CourseName,EmailAddress,ServiceCharge,Cost,Profit,Discount,DoneBy,WorkingStatus,DeliveryStatus,InfoStatus,Remark,Status,VenueFromId,VenueToId,SmsNo,CountryId")] ClientInfo client, int? id)
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
                    if (_db.ClientInfos.Count(x => x.Id == id) < 1)
                    {
                        TempData["Toastr"] = Toastr.HttpNotFound;
                        return RedirectToAction("Index");
                    }
                    var clientInfo = _db.ClientInfos.Single(x => x.Id == id);
                    if (clientInfo == null)
                    {
                        TempData["Toastr"] = Toastr.HttpNotFound;
                        return RedirectToAction("Index");
                    }

                    ModelState.Clear();
                    client.CustomerId = clientInfo.CustomerId;
                    client.EntryBy = clientInfo.EntryBy;
                    client.EntryDate = clientInfo.EntryDate;
                    client.DelStatus = clientInfo.DelStatus;

                    TryValidateModel(client);

                    if (!ModelState.IsValid) return View(client);

                    _db.ClientInfos
                        .Where(x => x.Id == id)
                        .Update(u => new ClientInfo
                        {
                            BranchId = client.BranchId,
                            ReferralType = client.ReferralType,
                            //have to do
                            Status = client.Status
                        });
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
                    ViewBag.StatusList = Common.ToSelectList<Status>(client.Status);
                }
            }
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
        public ActionResult GetAdditionalReferralFields(ReferralsType? referralType, int? id = null)
        {
            var model = id != null ? _db.ClientInfos.Find(id) : new ClientInfo();
            var finalModel = model ?? new ClientInfo();
            switch (referralType)
            {
                case null:
                    return Json(new { }, JsonRequestBehavior.AllowGet);
                case ReferralsType.Referrals:
                    return PartialView("_AdditionalReferralFields", finalModel);
                case ReferralsType.Office:
                    return PartialView("_AdditionalOfficeFields", finalModel);
                case ReferralsType.Agent:
                    return PartialView("_AdditionalAgentFields", finalModel);
                default:
                    return Json(new { }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public ActionResult GetAdditionalSupplierFields(RequireSuppiler? requireSupplier, int? id = null)
        {
            var model = id != null ? _db.ClientInfos.Find(id) : new ClientInfo();
            var finalModel = model ?? new ClientInfo();
            switch (requireSupplier)
            {
                case RequireSuppiler.No:
                    return Json(new { }, JsonRequestBehavior.AllowGet);
                case RequireSuppiler.Yes:
                    return PartialView("_AdditionalSupplierFields", finalModel);
                case null:
                    return Json(new { }, JsonRequestBehavior.AllowGet);
                default:
                    return Json(new { }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public ActionResult GetAdditionalServiceFields(string serviceName, int? serviceId, int? id=null)
        {
            var model = id != null ? _db.ClientInfos.Find(id) : new ClientInfo();
            var finalModel = model ?? new ClientInfo();

            if (serviceId == null || string.IsNullOrWhiteSpace(serviceName)) return Json(new { }, JsonRequestBehavior.AllowGet);
            if (!_db.ServiceInfos.Any(x => x.Id == serviceId && x.ServiceName == serviceName)) return Json(new { }, JsonRequestBehavior.AllowGet);

            switch (serviceName.ToUpper())
            {
                case "VISA CHECK":
                    return PartialView("_AdditionalCountryFilelds", finalModel);
                case "E-MAIL":
                    return PartialView("_AdditionalEmailFilelds", finalModel);
                case "STUDENT VISA":
                    return PartialView("_AdditionalStudentVisaFilelds", finalModel);
                case "TOURIST VISA":
                    return PartialView("_AdditionalCountryFilelds", finalModel);
                case "TKT+MP":
                    return PartialView("_AdditionalTktMp_NewTicketFields", finalModel);
                case "NEW TICKET":
                    return PartialView("_AdditionalTktMp_NewTicketFields", finalModel);
                case "RE-CONFIRM":
                    return PartialView("_AdditionalReConfirmFields", finalModel);
                case "DATE CHANGE":
                    return PartialView("_AdditionalDateChangeFields", finalModel);
                case "CONFIRM":
                    return PartialView("_AdditionalConfirmFields", finalModel);
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
