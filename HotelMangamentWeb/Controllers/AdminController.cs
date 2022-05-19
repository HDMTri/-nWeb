using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using HotelMangamentWeb.Models;
using HotelMangamentWeb.Models.ViewModels;
using HotelMangamentWeb.Models.Functions;
using System.Web.Security;
using System.IO;

namespace HotelMangamentWeb.Controllers
{
    public class AdminController : Controller
    {
        private hotelDBEntities db = new hotelDBEntities();
        private int MaxPhanTuMoiTrang = 8;

        //Hiển thị danh sách các tài khoản có trong DB

        [Authorize]
        public ActionResult DSTaiKhoan()
        {
            var list = db.TaiKhoans.ToList();
            int TongPhanTu = list.Count;
            int SoTrang = (TongPhanTu - 1) / MaxPhanTuMoiTrang + 1;
            int Trang = 1;
            try
            {
                string id = RouteData.Values["id"].ToString();
                Trang = Convert.ToInt16(id);
                if (Trang > SoTrang) Trang = SoTrang;
            }
            catch (Exception e) { }
            int PhanTuDau = (Trang - 1) * MaxPhanTuMoiTrang;
            int SoPhanTu = MaxPhanTuMoiTrang;
            if (Trang == SoTrang) SoPhanTu = TongPhanTu - (SoTrang - 1) * MaxPhanTuMoiTrang;
            var listMoiTrang = list.GetRange(PhanTuDau, SoPhanTu);
            ViewBag.STT = PhanTuDau;
            ViewBag.Trang = Trang;
            ViewBag.SoTrang = SoTrang;
            return View(listMoiTrang);
        }

        //Đăng xuất khỏi tài khoản hiện tại và trở về trang chính
        [Authorize]
        public ActionResult DangXuat()
        {
            FormsAuthentication.SignOut();
            Session["TaiKhoan"] = null;
            HttpCookie ckTaiKhoan = new HttpCookie("TenTaiKhoan"), ckMatKhau = new HttpCookie("MatKhau");
            ckTaiKhoan.Expires = DateTime.Now.AddDays(1);
            ckMatKhau.Expires = DateTime.Now.AddDays(1);
            Response.Cookies.Add(ckTaiKhoan);
            Response.Cookies.Add(ckMatKhau);
            return RedirectToAction("TrangChu", "TrangChu");
        }

        //Hiển thị danh sách các loại phòng có trong DB
        [Authorize]
        public ActionResult DSLoaiPhong()
        {
            var list = db.LoaiPhongs.ToList();
            return View(list);
        }

        ////Hiển thị danh sách các phòng có trong DB
        [Authorize]
        public ActionResult DSPhong()
        {
            var list = db.Phongs.ToList();
            int TongPhanTu = list.Count;
            int SoTrang = (TongPhanTu - 1) / MaxPhanTuMoiTrang + 1;
            int Trang = 1;
            try
            {
                string id = RouteData.Values["id"].ToString();
                Trang = Convert.ToInt16(id);
                if (Trang > SoTrang) Trang = SoTrang;
            }
            catch (Exception e) { }
            int PhanTuDau = (Trang - 1) * MaxPhanTuMoiTrang;
            int SoPhanTu = MaxPhanTuMoiTrang;
            if (Trang == SoTrang) SoPhanTu = TongPhanTu - (SoTrang - 1) * MaxPhanTuMoiTrang;
            var listMoiTrang = list.GetRange(PhanTuDau, SoPhanTu);
            ViewBag.STT = PhanTuDau;
            ViewBag.Trang = Trang;
            ViewBag.SoTrang = SoTrang;
            return View(listMoiTrang);
        }

        //Hiển thị danh sách các dịch vụ có trong DB
        [Authorize]
        public ActionResult DSDichVu()
        {
            var list = db.DichVus.ToList();
            int TongPhanTu = list.Count;
            int SoTrang = (TongPhanTu - 1) / MaxPhanTuMoiTrang + 1;
            int Trang = 1;
            try
            {
                string id = RouteData.Values["id"].ToString();
                Trang = Convert.ToInt16(id);
                if (Trang > SoTrang) Trang = SoTrang;
            }
            catch (Exception e) { }
            int PhanTuDau = (Trang - 1) * MaxPhanTuMoiTrang;
            int SoPhanTu = MaxPhanTuMoiTrang;
            if (Trang == SoTrang) SoPhanTu = TongPhanTu - (SoTrang - 1) * MaxPhanTuMoiTrang;
            var listMoiTrang = list.GetRange(PhanTuDau, SoPhanTu);
            ViewBag.STT = PhanTuDau;
            ViewBag.Trang = Trang;
            ViewBag.SoTrang = SoTrang;
            return View(listMoiTrang);
        }

