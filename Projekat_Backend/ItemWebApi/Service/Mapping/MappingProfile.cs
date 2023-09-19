using ItemWebApi.Model;
using Shared.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ItemWebApi.Services.Mapping
{
    public class MappingProfile: AutoMapper.Profile
    {
        public MappingProfile()
        {
            CreateMap<Item, ItemDTO>().ReverseMap();
            CreateMap<Item, NewItemDTO>().ReverseMap();
            CreateMap<OrderItem, OrderItemDTO>();
            CreateMap<Order, OrderDTO>()
                .ForMember(dest => dest.OrderItems, opt => opt.MapFrom(src => src.OrderItems));
        }
    }
}
