using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using App.Entity.Models;
using App.Web.Context;
using DataTables.AspNet.Core;
using DataTables.AspNet.Mvc5;
using EntityState = System.Data.Entity.EntityState;

namespace App.Web.Controllers
{
    public class BranchController : Controller
    {
        private readonly CrmDbContext _db;

        public BranchController()
        {
            _db = new CrmDbContext();
        }

        // GET: Branch
        public ActionResult Index()
        {
            return View(_db.BranchInfos.ToList());
        }

        // GET: Branch/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            BranchInfo branchInfo = _db.BranchInfos.Find(id);
            if (branchInfo == null)
            {
                return HttpNotFound();
            }
            return View(branchInfo);
        }

        // GET: Branch/Create
        public ActionResult Create()
        {
            ViewBag.Status = new SelectList(Enum.GetValues(typeof(Status)).Cast<Status>()
                            .Select(v => new SelectListItem { Text = v.ToString(), Value = ((int)v).ToString() }).ToList(), "Value", "Text");
            return View();
        }

        // POST: Branch/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "BranchName,BranchCode,Status")] BranchInfo branchInfo)
        {
            if (ModelState.IsValid)
            {
                _db.BranchInfos.Add(branchInfo);
                _db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.Status = new SelectList(Enum.GetValues(typeof(Status)).Cast<Status>()
                            .Select(v => new SelectListItem { Text = v.ToString(), Value = ((int)v).ToString() }).ToList(), "Value", "Text");
            return View(branchInfo);
        }

        // GET: Branch/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            BranchInfo branchInfo = _db.BranchInfos.Find(id);
            if (branchInfo == null)
            {
                return HttpNotFound();
            }
            return View(branchInfo);
        }

        // POST: Branch/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,BranchId,BranchName,BranchCode,Status,DelStatus,EntryDate,EntryBy")] BranchInfo branchInfo)
        {
            if (ModelState.IsValid)
            {
                _db.Entry(branchInfo).State = EntityState.Modified;
                _db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(branchInfo);
        }

        // GET: Branch/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            BranchInfo branchInfo = _db.BranchInfos.Find(id);
            if (branchInfo == null)
            {
                return HttpNotFound();
            }
            return View(branchInfo);
        }

        // POST: Branch/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            BranchInfo branchInfo = _db.BranchInfos.Find(id);
            _db.BranchInfos.Remove(branchInfo);
            _db.SaveChanges();
            return RedirectToAction("Index");
        }

        [HttpGet]
        public ActionResult GetBranch(IDataTablesRequest request)
        {
            var data =_db.BranchInfos;

            var filteredData = data.Where(item => item.BranchName.Contains(request.Search.Value));

            var dataPage = filteredData.Skip(request.Start).Take(request.Length);

            var response = DataTablesResponse.Create(request, data.Count(), filteredData.Count(), dataPage);

            return new DataTablesJsonResult(response, JsonRequestBehavior.AllowGet);
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
