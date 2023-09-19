using AutoMapper;
using ItemWebApi.Services.Interfaces;
using ItemWebApi.Model;
using ItemWebApi.Repository.IRepository;
using Shared.Common;
using Shared.Constants;
using Shared.DTOs;
using Newtonsoft.Json;
using System.Net.Http;

namespace ItemWebApi.Services.Implementations
{
    public class OrderService : IOrderService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IShipmentService _shipmentService;

        public OrderService(IUnitOfWork unitOfWork, IMapper mapper,IShipmentService shipmentService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _shipmentService= shipmentService;
        }

        public ResponsePackage<bool> AddOrder(NewOrderDTO orderDTO)
        {
            Order order = new Order();
            order.UserId = orderDTO.UserId;
            order.Address = orderDTO.Address;
            order.City = orderDTO.City;
            order.Zip = orderDTO.Zip;
            order.Comment = orderDTO.Comment;
            order.OrderItems = new List<OrderItem>();
            foreach (NewOrderItemDTO oi in orderDTO.Items)
            {
                try
                {
                    Item i = _unitOfWork.Item.GetFirstOrDefault(i => i.Id == oi.Id);
                    if (i.Amount < oi.Amount)
                        throw new Exception("Not enough items");
                    order.OrderItems.Add(new OrderItem()
                    {
                        ItemId = i.Id,
                        SellerId = i.UserId,
                        Amount = oi.Amount,
                    });
                    i.Amount -= oi.Amount;
                    _unitOfWork.Item.Update(i);

                }
                catch (Exception ex)
                {
                    return new ResponsePackage<bool>(false, ResponseStatus.InternalServerError, "There was an error while adding an order:" + ex.Message);
                }
            }

            try
            {
                _unitOfWork.Orders.Add(order);
                _unitOfWork.Save();

                int arrivalMinutes= _shipmentService.ScheduleShipment(order.Id);

                return new ResponsePackage<bool>(true, ResponseStatus.OK, $"Order successfully made. Items will be shipped in {arrivalMinutes} minutes.");
            }
            catch (Exception ex)
            {
                return new ResponsePackage<bool>(false, ResponseStatus.InternalServerError, "There was an error while adding an order:" + ex.Message);
            }
        }
       
        public ResponsePackage<IEnumerable<OrderDTO>> GetAll()
        {
            var list = _unitOfWork.Orders.GetAll(includeProperties:"OrderItems");
            var retList = new List<OrderDTO>();
            foreach (var order in list)
            {
                OrderDTO retOrder = _mapper.Map<OrderDTO>(order);
                foreach(OrderItemDTO item in retOrder.OrderItems)
                {
                    Item i = _unitOfWork.Item.GetFirstOrDefault(i => i.Id == item.ItemId);
                    item.Item = _mapper.Map<ItemDTO>(i);
                    item.SellerId = i.UserId;
                }
                retOrder.Canceled = order.Canceled;
                
                retOrder.Shipped = retOrder.Shipped;

                retOrder.Username = getUsername(order.UserId).Result;
                
                if(retOrder.Canceled == true)
                {
                    break;
                }
                else
                {
                    retList.Add(retOrder);
                }
            }

            return new ResponsePackage<IEnumerable<OrderDTO>>(retList, ResponseStatus.OK, "All orders");
        }

        public ResponsePackage<IEnumerable<OrderDTO>> GetByUser(long UserId)
        {
            var list = _unitOfWork.Orders.GetAll(o=> o.UserId == UserId && !o.Canceled, includeProperties: "OrderItems");
            var retList = new List<OrderDTO>();
            foreach (var order in list)
            {
                OrderDTO retOrder = _mapper.Map<OrderDTO>(order);
                foreach (OrderItemDTO item in retOrder.OrderItems)
                {
                    Item i = _unitOfWork.Item.GetFirstOrDefault(i => i.Id == item.ItemId);
                    item.Item = _mapper.Map<ItemDTO>(i);
                    item.SellerId = i.UserId;
                }
                //Get remaining time
                TimeSpan t = _shipmentService.GetRemainingTime(order.Id);
                retOrder.Minutes = (int)t.TotalMinutes;

                retOrder.Shipped = order.Shipped;
                retOrder.Canceled = order.Canceled;
                retOrder.Username = "";
                retList.Add(retOrder);
            }

            return new ResponsePackage<IEnumerable<OrderDTO>>(retList, ResponseStatus.OK, "All orders for user"+UserId);
        }

