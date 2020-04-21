using AutoMapper;
using UserService.Core.Domain;
using UserService.Core.Domain.DTOs;

namespace UserService.Extentions
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<User, UserDto>().ReverseMap();
            CreateMap<Address, AddressDto>().ReverseMap();
        }
    }
}
