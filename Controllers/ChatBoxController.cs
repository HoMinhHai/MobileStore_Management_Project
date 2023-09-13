using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DAMH_MuaBanVaSuaChuaDienThoai.Controllers
{
    public class ChatBoxController : Controller
    {
        // GET: ChatBox
        public ActionResult Index()
        {
            return PartialView("Index");
        }
    }
}