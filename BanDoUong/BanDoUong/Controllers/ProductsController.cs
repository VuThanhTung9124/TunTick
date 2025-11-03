using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using BanDoUong.Models;
using System.Collections; // nếu bạn muốn dùng ArrayList
using System.Collections.Generic; // nếu dùng List<>

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

        public ActionResult GioHang(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("Index");
            }

            // Tìm sản phẩm theo id
            Product product = db.Products.Find(id);
            if (product == null)
            {
                return HttpNotFound();
            }

            // Lấy giỏ hàng hiện có trong session (nếu có)
            List<CartItem> cart = Session["Cart"] as List<CartItem>;
            if (cart == null)
            {
                cart = new List<CartItem>();
            }

            // Kiểm tra xem sản phẩm đã có trong giỏ chưa
            CartItem existing = cart.FirstOrDefault(p => p.ProductId == product.product_id);
            if (existing != null)
            {
                existing.Quantity++;
            }
            else
            {
                CartItem item = new CartItem
                {
                    ProductId = product.product_id,
                    Name = product.name,
                    Thumbnail = product.thumbnail,
                    Price = product.price,
                    Quantity = 1
                };
                cart.Add(item);
            }

            // Cập nhật lại session
            Session["Cart"] = cart;

            // Sau khi thêm, chuyển hướng sang trang giỏ hàng
            return RedirectToAction("XemGioHang");
        }

        public ActionResult XemGioHang()
        {
            List<CartItem> cart = Session["Cart"] as List<CartItem>;
            if (cart == null)
            {
                cart = new List<CartItem>();
            }
            return View(cart);
        }

        public ActionResult XoaKhoiGio(int id)
        {
            List<CartItem> cart = Session["Cart"] as List<CartItem>;
            if (cart != null)
            {
                var item = cart.FirstOrDefault(x => x.ProductId == id);
                if (item != null)
                {
                    cart.Remove(item);
                }
                Session["Cart"] = cart;
            }
            return RedirectToAction("XemGioHang");
        }
        [HttpPost]
        public JsonResult CapNhatSoLuong(int productId, int quantity)
        {
            var cart = Session["cart"] as List<CartItem>;
            if (cart != null)
            {
                var item = cart.FirstOrDefault(p => p.ProductId == productId);
                if (item != null)
                {
                    item.Quantity = quantity;
                }
                Session["cart"] = cart;
            }
            return Json(new { success = true });
        }


        [HttpGet]
        public ActionResult ThanhToan(string ids)
        {
            // Kiểm tra xem có danh sách id sản phẩm không
            if (string.IsNullOrEmpty(ids))
            {
                return RedirectToAction("XemGioHang");
            }

            var cart = Session["Cart"] as List<CartItem>;
            if (cart == null)
            {
                return RedirectToAction("XemGioHang");
            }

            // Chuyển "1,3,5" thành danh sách int {1,3,5}
            var idList = ids.Split(',').Select(int.Parse).ToList();

            // Lọc các sản phẩm được chọn
            var selectedProducts = cart.Where(x => idList.Contains(x.ProductId)).ToList();

            if (!selectedProducts.Any())
            {
                return RedirectToAction("XemGioHang");
            }

            // Tính tổng
            decimal tongTien = selectedProducts.Sum(p => p.Price * p.Quantity);
            ViewBag.Total = tongTien;

            // Trả về View cùng danh sách sản phẩm đã chọn
            return View(selectedProducts);
        }



        [HttpPost]
        public ActionResult ThanhToan(List<CartItem> SelectedProducts)
        {
            if (SelectedProducts == null || !SelectedProducts.Any())
            {
                return RedirectToAction("Cart");
            }

            decimal tongTien = SelectedProducts.Sum(p => p.Price * p.Quantity);

            ViewBag.Total = tongTien;
            return View(SelectedProducts);
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
