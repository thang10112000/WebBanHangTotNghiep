using BotDetect.Web.Mvc;
using Model.DAO;
using Model.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Xml.Linq;
using WebShopOnline.Common;
using WebShopOnline.Models;

namespace WebShopOnline.Controllers
{
    public class UserController : Controller

    {
        [HttpGet]
        public ActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [CaptchaValidation("CaptchaCode", "registerCapcha", "Mã xác nhận không đúng!")]
        public ActionResult Register(RegisterModel model)
        {
            if (ModelState.IsValid)
            {
                var dao = new UserDao();
                if (dao.CheckUserName(model.UserName))
                {
                    ModelState.AddModelError("", "Tên đăng nhập đã tồn tại");
                }
                else if (dao.CheckEmail(model.Email))
                {
                    ModelState.AddModelError("", "Email đã tồn tại");
                }
                else
                {
                    var user = new User();
                    user.UserName = model.UserName;
                    user.Password = Encryptor.MD5Hash(model.Password);
                    // user.ConfirmNewPassword = Encryptor.MD5Hash(model.ConfirmPassword);

                    user.Phone = model.Phone;
                    user.Email = model.Email;
                    user.Name = model.Name;
                    user.Address = model.Address;
                    user.CreateDate = DateTime.Now;
                    user.Status = true;
                    var result = dao.Insert(user);
                    if (result > 0)
                    {
                        ViewBag.Success = "Đăng ký thành công";
                        model = new RegisterModel();
                    }
                    else
                    {
                        ModelState.AddModelError("", "Đăng ký không thành công.");
                    }
                }
            }
            return View(model);
        }
    }
}