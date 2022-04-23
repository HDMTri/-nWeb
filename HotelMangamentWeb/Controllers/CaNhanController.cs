using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Net;
using HotelMangamentWeb.Models;
using HotelMangamentWeb.Models.ViewModels;
using HotelMangamentWeb.Models.Functions;
using System.Web.Security;
using System.Data.Entity;

namespace HotelMangamentWeb.Controllers
{
    public class CaNhanController : Controller
    {
        private hotelDBEntities db = new hotelDBEntities();

        //Lấy thông tin trong DB để đăng nhập và sau đó trở về trang chính của chương trình
        public ActionResult DangNhap()
        {
            if (Session["TaiKhoan"] != null) return RedirectToAction("TrangChu", "TrangChu");
            return View(new TaiKhoanDangNhapView());
        }

        //Thêm thông tin vào trong DB để đăng ký và sau đó trở về trang chính của chương trình
        public ActionResult DangKy()
        {
            if (Session["TaiKhoan"] != null) return RedirectToAction("TrangChu", "TrangChu");
            return View(new TaiKhoanDangKyView());
        }

        //Vào trang cá nhân của tài khoản hiển thị các thông tin cá nhân của tài khoản
        public ActionResult CaNhan()
        {
            if (Session["TaiKhoan"] == null)
            {
                Session["TrangTruoc"] = Request.RawUrl;
                return Redirect("DangNhap");
            }
            var TaiKhoan = (TaiKhoan)Session["TaiKhoan"];
            var TaiKhoanCaNhan = new TaiKhoanDangKyView
            {
                TenTaiKhoan = TaiKhoan.TenTaiKhoan,
                MatKhau = TaiKhoan.MatKhau,
                XacNhanMatKhau = TaiKhoan.MatKhau,
                HoTen = TaiKhoan.HoTen,
                SoDienThoai = TaiKhoan.SoDienThoai,
                Email = TaiKhoan.Email
            };
            return View(TaiKhoanCaNhan);
        }

        //Vào trang cá nhân của tài khoản hiển thị các thông tin về lịch sử đặt phòng của tài khoản
        public ActionResult LichSu()
        {
            if (Session["TaiKhoan"] == null) return Redirect("DangNhap");
            TaiKhoan taiKhoan = (TaiKhoan)Session["TaiKhoan"];
            DateTime dateHomNay = DateTime.Now.AddDays(-1);
            var listLichSu = db.DatPhongs.Where(dp => dp.TenTaiKhoan == taiKhoan.TenTaiKhoan).Join(db.Phongs, dp => dp.MaPhong, p => p.MaPhong, (dp, p) => new
            {
                MaDatPhong = dp.MaDatPhong,
                TenPhong = p.TenPhong,
                NgayDat = dp.NgayDat,
                NgayDen = dp.NgayDen,
                NgayTra = dp.NgayTra,
                ThanhTien = dp.ThanhTien,
                DichVu = dp.DichVu
            }).AsEnumerable().Select(m =>
                new LichSuView()
                {
                    MaDatPhong = m.MaDatPhong,
                    TenPhong = m.TenPhong,
                    NgayDat = m.NgayDat.Value.ToString("dd/MM/yyyy"),
                    NgayDen = m.NgayDen.Value.ToString("dd/MM/yyyy"),
                    NgayTra = m.NgayTra.Value.ToString("dd/MM/yyyy"),
                    DichVu = m.DichVu,
                    ThanhTien = m.ThanhTien,
                    CoTheHuy = m.NgayDen > dateHomNay ? true : false
                }).ToList();
            return View(listLichSu);
        }

        //Hiển thị trang thông báo tài khoản đã được đăng ký đồng thời thể hiện các thông tin mà người dùng đã nhập
        public ActionResult DangKyThanhCong()
        {
            return View(new TaiKhoan());
        }

        //Chương trìng sẽ ,không đăng xuât tài khoản khỏi chương trình trong vào 1 ngày
        public ActionResult DangXuat()
        {
            Session["TaiKhoan"] = null;
            HttpCookie ckTaiKhoan = new HttpCookie("TenTaiKhoan"), ckMatKhau = new HttpCookie("MatKhau");
            ckTaiKhoan.Expires = DateTime.Now.AddDays(1);
            ckMatKhau.Expires = DateTime.Now.AddDays(1);
            Response.Cookies.Add(ckTaiKhoan);
            Response.Cookies.Add(ckMatKhau);
            return RedirectToAction("TrangChu", "TrangChu");
        }
        //Xóa lịch sử đặt phòng ra khỏi DB
        public ActionResult XoaDatPhong()
        {
            int MaHuy = Convert.ToInt16(RouteData.Values["id"].ToString());
            var HamDP = new HamDatPhong();
            HamDP.Delete(MaHuy);
            TempData["HuyDat"] = 1;
            return RedirectToAction("LichSu", "CaNhan");
        }

