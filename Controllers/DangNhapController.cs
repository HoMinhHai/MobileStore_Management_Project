using DAMH_MuaBanVaSuaChuaDienThoai.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DAMH_MuaBanVaSuaChuaDienThoai.Controllers
{
    public class DangNhapController : Controller
    {
        QuanLyDienThoaiDataContext db = new QuanLyDienThoaiDataContext();
        Connect connect = new Connect();
        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Index(string user, string pass)
        {
            if (connect.dangNhap(user, pass))
            {
                return View("_Layout_Admin");
            }
            else
            {
                ViewData["ThongBao"] = "Tài khoản hoặc mật khẩu không hợp lệ!";
            }
            return View();
        }
    }
}