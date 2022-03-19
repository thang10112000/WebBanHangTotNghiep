using Model.EF;
using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.DAO
{
    public class TagDao
    {
        private ShopBanHangDbContext db = null;

        public TagDao()
        {
            db = new ShopBanHangDbContext();
        }

        public List<Tag> ListTag()
        {
            return db.Tags.ToList();
        }

        public IEnumerable<Tag> ListAllPaging(string searchString, int page, int pageSize)
        {
            IQueryable<Tag> model = db.Tags;
            if (!string.IsNullOrEmpty(searchString))
            {
                model = model.Where(x => x.Name.Contains(searchString) || x.ID.Contains(searchString));
            }
            return model.OrderByDescending(x => x.ID).ToPagedList(page, pageSize);
        }

        public bool Delete(string id)
        {
            try
            {
                var tag = db.Tags.Find(id);
                db.Tags.Remove(tag);
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