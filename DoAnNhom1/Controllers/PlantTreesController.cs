using DoAnNhom1.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DoAnNhom1.Controllers
{
    public class PlantTreesController : Controller
    {
        // GET: PlantTrees
        private ApplicationDbContext db = new ApplicationDbContext();
        public ActionResult Index(int ?id)
        {
            var area = db.Areas.Where(s => s.IDRe == id);
            return View(area.ToList());
        }
        public ActionResult Indexs()
        {
            return View(db.Areas.ToList());
        }
    }
}