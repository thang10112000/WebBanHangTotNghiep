﻿using Model.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.DAO
{
    public class OrderDao
    {
        private ShopBanHangDbContext db = null;

        public OrderDao()
        {
            db = new ShopBanHangDbContext();
        }

        public long Insert(Order order)
        {
            db.Orders.Add(order);
            db.SaveChanges();
            return order.ID;
        }
    }
}