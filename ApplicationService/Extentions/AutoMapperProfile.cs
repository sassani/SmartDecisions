using ApplicationService.Core.Domain;
using ApplicationService.Core.Domain.DTOs;
using AutoMapper;
using AutoMapper.EquivalencyExpression;

namespace ApplicationService.Extentions
{
    public class AutoMapperProfile : AutoMapper.Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<Core.Domain.Profile, ProfileDto>();
            CreateMap<ProfileDto, Core.Domain.Profile>()
                .ForMember(u => u.Contacts, opt => opt.Ignore())
                .ForAllMembers(opt => opt.Condition((src, dest, srcMember) => srcMember != null));


            CreateMap<Contact, ContactDto>();
            CreateMap<ContactDto, Contact>()
                .ForMember(a=>a.Id, opt=>opt.Ignore())
                .EqualityComparison((dto, entity) => dto.Id == entity.Id)
                .ForAllMembers(opt => opt.Condition((src, dest, srcMember) => srcMember != null));

            CreateMap<Avatar, AvatarDto>().ReverseMap();
        }
    }
}
