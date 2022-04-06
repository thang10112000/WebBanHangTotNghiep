﻿using Model.DAO;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebShopOnline.Common;
using WebShopOnline.Models;

namespace WebShopOnline.Controllers
{
    public class HomeController : Controller
    {
        private const string CartSession = "CartSession";

        public ActionResult Index(string searchString, int page = 1, int pageSize = 10)
        {
            ViewBag.Slides = new SlideDao().ListAll();
            var productDao = new ProductDao();

            ViewBag.NewProducts = productDao.ListNewProduct(4);// hiển thị sản phẩm mới tối đa 4
            ViewBag.NewContents = new ContentDao().ListNewContent(3); // hiển thị tin tức mới tối đa 3
            ViewBag.ListFeatureProducts = productDao.ListFeatureProduct(4); // hiển thị sản phẩm có lượt viewcout nhiều tối đa 4
            //ViewBag.Review = new ReviewDao().ListAll(id);
            //set seo title
            ViewBag.Title = ConfigurationManager.AppSettings["HomeTitle"];
            ViewBag.Keywords = ConfigurationManager.AppSettings["HomeKeyword"];
            ViewBag.Descriptions = ConfigurationManager.AppSettings["HomeDescriptions"];
            return View();
        }

        [ChildActionOnly]
        [OutputCache(Duration = 3600 * 24)]
        public ActionResult MainMenu()
        {
            var model = new MenuDao().ListByGroupId(1);

            return PartialView(model);
        }

        [ChildActionOnly]
        public ActionResult TopMenu()
        {
            var model = new MenuDao().ListByGroupId(2);
            return PartialView(model);
        }

        [ChildActionOnly]
        public PartialViewResult HeaderCart()
        {
            var cart = Session[CommonConstants.CartSession];
            var list = new List<CartItem>();
            if (cart != null)
            {
                list = (List<CartItem>)cart;
            }

            return PartialView(list);
        }
    }
}