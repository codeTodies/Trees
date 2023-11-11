using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using DoAnNhom1.Filters;
using DoAnNhom1.Models;
using PagedList;

namespace DoAnNhom1.Controllers
{
    [CustomAuthorize(new UserRole[] { UserRole.Admin })]
    public class AreasController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Areas
        public ActionResult Index(string SortOrder, string currentFilter, string SearchString, int? page)
        {
            ViewBag.NameSortParm = String.IsNullOrEmpty(SortOrder) ? "Name_desc" : "";
            if (SearchString != null)
            {
                page = 1;
            }
            else
            {
                SearchString = currentFilter;
            }
            ViewBag.CurrentFilter = SearchString;
            var courses = from c in db.Areas
                          select c;
            if (!string.IsNullOrEmpty(SearchString))
            {
                courses = courses.Where(c => c.NameArea.Contains(SearchString));
            }
            switch (SortOrder)
            {
                case "Name_desc":
                    courses = courses.OrderByDescending(c => c.NameArea);
                    break;
                default:
                    courses = courses.OrderBy(c => c.NameArea);
                    break;
            }
            int pageSize = 5;
            int pageNumber = (page ?? 1);
            return View(courses.ToPagedList(pageNumber, pageSize));
        }

        public ActionResult SelectRegion()
        {
            Region se_cate = new Region();
            se_cate.ListRe = db.Regions.ToList<Region>();
            return PartialView(se_cate);
        }

        // GET: Areas/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Area area = db.Areas.Find(id);
            if (area == null)
            {
                return HttpNotFound();
            }
            return View(area);
        }

        // GET: Areas/Create
        public ActionResult Create()
        {
            Area area = new Area();
            return View(area);
        }

        // POST: Areas/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        public ActionResult Create(Area area)
        {
            try
            {
                if (area.UploadImage != null)
                {
                    string fileName = Path.GetFileNameWithoutExtension(area.UploadImage.FileName);
                    string extend = Path.GetExtension(area.UploadImage.FileName);
                    fileName = fileName + extend;
                    area.ImgArea = "~/image/" + fileName;
                    if (extend.ToLower() == ".jpg" || extend.ToLower() == ".jpeg" || extend.ToLower() == ".png")
                    {
                        if (area.UploadImage.ContentLength <= 6000000)
                        {
                            db.Areas.Add(area);
                            if (db.SaveChanges() > 0)
                            {
                                area.UploadImage.SaveAs(Path.Combine(Server.MapPath("~/image/"), fileName));
                                ViewBag.nofi = "Area added success";
                                ModelState.Clear();
                                return RedirectToAction("Index");
                            }
                        }
                        else
                        {
                            ModelState.AddModelError("UploadImage","File must be equal or less than 6MB");
                        }
                    }
                    else
                    {
                        ModelState.AddModelError("UploadImage", "Invalid File Type");
                    }
                }
                else
                {
                    ModelState.AddModelError("UploadImage", "Please choose a file.");
                }    
            }
            catch
            {
                return View();
            }
            return View(area);
        }

        // GET: Areas/Edit/5
        public ActionResult Edit(int? id)
        {
            var area = db.Areas.Where(s => s.IDArea == id).FirstOrDefault();
            Session["imgPath"] = area.ImgArea;
            ViewBag.Regions = new SelectList(db.Regions.OrderBy(r => r.NameRe), "IDRe", "NameRe", area.Region.IDRe);
            if (area == null)
            {
                return HttpNotFound(); // Trả về lỗi 404 nếu không tìm thấy đối tượng
            }

            return View(area);
        }

        // POST: Areas/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        public ActionResult Edit(Area area)
        {

            if (ModelState.IsValid)
            {

                if (area.UploadImage != null)
                {
                    string fileName = Path.GetFileNameWithoutExtension(area.UploadImage.FileName);
                    string extend = Path.GetExtension(area.UploadImage.FileName);
                    fileName = fileName + extend;
                    area.ImgArea = "~/image/" + fileName;
                    if (extend.ToLower() == ".jpg" || extend.ToLower() == ".jpeg" || extend.ToLower() == ".png")
                    {
                        if (area.UploadImage.ContentLength <= 6000000)
                        {
                            db.Entry(area).State = EntityState.Modified;

                            string oldImgPath = Request.MapPath(Session["imgPath"].ToString());
                            if (db.SaveChanges() > 0)
                            {
                                area.UploadImage.SaveAs(Path.Combine(Server.MapPath("~/image/"), fileName));
                                if (System.IO.File.Exists(oldImgPath))
                                {
                                    System.IO.File.Delete(oldImgPath);
                                }
                                TempData["nofi"] = "Update Success";
                                return RedirectToAction("Index");
                            }
                        }
                        else
                        {
                            ModelState.AddModelError("UploadImage", "File must be equal or less than 6MB");
                        }
                    }
                    else
                    {
                        ModelState.AddModelError("UploadImage", "Invalid File Type");
                    }
                }
                else
                {
                    area.ImgArea = Session["imgPath"].ToString();
                    db.Entry(area).State = EntityState.Modified;
                    if (db.SaveChanges() > 0)
                    {
                        TempData["nofi"] = "Update Success";
                        return RedirectToAction("Index");
                    }
                }
            }
            return View(area);
        }
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Area area = db.Areas.Find(id);
            if (area == null)
            {
                return HttpNotFound();
            }
            return View(area);
        }
        // POST: Areas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(System.Net.HttpStatusCode.BadRequest);
            }
            var area = db.Areas.Where(s => s.IDArea == id).FirstOrDefault();
            if (area == null)
            {
                return HttpNotFound();
            }
            string curImg = Request.MapPath(area.ImgArea);
            db.Entry(area).State = EntityState.Deleted;
            if (db.SaveChanges() > 0)
            {
                if (System.IO.File.Exists(curImg))
                {
                    System.IO.File.Delete(curImg);
                }
                TempData["nofi"] = "Area Deleted";
                return RedirectToAction("Index");
            }
            return View();
        }
    }
}
