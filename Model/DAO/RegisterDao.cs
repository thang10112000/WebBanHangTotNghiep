using Model.EF;
using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.DAO
{
    public class RegisterDao
    {
        private ShopBanHangDbContext db = null;

        public RegisterDao()
        {
            db = new ShopBanHangDbContext();
        }

        public IEnumerable<Register> ListAllPaging(string searchString, int page, int pageSize)
        {
            IQueryable<Register> model = db.Registers;
            if (!string.IsNullOrEmpty(searchString))
            {
                model = model.Where(x => x.Information.Contains(searchString));
            }
            return model.OrderByDescending(x => x.CreatedDate).ToPagedList(page, pageSize);
        }

        public bool Delete(int id)
        {
            try
            {
                var register = db.Registers.Find(id);
                db.Registers.Remove(register);
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