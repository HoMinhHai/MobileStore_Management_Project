using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DAMH_MuaBanVaSuaChuaDienThoai.Models
{
    public class CT_DonHang
    {
        public CT_DonHang() { }

        public string MASP { get; set; }
        public string TENSP { get; set; }
        public string ANHSP { get; set; }
        public string PX_MA { get; set; }
        public int SOLUONGXUAT { get; set; }
        public string GIA { get; set; }
        public string DUNGLUONG { get; set; }
    }
}