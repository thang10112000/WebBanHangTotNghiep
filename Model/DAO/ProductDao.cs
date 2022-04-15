using Model.EF;
using Model.ViewModel;
using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.DAO
{
    public class ProductDao
    {
        private ShopBanHangDbContext db = null;

        public ProductDao()
        {
            db = new ShopBanHangDbContext();
        }

        public List<Product> ListNewProduct(int top)
        {
            return db.Products.OrderByDescending(x => x.CreateDate).Take(top).ToList();
        }

        public List<Product> ListLowToHighProduct()
        {
            return db.Products.OrderByDescending(x => x.Price).ToList();
        }

        //public Review ViewReview(long id)
        //{
        //    return db.Reviews.Find(id);
        //}

        public List<string> ListName(string keyword)
        {
            return db.Products.Where(x => x.Name.Contains(keyword)).Select(x => x.Name).ToList();
        }

        public bool ChangeStatus(long id)
        {
            var product = db.Products.Find(id);
            product.Status = !product.Status;
            db.SaveChanges();
            return product.Status;
        }

        public IEnumerable<Product> ListAllPaging(string searchString, int page, int pageSize) // phương thức lấy ra các bảng ghi
        {
            IQueryable<Product> model = db.Products;
            if (!string.IsNullOrEmpty(searchString))
            {
                model = model.Where(x => x.Name.Contains(searchString) || x.Name.Contains(searchString));
            }
            return model.OrderByDescending(x => x.CreateDate).ToPagedList(page, pageSize);
        }

        public IEnumerable<Product> ListLowHight(int page, int pageSize)
        {
            var result = db.Products.OrderBy(x => x.PromotionPrice).ToList();
            var result1 = db.Products.OrderBy(x => x.Price).ToList();
            if (result == null)
            {
                return result.ToPagedList(page, pageSize);
            }
            else
            {
                return result1.ToPagedList(page, pageSize);
            }
        }

        public IEnumerable<Product> ListHightLow(int page, int pageSize)
        {
            var result = db.Products.OrderByDescending(x => x.PromotionPrice).ToList();
            var result1 = db.Products.OrderByDescending(x => x.Price).ToList();
            if (result == null)
            {
                return result.ToPagedList(page, pageSize);
            }
            else
            {
                return result1.ToPagedList(page, pageSize);
            }
        }

        public IEnumerable<Product> Hot(int page, int pageSize)
        {
            var result = db.Products.OrderByDescending(x => x.ViewCount).ToList();

            return result.ToPagedList(page, pageSize);
        }

        /// <summary>
        /// Get list product by category
        /// </summary>
        /// <param name="categoryID"></param>
        /// <returns></returns>
        public List<Product> ListByCategoryId(long categoryID, ref int totalRecord, int pageIndex = 1, int pageSize = 2)
        {
            totalRecord = db.Products.Where(x => x.CategoryID == categoryID).Count();
            var model = db.Products.Where(x => x.CategoryID == categoryID).OrderByDescending(x => x.CreateDate).Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList();
            return model;
        }

        public List<ProductViewModel> Search(string keyword, ref int totalRecord, int pageIndex = 1, int pageSize = 2)
        {
            totalRecord = db.Products.Where(x => x.Name == keyword).Count();
            var model = (from a in db.Products
                         join b in db.ProductCategories
                         on a.CategoryID equals b.ID
                         where a.Name.Contains(keyword)
                         select new
                         {
                             CateMetaTitle = b.MetaTitle,
                             CateName = b.Name,
                             CreatedDate = a.CreateDate,
                             ID = a.ID,
                             Images = a.Image,
                             Name = a.Name,
                             MetaTitle = a.MetaTitle,
                             Price = a.Price,
                             PromotionPrice = a.PromotionPrice
                         }).AsEnumerable().Select(x => new ProductViewModel()
                         {
                             CateMetaTitle = x.MetaTitle,
                             CateName = x.Name,
                             CreatedDate = x.CreatedDate,
                             ID = x.ID,
                             Images = x.Images,
                             Name = x.Name,
                             MetaTitle = x.MetaTitle,
                             Price = x.Price,
                             PromotionPrice = x.PromotionPrice
                         });
            model.OrderByDescending(x => x.CreatedDate).Skip((pageIndex - 1) * pageSize).Take(pageSize);
            return model.ToList();
        }

        /// <summary>
        /// List feature product
        /// </summary>
        /// <param name="top"></param>
        /// <returns></returns>

        public List<Product> ListFeatureProduct(int top)
        {
            var maxValue = db.Products.Max(x => x.ViewCount);
            return db.Products.Where(x => x.Status == true).OrderByDescending(x => x.ViewCount).Take(top).ToList();
        }

        public List<Product> ListRelatedProducts(long productId)
        {
            var product = db.Products.Find(productId);
            return db.Products.Where(x => x.ID != productId && x.CategoryID == product.CategoryID).ToList();
        }

        public Product ViewDetailAdmin(long id)
        {
            return db.Products.Find(id);
        }

        public Product ViewDetail(long id)
        {
            var model = db.Products.Find(id);
            model.ViewCount++;
            db.SaveChanges();
            return model;
        }

        public Product ViewItemCart(long id)
        {
            return db.Products.Find(id);
        }

        public long Insert(Product entity)
        {
            string image = "<image></image>";
            db.Products.Add(entity);
            entity.CreateDate = DateTime.Now;
            entity.ViewCount = 0;
            entity.MoreImages = image;
            db.SaveChanges();
            return entity.ID;
        }

        public int? InsertViewCount(Product entity)
        {
            var product = db.Products.Find(entity.ID);
            product.ViewCount = entity.ViewCount;
            db.SaveChanges();
            return product.ViewCount + 1;
        }

        public bool Update(Product entity)
        {
            try
            {
                var product = db.Products.Find(entity.ID);
                product.Name = entity.Name;
                product.MetaTitle = entity.MetaTitle;
                product.Description = entity.Description;
                product.Image = entity.Image;
                product.MoreImages = entity.MoreImages;
                product.Price = entity.Price;
                product.CategoryID = entity.CategoryID;
                product.Detail = entity.Detail;

                product.Status = entity.Status;
                product.ModifiedBy = entity.ModifiedBy;
                product.ModifiedDate = DateTime.Now;
                product.TopHot = entity.TopHot;
                product.ViewCount = entity.ViewCount;
                db.SaveChanges();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public IEnumerable<Product> ListPaging(string searchString, int page, int pageSize)
        {
            IQueryable<Product> model = db.Products;
            if (!string.IsNullOrEmpty(searchString))
            {
                model = model.Where(x => x.Name.Contains(searchString) || x.MetaTitle.Contains(searchString));
            }

            return model.OrderByDescending(x => x.CreateDate).ToPagedList(page, pageSize);
        }

        public bool Delete(int id)
        {
            try
            {
                var product = db.Products.Find(id);
                db.Products.Remove(product);
                db.SaveChanges();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public List<Product> ListAllProduct()
        {
            return db.Products.OrderByDescending(x => x.CreateDate).ToList();
        }

        public void UpdateImages(long productId, string images)
        {
            var product = db.Products.Find(productId);
            product.MoreImages = images;
            db.SaveChanges();
        }
    }
}