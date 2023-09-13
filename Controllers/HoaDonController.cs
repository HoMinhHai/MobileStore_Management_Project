using DAMH_MuaBanVaSuaChuaDienThoai.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DAMH_MuaBanVaSuaChuaDienThoai.Controllers
{
    public class HoaDonController : Controller
    {
        QuanLyDienThoaiDataContext db = new QuanLyDienThoaiDataContext();
        Connect connect = new Connect();
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult HoaDonNhap()
        {
            Shared shared = new Shared();
            List<HoaDonNhap> list = (from x in db.PHIEUNHAPs
                                     join n in db.NHANVIENs on x.NV_ID equals n.NV_ID
                                     join p in db.NHAPHANPHOIs on x.NPP_MA equals p.NPP_MA
                                     where x.PN_TINHTRANG == true
                                     select new HoaDonNhap()
                                     {
                                         PN_MA = x.PN_MA,
                                         NPP_MA = p.NPP_MA,
                                         NPP_TEN = p.NPP_TEN,
                                         KH_SDT = x.KH_SDT,
                                         NV_ID = n.NV_ID,
                                         NV_TEN = n.NV_TEN,
                                         PN_NGAYNHAP = DateTime.Parse(x.PN_NGAYNHAP.ToString()),
                                         PN_TONGTIEN = shared.AddCommas_Long(long.Parse(x.PN_TONGTIEN.ToString()))
                                     }).ToList();
            return PartialView("_HoaDonNhap", list);
        }

        public ActionResult ChiTietNhap(string id)
        {
            Shared shared = new Shared();
            List<CT_HDNhap> d = (from ct in db.CHITIETNHAPs
                                 join s in db.SANPHAMs on ct.MASP equals s.MASP
                                 where ct.PN_MA == id
                                 select new CT_HDNhap
                                 {
                                     MASP = ct.MASP,
                                     TENSP = s.TENSP,
                                     ANHSP = s.ANHSPs.FirstOrDefault().ANH_URL,
                                     PN_MA = ct.PN_MA,
                                     SOLUONGXUAT = int.Parse(ct.SOLUONGNHAP.ToString()),
                                     DUNGLUONG = s.DUNGLUONG,
                                     GIA = shared.AddCommas(int.Parse(ct.GIANHAP.ToString()))

                                 }).ToList();

            HoaDonNhap hoaDon = (from x in db.PHIEUNHAPs
                                 join n in db.NHANVIENs on x.NV_ID equals n.NV_ID
                                 join p in db.NHAPHANPHOIs on x.NPP_MA equals p.NPP_MA
                                 where x.PN_MA.Equals(id)
                                 select new HoaDonNhap()
                                 {
                                     PN_MA = x.PN_MA,
                                     NPP_MA = p.NPP_MA,
                                     NPP_TEN = p.NPP_TEN,
                                     KH_SDT = x.KH_SDT,
                                     NV_ID = n.NV_ID,
                                     NV_TEN = n.NV_TEN,
                                     PN_NGAYNHAP = DateTime.Parse(x.PN_NGAYNHAP.ToString()),
                                     PN_TONGTIEN = shared.AddCommas_Long(long.Parse(x.PN_TONGTIEN.ToString()))
                                 }).Single();
            ViewBag.HoaDon = hoaDon;
            return PartialView("_ChiTietNhap", d);
        }

        public ActionResult HoaDonXuat()
        {
            Shared shared = new Shared();
            List<DonHang> list = (from x in db.PHIEUXUATs
                                  join n in db.NHANVIENs on x.NV_ID equals n.NV_ID
                                  where x.PX_TINHTRANG.Value == true
                                  select new DonHang()
                                  {
                                      PX_MA = x.PX_MA,
                                      KH_SDT = x.KH_SDT,
                                      NV_ID = n.NV_ID,
                                      NV_TEN = n.NV_TEN,
                                      PX_NGAYXUAT = DateTime.Parse(x.PX_NGAYXUAT.ToString()),
                                      PX_TONGTIEN = shared.AddCommas_Long(long.Parse(x.PX_TONGTIEN.ToString())),
                                      PX_TINHTRANG = bool.Parse(x.PX_TINHTRANG.ToString())
                                  }).ToList();
            return PartialView("_HoaDonXuat", list);
        }

        public ActionResult ChiTietXuat(string id, DateTime ngayXuat)
        {
            Shared shared = new Shared();
            List<CT_DonHang> model = (from ct in db.CHITIETXUATs
                                      join s in db.SANPHAMs on ct.MASP equals s.MASP
                                      join c in db.CAPNHATGIAs on s.MASP equals c.MASP
                                      where ct.PX_MA == id //&& c.NGAYCAPNHAT < ngayXuat
                                      select new CT_DonHang
                                      {
                                          MASP = ct.MASP,
                                          TENSP = s.TENSP,
                                          ANHSP = s.ANHSPs.FirstOrDefault().ANH_URL,
                                          PX_MA = ct.PX_MA,
                                          SOLUONGXUAT = int.Parse(ct.SOLUONGXUAT.ToString()),
                                          DUNGLUONG = s.DUNGLUONG,
                                          GIA = shared.AddCommas(int.Parse(c.GIABAN.ToString()))

                                      }).ToList();
            DonHang donHang = (from x in db.PHIEUXUATs
                               join n in db.NHANVIENs on x.NV_ID equals n.NV_ID
                               where x.PX_TINHTRANG.Value == true && x.PX_MA == id
                               select new DonHang()
                               {
                                   PX_MA = x.PX_MA,
                                   KH_SDT = x.KH_SDT,
                                   NV_ID = n.NV_ID,
                                   NV_TEN = n.NV_TEN,
                                   PX_NGAYXUAT = DateTime.Parse(x.PX_NGAYXUAT.ToString()),
                                   PX_TONGTIEN = shared.AddCommas_Long(long.Parse(x.PX_TONGTIEN.ToString())),
                                   PX_TINHTRANG = bool.Parse(x.PX_TINHTRANG.ToString())
                               }).Single();
            ViewBag.HoaDon = donHang;
            return PartialView("_ChiTietXuat", model);
        }
    }
}