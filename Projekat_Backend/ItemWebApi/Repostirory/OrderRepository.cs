using ItemWebApi.Context;
using ItemWebApi.Model;
using ItemWebApi.Repository.IRepository;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ItemWebApi.Repository
{
    public class OrderRepository : Repository<Order>, IOrderRepository
    {
        private ApplicationDbContext _db;

        public OrderRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public IEnumerable<Order> GetHistory(long Id)
        {
            List<Order> allOrders = _db.Orders.Include("OrderItems").ToList();
            List<Order> retList = new List<Order>();
            foreach (Order order in allOrders)
                if (order.Canceled || order.Shipped)
                    retList.Add(order);
            return retList;
        }

        public IEnumerable<Order> GetNew(long Id)
        {
            List<Order> allOrders = _db.Orders.Include("OrderItems").ToList();
            List<Order> retList = new List<Order>();
            foreach (Order order in allOrders)
                if (!order.Canceled && !order.Shipped)
                    retList.Add(order);
            return retList;
        }

        public void Save()
        {
            _db.SaveChanges();
        }
        
    }
}
