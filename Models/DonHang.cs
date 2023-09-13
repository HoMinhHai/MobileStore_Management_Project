using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DAMH_MuaBanVaSuaChuaDienThoai.Models
{
    public class DonHang
    {
        public DonHang()
        {
        }

        public string PX_MA { get; set; }
        public string KH_SDT { get; set; }
        public int NV_ID { get; set; }
        public string NV_TEN { get; set; }
        public DateTime PX_NGAYXUAT { get; set; }
        public string PX_TONGTIEN { get; set; }
        public bool PX_TINHTRANG { get; set; }

        public List<CT_DonHang> detail { get; set; }
    }
}