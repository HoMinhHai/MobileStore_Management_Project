using DAMH_MuaBanVaSuaChuaDienThoai.Models;
using System;
using System.Collections.Generic;
using System.Data.Linq.SqlClient;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Mvc;

namespace DAMH_MuaBanVaSuaChuaDienThoai.Controllers
{
    public class SanPhamController : Controller
    {
        QuanLyDienThoaiDataContext db = new QuanLyDienThoaiDataContext();
        Shared shared = new Shared();

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult _ListSanPham(int pLoaiSP)
        {
            var list = (from sp in db.SANPHAMs
                        join cng in db.CAPNHATGIAs on sp.MASP equals cng.MASP into G1
                        join h in db.HANGs on sp.IDHANG equals h.IDHANG
                        join ctn in db.CHITIETNHAPs on sp.MASP equals ctn.MASP into G2
                        join ctx in db.CHITIETXUATs on sp.MASP equals ctx.MASP into G3
                        join a in db.ANHSPs on sp.MASP equals a.MASP into G4
                        where sp.LSP_MA.Equals(pLoaiSP)// && SqlMethods.Like(sp.TENSP, "%" + pTenSP + "%")
                        orderby sp.TENSP ascending
                        select new QuanLyDienThoai()
                        {
                            SP_Ma = G1.FirstOrDefault().MASP,
                            SP_Ten = sp.TENSP,
                            SP_GiaNhap = shared.AddCommas(int.Parse(sp.GIANHAPSP.ToString())),
                            SP_GiaBan = shared.AddCommas(int.Parse(G1.FirstOrDefault().GIABAN.ToString())),
                            H_TenHang = h.TENHANG,
                            SP_Anh = G4.FirstOrDefault().ANH_URL,
                            SP_DungLuong = sp.DUNGLUONG,
                            SP_SoLuong = int.Parse(sp.SOLUONGSP.ToString()),
                            SP_SoLuongMua = G2.Sum(c => c.SOLUONGNHAP).ToString(),
                            SP_SoLuongBan = G3.Sum(c => c.SOLUONGXUAT).ToString()

                        }).ToList();
            Session["LSP"] = pLoaiSP;
            return PartialView("_ListSanPham", list);
        }

        private string createMaSP()
        {
            var list = db.SANPHAMs.ToList();
            int max = 0;
            foreach (var item in list)
            {
                int tam = int.Parse(item.MASP.Substring(2, 4));
                if (tam > max)
                {
                    max = tam;
                }
            }
            max++;
            string ma = max.ToString();
            while (ma.Length < 4)
            {
                ma = "0" + ma;
            }

            return "SP" + ma;
        }

        public ActionResult AddNew()
        {

            return View();
        }

