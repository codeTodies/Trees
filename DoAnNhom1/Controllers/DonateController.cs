using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DoAnNhom1.Controllers
{
    public class DonateController : Controller
    {
        // GET: Donate
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Donate()
        {
            return View();
        }
        public ActionResult MB()
        {
            return View();
        }
    }
}