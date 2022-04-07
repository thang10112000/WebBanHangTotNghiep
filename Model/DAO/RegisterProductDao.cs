using Model.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.DAO
{
    public class RegisterProductDao
    {
        private ShopBanHangDbContext db = null;

        public RegisterProductDao()
        {
            db = new ShopBanHangDbContext();
        }

        public long InsertFeedBack(Register info)
        {
            db.Registers.Add(info);
            db.SaveChanges();
            return info.ID;
        }
    }
}