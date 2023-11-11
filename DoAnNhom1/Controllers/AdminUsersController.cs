using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using DoAnNhom1.Filters;
using DoAnNhom1.Models;
using PagedList;
using PagedList.Mvc;

namespace DoAnNhom1.Controllers
{
    [CustomAuthorize(new UserRole[] { UserRole.Admin})]
    public class AdminUsersController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: AdminUsers
        public ActionResult Index(string SortOrder, string currentFilter, string SearchString, int? page)
        {
            if (Session["NameUser"] == null)   
            {
                return RedirectToAction("Admin", "Login");
            }
            else
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
                var courses = from c in db.AdminUsers
                              select c;
                if (!string.IsNullOrEmpty(SearchString))
                {
                    courses = courses.Where(c => c.NameUser.Contains(SearchString));
                }
                switch (SortOrder)
                {
                    case "Name_desc":
                        courses = courses.OrderByDescending(c => c.NameUser);
                        break;
                    default:
                        courses = courses.OrderBy(c => c.NameUser);
                        break;
                }
                int pageSize = 5;
                int pageNumber = (page ?? 1);
                return View(courses.ToPagedList(pageNumber, pageSize));
            }
        }
            // GET: AdminUsers/Details/5
            public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AdminUser adminUser = db.AdminUsers.Find(id);
            if (adminUser == null)
            {
                return HttpNotFound();
            }
            return View(adminUser);
        }

        // GET: AdminUsers/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: AdminUsers/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        public ActionResult Create([Bind(Include = "ID,NameUser,RoleUser,PasswordUser")] AdminUser adminUser)
        {
            if (ModelState.IsValid)
            {
                db.AdminUsers.Add(adminUser);
                if (db.SaveChanges() > 0)
                {
                    TempData["nofi"] = "Thêm mới thành công";
                    ModelState.Clear();
                }
                return RedirectToAction("Index");
            }

            return View(adminUser);
        }

        // GET: AdminUsers/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AdminUser adminUser = db.AdminUsers.Find(id);
            if (adminUser == null)
            {
                return HttpNotFound();
            }
            return View(adminUser);
        }

        // POST: AdminUsers/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        public ActionResult Edit([Bind(Include = "ID,NameUser,RoleUser,PasswordUser")] AdminUser adminUser)
        {
            if (ModelState.IsValid)
            {
                db.Entry(adminUser).State = EntityState.Modified;
                if (db.SaveChanges() > 0)
                {
                    TempData["nofi"] = "Cập nhật thành công";
                }
                return RedirectToAction("Index");
            }
            return View(adminUser);
        }

        // GET: AdminUsers/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AdminUser adminUser = db.AdminUsers.Find(id);
            if (adminUser == null)
            {
                return HttpNotFound();
            }
            return View(adminUser);
        }

        // POST: AdminUsers/Delete/5
        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            AdminUser adminUser = db.AdminUsers.Find(id);
            db.AdminUsers.Remove(adminUser);
            if (db.SaveChanges() > 0)
            {
                TempData["nofi"] = "Xóa thành công";
            }
            return RedirectToAction("Index");
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
