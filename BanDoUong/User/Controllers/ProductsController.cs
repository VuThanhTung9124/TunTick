using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using BanDoUong.Models;

namespace BanDoUong.Controllers
{
    public class ProductsController : Controller
    {
        private DoUongDb db = new DoUongDb();

        // GET: Products
        public ActionResult Index()
        {
            return View(db.Products.ToList());
            ViewBag.ThongBaoEmail = "Cảm ơn bạn đã gửi email cho chúng tôi!!!";
        }

        // GET: Products/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product product = db.Products.Find(id);
            if (product == null)
            {
                return HttpNotFound();
            }

            


            return View(product);
        }

        [HttpGet]
        public ActionResult ThanhToan()
        {
            return View();
        }

        [HttpPost]
        public ActionResult ThanhToan(string name, string price)
        {
            ViewBag.Name = name;
            ViewBag.Price = price;
            return View();
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
