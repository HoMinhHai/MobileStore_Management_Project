using DAMH_MuaBanVaSuaChuaDienThoai.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Mvc;

namespace DAMH_MuaBanVaSuaChuaDienThoai.Controllers
{
    public class DonHangController : Controller
    {
        QuanLyDienThoaiDataContext db = new QuanLyDienThoaiDataContext();
        Shared shared = new Shared();
        // GET: DonHang
        public ActionResult Index()
        {
            Shared shared = new Shared();
            var list = (from x in db.PHIEUXUATs
                        join n in db.NHANVIENs on x.NV_ID equals n.NV_ID
                        where x.PX_TINHTRANG.Value == false
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

            ViewBag.listNhanVien = db.NHANVIENs.ToList();
            ViewBag.listKhachHang = db.KHACHHANGs.ToList();

            return PartialView("Index", list);
        }

        public ActionResult ChiTietDonHang(string id, DateTime ngayXuat)
        {

            var model = (from ct in db.CHITIETXUATs
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
                               where x.PX_TINHTRANG.Value == false && x.PX_MA == id
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
            Session["MPX"] = donHang.PX_MA;
            ViewBag.ListSanPham = db.SANPHAMs;

            return PartialView("_ChiTietDonHangPartial", model);
        }
        
        public ActionResult Add(string KH_SDT, string NV_ID)
        {
            try
            {
                PHIEUXUAT phieuXuat = new PHIEUXUAT();

                phieuXuat.PX_MA = shared.CreateMaHoaDon();
                phieuXuat.KH_SDT = KH_SDT;
                phieuXuat.NV_ID = int.Parse(NV_ID);
                phieuXuat.PX_NGAYXUAT = DateTime.Now;
                phieuXuat.PX_TONGTIEN = 0;
                phieuXuat.PX_TINHTRANG = false;

                db.PHIEUXUATs.InsertOnSubmit(phieuXuat);
                db.SubmitChanges();
            }
            catch (Exception)
            {
                throw;
            }

            var list = (from x in db.PHIEUXUATs
                        join n in db.NHANVIENs on x.NV_ID equals n.NV_ID
                        where x.PX_TINHTRANG.Value == false
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

            return PartialView("_listDonHangPartial", list);
        }
        public ActionResult Remove(string id)
        {
            try
            {
                var details = (from ct in db.CHITIETXUATs
                               where ct.PX_MA.Equals(id)
                               select ct);

                db.CHITIETXUATs.DeleteAllOnSubmit(details);

                PHIEUXUAT phieuXuat = db.PHIEUXUATs.First(t => t.PX_MA.Equals(id));

                db.PHIEUXUATs.DeleteOnSubmit(phieuXuat);
                db.SubmitChanges();
            }
            catch (Exception)
            {
                throw;
            }

            var list = (from x in db.PHIEUXUATs
                        join n in db.NHANVIENs on x.NV_ID equals n.NV_ID
                        where x.PX_TINHTRANG.Value == false
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
            return PartialView("_listDonHangPartial", list);
        }

        public ActionResult Confirm(string MAPX)
        {

            PHIEUXUAT px = db.PHIEUXUATs.First(p => p.PX_MA.Equals(MAPX));
            px.PX_TINHTRANG = true;

            db.SubmitChanges();


            var list = (from x in db.PHIEUXUATs
                        join n in db.NHANVIENs on x.NV_ID equals n.NV_ID
                        where x.PX_TINHTRANG.Value == false
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
            return PartialView("_listDonHangPartial", list);
        }

        public ActionResult AddDetail(string MAPX, string MASP_MOI, string SOLUONGXUAT_MOI)
        {
            try
            {
                CHITIETXUAT chiTiet = db.CHITIETXUATs.First(t => t.MASP.Equals(MASP_MOI) && t.PX_MA.Equals(MAPX));
                chiTiet.SOLUONGXUAT = int.Parse(SOLUONGXUAT_MOI);

                db.SubmitChanges();
            }
            catch (Exception)
            {
                CHITIETXUAT chiTiet = new CHITIETXUAT();
                chiTiet.PX_MA = MAPX;
                chiTiet.MASP = MASP_MOI;
                chiTiet.SOLUONGXUAT = int.Parse(SOLUONGXUAT_MOI);

                db.CHITIETXUATs.InsertOnSubmit(chiTiet);
                db.SubmitChanges();
            }

            var model = (from ct in db.CHITIETXUATs
                         join s in db.SANPHAMs on ct.MASP equals s.MASP
                         join c in db.CAPNHATGIAs on s.MASP equals c.MASP
                         where ct.PX_MA == MAPX //&& c.NGAYCAPNHAT < ngayXuat
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
            return PartialView("_ListChiTietDonHang", model);
        }
        public ActionResult UpdateDetail(string MAPX, string MASP_CU, string SOLUONGXUAT_CU)
        {
            try
            {
                CHITIETXUAT chiTiet = db.CHITIETXUATs.First(t => t.MASP.Equals(MASP_CU) && t.PX_MA.Equals(MAPX));
                chiTiet.SOLUONGXUAT = int.Parse(SOLUONGXUAT_CU);

                db.SubmitChanges();
            }
            catch (Exception)
            {
                throw;
            }

            var model = (from ct in db.CHITIETXUATs
                         join s in db.SANPHAMs on ct.MASP equals s.MASP
                         join c in db.CAPNHATGIAs on s.MASP equals c.MASP
                         where ct.PX_MA == MAPX //&& c.NGAYCAPNHAT < ngayXuat
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
            return PartialView("_ListChiTietDonHang", model);
        }

        public ActionResult RemoveDetail(string id, string pMaPX)
        {
            try
            {
                // Cập nhật lại tổng tiền 
                var ngayXuat = db.PHIEUXUATs.First(sp => (sp.PX_MA == pMaPX)).PX_NGAYXUAT;
                int gia = int.Parse(db.CAPNHATGIAs.First(sp => (sp.MASP == id && sp.NGAYCAPNHAT < ngayXuat)).GIABAN.ToString());
                int soLuong = int.Parse(db.CHITIETXUATs.First(sp => (sp.MASP == id && sp.PX_MA == pMaPX)).SOLUONGXUAT.ToString());

                PHIEUXUAT px = db.PHIEUXUATs.First(p => p.PX_MA.Equals(pMaPX));
                px.PX_TONGTIEN -= gia * soLuong;
                db.SubmitChanges();


                // Xóa chi tiết
                db.CHITIETXUATs.DeleteOnSubmit(db.CHITIETXUATs.First(sp => (sp.MASP == id && sp.PX_MA == pMaPX)));
                db.SubmitChanges();
            }
            catch
            {
                throw;
            }

            var model = (from ct in db.CHITIETXUATs
                         join s in db.SANPHAMs on ct.MASP equals s.MASP
                         join c in db.CAPNHATGIAs on s.MASP equals c.MASP
                         where ct.PX_MA == pMaPX //&& c.NGAYCAPNHAT < ngayXuat
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
            return PartialView("_ListChiTietDonHang", model);
        }
    }
}