using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Security.Policy;
using System.Web;
using System.Web.Mvc;
using BanDoUong.Models;
using Microsoft.Ajax.Utilities;

namespace BanDoUong.Controllers
{
    public class AccountsController : Controller
    {
        private AccountDb db = new AccountDb();

        [HttpGet]
        public ActionResult DangNhap()
        {
            return View();
        }

        [HttpPost]
        public ActionResult DangNhap(string Email, string MatKhau)
        {

            // Tìm user trong database
            var user = db.Accounts.FirstOrDefault(a => a.Email.Equals(Email) && a.MatKhau.Equals(MatKhau));

            if (user == null)
            {
                ViewBag.ErrorDangNhap = "Thông tin không chính xác! Vui lòng thử lại.";
                return View();
            }
            else
            {
                Session["User"] = user.HoVaTen;
                return RedirectToAction("Index", "Products");
            }
        }

        [HttpGet]
        public ActionResult DangKi()
        {
            return View();
        }

        [HttpPost]
        public ActionResult DangKi(string HoVaTen, string SoDienThoai, string Email, string MatKhau, string XacNhanMatKhau)
        {
            ViewBag.ErrorDangKi = "";
            var kiem_tra_email = (from a in db.Accounts
                                 where a.Email == Email
                                 select a)
                                 .FirstOrDefault();

            if (kiem_tra_email != null)
            {
                ViewBag.ErrorDangKi += "Email đã có trên hệ thống!";
                return View();
            }
            if(string.IsNullOrEmpty(HoVaTen) || string.IsNullOrEmpty(SoDienThoai) || string.IsNullOrEmpty(Email) || string.IsNullOrEmpty(MatKhau) || string.IsNullOrEmpty(XacNhanMatKhau)|| !MatKhau.Equals(XacNhanMatKhau))
            {
                ViewBag.ErrorDangKi += "Vui lòng kiểm tra lại thông tin!";
                return View();
            }

            else
            {
                Account account = new Account(Email, HoVaTen, SoDienThoai, MatKhau);
                db.Accounts.Add(account);
                db.SaveChanges();
                ViewBag.ErrorDangKi += "Đăng kí thành công!";


                return View();
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
