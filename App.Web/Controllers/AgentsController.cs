using System;
using System.Linq;
using System.Web.Mvc;
using App.Entity.Models;
using App.Web.Context;
using App.Web.Helper;
using App.Web.Models;
using EntityFramework.Extensions;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using EntityState = System.Data.Entity.EntityState;

namespace App.Web.Controllers
{
    [Authorize(Roles = "Admin")]
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
        //public ActionResult Details(int? id)
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

        // GET: Agents/Create
        public ActionResult Create()
        {
            ViewBag.UserRoleList = new SelectList(_context.Roles.Where(u => !u.Name.Contains("Admin")).ToList(), "Name", "Name");
            ViewBag.StatusList = Common.ToSelectList<Status>();
            return View();
        }

        // POST: Agents/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,OfficeName,AgentName,ContactName,MobileNo,Address,OfficeNo,FaxNo,Email,UserName,Password")] AgentInfo agent)
        {
            using (var dbTransaction = _db.Database.BeginTransaction())
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
                            dbTransaction.Rollback();
                            return View(agent);
                        }
                        _userManager.AddToRole(user.Id, "Agent");

                        _db.AgentInfos.Add(agent);
                        _db.SaveChanges();

                        dbTransaction.Commit();
                        TempData["Toastr"] = Toastr.Added;

                        return RedirectToAction("Index");
                    }
                    dbTransaction.Rollback();
                    return View(agent);
                }
                catch (Exception ex)
                {
                    dbTransaction.Rollback();
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

            using (var dbTransaction = _db.Database.BeginTransaction())
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
                    appUser.PasswordHash = Common.HasPassword(agent.Password);
                    _context.Entry(appUser).State = EntityState.Modified;
                    _context.SaveChanges();

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
            using (var dbTransaction = _db.Database.BeginTransaction())
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
