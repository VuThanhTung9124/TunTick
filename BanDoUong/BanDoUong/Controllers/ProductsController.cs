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
            List<Product> listSP = new List<Product>();
            foreach (var p in db.Products.ToList())
            {
                listSP.Add(p);
            }

            Session["GioHang"] = listSP;
            return View(listSP);

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
















        // Hàm này xử lý khi người dùng nhấn nút "Thêm vào giỏ hàng"
        // Nó nhận vào tham số id (id của sản phẩm được chọn)
        public ActionResult GioHang(int? id)
        {
            if (Session["User"] == null)
            {
                return RedirectToAction("DangNhap", "Accounts");
            }
            else
            {
// Bước 1: Kiểm tra xem có id sản phẩm được truyền lên không
            // Nếu người dùng không chọn sản phẩm (id = null)
            // thì quay về trang Index (tránh lỗi)

            if (id == null)
            {
                return RedirectToAction("Index");
            }

// Bước 2: Tìm sản phẩm trong CSDL theo id
// Dùng Entity Framework để tìm ra sản phẩm có khóa chính = id

            Product product = db.Products.Find(id);

// Nếu không tìm thấy sản phẩm (ví dụ id không tồn tại)
// => trả về trang báo lỗi 404

            if (product == null)
            {
                return HttpNotFound();
            }
// Bước 3: Lấy giỏ hàng hiện tại từ Session
// Session["Cart"] là nơi lưu tạm giỏ hàng của người dùng
// Dùng "as List<CartItem>" để ép kiểu về danh sách các CartItem

            List<CartItem> cart = Session["Cart"] as List<CartItem>;

// Nếu Session["Cart"] chưa có (lần đầu vào giỏ hàng)
// thì tạo mới một danh sách rỗng để bắt đầu thêm sản phẩm

            if (cart == null)
            {
                cart = new List<CartItem>();
            }

// Bước 4: Kiểm tra xem sản phẩm hiện tại đã có trong giỏ chưa
// Dùng LINQ để tìm sản phẩm có ProductId trùng với product_id
// Nếu có thì biến "existing" sẽ chứa sản phẩm đó, ngược lại là null

            CartItem existing = cart.FirstOrDefault(p => p.ProductId == product.product_id);

// Nếu sản phẩm đã có trong giỏ hàng
// thì chỉ cần tăng số lượng lên 1, không cần thêm dòng mới

            if (existing != null)
            {
                existing.Quantity++;
            }
// Bước 5: Nếu sản phẩm chưa có trong giỏ hàng
// thì tạo mới 1 CartItem (một hàng trong giỏ)
            else
            {
                CartItem item = new CartItem
                {
                    ProductId = product.product_id,   // ID sản phẩm
                    Name = product.name,              // Tên sản phẩm
                    Thumbnail = product.thumbnail,    // Ảnh sản phẩm
                    Price = product.price,            // Giá bán
                    Quantity = 1                      // Số lượng mặc định là 1
                };

                // Thêm sản phẩm mới vào danh sách giỏ hàng
                cart.Add(item);
            }

            // Bước 6: Cập nhật lại giỏ hàng trong Session
            // Vì Session chỉ lưu giá trị tạm thời trong bộ nhớ
            // nên sau khi thêm/xóa/sửa, ta phải gán lại
            Session["Cart"] = cart;

            // Bước 7: Chuyển hướng người dùng đến trang "Xem Giỏ Hàng"
            // để họ thấy danh sách sản phẩm mình đã thêm
            return RedirectToAction("XemGioHang");
            }

            
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
    

        [HttpGet]
        public ActionResult ThanhToan()
        {
            List<CartItem> cartItems = Session["Cart"] as List<CartItem>;
            string thongtin = "";

            foreach (var data in cartItems)
            {
                thongtin += $"Tên sản phẩm: " + data.Name + "<br>" +"Số lượng: " +data.Quantity + "<br>" +"Tổng tiền: "+ ((data.Price * data.Quantity).ToString("N0")) + " <br> " + "--------------------<br>";
            }

            Session["thong_tin"] = thongtin;
            Session["dat_hang"] = cartItems;

          
            return View(cartItems);
        }


        public ActionResult MuaNgay(int id)
        {

            if (Session["User"] == null)
            {
                return RedirectToAction("DangNhap", "Accounts");
            }

            else
            {
  var query = (from a in db.Products
                        where a.product_id == id
                        select a).FirstOrDefault();

            return View(query);
            }
          

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
