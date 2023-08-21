using OrderApp.Models;

namespace OrderApp.Repositories.Interfaces
{
    public interface IArticleRepository
    {
        Task<List<Article>> GetAll();
        Task<List<Article>> GetSellerArticles(long id);
        Task<Article> GetById(long id);
        Task<Article> InsertArticle(Article article);
        void DeleteArticle(long id);
        void UpdateArticle(Article article);
        Task<byte[]> GetArticleImage(long id);
    }
}
