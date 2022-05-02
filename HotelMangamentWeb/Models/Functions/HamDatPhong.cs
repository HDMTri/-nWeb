using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using HotelMangamentWeb.Models;

namespace HotelMangamentWeb.Models.Functions
{
    public class HamDatPhong
    {
        private hotelDBEntities db;

        public HamDatPhong()
        {
            db = new hotelDBEntities();
        }
        //IQueryable được dành cho các nhà cung cấp truy vấn triển khai
        public IQueryable<DatPhong> DatPhongs
        {
            get { return db.DatPhongs; }
        }

        //Thêm các thông tin đặt phòng vào trong DB
        public int Insert(DatPhong model)
        {
            db.DatPhongs.Add(model);
            db.SaveChanges();
            return model.MaDatPhong;
        }

        //Cập nhật các thông tin đặt phòng trong DB
        public int Update(DatPhong model)
        {
            DatPhong dbEntry = db.DatPhongs.Find(model.MaDatPhong);
            if (dbEntry == null)
            {
                return -1;
            }
            dbEntry.MaDatPhong = model.MaDatPhong;
            dbEntry.TenTaiKhoan = model.TenTaiKhoan;
            dbEntry.NgayDat = model.NgayDat;
            dbEntry.NgayDen = model.NgayDen;
            dbEntry.NgayTra = model.NgayTra;
            dbEntry.ThanhTien = model.ThanhTien;
            db.SaveChanges();
            return model.MaDatPhong;
        }
        //Xóa các thông tin đặt phòng khỏi DB
        public int Delete(int MaDatPhong)
        {
            DatPhong dbEntry = db.DatPhongs.Find(MaDatPhong);
            if (dbEntry == null)
            {
                return -1;
            }
            db.DatPhongs.Remove(dbEntry);
            db.SaveChanges();
            return MaDatPhong;
        }
    }
}
