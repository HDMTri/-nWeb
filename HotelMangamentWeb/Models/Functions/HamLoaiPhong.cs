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
    public class HamLoaiPhong
    {
        private hotelDBEntities db;

        public HamLoaiPhong()
        {
            db = new hotelDBEntities();
        }
        //IQueryable được dành cho các nhà cung cấp truy vấn triển khai
        public IQueryable<LoaiPhong> LoaiPhongs
        {
            get { return db.LoaiPhongs; }
        }

        //Thêm các thông tin về loại phòng vào trong DB
        public string Insert(LoaiPhong model)
        {
            db.LoaiPhongs.Add(model);
            db.SaveChanges();
            return model.MaLoai;
        }

        //Cập nhật các thông tin về loại phòng trong DB
        public string Update(LoaiPhong model)
        {
            LoaiPhong dbEntry = db.LoaiPhongs.Find(model.MaLoai);
            if (dbEntry == null)
            {
                return null;
            }
            dbEntry.MaLoai = model.MaLoai;
            dbEntry.TenLoai = model.TenLoai;
            dbEntry.GhiChu = model.GhiChu;
            dbEntry.DuongDanAnh = model.DuongDanAnh;
            db.SaveChanges();
            return model.MaLoai;
        }

        //Xóa các thông tin về loại phòng vào khỏi DB
        public string Delete(string MaLoai)
        {
            LoaiPhong dbEntry = db.LoaiPhongs.Find(MaLoai);
            if (dbEntry == null)
            {
                return null;
            }
            db.LoaiPhongs.Remove(dbEntry);
            db.SaveChanges();
            return MaLoai;
        }
    }
}

}
