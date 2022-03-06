using Model.EF;
using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.DAO
{
    public class MenuDao
    {
        ShopOnline db = null;
        public MenuDao()
        {
            db = new ShopOnline();
        }
        public List<Menu> ListByGroupId(int groupId)
        {
            return db.Menus.Where(x => x.TypeID == groupId && x.Status == true).OrderBy(x => x.DisplayOrder).ToList();
        }
        public long Insert(Menu entity)
        {
            db.Menus.Add(entity);

            db.SaveChanges();
            return entity.ID;
        }

        public bool Update(Menu entity)
        {
            try
            {
                var menu = db.Menus.Find(entity.ID);
                menu.Text = entity.Text;
                menu.Link = entity.Link;
                menu.DisplayOrder = entity.DisplayOrder;
                menu.Target = entity.Target;
                menu.TypeID = entity.TypeID;
                menu.Status = entity.Status;
                menu.ParentID = entity.ParentID;

                db.SaveChanges();
                return true;
            }
            catch (Exception)
            {
                return false;
            }

        }


        public Menu ViewDetail(int id)
        {
            return db.Menus.Find(id);
        }
        public IEnumerable<Menu> ListAllPaging(string searchString, int page, int pageSize)
        {
            IQueryable<Menu> model = db.Menus;
            if (!string.IsNullOrEmpty(searchString))
            {
                model = model.Where(x => x.Text.Contains(searchString) || x.Link.Contains(searchString));
            }
            return model.OrderByDescending(x => x.ID).ToPagedList(page, pageSize);
        }

        public bool Delete(int id)
        {
            try
            {
                var menu = db.Menus.Find(id);
                db.Menus.Remove(menu);
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
            var menu = db.Menus.Find(id);
            menu.Status = !menu.Status;
            db.SaveChanges();
            return menu.Status;
        }

    }
}

