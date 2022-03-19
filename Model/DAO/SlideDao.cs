using Model.EF;
using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.DAO
{
    public class SlideDao
    {
        private ShopBanHangDbContext db = null;

        public SlideDao()
        {
            db = new ShopBanHangDbContext();
        }

        public List<Slide> ListAll()
        {
            return db.Slides.Where(x => x.Status == true).OrderBy(y => y.DisplayOrder).ToList();
        }

        public long Insert(Slide entity)
        {
            db.Slides.Add(entity);
            entity.CreateDate = DateTime.Now;
            db.SaveChanges();
            return entity.ID;
        }

        public bool Update(Slide entity)
        {
            try
            {
                var slide = db.Slides.Find(entity.ID);
                slide.Image = entity.Image;
                slide.Name = entity.Name;
                slide.Detail = entity.Detail;
                slide.DisplayOrder = entity.DisplayOrder;
                slide.Link = entity.Link;
                slide.ModifiedBy = entity.ModifiedBy;
                slide.ModifiedDate = DateTime.Now;
                slide.Description = entity.Description;
                slide.Status = entity.Status;
                db.SaveChanges();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public Slide ViewDetail(int id)
        {
            return db.Slides.Find(id);
        }

        public IEnumerable<Slide> ListAllPaging(string searchString, int page, int pageSize)
        {
            IQueryable<Slide> model = db.Slides;
            if (!string.IsNullOrEmpty(searchString))
            {
                model = model.Where(x => x.Description.Contains(searchString) || x.Link.Contains(searchString));
            }
            return model.OrderByDescending(x => x.CreateDate).ToPagedList(page, pageSize);
        }

        public bool Delete(int id)
        {
            try
            {
                var slide = db.Slides.Find(id);
                db.Slides.Remove(slide);
                db.SaveChanges();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public bool ChangeStatus(long id)
        {
            var slide = db.Slides.Find(id);
            slide.Status = !slide.Status;
            db.SaveChanges();
            return slide.Status;
        }
    }
}