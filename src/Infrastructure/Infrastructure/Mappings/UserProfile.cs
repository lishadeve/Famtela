using AutoMapper;
using Famtela.Domain.Entities.Identity;
//using Famtela.Infrastructure.Models.Identity;
using Famtela.Application.Responses.Identity;

namespace Famtela.Infrastructure.Mappings
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            CreateMap<UserResponse, FamtelaUser>().ReverseMap();
            CreateMap<ChatUserResponse, FamtelaUser>().ReverseMap()
                .ForMember(dest => dest.EmailAddress, source => source.MapFrom(source => source.Email)); //Specific Mapping
        }
    }
}