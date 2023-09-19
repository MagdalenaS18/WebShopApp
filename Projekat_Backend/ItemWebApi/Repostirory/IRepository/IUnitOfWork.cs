using ItemWebApi.Repository.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ItemWebApi.Repository.IRepository
{
    public interface IUnitOfWork
    {
        IItemRepository Item { get; }
        IOrderRepository Orders { get; }
        void Save();
    }
}
