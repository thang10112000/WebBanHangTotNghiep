using Model.EF;
using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.DAO
{
    public class ReviewDao
    {
        private ShopBanHangDbContext db = null;

        public ReviewDao()
        {
            db = new ShopBanHangDbContext();
        }

        public Review ViewDetail(long id)
        {
            return db.Reviews.Find(id);
        }

        public List<Review> ListAll(long id)
        {
            return db.Reviews.Where(x => x.ProductID == id && x.AnswerID == null).OrderByDescending(x => x.CreateDate).ToList();
        }

        public List<Review> ListReviewAnswer(long productid, long reviewid)
        {
            return db.Reviews.Where(x => x.ProductID == productid && x.AnswerID == reviewid).OrderByDescending(x => x.CreateDate).ToList();
        }

        public long Insert(Review entity)
        {
            db.Reviews.Add(entity);
            db.SaveChanges();
            return entity.ID;
        }

        public IEnumerable<Review> ListAllPaging(string searchString, int page, int pageSize)
        {
            IQueryable<Review> model = db.Reviews;
            if (!string.IsNullOrEmpty(searchString))
            {
                model = model.Where(x => x.CreatedBy.Contains(searchString));
            }
            return model.OrderByDescending(x => x.CreateDate).ToPagedList(page, pageSize);
        }

        public bool Delete(int id)
        {
            try
            {
                var reviewcourse = db.Reviews.Find(id);
                db.Reviews.Remove(reviewcourse);
                db.SaveChanges();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}