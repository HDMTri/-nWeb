using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using HotelMangamentWeb.Models;
using HotelMangamentWeb.Models.ViewModels;
using HotelMangamentWeb.Models.Functions;

namespace HotelMangamentWeb.Controllers
{
    public class TrangChuController : Controller
    {
        private hotelDBEntities db = new hotelDBEntities();

        //Trang chính của chương trình sẽ xác thực tài khoản nào đang đăng nhập vào chương trình, nếu không có TK đăng nhập
        //thì chương trình chỉ cho phép xem các thông tin sơ bộ

        public ActionResult TrangChu()
        {
            if (Request.IsAuthenticated) return RedirectToAction("DSTaiKhoan", "Admin");
            if (Session["TaiKhoan"] == null)
            {
                if (Request.Cookies["TenTaiKhoan"] != null)
                {
                    HttpCookie TenTaiKhoan = Request.Cookies["TenTaiKhoan"];
                    HttpCookie MatKhau = Request.Cookies["MatKhau"];
                    var listTK = db.TaiKhoans.Where(m => m.TenTaiKhoan == TenTaiKhoan.Value && m.MatKhau == MatKhau.Value).ToList();
                    if (listTK.Count != 0)
                    {
                        TaiKhoan taiKhoan = listTK.First();
                        Session["TaiKhoan"] = taiKhoan;
                    }
                }
            }
            List<LoaiPhong> list = db.LoaiPhongs.ToList();
            return View(list);
        }

        public ActionResult TinTuc()
        {
            return View();
        }

        public ActionResult LienHe()
        {
            return View();
        }
        public ActionResult Home()
        {
            return View();
        }
    }
}