using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using CanTeenVLU.Models;
using CanTeenVLU.Areas.Admin.Middleware;
using System.Transactions;

namespace CanTeenVLU.Areas.Customer.Controllers
{
    [LoginVerification]
    public class FOODsController : Controller
    {
        private QUANLYCANTEENEntities db = new QUANLYCANTEENEntities();
        private const string PICTURE_PATH = "~/Upload/Foods/";

        // GET: Admin/FOODs
        public ActionResult Index()
        {
            var fOODs = db.FOODs.Include(f => f.CATEGORY);
            return View(fOODs.ToList());
        }
        public ActionResult Picture(int id)
        {
            var path = Server.MapPath(PICTURE_PATH);
            return File(path + id, "images");
        }

        //for customers to view products
        [AllowAnonymous]
        public ActionResult Index2()
        {
            var model = db.FOODs.ToList();
            return View(model);
        }

        // GET: Admin/FOODs/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            FOOD fOOD = db.FOODs.Find(id);
            if (fOOD == null)
            {
                return HttpNotFound();
            }
            return View(fOOD);
        }

        // GET: Admin/FOODs/Create
        public ActionResult Create()
        {
            ViewBag.CATEGORY_ID = new SelectList(db.CATEGORies, "ID", "CATEGORY_CODE");
            return View();
        }

        // POST: Admin/FOODs/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,FOOD_CODE,FOOD_NAME,CATEGORY_ID,DESCRIPTION,PRICE,IMAGE_URL,STATUS")] FOOD fOOD, HttpPostedFileBase picture)
        {
            if (ModelState.IsValid)
            {
                var product = new FOOD();
                if (picture != null)
                {
                    using (var scope = new TransactionScope())
                    {
                        product.ID = fOOD.ID;
                        product.FOOD_CODE = fOOD.FOOD_CODE;
                        product.FOOD_NAME = fOOD.FOOD_NAME;
                        product.CATEGORY_ID = fOOD.CATEGORY_ID;
                        product.DESCRIPTION = fOOD.DESCRIPTION;
                        product.PRICE = fOOD.PRICE;
                        product.STATUS = fOOD.STATUS;
                        db.FOODs.Add(product);
                        db.SaveChanges();

                        var path = Server.MapPath(PICTURE_PATH);
                        picture.SaveAs(path + product.ID);

                        scope.Complete();
                    }
                }
                else ModelState.AddModelError("", "Hình ảnh không được tìm thấy");
            }
            ViewBag.CATEGORY_ID = new SelectList(db.CATEGORies, "ID", "CATEGORY_CODE", fOOD.CATEGORY_ID);
            return RedirectToAction("Index");
        }

        // GET: Admin/FOODs/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            FOOD fOOD = db.FOODs.Find(id);
            if (fOOD == null)
            {
                return HttpNotFound();
            }
            ViewBag.CATEGORY_ID = new SelectList(db.CATEGORies, "ID", "CATEGORY_CODE", fOOD.CATEGORY_ID);
            return View(fOOD);
        }

        // POST: Admin/FOODs/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, [Bind(Include = "ID,FOOD_CODE,FOOD_NAME,CATEGORY_ID,DESCRIPTION,PRICE,IMAGE_URL,STATUS")] FOOD fOOD, HttpPostedFileBase picture)
        {
            if (ModelState.IsValid)
            {
                var product = db.FOODs.Find(id);
                if (ModelState.IsValid)
                {
                    using (var scope = new TransactionScope())
                    {
                        product.ID = fOOD.ID;
                        product.FOOD_CODE = fOOD.FOOD_CODE;
                        product.FOOD_NAME = fOOD.FOOD_NAME;
                        product.CATEGORY_ID = fOOD.CATEGORY_ID;
                        product.DESCRIPTION = fOOD.DESCRIPTION;
                        product.PRICE = fOOD.PRICE;
                        product.STATUS = fOOD.STATUS;

                        db.Entry(product).State = EntityState.Modified;
                        db.SaveChanges();

                        if (picture != null)
                        {
                            var path = Server.MapPath(PICTURE_PATH);
                            picture.SaveAs(path + product.ID);
                        }

                        scope.Complete();
                        return RedirectToAction("Index");

                    }
                }
                db.Entry(fOOD).State = EntityState.Modified;
            }
            ViewBag.CATEGORY_ID = new SelectList(db.CATEGORies, "ID", "CATEGORY_CODE", fOOD.CATEGORY_ID);
            return View(fOOD);
        }

        // GET: Admin/FOODs/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            FOOD fOOD = db.FOODs.Find(id);
            if (fOOD == null)
            {
                return HttpNotFound();
            }
            return View(fOOD);
        }

        // POST: Admin/FOODs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            FOOD fOOD = db.FOODs.Find(id);
            db.FOODs.Remove(fOOD);
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
