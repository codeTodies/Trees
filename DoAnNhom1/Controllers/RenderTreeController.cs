using DoAnNhom1.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DoAnNhom1.Controllers
{
    public class RenderTreeController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        // GET: RenderTree
        public ActionResult Index(int? id)
        {
            var area = db.Trees.Where(s => s.IDArea == id);
            return View(area.ToList());
        }
    }
}