using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using CanTeenVLU.Areas.Admin.Middleware;
using CanTeenVLU.Models;

namespace CanTeenVLU.Areas.Admin.Controllers
{
    [LoginVerification]
    public class ORDERsController : Controller
    {
        private QUANLYCANTEENEntities db = new QUANLYCANTEENEntities();

        // GET: Admin/ORDERs
        public ActionResult Index()
        {
            var oRDERs = db.ORDERs.Include(o => o.ACCOUNT).Include(o => o.CUSTOMER);
            return View(oRDERs.ToList());
        }

        // GET: Admin/ORDERs/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ORDER oRDER = db.ORDERs.Find(id);
            if (oRDER == null)
            {
                return HttpNotFound();
            }
            return View(oRDER);
        }

        // GET: Admin/ORDERs/Create
        public ActionResult Create()
        {
            ViewBag.ACCOUNT_ID = new SelectList(db.ACCOUNTs, "ID", "EMAIL");
            ViewBag.CUSTOMER_ID = new SelectList(db.CUSTOMERs, "ID", "EMAIL");
            return View();
        }

        // POST: Admin/ORDERs/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,ORDER_CODE,DATE,ACCOUNT_ID,CUSTOMER_ID,STATUS,FEEDBACK")] ORDER oRDER)
        {
            if (ModelState.IsValid)
            {
                db.ORDERs.Add(oRDER);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.ACCOUNT_ID = new SelectList(db.ACCOUNTs, "ID", "EMAIL", oRDER.ACCOUNT_ID);
            ViewBag.CUSTOMER_ID = new SelectList(db.CUSTOMERs, "ID", "EMAIL", oRDER.CUSTOMER_ID);
            return View(oRDER);
        }

        // GET: Admin/ORDERs/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ORDER oRDER = db.ORDERs.Find(id);
            if (oRDER == null)
            {
                return HttpNotFound();
            }
            ViewBag.ACCOUNT_ID = new SelectList(db.ACCOUNTs, "ID", "EMAIL", oRDER.ACCOUNT_ID);
            ViewBag.CUSTOMER_ID = new SelectList(db.CUSTOMERs, "ID", "EMAIL", oRDER.CUSTOMER_ID);
            return View(oRDER);
        }

        // POST: Admin/ORDERs/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,ORDER_CODE,DATE,ACCOUNT_ID,CUSTOMER_ID,STATUS,FEEDBACK")] ORDER oRDER)
        {
            if (ModelState.IsValid)
            {
                db.Entry(oRDER).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.ACCOUNT_ID = new SelectList(db.ACCOUNTs, "ID", "EMAIL", oRDER.ACCOUNT_ID);
            ViewBag.CUSTOMER_ID = new SelectList(db.CUSTOMERs, "ID", "EMAIL", oRDER.CUSTOMER_ID);
            return View(oRDER);
        }

        // GET: Admin/ORDERs/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ORDER oRDER = db.ORDERs.Find(id);
            if (oRDER == null)
            {
                return HttpNotFound();
            }
            return View(oRDER);
        }

        // POST: Admin/ORDERs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            ORDER oRDER = db.ORDERs.Find(id);
            db.ORDERs.Remove(oRDER);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
