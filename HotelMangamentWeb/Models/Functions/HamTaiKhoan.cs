using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using HotelMangamentWeb.Models;

namespace HotelMangamentWeb.Models.Functions
{
    //Hàm tài khoản
    public class HamTaiKhoan : Controller
    {
        private hotelDBEntities db;

        public HamTaiKhoan()
        {
            db = new hotelDBEntities();
        }
        //IQueryable được dành cho các nhà cung cấp truy vấn triển khai
        public IQueryable<TaiKhoan> TaiKhoans
        {
            get { return db.TaiKhoans; }
        }

        //Thêm các thông tin về tài khoản người dùng vào trong DB
        public string Insert(TaiKhoan model)
        {
            db.TaiKhoans.Add(model);
            db.SaveChanges();
            return model.TenTaiKhoan;
        }

        //Cập nhật các thông tin về tài khoản người dùng trong DB
        public string Update(TaiKhoan model)
        {
            TaiKhoan dbEntry = db.TaiKhoans.Find(model.TenTaiKhoan);
            if (dbEntry == null)
            {
                return null;
            }
            dbEntry.TenTaiKhoan = model.TenTaiKhoan;
            dbEntry.MatKhau = model.MatKhau;
            dbEntry.HoTen = model.HoTen;
            dbEntry.SoDienThoai = model.SoDienThoai;
            dbEntry.Email = model.Email;
            dbEntry.LaAdmin = model.LaAdmin;
            db.SaveChanges();
            return model.TenTaiKhoan;
        }

        //Xóa các thông tin về tài khoản người dùng khỏi DB
        public string Delete(string TenTaiKhoan)
        {
            TaiKhoan dbEntry = db.TaiKhoans.Find(TenTaiKhoan);
            if (dbEntry == null)
            {
                return null;
            }
            db.TaiKhoans.Remove(dbEntry);
            db.SaveChanges();
            return TenTaiKhoan;
        }
    }
}