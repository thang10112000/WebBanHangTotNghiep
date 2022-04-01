using Model.DAO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebShopOnline.Areas.Admin.Controllers
{
    public class HomeController : BaseController
    {
        // GET: Admin/Home

        public ActionResult Index()
        {
            return View();
        }

        [ChildActionOnly]
        public ActionResult Order()
        {
            var dao = new FeedbackDao();
            var model = dao.GetAll();

            return View(model);
        }
    }
}