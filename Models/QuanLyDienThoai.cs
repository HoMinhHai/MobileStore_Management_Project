using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DAMH_MuaBanVaSuaChuaDienThoai.Models
{
    public class QuanLyDienThoai
    {
        public string SP_Ma { get; set; }
        public string SP_Ten { get; set; }
        public string SP_GiaNhap { get; set; }
        public string SP_GiaBan { get; set; }
        public string SP_DungLuong { get; set; }
        public string SP_KhuyenMai { get; set; }
        public string SP_DiKem { get; set; }
        public string SP_Anh { get; set; }
        public string SP_PhuKienKem { get; set; }
        public string TSKT_Ten { get; set; }
        public string TSKT_GiaTri { get; set; }
        public int SP_SoLuong { get; set; }

        public string SP_SoLuongMua { get; set; }
        public string SP_SoLuongBan { get; set; }
        public string SP_Loai { get; set; }
        public string SP_TinhTrang { get; set; }
        public string H_TenHang { get; set; }
        public string KH_TENKH { get; set; }
        public string BL_ID { get; set; }
        public string BL_NOIDUNG { get; set; }
        public string BL_THOIGIAN { get; set; }
        public string HT_THOIGIAN { get; set; }
        public string HT_NOIDUNG { get; set; }


        // Thuận cute
        List<string> SP_DungLuongDT = new List<string>();
        List<string> SP_MaDT = new List<string>();
        List<List<string>> SP_AnhDT = new List<List<string>>();
        List<string> SP_GiamGiaDT = new List<string>();
        List<string> SP_CapNhatGiaDT = new List<string>();
        List<List<string>> SP_PhuKienKems = new List<List<string>>();
        List<List<ThongSoKT>> SP_ThongSos = new List<List<ThongSoKT>>();
        List<string> SP_GiaNhaps = new List<string>();
        List<string> SP_GiaBans = new List<string>();
        List<string> SP_TinhTrangs = new List<string>();
        List<string> SP_SOLUONGTONKHO = new List<string>();
        List<BinhLuan> BL_BINHLUANS = new List<BinhLuan>();

        public List<string> SP_DungLuongDT1 { get => SP_DungLuongDT; set => SP_DungLuongDT = value; }
        public List<string> SP_GiamGiaDT1 { get => SP_GiamGiaDT; set => SP_GiamGiaDT = value; }
        public List<string> SP_CapNhatGiaDT1 { get => SP_CapNhatGiaDT; set => SP_CapNhatGiaDT = value; }

        // 04 - 12
        public List<List<string>> SP_AnhDT1 { get => SP_AnhDT; set => SP_AnhDT = value; }
        public List<List<string>> SP_PhuKienKems1 { get => SP_PhuKienKems; set => SP_PhuKienKems = value; }
        public List<List<ThongSoKT>> SP_ThongSos1 { get => SP_ThongSos; set => SP_ThongSos = value; }
        public List<string> SP_TinhTrangs1 { get => SP_TinhTrangs; set => SP_TinhTrangs = value; }
        public List<string> SP_MaDT1 { get => SP_MaDT; set => SP_MaDT = value; }
        public List<string> SP_GiaNhaps1 { get => SP_GiaNhaps; set => SP_GiaNhaps = value; }
        public List<string> SP_GiaBans1 { get => SP_GiaBans; set => SP_GiaBans = value; }
        public List<string> SP_SOLUONGTONKHO1 { get => SP_SOLUONGTONKHO; set => SP_SOLUONGTONKHO = value; }
        public List<BinhLuan> BL_BINHLUANS1 { get => BL_BINHLUANS; set => BL_BINHLUANS = value; }
    }

    public class ThongSoKT
    {
        string tS_Ten, tS_GiaTri;

        public string TS_Ten { get => tS_Ten; set => tS_Ten = value; }
        public string TS_GiaTri { get => tS_GiaTri; set => tS_GiaTri = value; }
    }

    public class BinhLuan
    {
        string noiDung, thoiGian;

        public string NoiDung { get => noiDung; set => noiDung = value; }
        public string ThoiGian { get => thoiGian; set => thoiGian = value; }
    }

    public class KhachHang
    {
        public string HoTen { set; get; }
        public string GioiTinh { set; get; }
        public string SDT { set; get; }
        public string DiaChi { set; get; }

        public string SoLanMua { set; get; }
    }

    public class NhaPhanPhoi
    {
        public string MaNPP { set; get; }
        public string TenNPP { set; get; }
        public string SDT { set; get; }
        public string DiaChi { set; get; }
        public bool NhapHang { set; get; }
    }
}