using OrderApp.Infrastructure;
using OrderApp.Repositories.Interfaces;

namespace OrderApp.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly OrderDbContext _orderDbContext;

        public IOrderRepository Orders { get; }

        public IArticleRepository Articles { get; }

        public IOrderArticleRepository OrderArticles { get; }

        public UnitOfWork(OrderDbContext orderDbContext, IOrderRepository orders,
            IArticleRepository articles, IOrderArticleRepository orderArticles)
        {
            _orderDbContext = orderDbContext;
            Orders = orders;
            Articles = articles;
            OrderArticles = orderArticles;
        }

        public async Task Save()
        {
            await _orderDbContext.SaveChangesAsync();
        }
    }
}
