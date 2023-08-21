using OrderApp.DTO;
using OrderApp.DTOs;
using OrderApp.Models;
using System.Security.Claims;

namespace OrderApp.Services.Interfaces
{
    public interface IOrderService
    {
        //Task<Order> CreateNewOrder(NewOrder order);
        //Task<List<OrderView>> GetAllByBuyerId(string userId);
        //Task<bool> CancelOrder(CancelOrder order);
        //Task<OrderDetailsView> OrderDetails(OrderDetails orderDetails);
        //Task<OrderDetailsView> SellerOrderDetails(OrderDetails orderDetails);
        //Task<List<OrderView>> GetNewOrdersForSeller(string userId);
        //Task<List<OrderView>> GetMyOrdersForSeller(string userId);
        //Task<List<OrderView>> GetAllOrders();
        //Task<double> GetTotalPrice(List<OrderArticle> products);
        //Task<DateTime> GenerateDeliveringTime();

        Task<long> NewOrder(OrderDto orderDto, long buyerId);
        Task<OrderAllDto> GetOrder(long orderId);
        Task<List<AdminOrderDto>> GetOrders();
        Task<List<OrderAllDto>> GetUndeliveredOrders(long userId);
        Task<List<OrderAllDto>> GetNewOrders(long userId);
        Task<List<OrderAllDto>> GetDeliveredOrders(long userId);
        Task<List<OrderAllDto>> GetUserOrders(long userId);
        Task<bool> CancelOrder(long orderId);
        Task<bool> ConfirmOrder(long orderId);
        Task AddArticlesToOrder(long orderId, List<OrderArticleDto> orderItems);
        long GetUserIdFromToken(ClaimsPrincipal user);
    }
}
