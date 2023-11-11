using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DoAnNhom1.Filters
{
    public enum UserRole
    {
        Admin,
        User
    }
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, Inherited = true, AllowMultiple = true)]
    public class CustomAuthorizeAttribute : AuthorizeAttribute
    {
        private readonly UserRole[] _roles;
        private readonly bool _requireLogin;

        public CustomAuthorizeAttribute(UserRole[] roles, bool requireLogin = true)
        {
            _roles = roles;
            _requireLogin = requireLogin;
        }

        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            if (_requireLogin && httpContext.Session["UserRole"] == null)
            {
                return false; // Chưa đăng nhập.
            }

            if (Enum.TryParse(httpContext.Session["UserRole"]?.ToString(), out UserRole userRole))
            {
                return _roles.Any(r => r == userRole);
            }

            return false; // Lỗi khi chuyển đổi kiểu dữ liệu.
        }

        protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        {
            if (_requireLogin && filterContext.HttpContext.Session["UserRole"] == null)
            {
                // Người dùng chưa đăng nhập, chuyển hướng đến trang đăng nhập.
                filterContext.Result = new RedirectResult("~/Login/Index");
            }
            else
            {
                // Người dùng đã đăng nhập nhưng không có quyền, hiển thị thông báo và chuyển hướng về trang chủ.
                filterContext.Controller.TempData["ShowAlert"] = true;
                filterContext.Result = new RedirectResult("~/Home/Index");
            }
        }
    }
}