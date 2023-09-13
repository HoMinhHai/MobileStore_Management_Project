using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PagedList;
using DAMH_MuaBanVaSuaChuaDienThoai.Models;
using System.Xml.Linq;
using System.Web.Helpers;
using Newtonsoft.Json;

namespace DAMH_MuaBanVaSuaChuaDienThoai.Controllers
{
    public class StoreController : Controller
    {
        QuanLyDienThoaiDataContext DB = new QuanLyDienThoaiDataContext();
        Connect connect = new Connect();
        Shared shared = new Shared();
        List<QuanLyDienThoai> phones = new List<QuanLyDienThoai>();
        List<QuanLyDienThoai> accessories = new List<QuanLyDienThoai>();
        static string tenSP = "";
        // GET: Home

        public ActionResult Index()
        {
            return PartialView("_Layout");
        }

        public ActionResult Home()
        {
            phones = connect.GetGroupBySanPham(1);
            accessories = connect.GetGroupBySanPham(2);

            ViewBag.Phones = phones.Take(4);
            ViewBag.Accessories = accessories.Take(4);

            return PartialView("_Home");
        }
        public ActionResult Search(int? page, string SP_TEN)
        {
            tenSP = SP_TEN == null ? tenSP : SP_TEN;
            int pageSize = 8;
            int pageIndex = 1;
            pageIndex = page.HasValue ? Convert.ToInt32(page) : 1;
            IPagedList<QuanLyDienThoai> sanPham = null;

            List<QuanLyDienThoai> list = connect.GetGroupBySanPham().Where(t => t.SP_Ten.Contains(tenSP)).ToList();

            sanPham = list.ToPagedList(pageIndex, pageSize);
            ViewBag.SP_TEN = tenSP;
            ViewBag.SoLuong = list.Count;

            return PartialView("_Search", sanPham);
        }
        public ActionResult Phone(int? page)
        {
            int pageSize = 8;
            int pageIndex = 1;
            pageIndex = page.HasValue ? Convert.ToInt32(page) : 1;
            IPagedList<QuanLyDienThoai> sanPham = null;

            List<QuanLyDienThoai> list = connect.GetGroupBySanPham(1);

            sanPham = list.ToPagedList(pageIndex, pageSize);

            return PartialView("_Phone", sanPham);
        }
        public ActionResult Repair()
        {
            return View("_Error");
        }
        public ActionResult OldPhone()
        {
            return View("_Error");
        }
        public ActionResult Accessory(int? page)
        {
            int pageSize = 8;
            int pageIndex = 1;
            pageIndex = page.HasValue ? Convert.ToInt32(page) : 1;
            IPagedList<QuanLyDienThoai> sanPham = null;

            List<QuanLyDienThoai> list = connect.GetGroupBySanPham(2);

            sanPham = list.ToPagedList(pageIndex, pageSize);

            return PartialView("_Accessory",sanPham);
        }
        public ActionResult ProductDetail(string id, int index)
        {
            SANPHAM product = DB.SANPHAMs.Single(t => t.MASP.Equals(id));

            if (product.LSP_MA.Equals(1))
            {
                ViewBag.FullInfo = connect.GetGroupBySanPham().Single(t => t.SP_Ten.Equals(product.TENSP));
            }
            else if (product.LSP_MA.Equals(2))
            {
                ViewBag.FullInfo = connect.GetGroupBySanPham(2).Single(t => t.SP_Ten.Equals(product.TENSP));

            }
            ViewBag.SuggestProduct = shared.ProductCarousel(connect.GetGroupBySanPham(2), 4);
            try
            {
                ViewBag.Comments = connect.GetGroupByBinhLuan(id);

            }
            catch (Exception)
            {

            }
            ViewBag.Index = index;
            ViewBag.Id = id;

            return PartialView("_ProductDetail");
        }
        public ActionResult UserCart()
        {
            return PartialView("_UserCart");
        }
        public ActionResult UpdatePhieuXuat(string orderDetail)
        {
            dynamic order = JsonConvert.DeserializeObject(orderDetail);

            string KH_SDT = order.customer.phone;
            string KH_GIOITINH = order.customer.gender;
            try
            {
                KHACHHANG khachHang = DB.KHACHHANGs.Single(t => t.KH_SDT.Equals(KH_SDT));
                // Cập nhập thông tin khách hàng
                khachHang.KH_GIOITINH = bool.Parse(KH_GIOITINH);
                khachHang.KH_DIACHI = order.customer.address;
                khachHang.KH_TEN = order.customer.name;
                DB.SubmitChanges();
            }
            catch
            {
                // Tạo tài khoản cho khách hàng
                KHACHHANG khachHang = new KHACHHANG();
                khachHang.KH_SDT = order.customer.phone;
                khachHang.KH_GIOITINH = bool.Parse(order.customer.gender);
                khachHang.KH_DIACHI = order.customer.address;
                khachHang.KH_TEN = order.customer.name;

                DB.KHACHHANGs.InsertOnSubmit(khachHang);
                DB.SubmitChanges();
            }

            PHIEUXUAT phieu = new PHIEUXUAT();
            string maPX = shared.CreateMaHoaDon();

            try
            {
                // Tạo phiếu xuất
                phieu.PX_MA = maPX;
                phieu.KH_SDT = order.customer.phone;
                phieu.PX_NGAYXUAT = DateTime.Now;
                phieu.PX_TONGTIEN = 0;
                phieu.PX_TINHTRANG = false;
                phieu.NV_ID = 2;
                DB.PHIEUXUATs.InsertOnSubmit(phieu);
                DB.SubmitChanges();

                // Thêm chi tiết phiếu xuất
                for (int i = 0; i < order.products.Count; i++)
                {
                    string maSP = order.products[i].id;
                    string soLuong = order.products[i].quantity;
                    CHITIETXUAT ctPhieu = new CHITIETXUAT();

                    ctPhieu.MASP = maSP;
                    ctPhieu.PX_MA = maPX;
                    ctPhieu.SOLUONGXUAT = int.Parse(soLuong);

                    DB.CHITIETXUATs.InsertOnSubmit(ctPhieu);
                    DB.SubmitChanges();
                }
            }
            catch
            {
                //Cần xóa chi tiết xuất của phiếu xuất này trước
                try
                {
                    while (DB.CHITIETXUATs.ToList().Count > 0)
                    {
                        DB.CHITIETXUATs.DeleteOnSubmit(DB.CHITIETXUATs.Where(n => n.PX_MA == maPX).Single());
                    }
                }
                catch (Exception)
                {
                }

                // Xóa hóa đơn xuất vừa tạo khi chi tiết xảy ra lỗi
                DB.PHIEUXUATs.DeleteOnSubmit(DB.PHIEUXUATs.Where(n => n.PX_MA == maPX).Single());
                DB.SubmitChanges();
            }

            return UserCart();
        }

