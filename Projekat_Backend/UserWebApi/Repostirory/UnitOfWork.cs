using UserWebApi.Context;
using UserWebApi.Repository.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UserWebApi.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        private ApplicationDbContext _db;
        public UnitOfWork(ApplicationDbContext db)
        {
            _db = db;

            User = new UserRepository(_db);
        }
        public IUserRepository User { get; set; }

        public void Save()
        {

            _db.SaveChanges();
        }
    }
}
