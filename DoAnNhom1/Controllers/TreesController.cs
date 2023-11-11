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
    public class TreesController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Trees
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
            var courses = from c in db.Trees
                          select c;
            if (!string.IsNullOrEmpty(SearchString))
            {
                courses = courses.Where(c => c.NameTree.Contains(SearchString));
            }
            switch (SortOrder)
            {
                case "Name_desc":
                    courses = courses.OrderByDescending(c => c.NameTree);
                    break;
                default:
                    courses = courses.OrderBy(c => c.NameTree);
                    break;
            }
            int pageSize = 5;
            int pageNumber = (page ?? 1);
            return View(courses.ToPagedList(pageNumber, pageSize));
        }

        // GET: Trees/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Tree tree = db.Trees.Find(id);
            if (tree == null)
            {
                return HttpNotFound();
            }
            return View(tree);
        }

        public ActionResult SelectArea()
        {
            Area se_cate = new Area();
            se_cate.ListArea = db.Areas.ToList<Area>();
            return PartialView(se_cate);
        }

        // GET: Trees/Create
        public ActionResult Create()
        {
            Tree tree = new Tree();
            return View(tree);
        }

        // POST: Trees/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        public ActionResult Create(Tree tree)
        {
            try
            {
                if (tree.UploadImage != null)
                {
                    string fileName = Path.GetFileNameWithoutExtension(tree.UploadImage.FileName);
                    string extend = Path.GetExtension(tree.UploadImage.FileName);
                    fileName = fileName + extend;
                    tree.ImageTree = "~/image/" + fileName;
                    if (extend.ToLower() == ".jpg" || extend.ToLower() == ".jpeg" || extend.ToLower() == ".png")
                    {
                        if (tree.UploadImage.ContentLength <= 6000000)
                        {
                            db.Trees.Add(tree);
                            if (db.SaveChanges() > 0)
                            {
                                tree.UploadImage.SaveAs(Path.Combine(Server.MapPath("~/image/"), fileName));
                                ViewBag.nofi = "Thêm thành công";
                                ModelState.Clear();
                                return RedirectToAction("Index");
                            }
                        }
                        else
                        {
                            ModelState.AddModelError("UploadImage", "File Size must be Equal or less than 4mb");
                        }
                    }
                    else
                    {
                        ModelState.AddModelError("UploadImage", "Định dạng không hợp lệ");
                    }
                }
                else
                {
                    ModelState.AddModelError("UploadImage", "Mời chọn file hình ảnh.");
                }
            }
            catch
            {
                return View();
            }
            return View(tree);
        }

        public ActionResult Edit(int? id)
        {
            var tree = db.Trees.Where(s => s.TreeID == id).FirstOrDefault();
            Session["imgPath"] = tree.ImageTree;
            ViewBag.Regions = new SelectList(db.Areas.OrderBy(r => r.NameArea), "IDArea", "NameArea", tree.Area.IDArea);
            if (tree == null)
            {
                return HttpNotFound(); // Trả về lỗi 404 nếu không tìm thấy đối tượng
            }

            return View(tree);
        }
        // POST: Trees/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        public ActionResult Edit(Tree tree)
        {
            if (ModelState.IsValid)
            {

                if (tree.UploadImage != null)
                {
                    string fileName = Path.GetFileNameWithoutExtension(tree.UploadImage.FileName);
                    string extend = Path.GetExtension(tree.UploadImage.FileName);
                    fileName = fileName + extend;
                    tree.ImageTree = "~/image/" + fileName;
                    if (extend.ToLower() == ".jpg" || extend.ToLower() == ".jpeg" || extend.ToLower() == ".png")
                    {
                        if (tree.UploadImage.ContentLength <= 6000000)
                        {
                            db.Entry(tree).State = EntityState.Modified;

                            string oldImgPath = Request.MapPath(Session["imgPath"].ToString());
                            if (db.SaveChanges() > 0)
                            {
                                tree.UploadImage.SaveAs(Path.Combine(Server.MapPath("~/image/"), fileName));
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
                            ViewBag.nofi = "File Size must be Equal or less than 6mb";
                        }
                    }
                    else
                    {
                        ViewBag.nofi = "Invalid File Type";
                    }
                }
                else
                {
                    tree.ImageTree = Session["imgPath"].ToString();
                    db.Entry(tree).State = EntityState.Modified;
                    if (db.SaveChanges() > 0)
                    {
                        TempData["nofi"] = "Update Success";
                        return RedirectToAction("Index");
                    }
                }
            }
            return View();
        }
        // GET: Trees/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Tree tree = db.Trees.Find(id);
            if (tree == null)
            {
                return HttpNotFound();
            }
            return View(tree);
        }

        // POST: Trees/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int? id)
        {

            if (id == null)
            {
                return new HttpStatusCodeResult(System.Net.HttpStatusCode.BadRequest);
            }
            var tree = db.Trees.Where(s => s.TreeID == id).FirstOrDefault();
            if (tree == null)
            {
                return HttpNotFound();
            }
            string curImg = Request.MapPath(tree.ImageTree);
            db.Entry(tree).State = EntityState.Deleted;
            if (db.SaveChanges() > 0)
            {
                if (System.IO.File.Exists(curImg))
                {
                    System.IO.File.Delete(curImg);
                }
                TempData["nofi"] = "Tree Deleted";
                return RedirectToAction("Index");
            }
            return View();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
