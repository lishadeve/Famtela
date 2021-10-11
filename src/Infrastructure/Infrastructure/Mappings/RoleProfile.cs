using AutoMapper;
using Famtela.Infrastructure.Models.Identity;
using Famtela.Application.Responses.Identity;

namespace Famtela.Infrastructure.Mappings
{
    public class RoleProfile : Profile
    {
        public RoleProfile()
        {
            CreateMap<RoleResponse, FamtelaRole>().ReverseMap();
        }
    }
}