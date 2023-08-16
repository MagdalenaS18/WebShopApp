using AutoMapper;
using WebApp.DTO;
using WebApp.Models;

namespace WebApp.Mapper
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<User, UserDto>().ReverseMap();
            CreateMap<User, RegistrationUserDto>().ReverseMap();
            CreateMap<User, ExternalUserDto>().ReverseMap();
            CreateMap<User, LoginUserDto>().ReverseMap();
            CreateMap<User, UpdateUserDto>().ReverseMap();
            CreateMap<User, ActivationUserDto>().ReverseMap();
            //CreateMap<User, UserDto>().ReverseMap();
        }
    }
}
