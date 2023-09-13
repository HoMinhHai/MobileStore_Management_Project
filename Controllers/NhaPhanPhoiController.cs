using DAMH_MuaBanVaSuaChuaDienThoai.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DAMH_MuaBanVaSuaChuaDienThoai.Controllers
{
    public class NhaPhanPhoiController : Controller
    {
        QuanLyDienThoaiDataContext db = new QuanLyDienThoaiDataContext();
        Connect connect = new Connect();
        public ActionResult Index()
        {
            var NhaPhanPhois = (from pp in db.NHAPHANPHOIs
                                select new NhaPhanPhoi
                                {
                                    MaNPP = pp.NPP_MA,
                                    TenNPP = pp.NPP_TEN,
                                    SDT = pp.NPP_SDT,
                                    DiaChi = pp.NPP_DIACHI,
                                    NhapHang = ((from pn in db.PHIEUNHAPs
                                                 where pn.NPP_MA.Equals(pp.NPP_MA)
                                                 select pn).ToList().Count() == 0 ? false : true)
                                }).ToList();

            return PartialView("Index", db.NHAPHANPHOIs);
        }
        public ActionResult Remove(string NPP_MA)
        {
            try
            {
                NHAPHANPHOI npp = db.NHAPHANPHOIs.First(t => t.NPP_MA.Equals(NPP_MA));

                db.NHAPHANPHOIs.DeleteOnSubmit(npp);
                db.SubmitChanges();
            }
            catch (Exception)
            {
                throw;
            }

            return PartialView("_listNhaPhanPhoi", db.NHAPHANPHOIs);
        }
        public ActionResult Add(string NPP_MA, string NPP_TEN, string NPP_SDT, string NPP_DIACHI)
        {
            try
            {
                NHAPHANPHOI npp = new NHAPHANPHOI()
                {
                    NPP_MA = NPP_MA,
                    NPP_TEN = NPP_TEN,
                    NPP_SDT = NPP_SDT,
                    NPP_DIACHI = NPP_DIACHI
                };

                db.NHAPHANPHOIs.InsertOnSubmit(npp);
                db.SubmitChanges();
            }
            catch (Exception)
            {
                throw;
            }

            return PartialView("_listNhaPhanPhoi", db.NHAPHANPHOIs);
        }
        public ActionResult Update(string NPP_MA, string NPP_TEN, string NPP_SDT, string NPP_DIACHI)
        {
            try
            {
                NHAPHANPHOI npp = db.NHAPHANPHOIs.First(t => t.NPP_MA.Equals(NPP_MA));

                npp.NPP_TEN = NPP_TEN;
                npp.NPP_SDT = NPP_SDT;
                npp.NPP_DIACHI = NPP_DIACHI;

                db.SubmitChanges();
            }
            catch (Exception)
            {
                throw;
            }

            return PartialView("_listNhaPhanPhoi", db.NHAPHANPHOIs);
        }
    }
}