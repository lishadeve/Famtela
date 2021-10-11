using AutoMapper;
using Famtela.Application.Requests.Identity;
using Famtela.Application.Responses.Identity;
using Famtela.Infrastructure.Models.Identity;

namespace Famtela.Infrastructure.Mappings
{
    public class RoleClaimProfile : Profile
    {
        public RoleClaimProfile()
        {
            CreateMap<RoleClaimResponse, FamtelaRoleClaim>()
                .ForMember(nameof(FamtelaRoleClaim.ClaimType), opt => opt.MapFrom(c => c.Type))
                .ForMember(nameof(FamtelaRoleClaim.ClaimValue), opt => opt.MapFrom(c => c.Value))
                .ReverseMap();

            CreateMap<RoleClaimRequest, FamtelaRoleClaim>()
                .ForMember(nameof(FamtelaRoleClaim.ClaimType), opt => opt.MapFrom(c => c.Type))
                .ForMember(nameof(FamtelaRoleClaim.ClaimValue), opt => opt.MapFrom(c => c.Value))
                .ReverseMap();
        }
    }
}