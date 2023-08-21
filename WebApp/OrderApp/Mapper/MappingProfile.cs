using AutoMapper;
using OrderApp.DTO;
using OrderApp.DTOs;
using OrderApp.Models;

namespace OrderApp.Mapper
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            //CreateMap<Order, CancelOrder>().ReverseMap();
            //CreateMap<Order, NewOrder>().ReverseMap();
            //CreateMap<Order, OrderDetails>().ReverseMap();
            //CreateMap<Order, OrderDetailsView>().ReverseMap();
            //CreateMap<Order, OrderView>().ReverseMap();

            //CreateMap<Article, ArticleView>().ReverseMap();
            //CreateMap<Article, DeleteArticle>().ReverseMap();
            //CreateMap<Article, NewArticle>().ReverseMap();
            //CreateMap<Article, UpdateArticle>().ReverseMap();

            CreateMap<Order, OrderDto>().ReverseMap();
            CreateMap<Order, OrderAllDto>().ReverseMap();
            CreateMap<Order, AdminOrderDto>().ReverseMap();

            CreateMap<OrderArticle, OrderArticlesDto>().ReverseMap();
            CreateMap<OrderArticle, OrderArticleDto>().ReverseMap();

            CreateMap<Article, ArticleDto>().ReverseMap();
            CreateMap<Article, UpdateArticleDto>().ReverseMap();
            CreateMap<Article, ArticleImageDto>().ReverseMap();
            CreateMap<Article, GetArticleDto>().ReverseMap();
            CreateMap<Article, ArticleUploadImageDto>().ReverseMap();
        }
    }
}
