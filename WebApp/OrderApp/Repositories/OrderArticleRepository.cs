using Microsoft.EntityFrameworkCore;
using OrderApp.Infrastructure;
using OrderApp.Models;
using OrderApp.Repositories.Interfaces;

namespace OrderApp.Repositories
{
    public class OrderArticleRepository : IOrderArticleRepository
    {
        private readonly OrderDbContext _orderDbContext;

        public OrderArticleRepository(OrderDbContext orderDbContext)
        {
            _orderDbContext = orderDbContext;
        }

        //public void Delete(long id)
        //{
        //    _orderDbContext.OrderArticles.Remove(_orderDbContext.OrderArticles.Find(id));
        //}

        public void Delete(long orderId, long articleId)
        {
            _orderDbContext.OrderArticles.Remove(_orderDbContext.OrderArticles.Find(orderId, articleId));
        }

        //public async Task<int> GetNumberOfSellers(long orderId)
        //{
        //    var articles = await _orderDbContext.OrderArticles.Where(o => o.OrderId == orderId).GroupBy(a => a.SellerId).ToListAsync();
        //    return articles.Count;
        //}

        public async Task<List<OrderArticle>> GetAll()
        {
            return await _orderDbContext.OrderArticles.ToListAsync();
        }

        public async Task<OrderArticle> GetById(long id)
        {
            return await _orderDbContext.OrderArticles.FindAsync(id);
        }

        //public async Task<OrderArticle> Insert(OrderArticle orderArticle)
        //{
        //    await _orderDbContext.OrderArticles.AddAsync(orderArticle);
        //    return orderArticle;
        //}

        public async Task<OrderArticle> InsertArticleToOrder(OrderArticle orderArticle)
        {
            await _orderDbContext.OrderArticles.AddAsync(orderArticle);
            return orderArticle;
        }

        public void Update(OrderArticle orderArticle)
        {
            OrderArticle newOrderArticle = _orderDbContext.OrderArticles.Find(orderArticle.OrderId, orderArticle.ArticleId);
            newOrderArticle = orderArticle;
            _orderDbContext.OrderArticles.Update(newOrderArticle);
        }
    }
}
