using DoAnNhom1.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DoAnNhom1.Controllers
{
    public class LoginController : Controller
    {
        // GET: Login
        private ApplicationDbContext db = new ApplicationDbContext();
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Admin()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Login(AdminUser user)
        {
            var check = db.AdminUsers.Where(s => s.NameUser == user.NameUser && s.PasswordUser == user.PasswordUser && (s.RoleUser == "User" || s.RoleUser=="Admin")).FirstOrDefault();
            if (check == null)
            {
                ViewBag.ErrorInfo = "Tên đăng nhập hoặc tài khoản không đúng";
                return View("Index");

            }
            else
            {
                db.Configuration.ValidateOnSaveEnabled = false;
                Session["NameUser"] = user.NameUser;
                Session["UserRole"] = "User";
                Session["PasswordUser"] = user.PasswordUser;
                return RedirectToAction("Index", "Home");
            }
        }
        [HttpPost]
        public ActionResult LoginAdmin(AdminUser user)
        {
            var check = db.AdminUsers.Where(s => s.NameUser == user.NameUser && s.PasswordUser == user.PasswordUser && s.RoleUser == "Admin").FirstOrDefault();
            if (check == null)
            {
                ViewBag.ErrorInfo = "Sai Info hoặc bạn không có quyền vào trang này";
                return View("Index");

            }
            else
            {
                db.Configuration.ValidateOnSaveEnabled = false;
                Session["NameUser"] = user.NameUser;
                Session["UserRole"] = "Admin";
                Session["PasswordUser"] = user.PasswordUser;
                return RedirectToAction("Index","AdminUsers");
            }
        }
        public ActionResult LogOutUser()
        {
            Session.Abandon();
            return RedirectToAction("Index", "Login");
        } 
    }
}