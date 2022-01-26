using Model.DAO;
using ShopThoiTrang.Areas.Admin.Models;
using ShopThoiTrang.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ShopThoiTrang;
using System.Web.Security;

namespace ShopThoiTrang.Areas.Admin.Controllers
{
    public class LoginController : Controller
    {
        // GET: Admin/Login
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Login(LoginModel model)
        {
            if (ModelState.IsValid)
            {
                var dao = new UserDao();
                var result = dao.Login(model.UserName, Encryptor.MD5Hash(model.Password), true);
                if (result == 1)
                {
                    var user = dao.GetById(model.UserName);
                    var userSession = new UserLogin();
                    userSession.UserName = user.UserName;
                    userSession.UserID = user.ID;
                    userSession.GroupID = user.GroupID;
                    var listCredentials = dao.GetListCredential(model.UserName);

                    Session.Add(CommonConstants.SESSION_CREDENTIALS, listCredentials);
                    Session.Add(CommonConstants.USER_SESSION, userSession);
                    return RedirectToAction("Index", "Home");
                }
                else if (result == 0)
                {
                    ModelState.AddModelError("", "Tài khoản không tồn tại.");
                }
                else if (result == -1)
                {
                    ModelState.AddModelError("", "Tài khoản đang bị khóa.");
                }
                else if (result == -2)
                {
                    ModelState.AddModelError("", "Sai mật khẩu.");
                }
                else if (result == -3)
                {
                    ModelState.AddModelError("", "Bạn không có quyền đăng nhập.");
                }
                else
                {
                    ModelState.AddModelError("", "Đăng nhập không thành công.");
                }
            }
            return View("Index");
        }
        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Index", "Login");
        }
    }
}