        public ActionResult AddComment(string BL_NOIDUNG, string MASP, string KH_SDT, string KH_GIOITINH, string KH_TEN)
        {
            BINHLUAN bl = new BINHLUAN();
            bl.KH_SDT = KH_SDT;
            bl.MASP = MASP;
            bl.BL_NOIDUNG = BL_NOIDUNG;
            bl.BL_THOIGIAN = DateTime.Now;

            DB.BINHLUANs.InsertOnSubmit(bl);
            try
            {
                KHACHHANG khachHang = DB.KHACHHANGs.Single(t => t.KH_SDT.Equals(KH_SDT));
                khachHang.KH_GIOITINH = bool.Parse(KH_GIOITINH);
                khachHang.KH_TEN = KH_TEN;

            }
            catch
            {
                KHACHHANG khachHang = new KHACHHANG();

                khachHang.KH_SDT = KH_SDT;
                khachHang.KH_TEN = KH_TEN;
                khachHang.KH_DIACHI = "Chưa cập nhập";
                khachHang.KH_GIOITINH = bool.Parse(KH_GIOITINH);

                DB.KHACHHANGs.InsertOnSubmit(khachHang);
            }
            DB.SubmitChanges();

            var comments = connect.GetGroupByBinhLuan(MASP);

            return PartialView("CommentPartial", comments);
        }
    }
}