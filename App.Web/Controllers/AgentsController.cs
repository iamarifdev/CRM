using System;
using System.Data.Entity;
using System.Linq;
using System.Transactions;
using System.Web.Mvc;
using App.Entity.Models;
using App.Web.Context;
using App.Web.Helper;
using App.Web.Models;
using EntityFramework.Extensions;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace App.Web.Controllers
{
    [CrmAuthorize(Roles = "Admin")]
    public class AgentsController : Controller
    {
        #region Private Zone
        private readonly CrmDbContext _db;
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        #endregion

        public AgentsController()
        {
            _db = new CrmDbContext();
            _context = new ApplicationDbContext();
            _userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(_context));
        }

        // GET: Agents
        public ActionResult Index()
        {
            return View();
        }

        // GET: Agents/Details/5
        public ActionResult Details(int? id)
        {
            try
            {
                if (id == null)
                {
                    TempData["Toastr"] = Toastr.BadRequest;
                    return RedirectToAction("Index");
                }
                var agentInfo = _db.AgentInfos.Find(id);
                if (agentInfo == null)
                {
                    return HttpNotFound();
                }
                return View(agentInfo);
            }
            catch (Exception ex)
            {
                TempData["Toastr"] = Toastr.DbError(ex.Message);
                return RedirectToAction("Index");
            }
            
        }

        // GET: Agents/Create
        public ActionResult Create()
        {
            try
            {
                ViewBag.UserRoleList = new SelectList(_context.Roles.Where(u => !u.Name.Contains("Admin")).ToList(), "Name", "Name");
                ViewBag.StatusList = Common.ToSelectList<Status>();
                return View();
            }
            catch (Exception ex)
            {
                TempData["Toastr"] = Toastr.DbError(ex.Message);
                return RedirectToAction("Index");
            }
            
        }

        // POST: Agents/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,OfficeName,AgentName,ContactName,MobileNo,Address,OfficeNo,FaxNo,Email,UserName,Password")] AgentInfo agent)
        {
            using (var transaction = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                try
                {
                    ModelState.Clear();
                    agent.AgentId = string.Format("AI-{0:000000}", _db.AgentInfos.Count() + 1);
                    agent.EntryBy = _db.Users.First(x => x.UserName == User.Identity.Name).Id;
                    agent.EntryDate = DateTime.Now;
                    TryValidateModel(agent);
                    if (ModelState.IsValid)
                    {
                        var user = new ApplicationUser {Email = agent.Email, UserName = agent.UserName};
                        var ack = _userManager.Create(user, agent.Password);
                        if (!ack.Succeeded)
                        {
                            transaction.Complete();
                            return View(agent);
                        }
                        _userManager.AddToRole(user.Id, "Agent");

                        _db.AgentInfos.Add(agent);
                        _db.SaveChanges();

                        transaction.Complete();
                        TempData["Toastr"] = Toastr.Added;

                        return RedirectToAction("Index");
                    }
                    Transaction.Current.Rollback();
                    return View(agent);
                }
                catch (Exception ex)
                {
                    Transaction.Current.Rollback();
                    TempData["Toastr"] = Toastr.DbError(ex.Message);
                    return RedirectToAction("Index");
                }
                finally
                {
                    ViewBag.StatusList = Common.ToSelectList<Status>();
                }

            }
        }

        // GET: Agents/Edit/5
        public ActionResult Edit(int? id)
        {
            try
            {
                if (id == null)
                {
                    TempData["Toastr"] = Toastr.BadRequest;
                    return RedirectToAction("Index");
                }
                var agent = _db.AgentInfos.Find(id);
                if (agent == null)
                {
                    TempData["Toastr"] = Toastr.HttpNotFound;
                    return RedirectToAction("Index");
                }
                ViewBag.StatusList = Common.ToSelectList<Status>(agent.Status);

                return View(agent);
            }
            catch (Exception ex)
            {
                TempData["Toastr"] = Toastr.DbError(ex.Message);
                return RedirectToAction("Index");
            }
        }

        // POST: Agents/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,OfficeName,AgentName,ContactName,MobileNo,Address,OfficeNo,FaxNo,Email,UserName,Password,Status")] AgentInfo agent, int? id)
        {

            using (var transaction = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                try
                {
                    if (id == null)
                    {
                        TempData["Toastr"] = Toastr.HttpNotFound;
                        return RedirectToAction("Index");
                    }
                    if (_db.AgentInfos.Count(x => x.Id == id) < 1)
                    {
                        TempData["Toastr"] = Toastr.HttpNotFound;
                        return RedirectToAction("Index");
                    }
                    var agentInfo = _db.AgentInfos.Single(x => x.Id == id);
                    if (agentInfo == null)
                    {
                        TempData["Toastr"] = Toastr.HttpNotFound;
                        return RedirectToAction("Index");
                    }

                    ModelState.Clear();
                    agent.AgentId = agentInfo.AgentId;
                    agent.EntryBy = agentInfo.EntryBy;
                    agent.EntryDate = agentInfo.EntryDate;
                    agent.DelStatus = agentInfo.DelStatus;

                    TryValidateModel(agent);

                    if (!ModelState.IsValid) return View(agent);
                    
                    _db.AgentInfos
                        .Where(x => x.Id == id)
                        .Update(u => new AgentInfo
                        {
                            OfficeName = agent.OfficeName,
                            AgentName = agent.AgentName,
                            ContactName = agent.ContactName,
                            MobileNo = agent.MobileNo,
                            Address = agent.Address,
                            OfficeNo = agent.OfficeNo,
                            FaxNo = agent.FaxNo,
                            Email = agent.Email,
                            UserName = agent.UserName,
                            Password = agent.Password,
                            Status = agent.Status
                        });

                    // ReSharper disable once PossibleNullReferenceException
                    var appUser = _userManager.FindByName(_db.AgentInfos.Find(id).UserName);
                    appUser.UserName = agent.UserName;
                    appUser.Email = agent.Email;
                    appUser.PasswordHash = Common.HashPassword(agent.Password);
                    _context.Entry(appUser).State = EntityState.Modified;
                    _context.SaveChanges();

                    transaction.Complete();

                    TempData["Toastr"] = Toastr.Updated;
                    return RedirectToAction("Index");
                }
                catch (Exception ex)
                {
                    Transaction.Current.Rollback();
                    TempData["Toastr"] = Toastr.DbError(ex.Message);
                    return RedirectToAction("Index");
                }
                finally
                {
                    ViewBag.StatusList = Common.ToSelectList<Status>(agent.Status);
                }
            }
        }

        //// GET: Agents/Delete/5
        //public ActionResult Delete(int? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    AgentInfo agentInfo = _db.AgentInfos.Find(id);
        //    if (agentInfo == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(agentInfo);
        //}

        // POST: Agents/Delete/5
        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int? id)
        {
            using (var transaction = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                try
                {
                    if (id == null)
                    {
                        TempData["Toastr"] = Toastr.BadRequest;
                        return RedirectToAction("Index");
                    }
                    var agent = _db.AgentInfos.Find(id);
                    if (agent == null)
                    {
                        TempData["Toastr"] = Toastr.HttpNotFound;
                        return RedirectToAction("Index");
                    }

                    var appUser = _userManager.FindByName(agent.UserName);
                    _context.Users.Remove(appUser);
                    _context.SaveChanges();

                    _db.AgentInfos.Remove(agent);
                    _db.SaveChanges();

                    transaction.Complete();

                    TempData["Toastr"] = Toastr.Deleted;
                    return RedirectToAction("Index");
                }
                catch (Exception ex)
                {
                    Transaction.Current.Rollback();
                    TempData["Toastr"] = Toastr.DbError(ex.Message);
                    return RedirectToAction("Index");
                }
            }
        }

        [HttpPost]
        public JsonResult IsEmailAvailable(string email, int? id)
        {
            try
            {
                var flag = true;
                //create mode
                if (id == null)
                {
                    flag = !_db.Users.Any(x => x.Email == email);
                    if (flag) flag = !_db.AgentInfos.Any(x => x.Email == email);
                }
                // edit mode
                else
                {
                    var agent = _db.AgentInfos.Find(id);
                    if (agent == null) return Json(false, JsonRequestBehavior.AllowGet);
                    if (agent.Email != email)
                    {
                        flag = !_db.Users.Any(x => x.Email == email);
                        if (flag) flag = !_db.AgentInfos.Any(x => x.Email == email);
                    }
                }
                return Json(flag, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return Json(false, JsonRequestBehavior.AllowGet);
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
