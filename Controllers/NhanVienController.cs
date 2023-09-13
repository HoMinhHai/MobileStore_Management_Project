using DAMH_MuaBanVaSuaChuaDienThoai.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DAMH_MuaBanVaSuaChuaDienThoai.Controllers
{
    public class NhanVienController : Controller
    {
        QuanLyDienThoaiDataContext db = new QuanLyDienThoaiDataContext();

        public ActionResult Index()
        {
            return View(db.NHANVIENs.ToList());
        }

        public ActionResult Add(string NV_HOTEN, string NV_DIACHI, string NV_GIOITINH, string NV_NGAYSINH, 
                                string NV_SDT, string NV_TINHTRANG)
        {
            try
            {
                NHANVIEN nhanVien = new NHANVIEN();

                nhanVien.CH_DIACHI = "160 Hà Đặc, Trung Mỹ Tây, Quận 12, TP. HCM";
                nhanVien.NV_TEN = NV_HOTEN;
                nhanVien.NV_DIACHI = NV_DIACHI;
                nhanVien.NV_GIOITINH = bool.Parse(NV_GIOITINH);
                nhanVien.NV_NGAYSINH = DateTime.Parse(NV_NGAYSINH);
                nhanVien.NV_SDT = NV_SDT;
                nhanVien.NV_DELETE = bool.Parse(NV_TINHTRANG);

                db.NHANVIENs.InsertOnSubmit(nhanVien);
                db.SubmitChanges();

            }
            catch (Exception)
            {
                throw;
            }

            return PartialView("_ListNhanVien", db.NHANVIENs);
        }

        public ActionResult Update(string NV_ID, string NV_HOTEN, string NV_DIACHI, string NV_GIOITINH, string NV_NGAYSINH,
                                string NV_SDT, string NV_TINHTRANG)
        {
            try
            {
                NHANVIEN nhanVien = db.NHANVIENs.First(t => t.NV_ID.Equals(NV_ID));

                nhanVien.NV_TEN = NV_HOTEN;
                nhanVien.NV_DIACHI = NV_DIACHI;
                nhanVien.NV_GIOITINH = bool.Parse(NV_GIOITINH);
                nhanVien.NV_NGAYSINH = DateTime.Parse(NV_NGAYSINH);
                nhanVien.NV_SDT = NV_SDT;
                nhanVien.NV_DELETE = bool.Parse(NV_TINHTRANG);

                db.SubmitChanges();
            }
            catch (Exception)
            {
                throw;
            }

            return PartialView("_ListNhanVien", db.NHANVIENs);
        }
        public ActionResult Remove(string NV_ID)
        {
            try
            {
                NHANVIEN nhanVien = db.NHANVIENs.First(t => t.NV_ID.Equals(NV_ID));

                db.NHANVIENs.DeleteOnSubmit(nhanVien);
                db.SubmitChanges();
            }
            catch (Exception)
            {
                throw;
            }

            return PartialView("_ListNhanVien", db.NHANVIENs);
        }
    }
}