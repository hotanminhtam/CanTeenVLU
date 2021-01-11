using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using CanTeenVLU.Models;

namespace CanTeenVLU.Areas.Customer.Controllers
{
    public class ORDER_DETAILController : Controller
    {
        private QUANLYCANTEENEntities db = new QUANLYCANTEENEntities();

        private List<FOOD> ShoppingCart = null;

        private void GetShoppingCart()
        {
            if (Session["ShoppingCart"] != null)
                ShoppingCart = Session["ShoppingCart"] as List<FOOD>;
            else
            {
                ShoppingCart = new List<FOOD>();
                Session["ShoppingCart"] = ShoppingCart;
            }
        }
        // GET: Customer/ORDER_DETAIL
        public ActionResult Index()
        {
            var oRDER_DETAIL = db.ORDER_DETAIL.Include(o => o.MENU).Include(o => o.ORDER);
            return View(oRDER_DETAIL.ToList());
        }

        // GET: Customer/ORDER_DETAIL/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ORDER_DETAIL oRDER_DETAIL = db.ORDER_DETAIL.Find(id);
            if (oRDER_DETAIL == null)
            {
                return HttpNotFound();
            }
            return View(oRDER_DETAIL);
        }

        // GET: Customer/ORDER_DETAIL/Create
        public ActionResult Create()
        {
            ViewBag.MENU_ID = new SelectList(db.MENUs, "ID", "ID");
            ViewBag.ORDER_ID = new SelectList(db.ORDERs, "ID", "ORDER_CODE");
            return View();
        }

        // POST: Customer/ORDER_DETAIL/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,ORDER_ID,MENU_ID,QUANTITY,PRICE")] ORDER_DETAIL oRDER_DETAIL)
        {
            if (ModelState.IsValid)
            {
                db.ORDER_DETAIL.Add(oRDER_DETAIL);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.MENU_ID = new SelectList(db.MENUs, "ID", "ID", oRDER_DETAIL.MENU_ID);
            ViewBag.ORDER_ID = new SelectList(db.ORDERs, "ID", "ORDER_CODE", oRDER_DETAIL.ORDER_ID);
            return View(oRDER_DETAIL);
        }

        // GET: Customer/ORDER_DETAIL/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ORDER_DETAIL oRDER_DETAIL = db.ORDER_DETAIL.Find(id);
            if (oRDER_DETAIL == null)
            {
                return HttpNotFound();
            }
            ViewBag.MENU_ID = new SelectList(db.MENUs, "ID", "ID", oRDER_DETAIL.MENU_ID);
            ViewBag.ORDER_ID = new SelectList(db.ORDERs, "ID", "ORDER_CODE", oRDER_DETAIL.ORDER_ID);
            return View(oRDER_DETAIL);
        }

        // POST: Customer/ORDER_DETAIL/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,ORDER_ID,MENU_ID,QUANTITY,PRICE")] ORDER_DETAIL oRDER_DETAIL)
        {
            if (ModelState.IsValid)
            {
                db.Entry(oRDER_DETAIL).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.MENU_ID = new SelectList(db.MENUs, "ID", "ID", oRDER_DETAIL.MENU_ID);
            ViewBag.ORDER_ID = new SelectList(db.ORDERs, "ID", "ORDER_CODE", oRDER_DETAIL.ORDER_ID);
            return View(oRDER_DETAIL);
        }

        // GET: Customer/ORDER_DETAIL/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ORDER_DETAIL oRDER_DETAIL = db.ORDER_DETAIL.Find(id);
            if (oRDER_DETAIL == null)
            {
                return HttpNotFound();
            }
            return View(oRDER_DETAIL);
        }

        // POST: Customer/ORDER_DETAIL/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            ORDER_DETAIL oRDER_DETAIL = db.ORDER_DETAIL.Find(id);
            db.ORDER_DETAIL.Remove(oRDER_DETAIL);
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
