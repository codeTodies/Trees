using DoAnNhom1.Filters;
using DoAnNhom1.Models;
using PagedList;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace DoAnNhom1.Controllers
{
    public class ContactController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        // GET: Contact
        public ActionResult Index()
        {
            return View();
        }
        [CustomAuthorize(new UserRole[] { UserRole.Admin })]
        public ActionResult GetInfo(string SortOrder, string currentFilter, string SearchString, int? page)
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
                var courses = from c in db.Customers
                              select c;
                if (!string.IsNullOrEmpty(SearchString))
                {
                    courses = courses.Where(c => c.NameCus.Contains(SearchString));
                }
                switch (SortOrder)
                {
                    case "Name_desc":
                        courses = courses.OrderByDescending(c => c.NameCus);
                        break;
                    default:
                        courses = courses.OrderBy(c => c.NameCus);
                        break;
                }
                int pageSize = 5;
                int pageNumber = (page ?? 1);
                return View(courses.ToPagedList(pageNumber, pageSize));
            }
        }
        [HttpPost]
        public ActionResult Index([Bind(Include ="IDCus,NameCus,PhoneCus,EmailCus")] Customer customer)
        {
            if (ModelState.IsValid)
            {
                db.Customers.Add(customer);
                if (db.SaveChanges() > 0)
                {
                    TempData["nofi"] = "Gửi thông tin thành công";
                    ModelState.Clear();
                }
                return View();
            }

            return View(customer);
        }
        [CustomAuthorize(new UserRole[] { UserRole.Admin })]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Customer customer = db.Customers.Find(id);
            if (customer == null)
            {
                return HttpNotFound();
            }
            return View(customer);
        }

        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        public ActionResult Edit([Bind(Include = "IDCus,NameCus,PhoneCus,EmailCus")] Customer customer)
        {
            if (ModelState.IsValid)
            {
                db.Entry(customer).State = EntityState.Modified;
                if (db.SaveChanges() > 0)
                {
                    TempData["nofi"] = "Cập nhật thành công";
                }
                return RedirectToAction("GetInfo");
            }
            return View(customer);
        }
        [CustomAuthorize(new UserRole[] { UserRole.Admin })]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Customer customer = db.Customers.Find(id);
            if (customer == null)
            {
                return HttpNotFound();
            }
            return View(customer);
        }

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            Customer customer = db.Customers.Find(id);
            db.Customers.Remove(customer);
            if (db.SaveChanges() > 0)
            {
                TempData["nofi"] = "Xóa thành công";
            }
            return RedirectToAction("GetInfo");
        }

    }
}