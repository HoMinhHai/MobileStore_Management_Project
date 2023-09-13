using DAMH_MuaBanVaSuaChuaDienThoai.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DAMH_MuaBanVaSuaChuaDienThoai.Controllers
{
    public class KhachHangController : Controller
    {
        QuanLyDienThoaiDataContext db = new QuanLyDienThoaiDataContext();
        // GET: KhachHang
        public ActionResult Index()
        {
            return View(db.KHACHHANGs.ToList());
        }

        public ActionResult Add(string KH_TEN, string KH_DIACHI, string KH_SDT, string KH_GIOITINH)
        {
            try
            {
                KHACHHANG khachHang = new KHACHHANG();

                khachHang.KH_SDT = KH_SDT;
                khachHang.KH_TEN = KH_TEN;
                khachHang.KH_DIACHI = KH_DIACHI;
                khachHang.KH_GIOITINH = bool.Parse(KH_GIOITINH);

                db.KHACHHANGs.InsertOnSubmit(khachHang);
                db.SubmitChanges();

            }
            catch (Exception)
            {
                throw;
            }

            return PartialView("_ListKhachHang", db.KHACHHANGs);
        }

        public ActionResult Update(string KH_TEN, string KH_DIACHI, string KH_SDT, string KH_GIOITINH)
        {
            try
            {
                KHACHHANG khachHang = db.KHACHHANGs.First(t => t.KH_SDT.Equals(KH_SDT));

                khachHang.KH_TEN = KH_TEN;
                khachHang.KH_DIACHI = KH_DIACHI;
                khachHang.KH_GIOITINH = bool.Parse(KH_GIOITINH);

                db.SubmitChanges();
            }
            catch (Exception)
            {
                throw;
            }

            return PartialView("_ListKhachHang", db.KHACHHANGs);
        }
        public ActionResult Remove(string KH_SDT)
        {
            try
            {
                KHACHHANG khachHang = db.KHACHHANGs.First(t => t.KH_SDT.Equals(KH_SDT));

                db.KHACHHANGs.DeleteOnSubmit(khachHang);
                db.SubmitChanges();
            }
            catch (Exception)
            {
                throw;
            }

            return PartialView("_ListKhachHang", db.KHACHHANGs);
        }
    }
}