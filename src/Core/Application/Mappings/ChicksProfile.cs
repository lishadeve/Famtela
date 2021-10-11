using AutoMapper;
using Famtela.Application.Features.Chicks.Commands.AddEdit;
using Famtela.Application.Features.Chicks.Queries.GetAll;
using Famtela.Application.Features.Chicks.Queries.GetById;
using Famtela.Domain.Entities.Chicken;

namespace Famtela.Application.Mappings
{
    public class ChicksProfile : Profile
    {
        public ChicksProfile()
        {
            CreateMap<AddEditChickCommand, Chick>().ReverseMap();
            CreateMap<GetChickByIdResponse, Chick>().ReverseMap();
            CreateMap<GetAllChicksResponse, Chick>().ReverseMap();
        }
    }
}