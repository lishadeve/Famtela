using AutoMapper;
using Famtela.Application.Requests.Identity;
using Famtela.Application.Responses.Identity;

namespace Famtela.Client.Infrastructure.Mappings
{
    public class RoleProfile : Profile
    {
        public RoleProfile()
        {
            CreateMap<PermissionResponse, PermissionRequest>().ReverseMap();
            CreateMap<RoleClaimResponse, RoleClaimRequest>().ReverseMap();
        }
    }
}