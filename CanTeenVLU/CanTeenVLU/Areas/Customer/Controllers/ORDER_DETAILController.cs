﻿using System;
using System.Collections;
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

        private List<MENU> ShoppingCart = null;

        private void GetShoppingCart()
        {
            if (Session["ShoppingCart"] != null)
                ShoppingCart = Session["ShoppingCart"] as List<MENU>;
            else
            {
                ShoppingCart = new List<MENU>();
                Session["ShoppingCart"] = ShoppingCart;
            }
        }
        // GET: ShoppingCart
        public ActionResult Index()
        {
            GetShoppingCart();
            var hashtable = new Hashtable();
            foreach (var billDetail in ShoppingCart)
            {
                if (hashtable[billDetail.FOOD.ID] != null)
                {
                    (hashtable[billDetail.FOOD.ID] as MENU).QUANTITY += billDetail.QUANTITY;
                }
                else hashtable[billDetail.FOOD.ID] = billDetail;
            }

            ShoppingCart.Clear();
            foreach (MENU billDetail in hashtable.Values)
                ShoppingCart.Add(billDetail);
            return View(ShoppingCart);
        }

        // GET: ShoppingCart/Create
        public ActionResult Create(int productId, int quantity)
        {
            GetShoppingCart();
            var product = db.FOODs.Find(productId);
            ShoppingCart.Add(new MENU
            {
                FOOD = product,
                QUANTITY = quantity
            });

            return RedirectToAction("Index");
        }

        // GET: ShoppingCart/Edit/5
        [HttpPost]
        public ActionResult Edit(int[] product_id, int[] quantity)
        {
            GetShoppingCart();
            ShoppingCart.Clear();
            if (product_id != null)
                for (int i = 0; i < product_id.Length; i++)
                    if (quantity[i] > 0)
                    {
                        var product = db.FOODs.Find(product_id[i]);
                        ShoppingCart.Add(new MENU
                        {
                            FOOD = product,
                            QUANTITY = quantity[i]
                        });
                    }
            Session["ShoppingCart"] = ShoppingCart;

            return RedirectToAction("Index");
        }

        // GET: ShoppingCart/Delete/5
        public ActionResult Delete()
        {
            GetShoppingCart();
            ShoppingCart.Clear();
            Session["ShoppingCart"] = ShoppingCart;
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
