using Model.DAO;
using Model.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebShopOnline.Areas.Admin.Controllers
{
    public class UserGroupController : BaseController
    {
        // GET: Admin/UserGroup
        [HasCredential(RoleID = "VIEW_USER")]
        public ActionResult Index(string searchString, int page = 1, int pageSize = 10)
        {
            var dao = new UserGroupDao();
            var model = dao.ListAllPaging(searchString, page, pageSize);
            ViewBag.SearchString = searchString;
            return View(model);
        }
        [HttpGet]
        [HasCredential(RoleID = "ADD_USER")]
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [HasCredential(RoleID = "ADD_USER")]
        [ValidateInput(false)]
        public ActionResult Create(UserGroup usergroup)
        {
            if (ModelState.IsValid)
            {
                var dao = new UserGroupDao();
                string id = dao.Insert(usergroup);
                if (id != null)
                {
                    SetAlert("Thêm Thành Công ", "success");
                    return RedirectToAction("Index", "UserGroup");
                }
                else
                {
                    ModelState.AddModelError("", "Thêm Không Thành Công");
                }
            }
            return View("Index");
        }
        [HasCredential(RoleID = "EDIT_USER")]
        public ActionResult Edit(string id)
        {
            var usergroup = new UserGroupDao().ViewDetail(id);
            return View(usergroup);
        }
        [HttpPost]
        [HasCredential(RoleID = "EDIT_USER")]
        public ActionResult Edit(UserGroup usergroup)
        {
            if (ModelState.IsValid)
            {
                var dao = new UserGroupDao();
                var result = dao.Update(usergroup);
                if (result)
                {
                    SetAlert("Sửa Thành Công ", "success");
                    return RedirectToAction("Index", "UserGroup");
                }
                else
                {
                    ModelState.AddModelError("", "Update Không thành công");
                }
            }
            return View("Index");
        }
        [HasCredential(RoleID = "DELETE_USER")]
        public ActionResult Delete(string id)
        {
            new UserGroupDao().Delete(id);
            return RedirectToAction("Index");
        }
       
    }
}