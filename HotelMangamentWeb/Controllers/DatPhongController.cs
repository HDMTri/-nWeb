using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using HotelMangamentWeb.Models;
using HotelMangamentWeb.Models.ViewModels;
using HotelMangamentWeb.Models.Functions;

namespace HotelMangamentWeb.Controllers
{
    public class DatPhongController : Controller
    {
        private hotelDBEntities db = new hotelDBEntities();

        //Láy các thông tin và hiển thị các thông tin đó lên 1 danh sách 
        public ActionResult DSPhong()
        {
            var HamP = new HamPhong();
            return View(HamP.toanBoPhong());
        }

        //Láy các thông tin và hiển thị các thông tin đó lên 1 danh sách 
        public ActionResult LoaiPhong()
        {
            ViewBag.Success = 0;
            string id = (string)RouteData.Values["id"];
            LoaiPhong lp = db.LoaiPhongs.Find(id);
            if (lp == null)
            {
                return View(new List<Phong>());
            }
            ViewBag.Success = 1;
            ViewBag.TenLoai = lp.TenLoai;
            ViewBag.DuongDanAnh = lp.DuongDanAnh;
            ViewBag.GhiChu = lp.GhiChu;
            var listPhong = db.Phongs.Where(m => m.MaLoai == id).ToList();
            return View(listPhong);
        }

        //Láy các thông tin qua quá trình tìm kiếm của chương trình và hiển thị các thông tin đó lên 1 danh sách 
        public ActionResult TimKiem()
        {
            string loaiTimKiem = Request.QueryString["LoaiTimKiem"];
            string mucTimKiem = Request.QueryString["MucTimKiem"];
            string giaTri = Request.QueryString["GiaTri"];
            int giaTriTimKiem;
            if (giaTri == "") giaTriTimKiem = 0;
            else giaTriTimKiem = Convert.ToInt32(giaTri);
            var list = new HamPhong();
            return View(list.timKiem(loaiTimKiem, mucTimKiem, giaTriTimKiem));
        }

        //Láy các thông tin đã nhập trong ô và hiển thị các thông tin lên 1 bảng đặt phòng của tài khoản đó
        public ActionResult DatPhong()
        {
            if (Session["TaiKhoan"] == null)
            {
                Session["TrangTruoc"] = Request.RawUrl;
                return RedirectToAction("DangNhap", "CaNhan");
            }
            var listDV = db.DichVus.ToList();
            ViewBag.DSDichVu = listDV;
            string MaPhong = (string)RouteData.Values["id"];
            var phong = db.Phongs.Where(m => m.MaPhong == MaPhong).First();
            var loaiPhong = db.LoaiPhongs.Where(m => m.MaLoai == phong.MaLoai).First();
            ViewBag.DuongDanAnh = loaiPhong.DuongDanAnh;
            ViewBag.TenLoai = loaiPhong.TenLoai;
            return View(phong);
        }

        //Nếu đặt phòng thành công chương trình sẽ hiển thi ô thông báo
        public ActionResult DatPhongThanhCong()
        {
            return View();
        }

