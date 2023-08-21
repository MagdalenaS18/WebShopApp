using Microsoft.EntityFrameworkCore;
using OrderApp.Infrastructure;
using OrderApp.Models;
using OrderApp.Repositories.Interfaces;

namespace OrderApp.Repositories
{
    public class ArticleRepository : IArticleRepository
    {
        private readonly OrderDbContext _orderDbContext;

        public ArticleRepository(OrderDbContext orderDbContext)
        {
            _orderDbContext = orderDbContext;
        }
        public void DeleteArticle(long id)
        {
            _orderDbContext.Articles.Remove(_orderDbContext.Articles.Find(id));
        }

        public async Task<List<Article>> GetAll()
        {
            return await _orderDbContext.Articles.ToListAsync();
        }

        public async Task<byte[]> GetArticleImage(long id)
        {
            Article article = await _orderDbContext.Articles.FindAsync(id);
            return article.PhotoUrl;
        }

        public async Task<Article> GetById(long id)
        {
            return await _orderDbContext.Articles.FindAsync(id);
        }

        public async Task<List<Article>> GetSellerArticles(long id)
        {
            return await _orderDbContext.Articles.Where(a => a.SellerId == id).ToListAsync();
        }

        public async Task<Article> InsertArticle(Article article)
        {
            await _orderDbContext.Articles.AddAsync(article);
            return article;
        }

        public void UpdateArticle(Article article)
        {
            Article newArticle = _orderDbContext.Articles.Find(article.Id);
            newArticle = article;
            _orderDbContext.Articles.Update(newArticle);
        }
    }
}
