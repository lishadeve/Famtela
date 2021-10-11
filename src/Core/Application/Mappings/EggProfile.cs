using AutoMapper;
using Famtela.Application.Features.Eggs.Commands.AddEdit;
using Famtela.Application.Features.Eggs.Queries.GetAll;
using Famtela.Application.Features.Eggs.Queries.GetById;
using Famtela.Domain.Entities.Chicken;

namespace Famtela.Application.Mappings
{
    public class EggProfile : Profile
    {
        public EggProfile()
        {
            CreateMap<AddEditEggCommand, Egg>().ReverseMap();
            CreateMap<GetEggByIdResponse, Egg>().ReverseMap();
            CreateMap<GetAllEggsResponse, Egg>().ReverseMap();
        }
    }
}