       //Trang đặt phòng sẽ lấy các thông tin mà người dùng đã nhập trong ô vầ thêm các thông tin đó vào DB đồng thời hiển thị trên màn hình
        [HttpPost]
        public ActionResult DatPhong(string NgayDen, string SoNgay)
        {
            var listDV = db.DichVus.ToList();
            ViewBag.DSDichVu = listDV;
            string MaPhong = (string)RouteData.Values["id"];
            var phong = db.Phongs.Where(m => m.MaPhong == MaPhong).First();
            if (SoNgay == "")
            {
                ViewBag.Success = -1;
                var loaiPhong = db.LoaiPhongs.Where(m => m.MaLoai == phong.MaLoai).First();
                ViewBag.DuongDanAnh = loaiPhong.DuongDanAnh;
                ViewBag.TenLoai = loaiPhong.TenLoai;
                return View(phong);
            }
            int SoNgayThue = Convert.ToInt16(SoNgay);
            DateTime dateNgayDat, dateNgayDen, dateNgayTra;
            dateNgayDat = DateTime.Today;
            dateNgayDen = Convert.ToDateTime(NgayDen);
            dateNgayTra = dateNgayDen.AddDays(SoNgayThue);
            var kiemTraPhongBiDatChua = db.DatPhongs.
                Where(m => m.MaPhong == MaPhong && !(m.NgayDen >= dateNgayTra || m.NgayTra <= dateNgayDen)).ToList();
            if (kiemTraPhongBiDatChua.Count > 0)
            {
                var listDaBiDat = db.DatPhongs.Where(m => m.NgayDen < dateNgayTra && m.NgayTra > dateNgayDen).Select(m => m.MaPhong);
                var listPhongDatDuoc = db.Phongs.Where(m => !listDaBiDat.Contains(m.MaPhong)).Join(db.LoaiPhongs, p => p.MaLoai, lp => lp.MaLoai, (p, lp) =>
                    new PhongView
                    {
                        MaPhong = p.MaPhong,
                        TenPhong = p.TenPhong,
                        DienTich = p.DienTich,
                        GiaThue = p.GiaThue,
                        MaLoai = lp.MaLoai,
                        TenLoai = lp.TenLoai,
                        DuongDanAnh = lp.DuongDanAnh
                    });
                ViewBag.Success = 1;
                var loaiPhong = db.LoaiPhongs.Where(m => m.MaLoai == phong.MaLoai).First();
                ViewBag.DuongDanAnh = loaiPhong.DuongDanAnh;
                ViewBag.TenLoai = loaiPhong.TenLoai;
                ViewBag.ListDatDuoc = listPhongDatDuoc.ToList();
                return View(phong);
            }
            int MaDatPhong;
            var listDatPhong = db.DatPhongs.ToList();
            if (listDatPhong.Count == 0) MaDatPhong = 1;
            else MaDatPhong = listDatPhong.Last().MaDatPhong + 1;
            int ThanhTien = 0;
            string DichVuSuDung = "";
            foreach (DichVu dv in listDV)
            {
                if (Request.Form[dv.MaDichVu] == "on")
                {
                    if (ThanhTien > 0) DichVuSuDung += ", ";
                    DichVuSuDung += dv.TenDichVu;
                    ThanhTien += (int)dv.GiaDichVu;
                }
            }
            //Giá tiền dịch vụ sẽ bằng 0 nếu khách hàng không có nhu cầu sử dụng 
            if (ThanhTien == 0) DichVuSuDung = "Không Sử Dụng";
            ThanhTien += phong.GiaThue.Value;
            ThanhTien *= SoNgayThue;
            string TenTaiKhoan = ((TaiKhoan)Session["TaiKhoan"]).TenTaiKhoan;
            DatPhong datPhong = new DatPhong
            {
                MaDatPhong = MaDatPhong,
                TenTaiKhoan = TenTaiKhoan,
                MaPhong = MaPhong,
                NgayDat = dateNgayDat,
                NgayDen = dateNgayDen,
                NgayTra = dateNgayTra,
                DichVu = DichVuSuDung,
                ThanhTien = ThanhTien
            };
            HamDatPhong HamDP = new HamDatPhong();
            HamDP.Insert(datPhong);
            ViewBag.TenPhong = phong.TenPhong;
            ViewBag.DienTich = phong.DienTich;
            ViewBag.GiaThue = phong.GiaThue;
            ViewBag.NgayDat = dateNgayDat.ToString("dd/MM/yyyy");
            ViewBag.NgayDen = dateNgayDen.ToString("dd/MM/yyyy");
            ViewBag.NgayTra = dateNgayTra.ToString("dd/MM/yyyy");
            ViewBag.DichVu = DichVuSuDung;
            ViewBag.ThanhTien = ThanhTien;
            return View("DatPhongThanhCong");
        }

    }
}
