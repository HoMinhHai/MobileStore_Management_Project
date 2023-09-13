using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DAMH_MuaBanVaSuaChuaDienThoai.Models
{
    public class HoaDonNhap
    {
        public HoaDonNhap() { }

        public string PN_MA { get; set; }
        public string KH_SDT { get; set; }
        public string NPP_MA { get; set; }
        public string NPP_TEN { get; set; }
        public int NV_ID { get; set; }
        public string NV_TEN { get; set; }
        public DateTime PN_NGAYNHAP { get; set; }
        public string PN_TONGTIEN { get; set; }

        public List<CT_HDNhap> detail { get; set; }
    }
}