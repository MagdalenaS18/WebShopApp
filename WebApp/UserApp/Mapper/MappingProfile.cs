using AutoMapper;
using UserApp.DTO;
using UserApp.Models;

namespace UserApp.Mapper
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
            CreateMap<User, UserImageDto>().ReverseMap();
        }
    }
}
