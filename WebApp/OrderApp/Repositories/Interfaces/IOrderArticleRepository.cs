using OrderApp.Models;

namespace OrderApp.Repositories.Interfaces
{
    public interface IOrderArticleRepository
    {
        Task<List<OrderArticle>> GetAll();
        Task<OrderArticle> GetById(long id);
        Task<OrderArticle> InsertArticleToOrder(OrderArticle orderArticle);
        void Delete(long orderId, long articleId);
        void Update(OrderArticle orderArticle);
        //Task<int> GetNumberOfSellers(long orderId);
    }
}
