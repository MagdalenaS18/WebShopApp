using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OrderApp.DTOs;
using OrderApp.Services.Interfaces;
using System.Runtime.CompilerServices;

namespace OrderApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly IOrderService _orderService;

        public OrderController(IOrderService orderService)
        {
            _orderService = orderService;
        }

        [HttpPost("create-order")]
        [Authorize(Roles = "BUYER")]
        public async Task<IActionResult> NewOrder([FromBody] OrderDto orderDto)
        {
            return Ok(await _orderService.NewOrder(orderDto, _orderService.GetUserIdFromToken(User)));
        }

        [HttpGet("order")]
        [Authorize]
        public async Task<IActionResult> GetOrder(long id)
        {
            return Ok(await _orderService.GetOrder(id));
        }

        [HttpGet("admin-orders")]
        [Authorize(Roles = "ADMIN")]
        public async Task<IActionResult> GetOrders()
        {
            return Ok(await _orderService.GetOrders());
        }

        [HttpGet("buyer-undelivered-orders")]
        [Authorize(Roles = "BUYER")]
        public async Task<IActionResult> GetUndeliveredOrders()
        {
            return Ok(await _orderService.GetUndeliveredOrders(_orderService.GetUserIdFromToken(User)));
        }

        [HttpGet("seller-undelivered-orders")]
        [Authorize(Roles = "SELLER")]
        public async Task<IActionResult> GetSellerUndeliveredOrders()
        {
            return Ok(await _orderService.GetNewOrders(_orderService.GetUserIdFromToken(User)));
        }

        [HttpGet("seller-delivered-orders")]
        [Authorize(Roles = "SELLER")]
        public async Task<IActionResult> GetSellerDeliveredOrders()
        {
            return Ok(await _orderService.GetDeliveredOrders(_orderService.GetUserIdFromToken(User)));
        }

        [HttpGet("buyer-orders")]
        [Authorize(Roles = "BUYER")]
        public async Task<IActionResult> GetUserOrders()
        {
            return Ok(await _orderService.GetUserOrders(_orderService.GetUserIdFromToken(User)));
        }

        [HttpPut("order-cancellation")]
        [Authorize(Roles = "BUYER")]
        public async Task<IActionResult> CancelOrder([FromForm] long orderId)
        {
            return Ok(await _orderService.CancelOrder(orderId));
        }

        [HttpPut("order-confirmation")]
        [Authorize(Roles = "SELLER")]
        public async Task<IActionResult> ApproveOrder([FromForm] long orderId)
        {
            return Ok(await _orderService.ConfirmOrder(orderId));
        }

        [HttpPut("add-to-cart")]
        [Authorize(Roles = "BUYER")]
        public async Task<IActionResult> AddArticlesToOrder([FromBody] OrderArticlesDto orderArticlesDto)
        {
            await _orderService.AddArticlesToOrder(orderArticlesDto.OrderId, orderArticlesDto.OrderArticles);
            return Ok();
        }
    }
}