        //Đăng nhập vào chương trình sẽ qua những bước chạy ngầm để xác định các thông tin như TK, MK của người đó có đúng hay không
        [HttpPost]
        public ActionResult DangNhap(TaiKhoanDangNhapView tk)
        {
            if (ModelState.IsValid)
            {
                var list = db.TaiKhoans.Where(m => m.TenTaiKhoan == tk.TenTaiKhoan && m.MatKhau == tk.MatKhau).ToList();
                if (list.Count == 0)
                {
                    ModelState.AddModelError("TenTaiKhoan", "Tài Khoản hoặc Mật Khẩu không chính xác");
                    return View(tk);
                }
                TaiKhoan taiKhoan = list.First();
                if (taiKhoan.LaAdmin)
                {
                    //FormsAuthentication được dùng để xác thực tài khoaản mà không cần phải dùng Windows Authentication
                    //SetAuthCookie được dùng để giữ người dùng luôn luôn được Log in trừ khi Website bị đóng
                    FormsAuthentication.SetAuthCookie(taiKhoan.HoTen, tk.TuDongDangNhap);
                    return RedirectToAction("DSTaiKhoan", "Admin");
                }
                Session["TaiKhoan"] = taiKhoan;
                if (tk.TuDongDangNhap)
                //Tài khoản sẽ được nhớ trong vòng 15 ngày nếu đánh vào "Tự độnng đăng nhập" để người dùng dễ dàng Login
                {
                    HttpCookie ckTaiKhoan = new HttpCookie("TenTaiKhoan"), ckMatKhau = new HttpCookie("MatKhau");
                    ckTaiKhoan.Value = taiKhoan.TenTaiKhoan; ckMatKhau.Value = taiKhoan.MatKhau;
                    ckTaiKhoan.Expires = DateTime.Now.AddDays(15);
                    ckMatKhau.Expires = DateTime.Now.AddDays(15);
                    Response.Cookies.Add(ckTaiKhoan);
                    Response.Cookies.Add(ckMatKhau);
                }
                if (Session["TrangTruoc"] != null)
                {
                    return Redirect(Session["TrangTruoc"].ToString());
                }
                return RedirectToAction("TrangChu", "TrangChu");
            }
            return RedirectToAction("DangNhap", "CaNhan");
        }

        //Nếu đăng ký tài khoản mới mà trùng lặp thông tin với tài khoản đã tồn tại chương trình sẽ thông báo lỗi
        [HttpPost]
        public ActionResult DangKy(TaiKhoanDangKyView tk)
        {
            if (ModelState.IsValid)
            {
                var taiKhoan = db.TaiKhoans.Find(tk.TenTaiKhoan);
                if (taiKhoan != null)
                {
                    ModelState.AddModelError("TenTaiKhoan", "Tài Khoản đã tồn tại");
                    return View(tk);
                }
                taiKhoan = new TaiKhoan()
                {
                    TenTaiKhoan = tk.TenTaiKhoan,
                    MatKhau = tk.MatKhau,
                    HoTen = tk.HoTen,
                    SoDienThoai = tk.SoDienThoai,
                    Email = tk.Email,
                    LaAdmin = false
                };
                var hTK = new HamTaiKhoan();
                hTK.Insert(taiKhoan);
                return View("DangKyThanhCong", taiKhoan);
            }
            return View(tk);
        }

        //Nếu đăng ký tài khoản mới thì chương trình sẽ chuyển sang trang mới (Models/ViewModels/TaiKhoanDangKyView) để thực hiện điều đó
        [HttpPost]
        public ActionResult CaNhan(TaiKhoanDangKyView tk)
        {
            if (ModelState.IsValid)
            {
                var taiKhoan = new TaiKhoan()
                {
                    TenTaiKhoan = tk.TenTaiKhoan,
                    MatKhau = tk.MatKhau,
                    HoTen = tk.HoTen,
                    SoDienThoai = tk.SoDienThoai,
                    Email = tk.Email,
                    LaAdmin = false
                };
                Session["TaiKhoan"] = taiKhoan;
                var HamTK = new HamTaiKhoan();
                HamTK.Update(taiKhoan);
                ViewBag.Success = 1;
            }
            return View(tk);
        }

    }
}