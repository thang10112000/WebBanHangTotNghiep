using Model.EF;
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
            return db.Reviews.Where(x => x.ProductID == id).OrderByDescending(x => x.CreateDate).ToList();
        }

        public long Insert(Review entity)
        {
            db.Reviews.Add(entity);
            db.SaveChanges();
            return entity.ID;
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