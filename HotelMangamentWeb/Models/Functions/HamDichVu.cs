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
    //Hàm Dịch vụ
    public class HamDichVu
    {
        private hotelDBEntities db;

        public HamDichVu()
        {
            db = new hotelDBEntities();
        }
        //IQueryable được dành cho các nhà cung cấp truy vấn triển khai
        public IQueryable<DichVu> DichVus
        {
            get { return db.DichVus; }
        }

        //Thêm các thông tin về dịch vụ vào trong DB
        public string Insert(DichVu model)
        {
            db.DichVus.Add(model);
            db.SaveChanges();
            return model.MaDichVu;
        }

        //Cập nhật các thông tin về dịch vụ trong DB
        public string Update(DichVu model)
        {
            DichVu dbEntry = db.DichVus.Find(model.MaDichVu);
            if (dbEntry == null)
            {
                return null;
            }
            dbEntry.MaDichVu = model.MaDichVu;
            dbEntry.TenDichVu = model.TenDichVu;
            dbEntry.GiaDichVu = model.GiaDichVu;
            db.SaveChanges();
            return model.MaDichVu;
        }

        //Xóa các thông tin về dịch vụ khỏi DB
        public string Delete(string MaDichVu)
        {
            DichVu dbEntry = db.DichVus.Find(MaDichVu);
            if (dbEntry == null)
            {
                return null;
            }
            db.DichVus.Remove(dbEntry);
            db.SaveChanges();
            return MaDichVu;
        }
    }
}