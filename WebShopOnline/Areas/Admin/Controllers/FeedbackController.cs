using Model.DAO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebShopOnline.Areas.Admin.Controllers
{
    public class FeedbackController : BaseController
    {
        // GET: Admin/Feedback
        [HasCredential(RoleID = "VIEW_USER")]
        public ActionResult Index(string searchString, int page = 1, int pageSize = 5)
        {
            var dao = new FeedbackDao();
            var model = dao.ListAllPaging(searchString, page, pageSize);
            ViewBag.SearchString = searchString;
            return View(model);
        }
        [HasCredential(RoleID = "DELETE_USER")]
        public ActionResult Delete(int id)
        {
            new FeedbackDao().Delete(id);
            return RedirectToAction("Index");
        }
    }
}