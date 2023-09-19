using ItemWebApi.Context;
using ItemWebApi.Repository.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ItemWebApi.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        private ApplicationDbContext _db;
        public UnitOfWork(ApplicationDbContext db)
        {
            _db = db;

            Item = new ItemRepository(_db);
            Orders = new OrderRepository(_db);
        }
        public IItemRepository Item { get; set; }
        public IOrderRepository Orders { get; set; }

        public void Save()
        {
            _db.SaveChanges();
        }
    }
}
