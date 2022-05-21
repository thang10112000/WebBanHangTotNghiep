using ASPSnippets.GoogleAPI;
using BotDetect.Web.Mvc;
using Facebook;
using Model.DAO;
using Model.EF;
using Model.ViewModel;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using System.Web.Security;
using System.Xml.Linq;
using WebShopOnline.Common;
using WebShopOnline.LoginGoogle;
using WebShopOnline.Models;

namespace WebShopOnline.Controllers
{
    public class UserController : Controller

    {
        private Uri RedirectUri
        {
            get
            {
                var uriBuilder = new UriBuilder(Request.Url);
                uriBuilder.Query = null;
                uriBuilder.Fragment = null;
                uriBuilder.Path = Url.Action("FacebookCallback");
                return uriBuilder.Uri;
            }
        }

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

        public ActionResult Infomation()
        {
            if (Session[CommonConstants.USER_SESSION] == null)
            {
                return View("Index");
            }
            return View();
        }

        public ActionResult ChangePassword(int id)
        {
            return View();
        }

        [HttpPost]
        public ActionResult ChangePassword(ChangePassword request, int id)
        {
            if (ModelState.IsValid)
            {
                var dao = new UserDao();
                var checkpw = dao.ViewDetail(id);
                if (!string.IsNullOrEmpty(request.Password) && !string.IsNullOrEmpty(request.NewPassword) && !string.IsNullOrEmpty(request.ConfirmPassword))
                {
                    var encryptedMd5Pas = Encryptor.MD5Hash(request.Password);
                    var encryptedMd5Pass = Encryptor.MD5Hash(request.NewPassword);
                    var encryptedMd5Passold = Encryptor.MD5Hash(request.ConfirmPassword);
                    request.Password = encryptedMd5Pas;
                    request.NewPassword = encryptedMd5Pass;
                    request.ConfirmPassword = encryptedMd5Passold;
                }
                if (checkpw.Password == request.Password)
                {
                    var result = dao.UpdatePassword(request);
                    if (result)
                    {
                        return RedirectToAction("Logout1", "User");
                    }
                    else
                    {
                        ModelState.AddModelError("", "Có lỗi xảy ra vui lòng thử lại. ");
                    }
                }
                else
                {
                    ModelState.AddModelError("", "Password Cũ không đúng");
                }
            }
            return View(request);
        }

        public ActionResult NewChangePassword(int id)
        {
            return View();
        }

        [HttpPost]
        public ActionResult NewChangePassword(NewChangePassword request, int id)
        {
            if (ModelState.IsValid)
            {
                var dao = new UserDao();
                var checkpw = dao.ViewDetail(id);
                if (!string.IsNullOrEmpty(request.NewPassword) && !string.IsNullOrEmpty(request.ConfirmPassword))
                {
                    var encryptedMd5Pass = Encryptor.MD5Hash(request.NewPassword);
                    var encryptedMd5Passold = Encryptor.MD5Hash(request.ConfirmPassword);
                    request.NewPassword = encryptedMd5Pass;
                    request.ConfirmPassword = encryptedMd5Passold;
                }

                var result = dao.ForgotPassword(request);
                if (result)
                {
                    return RedirectToAction("Logout1", "User");
                }
                else
                {
                    ModelState.AddModelError("", "Có lỗi trong quá trình đổi mật khẩu vui lòng thử lại.");
                }
            }

            return View(request);
        }

        public ActionResult ChangeProflle(int id)
        {
            return View();
        }

        [HttpPost]
        public ActionResult ChangeProflle(ChangeProflle request, int id)
        {
            if (ModelState.IsValid)
            {
                var dao = new UserDao();
                var result = dao.UpdateProfile(request);

                if (result)
                {
                    return RedirectToAction("Infomation", "User");
                }
                else
                {
                    ModelState.AddModelError("", "Cập nhật không thành công");
                }
            }

            return View(request);
        }

        public ActionResult LoginFacebook()
        {
            var fb = new FacebookClient();
            var loginUrl = fb.GetLoginUrl(new
            {
                client_id = ConfigurationManager.AppSettings["FbAppId"],
                client_secret = ConfigurationManager.AppSettings["FbAppSecret"],
                redirect_uri = RedirectUri.AbsoluteUri,
                response_type = "code",
                scope = "email",
            });

            return Redirect(loginUrl.AbsoluteUri);
        }

