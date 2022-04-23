using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;


namespace HotelMangamentWeb.Models.ViewModels
{
    public class TaiKhoanDangNhapView
    {
        [Required(ErrorMessage = "Không được để trống Tài Khoản")]
        public string TenTaiKhoan { get; set; }

        [Required(ErrorMessage = "Không được để trống Mật Khẩu")]
        public string MatKhau { get; set; }

        public bool TuDongDangNhap { get; set; }

    }
}