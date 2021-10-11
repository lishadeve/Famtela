using AutoMapper;
using Famtela.Application.Features.TypesofFarming.Commands.AddEdit;
using Famtela.Application.Features.TypesofFarming.Queries.GetAll;
using Famtela.Application.Features.TypesofFarming.Queries.GetById;
using Famtela.Domain.Entities.Catalog;

namespace Famtela.Application.Mappings
{
    public class TypeofFarmingProfile : Profile
    {
        public TypeofFarmingProfile()
        {
            CreateMap<AddEditTypeofFarmingCommand, TypeofFarming>().ReverseMap();
            CreateMap<GetTypeofFarmingByIdResponse, TypeofFarming>().ReverseMap();
            CreateMap<GetAllTypesofFarmingResponse, TypeofFarming>().ReverseMap();
        }
    }
}