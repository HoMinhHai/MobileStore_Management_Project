using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DAMH_MuaBanVaSuaChuaDienThoai.Models
{
    public class Connect
    {
        QuanLyDienThoaiDataContext db = new QuanLyDienThoaiDataContext();
        Shared shared = new Shared();
        List<QuanLyDienThoai> sanPhams;
        public List<QuanLyDienThoai> SanPhams { get => sanPhams; set => sanPhams = value; }

        public Connect()
        {
            SanPhams = GetSanPham();
        }
        public List<QuanLyDienThoai> GetSanPham()
        {
            var list = (from sp in db.SANPHAMs
                        join gia in db.CAPNHATGIAs on sp.MASP equals gia.MASP
                        join anh in db.ANHSPs on sp.MASP equals anh.MASP into anh1
                        select new QuanLyDienThoai()
                        {
                            SP_Anh = anh1.FirstOrDefault().ANH_URL,
                            SP_Ma = sp.MASP,
                            SP_Ten = sp.TENSP,
                            SP_GiaBan = gia.GIABAN.ToString(),
                            SP_DungLuong = sp.DUNGLUONG,
                            SP_Loai = sp.LSP_MA.ToString(),
                            SP_TinhTrang = sp.SP_TINHTRANG.ToString()
                        }).ToList();
            return list;
        }
        public List<QuanLyDienThoai> GetGroupBySanPham(int type = -1, string productName = "")
        {
            var maxType = db.SANPHAMs.Max(t => t.LSP_MA);
            var sanPhams = (from sp in db.SANPHAMs
                            join cng in db.CAPNHATGIAs on sp.MASP equals cng.MASP into G1
                            join h in db.HANGs on sp.IDHANG equals h.IDHANG
                            join anh in db.ANHSPs on sp.MASP equals anh.MASP into G6
                            where (type <= maxType && type >= 0 ? sp.LSP_MA == type : sp.LSP_MA <= maxType) && 
                                  (productName.Equals("") ? sp.TENSP != null : sp.TENSP.Equals(productName))

                            group (sp) by sp.TENSP into dt

                            select new
                            {
                                SP_Ten = dt.Key,
                                info = dt.ToList()
                            }).ToList();

            List<QuanLyDienThoai> list = new List<QuanLyDienThoai>();

            foreach (var item in sanPhams)
            {
                QuanLyDienThoai dt = new QuanLyDienThoai()
                {
                    SP_Ten = item.info.FirstOrDefault().TENSP
                };

                foreach (var i in item.info)
                {
                    dt.SP_MaDT1.Add(i.MASP);
                    dt.SP_GiaBans1.Add(shared.AddCommas(int.Parse(i.CAPNHATGIAs.FirstOrDefault().GIABAN.ToString())));
                    dt.SP_DungLuongDT1.Add(i.DUNGLUONG);
                    dt.SP_TinhTrangs1.Add(i.SP_TINHTRANG.ToString());
                    dt.SP_SOLUONGTONKHO1.Add(i.SOLUONGSP.ToString());

                    List<string> kemTheos = new List<string>();
                    foreach (var kemTheo in i.KEMTHEOs)
                    {
                        kemTheos.Add(kemTheo.PHUKIENKEM.TENPKK);
                    }
                    dt.SP_PhuKienKems1.Add(kemTheos);

                    //===============================================

                    List<string> anhs = new List<string>();
                    foreach (var anh in i.ANHSPs)
                    {
                        anhs.Add(anh.ANH_URL.ToString());
                    }
                    dt.SP_AnhDT1.Add(anhs);

                    //===============================================

                    List<ThongSoKT> thongSos = new List<ThongSoKT>();
                    foreach (var thongSo in i.SP_CO_KTs)
                    {
                        thongSos.Add(new ThongSoKT() {
                            TS_Ten = thongSo.TS_KYTHUAT.KT_TEN,
                            TS_GiaTri = thongSo.TS_KYTHUAT.KT_GIATRI
                        });
                    }
                    dt.SP_ThongSos1.Add(thongSos);
                }
                list.Add(dt);
            }
            return list;
        }
        public List<QuanLyDienThoai> GetGroupByBinhLuan(string id) {
            var binhLuans = (from sp in db.SANPHAMs
                             join bl in db.BINHLUANs on sp.MASP equals bl.MASP into G1
                             from t1 in G1.DefaultIfEmpty()
                             join kh in db.KHACHHANGs on t1.KH_SDT equals kh.KH_SDT into G2
                             join ht in db.HOTROs on t1.BL_ID equals ht.BL_ID into G3
                             where sp.MASP.Equals(id)
                             group (t1) by t1.BL_ID into bls
                             select new
                             {
                                 BL_ID = bls.Key,
                                 info = bls.ToList()
                             }).ToList();


            List<QuanLyDienThoai> list = new List<QuanLyDienThoai>();

            foreach (var binhLuan in binhLuans)
            {
                QuanLyDienThoai dt = new QuanLyDienThoai()
                {
                    BL_ID = binhLuan.info.FirstOrDefault().BL_ID.ToString()
                };

                dt.BL_NOIDUNG = binhLuan.info[0].BL_NOIDUNG;
                dt.BL_THOIGIAN = DateTime.Parse(binhLuan.info[0].BL_THOIGIAN.ToString()).ToString("HH:mm");
                dt.KH_TENKH = binhLuan.info[0].KHACHHANG.KH_TEN;
                dt.SP_Ma = binhLuan.info[0].MASP;

                foreach (var info in binhLuan.info)
                {
                    foreach (var item in info.HOTROs)
                    {
                        dt.BL_BINHLUANS1.Add(new BinhLuan()
                        {
                            NoiDung = item.HT_NOIDUNG,
                            ThoiGian = DateTime.Parse(item.HT_THOIGIAN.ToString()).ToString("HH:mm")
                        });
                    }

                }

                list.Add(dt);
            }

            return list;
        }


        // ================================ Admin ================================
        public bool dangNhap(string user, string pass)
        {
            var info = (from c in db.CUAHANGs
                       select new
                       {
                           tk = c.TAIKHOAN,
                           mk = c.MATKHAU
                       }).Single();
            if (user.Equals(info.tk) && pass.Equals(info.mk))
            {
                return true;
            }
            return false;
        }
    }
}