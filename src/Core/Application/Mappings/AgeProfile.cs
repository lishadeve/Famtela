using AutoMapper;
using Famtela.Application.Features.Ages.Commands.AddEdit;
using Famtela.Application.Features.Ages.Queries.GetAll;
using Famtela.Application.Features.Ages.Queries.GetById;
using Famtela.Domain.Entities.Chicken;

namespace Famtela.Application.Mappings
{
    public class AgeProfile : Profile
    {
        public AgeProfile()
        {
            CreateMap<AddEditAgeCommand, Age>().ReverseMap();
            CreateMap<GetAgeByIdResponse, Age>().ReverseMap();
            CreateMap<GetAllAgesResponse, Age>().ReverseMap();
        }
    }
}