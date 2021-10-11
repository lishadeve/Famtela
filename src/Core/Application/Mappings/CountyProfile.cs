using AutoMapper;
using Famtela.Application.Features.Counties.Commands.AddEdit;
using Famtela.Application.Features.Counties.Queries.GetAll;
using Famtela.Application.Features.Counties.Queries.GetById;
using Famtela.Domain.Entities.Catalog;

namespace Famtela.Application.Mappings
{
    public class CountyProfile : Profile
    {
        public CountyProfile()
        {
            CreateMap<AddEditCountyCommand, County>().ReverseMap();
            CreateMap<GetCountyByIdResponse, County>().ReverseMap();
            CreateMap<GetAllCountiesResponse, County>().ReverseMap();
        }
    }
}