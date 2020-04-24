﻿using AutoMapper;
using AutoMapper.EquivalencyExpression;
using DecissionService.Core.Domain;
using DecissionService.Core.Domain.DTOs;

namespace DecissionService.Extentions
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<User, UserDto>();
            CreateMap<UserDto, User>()
                .ForMember(u => u.Addresses, opt => opt.Ignore())
                .ForAllMembers(opt => opt.Condition((src, dest, srcMember) => srcMember != null));


            CreateMap<Address, AddressDto>();
            CreateMap<AddressDto, Address>()
                .ForMember(a=>a.Id, opt=>opt.Ignore())
                .EqualityComparison((dto, entity) => dto.Id == entity.Id)
                .ForAllMembers(opt => opt.Condition((src, dest, srcMember) => srcMember != null));
        }
    }
}