        public ActionResult FacebookCallback(string code)
        {
            var fb = new FacebookClient();
            dynamic result = fb.Post("oauth/access_token", new
            {
                client_id = ConfigurationManager.AppSettings["FbAppId"],
                client_secret = ConfigurationManager.AppSettings["FbAppSecret"],
                redirect_uri = RedirectUri.AbsoluteUri,
                code = code
            });

            var accessToken = result.access_token;
            if (!string.IsNullOrEmpty(accessToken))
            {
                fb.AccessToken = accessToken;
                // Get the user's information, like email, first name, middle name etc
                dynamic me = fb.Get("me?fields=first_name,middle_name,last_name,id,email");
                string email = me.email;
                string userName = me.email;
                string firstname = me.first_name;
                string middlename = me.middle_name;
                string lastname = me.last_name;

                var user = new User();
                user.Email = email;
                user.UserName = email;
                user.Status = true;
                user.Name = firstname + " " + middlename + " " + lastname;
                user.CreateDate = DateTime.Now;
                var resultInsert = new UserDao().InsertForFacebook(user);
                if (resultInsert > 0)
                {
                    var userSession = new UserLogin();
                    userSession.UserName = user.UserName;
                    userSession.UserID = user.ID;
                    Session.Add(CommonConstants.USER_SESSION, userSession);
                }
            }
            return Redirect("/");
        }

        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(LoginModel model)
        {
            if (ModelState.IsValid)
            {
                var dao = new UserDao();
                var result = dao.Login(model.UserName, Encryptor.MD5Hash(model.Password));
                if (result == 1)
                {
                    var user = dao.GetById(model.UserName);
                    var userSession = new UserLogin();
                    userSession.UserName = user.UserName;
                    userSession.UserID = user.ID;
                    userSession.Password = user.Password;
                    userSession.Name = user.Name;
                    userSession.Phone = user.Phone;
                    userSession.Email = user.Email;
                    userSession.Address = user.Address;
                    userSession.GroupID = user.GroupID;
                    Session.Add(CommonConstants.USER_SESSION, userSession);
                    return Redirect("/");
                }
                else if (result == 0)
                {
                    ModelState.AddModelError("", "Tài khoản không tồn tại.");
                }
                else if (result == -1)
                {
                    ModelState.AddModelError("", "Tài khoản đang bị khoá.");
                }
                else if (result == -2)
                {
                    ModelState.AddModelError("", "Mật khẩu không đúng.");
                }
                else
                {
                    ModelState.AddModelError("", "đăng nhập không đúng.");
                }
            }
            return View(model);
        }

        public ActionResult Logout()
        {
            Session[CommonConstants.USER_SESSION] = null;
            return Redirect("/");
        }

        public ActionResult Logout1()
        {
            Session[CommonConstants.USER_SESSION] = null;
            FormsAuthentication.SignOut();
            return Redirect("/User/Login");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public void LoginWithGooglePlus()
        {
            GoogleConnect.ClientId = "620272693439-7f93ghgcitmvime28r27nkg3iqn3e430.apps.googleusercontent.com";
            GoogleConnect.ClientSecret = "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";
            GoogleConnect.RedirectUri = Request.Url.AbsoluteUri.Split('?')[0];
            GoogleConnect.Authorize("profile", "email");
        }

        [ActionName("LoginWithGooglePlus")]
        public ActionResult LoginWithGooglePlusConfirmed()
        {
            if (!string.IsNullOrEmpty(Request.QueryString["code"]))
            {
                string code = Request.QueryString["code"];
                string json = GoogleConnect.Fetch("me", code);
                GoogleProfile profile = new JavaScriptSerializer().Deserialize<GoogleProfile>(json);

                User khachHang = new User()
                {
                    UserName = profile.Id,
                    Name = profile.Name,
                    Email = profile.email,
                    CreateDate = DateTime.Now,
                    CreateBy = profile.Name,
                    Status = true,
                    GroupID = "MEMBER"
                };
                var resultInsert = new UserDao().InsertForGoogle(khachHang);
                if (resultInsert > 0)
                {
                    var nguoidung = new UserDao().GetById(khachHang.UserName);
                    var userSession = new UserLogin();
                    userSession.UserName = nguoidung.UserName;
                    userSession.UserID = nguoidung.ID;
                    userSession.GroupID = nguoidung.GroupID;
                    Session.Add(CommonConstants.USER_SESSION, userSession);
                }
            }
            if (Request.QueryString["error"] == "access_denied")
            {
                return Content("access_denied");
            }
            return Redirect("/");
        }

        public ActionResult ForgotPassword()
        {
            return View();
        }

        [HttpPost]
        [CaptchaValidation("CaptchaCode", "forgotpassword", "Mã xác nhận không đúng!")]
        public ActionResult ForgotPassword(string email)
        {
            if (ModelState.IsValid)
            {
                var forgot = new UserDao().ViewEmail(email);
                if (forgot == null)
                {
                    ViewBag.Message = "Lỗi Email Không Tồn Tại";
                }
                else
                {
                    string a = System.IO.File.ReadAllText(Server.MapPath("/Assets/Client/template/newForgotPassword.html"));
                    a = a.Replace("{{CustomerName}}", email);
                    a = a.Replace("{{link}}", "https://zeroone.somee.com/User/NewChangePassword/" + forgot.ID);
                    SendMail(email, "Email Quên Mật Khẩu Mới", a);
                    return RedirectToAction("SuccessEmail");
                }
            }
            return View();
        }

        public ActionResult SuccessEmail()
        {
            return View();
        }

        public void SendMail(string toEmailAddress, string subject, string content)
        {
            var fromEmailAddress = ConfigurationManager.AppSettings["FromEmailAddress"].ToString();
            var fromEmailDisplayName = ConfigurationManager.AppSettings["FromEmailDisplayName"].ToString();
            var fromEmailPassword = ConfigurationManager.AppSettings["FromEmailPassword"].ToString();
            var smtpHost = ConfigurationManager.AppSettings["SMTPHost"].ToString();
            var smtpPort = ConfigurationManager.AppSettings["SMTPPort"].ToString();

            bool enabledSsl = bool.Parse(ConfigurationManager.AppSettings["EnabledSSL"].ToString());

            string body = content;
            MailMessage message = new MailMessage(new MailAddress(fromEmailAddress, fromEmailDisplayName), new MailAddress(toEmailAddress));
            message.Subject = subject;
            message.IsBodyHtml = true;
            message.Body = body;

            var client = new SmtpClient();
            client.Credentials = new NetworkCredential(fromEmailAddress, fromEmailPassword);
            client.Host = smtpHost;
            client.EnableSsl = enabledSsl;
            client.Port = !string.IsNullOrEmpty(smtpPort) ? Convert.ToInt32(smtpPort) : 0;
            client.Send(message);
        }
    }
}