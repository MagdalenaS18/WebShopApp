using AutoMapper;
using OrderApp.DTO;
using OrderApp.DTOs;
using OrderApp.Models;
using OrderApp.Repositories.Interfaces;
using OrderApp.Services.Interfaces;
using System.Security.Claims;
using static Common.Constants;

namespace OrderApp.Services
{
    public class OrderService : IOrderService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public OrderService(IUnitOfWork unitOfWork,  IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        //long.TryParse(User.Claims.First(c => c.Type == "Id").Value, out int id)

        public async Task AddArticlesToOrder(long orderId, List<OrderArticleDto> orderItems)
        {
            Order order = await _unitOfWork.Orders.GetById(orderId);
            if(order == null)
            {
                throw new Exception("Order doesn't exist.");
            }

            List<Article> articles = await _unitOfWork.Articles.GetAll();
            List<OrderArticle> orderArticles = new List<OrderArticle>();

            foreach(var ordArt in orderItems)
            {
                orderArticles.Add(_mapper.Map<OrderArticle>(ordArt));
            }

            foreach(var article in articles)
            {
                foreach(var ordArt in orderArticles)
                {
                    ordArt.OrderId = orderId;
                }
            }
            order.OrderArticles = orderArticles;

            foreach(var article in orderArticles)
            {
                await _unitOfWork.OrderArticles.InsertArticleToOrder(article);
            }

            //var articlesBySellers = await _unitOfWork.Articles.GetAll();
            //var sellerCount = await _unitOfWork.OrderArticles.GetNumberOfSellers(orderId);

            foreach(var article in articles)
            {
                foreach(var  orderArticle in orderArticles)
                {
                    if(orderArticle.ArticleId == article.Id)
                    {
                        orderArticle.Price = article.Price * orderArticle.ArticleQuantity;
                        //oItem.SellerId = item.SellerId;
                        orderArticle.SellerId = article.SellerId;
                    }
                    order.TotalPrice = orderArticle.Price;
                }
            }

            List<long> ids = new List<long>();
            List<long> sellers = new List<long>();

            foreach (var articleOrd in order.OrderArticles)
            {
                ids.Add(articleOrd.SellerId);
                foreach(var article in articles)
                {
                    if(articleOrd.ArticleId == article.Id)
                    {
                        if(article.MaxQuantity >= articleOrd.ArticleQuantity)
                        {
                            article.MaxQuantity = article.MaxQuantity - articleOrd.ArticleQuantity;
                            _unitOfWork.Articles.UpdateArticle(article);
                            //articleOrd.Price = article.Price * articleOrd.ArticleQuantity;
                        }
                        //articleOrd.Price += article.Price;
                    }
                }
                //ids.Add(articleOrd.SellerId);
                foreach(var id in ids)
                {
                    if (ids.Contains(articleOrd.SellerId))
                    {
                        if (!sellers.Contains(articleOrd.SellerId))
                        {
                            sellers.Add(articleOrd.SellerId);
                        }
                    }
                }
            }

            //var sellerCount = await _unitOfWork.OrderArticles.GetNumberOfSellers(order.Id);

            //foreach (var seller in sellerCount)
            //{
            //    if (ids.Contains(article.SellerId))
            //    {
            //        if (!sellers.Contains(seller))
            //        {
            //            sellers.Add(seller);
            //        }
            //    }
            //}

            //List<int> sellers = new List<int>();
            //foreach(var id in ids)
            //{
            //    if(ids.Contains(articles))
            //}



            order.TotalPrice += DeliveryCost * sellers.Count();

            _unitOfWork.Orders.UpdateOrder(order);
            await _unitOfWork.Save();
        }

        private async Task<List<OrderArticle>> GetArticlesForOrderId(long id)
        {
            List<OrderArticle> articles = await _unitOfWork.OrderArticles.GetAll();
            return articles.Where(oa => oa.OrderId == id).ToList();
        }

        public async Task<bool> CancelOrder(long orderId)
        {
            var order = await _unitOfWork.Orders.GetById(orderId);
            if(order == null)
            {
                throw new Exception("Order doesn't exist.");
            }
            order.OrderArticles = await GetArticlesForOrderId(orderId);
            List<Article> articles = await _unitOfWork.Articles.GetAll();

            foreach(var articleOrder in order.OrderArticles)
            {
                foreach(var article in articles)
                {
                    if(articleOrder.ArticleId == article.Id)
                    {
                        article.MaxQuantity = article.MaxQuantity + articleOrder.ArticleQuantity;
                        _unitOfWork.Articles.UpdateArticle(article);
                    }
                }
            }

            order.Confirmed = false;
            _unitOfWork.Orders.UpdateOrder(order);
            await _unitOfWork.Save();
            return true;
        }

        public async Task<bool> ConfirmOrder(long orderId)
        {
            var order = await _unitOfWork.Orders.GetById(orderId);
            if(order == null)
            {
                throw new Exception("Order doesn't exist.");
            }
            
            order.Approved = true;
            Random random = new Random();
            int minutes = random.Next(65, 180);
            order.DeliveryDate = DateTime.Now.AddMinutes(minutes);

            _unitOfWork.Orders.UpdateOrder(order);
            await _unitOfWork.Save();
            return true;
        }

        public async Task<List<OrderAllDto>> GetDeliveredOrders(long userId)
        {
            var sellerOrders = await _unitOfWork.Orders.GetSellerOrders(userId, true);
            foreach(var order in sellerOrders)
            {
                order.OrderArticles = await GetArticlesForOrderId(order.Id);
            }

            return _mapper.Map<List<OrderAllDto>>(sellerOrders);
        }

        public async Task<List<OrderAllDto>> GetNewOrders(long userId)
        {
            var sellerOrders = await _unitOfWork.Orders.GetSellerOrders(userId, false);
            foreach(var order in sellerOrders)
            {
                order.OrderArticles = await GetArticlesForOrderId(order.Id);
            }

            return _mapper.Map<List<OrderAllDto>>(sellerOrders);
        }

        public async Task<OrderAllDto> GetOrder(long orderId)
        {
            Order order = await _unitOfWork.Orders.GetById(orderId);
            order.OrderArticles = await GetArticlesForOrderId(orderId);
            return _mapper.Map<OrderAllDto>(order);
        }

        public async Task<List<AdminOrderDto>> GetOrders()
        {
            var orders = await _unitOfWork.Orders.GetAll();
            foreach(var order in orders)
            {
                order.OrderArticles = await GetArticlesForOrderId(order.Id);
            }
            
            return _mapper.Map<List<AdminOrderDto>>(orders);
        }

        public async Task<List<OrderAllDto>> GetUndeliveredOrders(long userId)
        {
            List<Order> orders = await _unitOfWork.Orders.GetAll();
            var buyerOrders = orders.Where(o => o.BuyerId == userId).ToList();
            foreach(var order in buyerOrders)
            {
                order.OrderArticles = await GetArticlesForOrderId(order.Id);
            }

            return _mapper.Map<List<OrderAllDto>>(buyerOrders.Where(o => o.DeliveryDate > DateTime.Now && o.Confirmed)); ;
        }

        public long GetUserIdFromToken(ClaimsPrincipal user)
        {
            long id;
            long.TryParse(user.Identity.Name, out id);
            return id;
        }

        public async Task<List<OrderAllDto>> GetUserOrders(long userId)
        {
            List<Order> orders = await _unitOfWork.Orders.GetAll();
            var userOrders = orders.Where(o => o.BuyerId == userId).ToList();
            foreach(var order in userOrders)
            {
                order.OrderArticles = await GetArticlesForOrderId(order.Id);
            }
            
            return _mapper.Map<List<OrderAllDto>>(userOrders.Where(o => o.DeliveryDate < DateTime.Now && o.Confirmed));
        }

        public async Task<long> NewOrder(OrderDto orderDto, long buyerId)
        {
            bool isPayed = orderDto.IsPayed;
            Order newOrder = _mapper.Map<Order>(orderDto);
            newOrder.Confirmed = true;
            if (isPayed)
            {
                newOrder.Approved = false;
                newOrder.Address = "";
                newOrder.DeliveryDate = DateTime.Now.AddMinutes(10);
            }
            else
            {
                newOrder.Approved = true;
                Random random = new Random();
                int minutes = random.Next(65, 180);
                newOrder.DeliveryDate = DateTime.Now.AddMinutes(minutes);
            }
            newOrder.BuyerId = buyerId;

            await _unitOfWork.Orders.InsertOrder(newOrder);
            await _unitOfWork.Save();

            var lastOrderId = await _unitOfWork.Orders.GetNewestOrderId();
            return lastOrderId;
        }
    }
}