        // Thêm tài khoản vào trong DB
        [Authorize]
        public ActionResult ThemTaiKhoan()
        {
            return View(new TaiKhoan());
        }

        // Thêm loại phòng vào trong DB
        [Authorize]
        public ActionResult ThemLoaiPhong()
        {
            return View(new LoaiPhong());
        }

        // Thêm phòng vào trong DB
        [Authorize]
        public ActionResult ThemPhong()
        {
            ViewBag.listLoaiPhong = db.LoaiPhongs.ToList();
            return View(new Phong());
        }

        // Thêm cịch vụ vào trong DB
        [Authorize]
        public ActionResult ThemDichVu()
        {
            return View(new DichVu());
        }

        // Sửa các thông tin của tài khoản như MK, name, SDT, email và quyền
        [Authorize]
        public ActionResult SuaTaiKhoan()
        {
            string TenTaiKhoan = RouteData.Values["id"].ToString();
            var taiKhoan = db.TaiKhoans.Find(TenTaiKhoan);
            return View(taiKhoan);
        }

        // Sửa các thông tin của loại phòng
        [Authorize]
        public ActionResult SuaLoaiPhong()
        {
            string MaLoai = RouteData.Values["id"].ToString();
            var loaiPhong = db.LoaiPhongs.Find(MaLoai);
            return View(loaiPhong);
        }

        // Sửa các thông tin của phòng
        [Authorize]
        public ActionResult SuaPhong()
        {
            string MaPhong = RouteData.Values["id"].ToString();
            ViewBag.listLoaiPhong = db.LoaiPhongs.ToList();
            var phong = db.Phongs.Find(MaPhong);
            return View(phong);
        }

        // Sửa các thông tin của dịch vụ
        [Authorize]
        public ActionResult SuaDichVu()
        {
            string MaDichVu = RouteData.Values["id"].ToString();
            var dichVu = db.DichVus.Find(MaDichVu);
            return View(dichVu);
        }

        // Xóa tài khoản khỏi DB
        [Authorize]
        public ActionResult XoaTaiKhoan()
        {
            string TenTaiKhoan = RouteData.Values["id"].ToString();
         // Trước khi xóa Tài Khoản phải xóa thông tin đặt phòng
            var listDatPhong = db.DatPhongs.Where(m => m.TenTaiKhoan == TenTaiKhoan).ToList();
            var HamDP = new HamDatPhong();
            foreach (DatPhong dp in listDatPhong)
                HamDP.Delete(dp.MaDatPhong);
         // Sau đó mới xóa Tài Khoản
            var HamTK = new HamTaiKhoan();
            HamTK.Delete(TenTaiKhoan);
            return RedirectToAction("DSTaiKhoan", "Admin");
        }

        // Sửa các thông tin của loại phòng trong DB
        [Authorize]
        public ActionResult XoaLoaiPhong()
        {
            string MaLoai = RouteData.Values["id"].ToString();
         // Nhưng muốn xóa Phòng phải xóa Đặt Phòng trước
            var listPhong = db.Phongs.Where(m => m.MaLoai == MaLoai).ToList();
            var HamP = new HamPhong();
            var HamDP = new HamDatPhong();
            foreach (Phong p in listPhong)
            {
                var listDatPhong = db.DatPhongs.Where(m => m.MaPhong == p.MaPhong).ToList();
                foreach (DatPhong dp in listDatPhong)
                    HamDP.Delete(dp.MaDatPhong);
                HamP.Delete(p.MaPhong);
            }
            // Sau đó mới xóa Loại Phòng
            var HamLP = new HamLoaiPhong();
            HamLP.Delete(MaLoai);
            return RedirectToAction("DSLoaiPhong", "Admin");
        }

        // Sửa các thông tin của phòng trong DB
        [Authorize]
        public ActionResult XoaPhong()
        {
            string MaPhong = RouteData.Values["id"].ToString();
            // Trước khi xóa Phòng phải xóa thông tin đặt phòng
            var listDatPhong = db.DatPhongs.Where(m => m.MaPhong == MaPhong).ToList();
            var HamDP = new HamDatPhong();
            foreach (DatPhong dp in listDatPhong)
                HamDP.Delete(dp.MaDatPhong);
            // Sau đó mới xóa Phòng
            var HamP = new HamPhong();
            HamP.Delete(MaPhong);
            return RedirectToAction("DSPhong", "Admin");
        }

        // Sửa các thông tin của dịch vụ ra khỏi DB
        [Authorize]
        public ActionResult XoaDichVu()
        {
            string MaDichVu = RouteData.Values["id"].ToString();
            var HamDV = new HamDichVu();
            HamDV.Delete(MaDichVu);
            return RedirectToAction("DSDichVu", "Admin");
        }

