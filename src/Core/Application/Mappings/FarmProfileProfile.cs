using AutoMapper;
using Famtela.Application.Features.FarmProfiles.Commands.AddEdit;
using Famtela.Domain.Entities.Catalog;

namespace Famtela.Application.Mappings
{
    public class FarmProfileProfile : Profile
    {
        public FarmProfileProfile()
        {
            CreateMap<AddEditFarmProfileCommand, FarmProfile>().ReverseMap();
        }
    }
}