﻿using Model.DAO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebShopOnline.Areas.Admin.Controllers
{
    public class ReviewController : BaseController
    {
        [HasCredential(RoleID = "VIEW_USER")]
        public ActionResult Index(string searchString, int page = 1, int pageSize = 10)
        {
            var dao = new ReviewDao();
            var model = dao.ListAllPaging(searchString, page, pageSize);
            ViewBag.SearchString = searchString;
            return View(model);
        }

        [HasCredential(RoleID = "DELETE_USER")]
        public ActionResult Delete(int id)
        {
            new ReviewDao().Delete(id);
            return RedirectToAction("Index");
        }
    }
}