        //Thêm thông tin về tài khoản vào DB
        [Authorize]
        [HttpPost]
        public ActionResult ThemTaiKhoan(TaiKhoan tk)
        {
            if (ModelState.IsValid)
            {
                var taiKhoan = db.TaiKhoans.Find(tk.TenTaiKhoan);
                if (taiKhoan != null)
                {
                    ModelState.AddModelError("TenTaiKhoan", "Tài Khoản đã tồn tại");
                    return View(tk);
                }
                var hTK = new HamTaiKhoan();
                hTK.Insert(tk);
                return RedirectToAction("DSTaiKhoan", "Admin");
            }
            return View(tk);
        }

        //Thêm thông tin về tài khoản vào DB
        [Authorize]
        [HttpPost]
        public ActionResult ThemLoaiPhong(LoaiPhong lp, HttpPostedFileBase file)
        {
            if (ModelState.IsValid)
            {
                var loaiPhong = db.LoaiPhongs.Find(lp.MaLoai);
                if (loaiPhong != null)
                {
                    ModelState.AddModelError("MaLoai", "Mã Loại đã tồn tại");
                    return View(lp);
                }
                if (file != null && file.ContentLength > 0)
                {
                    var fileName = Path.GetFileName(file.FileName);
                    var path = Path.Combine(Server.MapPath("~/Content/images/"), fileName);
                    file.SaveAs(path);
                }
                var hLP = new HamLoaiPhong();
                hLP.Insert(lp);
                return RedirectToAction("DSLoaiPhong", "Admin");
            }
            return View(lp);
        }

        //Thêm thông tin về phòng vào DB
        [Authorize]
        [HttpPost]
        public ActionResult ThemPhong(Phong p)
        {
            if (ModelState.IsValid)
            {
                var phong = db.Phongs.Find(p.MaPhong);
                if (phong != null)
                {
                    ModelState.AddModelError("MaPhong", "Mã Phòng đã tồn tại");
                    return View(p);
                }
                var hP = new HamPhong();
                hP.Insert(p);
                return RedirectToAction("DSPhong", "Admin");
            }
            ViewBag.listLoaiPhong = db.LoaiPhongs.ToList();
            return View(p);
        }

        //Thêm thông tin về dịch vụ vào DB
        [Authorize]
        [HttpPost]
        public ActionResult ThemDichVu(DichVu dv)
        {
            if (ModelState.IsValid)
            {
                var dichVu = db.DichVus.Find(dv.MaDichVu);
                if (dichVu != null)
                {
                    ModelState.AddModelError("MaDichVu", "Mã Dịch Vụ đã tồn tại");
                    return View(dv);
                }
                var hDV = new HamDichVu();
                hDV.Insert(dv);
                return RedirectToAction("DSDichVu", "Admin");
            }
            return View(dv);
        }

        //Chỉnh sửa, cập nhật các thông tin của tài khoản
        [Authorize]
        [HttpPost]
        public ActionResult SuaTaiKhoan(TaiKhoan tk)
        {
            if (ModelState.IsValid)
            {
                var HamTK = new HamTaiKhoan();
                HamTK.Update(tk);
                return RedirectToAction("DSTaiKhoan", "Admin");
            }
            return View(tk);
        }

        //Chỉnh sửa, cập nhật các thông tin của loại phòng
        [Authorize]
        [HttpPost]
        public ActionResult SuaLoaiPhong(LoaiPhong lp, HttpPostedFileBase file)
        {
            if (ModelState.IsValid)
            {
                if (file != null && file.ContentLength > 0)
                {
                    var fileName = Path.GetFileName(file.FileName);
                    var path = Path.Combine(Server.MapPath("~/Content/images/"), fileName);
                    file.SaveAs(path);
                }
                var HamLP = new HamLoaiPhong();
                HamLP.Update(lp);
                return RedirectToAction("DSLoaiPhong", "Admin");
            }
            return View(lp);
        }

        //Chỉnh sửa, cập nhật các thông tin của phòng
        [Authorize]
        [HttpPost]
        public ActionResult SuaPhong(Phong p)
        {
            if (ModelState.IsValid)
            {
                var HamP = new HamPhong();
                HamP.Update(p);
                return RedirectToAction("DSPhong", "Admin");
            }
            ViewBag.listLoaiPhong = db.LoaiPhongs.ToList();
            return View(p);
        }

        //Chỉnh sửa, cập nhật các thông tin của dịch vụ
        [Authorize]
        [HttpPost]
        public ActionResult SuaDichVu(DichVu dv)
        {
            if (ModelState.IsValid)
            {
                var HamDV = new HamDichVu();
                HamDV.Update(dv);
                return RedirectToAction("DSDichVu", "Admin");
            }
            return View(dv);
        }
    }
}