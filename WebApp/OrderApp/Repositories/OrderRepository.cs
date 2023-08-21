using Microsoft.EntityFrameworkCore;
using OrderApp.Infrastructure;
using OrderApp.Models;
using OrderApp.Repositories.Interfaces;

namespace OrderApp.Repositories
{
    public class OrderRepository : IOrderRepository
    {
        private readonly OrderDbContext _orderDbContext;

        public OrderRepository(OrderDbContext orderDbContext)
        {
            _orderDbContext = orderDbContext;
        }

        public void DeleteOrder(long id)
        {
            _orderDbContext.Orders.Remove(_orderDbContext.Orders.Find(id));
        }

        public async Task<List<Order>> GetAll()
        {
            return await _orderDbContext.Orders.ToListAsync();
        }

        public async Task<Order> GetById(long id)
        {
            return await _orderDbContext.Orders.FindAsync(id);
        }

        public async Task<long> GetNewestOrderId()
        {
            var orders = await _orderDbContext.Orders.OrderByDescending(o => o.Id).ToListAsync();
            return orders.FirstOrDefault().Id;
        }

        public async Task<List<Order>> GetSellerOrders(long sellerId, bool delivered)
        {
            List<Order> orders = await _orderDbContext.Orders.Where(o => o.Confirmed == true && 
                                        delivered ? o.DeliveryDate < DateTime.Now : o.DeliveryDate > DateTime.Now).ToListAsync();
            return orders;
        }

        public async Task<Order> InsertOrder(Order order)
        {
            await _orderDbContext.Orders.AddAsync(order);
            return order;
        }

        public void UpdateOrder(Order order)
        {
            Order newOrder = _orderDbContext.Orders.Find(order.Id);
            newOrder = order;
            _orderDbContext.Orders.Update(newOrder);
        }
    }
}
