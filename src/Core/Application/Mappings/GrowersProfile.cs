using AutoMapper;
using Famtela.Application.Features.Growers.Commands.AddEdit;
using Famtela.Application.Features.Growers.Queries.GetAll;
using Famtela.Application.Features.Growers.Queries.GetById;
using Famtela.Domain.Entities.Chicken;

namespace Famtela.Application.Mappings
{
    public class GrowersProfile : Profile
    {
        public GrowersProfile()
        {
            CreateMap<AddEditGrowerCommand, Grower>().ReverseMap();
            CreateMap<GetGrowerByIdResponse, Grower>().ReverseMap();
            CreateMap<GetAllGrowersResponse, Grower>().ReverseMap();
        }
    }
}