using OrderApp.Models;

namespace OrderApp.Repositories.Interfaces
{
    public interface IOrderRepository
    {
        Task<List<Order>> GetAll();
        Task<Order> GetById(long id);
        Task<Order> InsertOrder(Order order);
        void DeleteOrder(long id);
        void UpdateOrder(Order order);
        Task<List<Order>> GetSellerOrders(long sellerId, bool delivered);
        Task<long> GetNewestOrderId();
    }
}
