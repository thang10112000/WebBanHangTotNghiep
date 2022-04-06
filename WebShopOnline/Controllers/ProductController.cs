using Model.DAO;
using Model.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Xml.Linq;
using WebShopOnline.Common;

namespace WebShopOnline.Controllers
{
    public class ProductController : Controller
    {
        // GET: Product
        public ActionResult Index(string searchString, int page = 1, int pageSize = 10)
        {
            var model = new ProductDao().ListAllPaging(searchString, page, pageSize);
            int totalRecord = 0;

            ViewBag.Total = totalRecord;
            ViewBag.Page = page;

            int maxPage = 5;
            int totalPage = 0;

            totalPage = (int)Math.Ceiling((double)(totalRecord / pageSize));
            ViewBag.TotalPage = totalPage;
            ViewBag.MaxPage = maxPage;
            ViewBag.First = 1;
            ViewBag.Last = totalPage;
            ViewBag.Next = page + 1;
            ViewBag.Prev = page - 1;
            return View(model);
        }

        [ChildActionOnly]
        [OutputCache(Duration = 3600 * 24)]
        public PartialViewResult ProductCategory()
        {
            var model = new ProductCategoryDao().ListAll();
            return PartialView(model);
        }

        [ChildActionOnly]
        [OutputCache(Duration = 3600 * 24)]
        public PartialViewResult ProductCategory2()
        {
            var model = new ProductCategoryDao().ListAll();
            return PartialView(model);
        }

        public JsonResult ListName(string q)
        {
            var data = new ProductDao().ListName(q);
            return Json(new
            {
                data = data,
                status = true
            }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Category(long cateId, int page = 1, int pageSize = 1)
        {
            var category = new ProductCategoryDao().ViewDetail(cateId);
            ViewBag.Category = category;
            int totalRecord = 0;
            var model = new ProductDao().ListByCategoryId(cateId, ref totalRecord, page, pageSize);

            ViewBag.Total = totalRecord;
            ViewBag.Page = page;

            int maxPage = 5;
            int totalPage = 0;

            totalPage = (int)Math.Ceiling((double)(totalRecord / pageSize));

            ViewBag.TotalPage = totalPage;
            ViewBag.MaxPage = maxPage;
            ViewBag.First = 1;
            ViewBag.Last = totalPage;
            ViewBag.Next = page + 1;
            ViewBag.Prev = page - 1;
            return View(model);
        }

        public ActionResult Search(string keyword, int page = 1, int pageSize = 1)
        {
            int totalRecord = 0;
            var model = new ProductDao().Search(keyword, ref totalRecord, page, pageSize);

            ViewBag.Total = totalRecord;
            ViewBag.Page = page;
            ViewBag.Keyword = keyword;
            int maxPage = 5;
            int totalPage = 0;

            totalPage = (int)Math.Ceiling((double)(totalRecord / pageSize));
            ViewBag.TotalPage = totalPage;
            ViewBag.MaxPage = maxPage;
            ViewBag.First = 1;
            ViewBag.Last = totalPage;
            ViewBag.Next = page + 1;
            ViewBag.Prev = page - 1;

            return View(model);
        }

        //[OutputCache(CacheProfile = "Cache1DayForProduct")]
        //public ActionResult Detail(long id)
        //{
        //    var product = new ProductDao().ViewDetail(id);
        //    ViewBag.Category = new ProductCategoryDao().ViewDetail(product.CategoryID.Value);
        //    ViewBag.RelatedProducts = new ProductDao().ListRelatedProducts(id);
        //    return View(product);
        //}

        public ActionResult Detail(long id)
        {
            var product = new ProductDao().ViewDetail(id);
            //var listReview = new ReviewDao().ListAll(id);
            var images = product.MoreImages;
            XElement xImages = XElement.Parse(images);
            List<string> listImagesReturn = new List<string>();

            foreach (XElement element in xImages.Elements())
            {
                listImagesReturn.Add(element.Value);
            }

            ViewBag.Category = new ProductCategoryDao().ViewDetail(product.CategoryID.Value);
            ViewBag.RelatedProducts = new ProductDao().ListRelatedProducts(id); // xem sản phẩm liên quan
            ViewBag.loadImage = listImagesReturn;
            ViewBag.Review = new ReviewDao().ListAll(id);
            ViewBag.Product = product;
            var review = new Review()
            {
                ProductID = product.ID
            };
            return View(review);
        }

        public ActionResult SendReview(Review review, float rating)
        {
            var dao = new ReviewDao();
            var session = (UserLogin)Session[CommonConstants.USER_SESSION];
            review.CreateDate = DateTime.Now;
            review.UserID = session.UserID;
            review.CreatedBy = session.UserName;
            review.Status = true;
            review.Rating = rating;
            var result = dao.Insert(review);
            if (result > 0)
            {
                return RedirectToAction("Detail", "Product", new { id = review.ProductID });
            }

            return View(review);
        }

        [HttpDelete]
        public ActionResult Delete(int id)
        {
            new ReviewDao().Delete(id);

            return RedirectToAction("Detail", "Product");
        }
    }
}