        private List<CT_HDNhap> Query_Detail(string pMaPN)
        {
            List<CT_HDNhap> list = (from ct in db.CHITIETNHAPs
                                    join s in db.SANPHAMs on ct.MASP equals s.MASP
                                    where ct.PN_MA == pMaPN
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
            return list;
        }

        private HoaDonNhap Query_Order(string pMaPN)
        {
            HoaDonNhap hoaDon = (from x in db.PHIEUNHAPs
                                 join n in db.NHANVIENs on x.NV_ID equals n.NV_ID
                                 join p in db.NHAPHANPHOIs on x.NPP_MA equals p.NPP_MA
                                 where x.PN_MA.Equals(pMaPN)
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
            return hoaDon;
        }

        private void AddOrder(out string pMaPN)
        {
            PHIEUNHAP phieuNhap = new PHIEUNHAP();
            pMaPN = shared.CreateMaPhieuNhap();
            phieuNhap.PN_MA = pMaPN;

            phieuNhap.NPP_MA = db.NHAPHANPHOIs.FirstOrDefault().NPP_MA;
            phieuNhap.NV_ID = db.NHANVIENs.FirstOrDefault().NV_ID;
            phieuNhap.PN_NGAYNHAP = DateTime.Now;
            phieuNhap.PN_TONGTIEN = 0;
            phieuNhap.PN_TINHTRANG = false;

            db.PHIEUNHAPs.InsertOnSubmit(phieuNhap);
            db.SubmitChanges();
        }

        public ActionResult UpdateOrder(string pMaPN, string pMaNPP, string pNV)
        {
            if (db.PHIEUNHAPs.Where(p => p.PN_TINHTRANG == false).Count() == 0)
            {
                AddOrder(out pMaPN);
            }
            else
            {
                PHIEUNHAP pn = db.PHIEUNHAPs.First(p => p.PN_TINHTRANG == false);
                pMaPN = pn.PN_MA;
                if(pNV != null && pMaNPP != null)
                {
                    try
                    {
                        PHIEUNHAP nhap = db.PHIEUNHAPs.Where(p => p.PN_MA == pMaPN).Single();
                        nhap.NV_ID = int.Parse(pNV);
                        nhap.NPP_MA = pMaNPP;

                        nhap.NHANVIEN = db.NHANVIENs.First(n => n.NV_ID == nhap.NV_ID);
                        nhap.NHAPHANPHOI = db.NHAPHANPHOIs.First(n => n.NPP_MA.Equals(nhap.NPP_MA));

                        db.SubmitChanges();
                    }
                    catch
                    {
                        throw;
                    }
                }    
            }

            List<CT_HDNhap> list = Query_Detail(pMaPN);

            HoaDonNhap hoaDon = Query_Order(pMaPN);

            ViewBag.HoaDon = hoaDon;
            ViewBag.listNhanVien = db.NHANVIENs.ToList();
            ViewBag.listNhaPhanPhoi = db.NHAPHANPHOIs.ToList();
            ViewBag.listSanPham = db.SANPHAMs.ToList();

            
            return PartialView("_NhapHang", list);
        }

        public ActionResult ComfirmOrder(string pMaPN)
        {
            try
            {
                if(db.CHITIETNHAPs.Where(c => c.PN_MA.Equals(pMaPN)).Count() == 0)
                {
                    throw new Exception();
                }    

                PHIEUNHAP nhap = db.PHIEUNHAPs.Where(p => p.PN_MA == pMaPN).Single();

                nhap.PN_TINHTRANG = true;

                nhap.NHANVIEN = db.NHANVIENs.First(n => n.NV_ID == nhap.NV_ID);
                nhap.NHAPHANPHOI = db.NHAPHANPHOIs.First(n => n.NPP_MA.Equals(nhap.NPP_MA));

                db.SubmitChanges();
            }
            catch
            {
                throw;
            }

            AddOrder(out pMaPN);

            List<CT_HDNhap> list = Query_Detail(pMaPN);
            HoaDonNhap hoaDon = Query_Order(pMaPN);
            ViewBag.HoaDon = hoaDon;

            ViewBag.listNhanVien = db.NHANVIENs.ToList();
            ViewBag.listNhaPhanPhoi = db.NHAPHANPHOIs.ToList();
            ViewBag.listSanPham = db.SANPHAMs.ToList();

            return PartialView("_NhapHang", list);
        }

        public ActionResult DeleteOrder(string pMaPN)
        {
            try
            {
                // Xóa chi tiết trước
                List<CHITIETNHAP> list1 = db.CHITIETNHAPs.Where(c =>  c.PN_MA.Equals(pMaPN)).ToList();
                int count = list1.Count;
                while (count > 0)
                {
                    db.CHITIETNHAPs.DeleteOnSubmit(list1.First());
                    db.SubmitChanges();
                    count--;
                }

                // Xóa đơn nhập sau
                db.PHIEUNHAPs.DeleteOnSubmit(db.PHIEUNHAPs.First(n => n.PN_MA.Equals(pMaPN)));
                db.SubmitChanges();
            }
            catch
            {
                throw;
            }
            AddOrder(out pMaPN);

            List<CT_HDNhap> list = Query_Detail(pMaPN);
            HoaDonNhap hoaDon = Query_Order(pMaPN);
            ViewBag.HoaDon = hoaDon;

            ViewBag.listNhanVien = db.NHANVIENs.ToList();
            ViewBag.listNhaPhanPhoi = db.NHAPHANPHOIs.ToList();
            ViewBag.listSanPham = db.SANPHAMs.ToList();

            return PartialView("_NhapHang", list);
        }

        public ActionResult AddNewProductOrder(string pMaPN, string pMaSP, string SoLuong, string GiaNhap)
        {
            try
            {
                CHITIETNHAP chiTiet = new CHITIETNHAP();
                chiTiet.PN_MA = pMaPN;
                chiTiet.MASP = pMaSP;
                chiTiet.SOLUONGNHAP = int.Parse(SoLuong);
                chiTiet.GIANHAP = long.Parse(GiaNhap);

                db.CHITIETNHAPs.InsertOnSubmit(chiTiet);
                db.SubmitChanges();
            }
            catch
            {
                throw;
            }

            List<CT_HDNhap> list = Query_Detail(pMaPN);
            HoaDonNhap hoaDon = Query_Order(pMaPN);
            ViewBag.HoaDon = hoaDon;

            ViewBag.listNhanVien = db.NHANVIENs.ToList();
            ViewBag.listNhaPhanPhoi = db.NHAPHANPHOIs.ToList();
            ViewBag.listSanPham = db.SANPHAMs.ToList();

            return PartialView("_NhapHang", list);
        }
    }
}