using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DAMH_MuaBanVaSuaChuaDienThoai.Controllers
{
    public class AdminController : Controller
    {
        // GET: Admin
        public ActionResult Index()
        {
            return View("_Layout_Admin");
        }
        public ActionResult Error()
        {
            return PartialView("_AdminError");
        }
    }
}