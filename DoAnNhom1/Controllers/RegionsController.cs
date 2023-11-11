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
    public class RegionsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Regions
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
            var courses = from c in db.Regions
                          select c;
            if (!string.IsNullOrEmpty(SearchString))
            {
                courses = courses.Where(c => c.NameRe.Contains(SearchString));
            }
            switch (SortOrder)
            {
                case "Name_desc":
                    courses = courses.OrderByDescending(c => c.NameRe);
                    break;
                default:
                    courses = courses.OrderBy(c => c.NameRe);
                    break;
            }
            int pageSize = 5;
            int pageNumber = (page ?? 1);
            return View(courses.ToPagedList(pageNumber, pageSize));
        }


        // GET: Regions/Create
        public ActionResult Create()
        {

            Region region = new Region();
            return View(region);
        }

        // POST: Regions/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        public ActionResult Create(Region region)
        {
            try
            {
                if (region.UploadImage != null)
                {
                    string fileName = Path.GetFileNameWithoutExtension(region.UploadImage.FileName);
                    string extension = Path.GetExtension(region.UploadImage.FileName);
                    fileName = fileName + extension;
                    region.ImgRe = "~/image/" + fileName;

                    if (extension.ToLower() == ".jpg" || extension.ToLower() == ".jpeg" || extension.ToLower() == ".png")
                    {
                        if (region.UploadImage.ContentLength <= 4000000)
                        {
                            db.Regions.Add(region);
                            if (db.SaveChanges() > 0)
                            {
                                region.UploadImage.SaveAs(Path.Combine(Server.MapPath("~/image/"), fileName));
                                ViewBag.nofi = "Thêm thành công";
                                ModelState.Clear(); // Xóa ModelState để tránh việc hiển thị lại dữ liệu cũ
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
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, "An error occurred while processing your request.");
            }

            return View(region); // Trả về trang Create với lỗi và dữ liệu đã nhập
        }

        // GET: Regions/Edit/5
        public ActionResult Edit(int? id)
        {
            // Lấy đối tượng Region từ ID
            var region = db.Regions.Where(s => s.IDRe == id).FirstOrDefault();
            Session["imgPath"] = region.ImgRe;
            if (region == null)
            {
                return HttpNotFound(); // Trả về lỗi 404 nếu không tìm thấy đối tượng
            }

            return View(region);
        }

        // POST: Regions/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        public ActionResult Edit(Region region)
        {
            if (ModelState.IsValid)
            {

                if (region.UploadImage != null)
                {
                    string fileName = Path.GetFileNameWithoutExtension(region.UploadImage.FileName);
                    string extend = Path.GetExtension(region.UploadImage.FileName);
                    fileName = fileName + extend;
                    region.ImgRe = "~/image/" + fileName;
                    if (extend.ToLower() == ".jpg" || extend.ToLower() == ".jpeg" || extend.ToLower() == ".png")
                    {
                        if (region.UploadImage.ContentLength <= 6000000)
                        {
                            db.Entry(region).State = EntityState.Modified;

                            string oldImgPath = Request.MapPath(Session["imgPath"].ToString());
                            if (db.SaveChanges() > 0)
                            {
                                region.UploadImage.SaveAs(Path.Combine(Server.MapPath("~/image/"), fileName));
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
                            ModelState.AddModelError("UploadImage", "File Size must be Equal or less than 4mb");
                        }
                    }
                    else
                    {
                        ModelState.AddModelError("UploadImage", "Invalid File Type");
                    }
                }
                else
                {
                    region.ImgRe = Session["imgPath"].ToString();
                    db.Entry(region).State = EntityState.Modified;
                    if (db.SaveChanges() > 0)
                    {
                        TempData["nofi"] = "Update Success";
                        return RedirectToAction("Index");
                    }
                }
            }
            return View(region); // Trả về trang Create với lỗi và dữ liệu đã nhập
        }

        // GET: Regions/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Region region = db.Regions.Find(id);
            if (region == null)
            {
                return HttpNotFound();
            }
            return View(region);
        }

        // POST: Regions/Delete/5
        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(System.Net.HttpStatusCode.BadRequest);
            }
            var region = db.Regions.Where(s => s.IDRe == id).FirstOrDefault();
            if (region == null)
            {
                return HttpNotFound();
            }
            string curImg = Request.MapPath(region.ImgRe);
            db.Entry(region).State = EntityState.Deleted;
            if (db.SaveChanges() > 0)
            {
                if (System.IO.File.Exists(curImg))
                {
                    System.IO.File.Delete(curImg);
                }
                TempData["nofi"] = "Region Deleted";
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