        //History for seller
        public ResponsePackage<IEnumerable<OrderDTO>> GetHistory(long UserId)
        {
            var list = _unitOfWork.Orders.GetHistory(UserId);
            var retList = new List<OrderDTO>();

            if(list == null)
            {
                return new ResponsePackage<IEnumerable<OrderDTO>>(retList, ResponseStatus.NotFound, "There are no orders from users" + UserId);
            }

            foreach (var order in list)
            {
                OrderDTO retOrder = _mapper.Map<OrderDTO>(order);
                foreach (OrderItemDTO item in retOrder.OrderItems)
                {
                    if (item.SellerId != UserId)
                        retOrder.OrderItems.Remove(item);
                    else
                    {
                        Item i = _unitOfWork.Item.GetFirstOrDefault(i => i.Id == item.ItemId);
                        item.Item = _mapper.Map<ItemDTO>(i);
                        item.SellerId = i.UserId;
                    }
                }
                retOrder.Canceled = order.Canceled;
                retOrder.Shipped = retOrder.Shipped;
                retOrder.Username = getUsername(order.UserId).Result;
                retList.Add(retOrder);
            }

            return new ResponsePackage<IEnumerable<OrderDTO>>(retList, ResponseStatus.OK, "All previous orders for user" + UserId);
        }

        //Get new orders for seller
        public ResponsePackage<IEnumerable<OrderDTO>> GetNew(long UserId)
        {
            var list = _unitOfWork.Orders.GetNew(UserId);
            var retList = new List<OrderDTO>();
            foreach (var order in list)
            {
                OrderDTO retOrder = _mapper.Map<OrderDTO>(order);
                foreach (OrderItemDTO item in retOrder.OrderItems)
                {
                    Item i = _unitOfWork.Item.GetFirstOrDefault(i => i.Id == item.ItemId);
                    item.Item = _mapper.Map<ItemDTO>(i);
                    item.SellerId = i.UserId;
                }
                //Get remaining time
                retOrder.Minutes = (int)_shipmentService.GetRemainingTime(order.Id).TotalMinutes;

                retOrder.Shipped = order.Shipped;
                retOrder.Canceled = order.Canceled;
                retOrder.Username = getUsername(order.UserId).Result;
                retList.Add(retOrder);
            }

            return new ResponsePackage<IEnumerable<OrderDTO>>(retList, ResponseStatus.OK, "All new orders for user" + UserId);
        }

        //Cancel order
        public ResponsePackage<bool> CancelOrder(long id)
        {
            try
            {
                if (_shipmentService.CancelShipment(id))
                {
                    _unitOfWork.Orders.GetFirstOrDefault(o => o.Id == id).Canceled = true;
                    _unitOfWork.Save();
                    return new ResponsePackage<bool>(true, ResponseStatus.OK, "Order canceled");
                }
                else
                    return new ResponsePackage<bool>(false, ResponseStatus.InternalServerError, "Order can't be canceled");
                
            }
            catch {
                return new ResponsePackage<bool>(false, ResponseStatus.InternalServerError, "There was an error while canceling the order");
            }
        }

        //Ship order
        public bool MarkAsShiped(long id)
        {
            try
            {
                _unitOfWork.Orders.GetFirstOrDefault(o => o.Id == id).Shipped = true;
                _unitOfWork.Orders.Save();
                return true;
            }
            catch
            {
                return false;
            }
        }

        private async Task<string> getUsername(long id)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("https://localhost:5001/");
                var responseTask = await client.GetAsync($"api/users/user?id={id}");
                
                if (responseTask.IsSuccessStatusCode)
                {
                    var userContent = responseTask.Content.ReadAsStringAsync().Result;
                    var user = JsonConvert.DeserializeObject<ProfileDTO>(userContent);
                    return user.UserName;
                }
                else
                {
                    Console.WriteLine(responseTask); // Handle error response if needed
                    return "";
                }
            }
        }

    }
}
