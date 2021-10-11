using AutoMapper;
using Famtela.Application.Features.Layers.Commands.AddEdit;
using Famtela.Application.Features.Layers.Queries.GetAll;
using Famtela.Application.Features.Layers.Queries.GetById;
using Famtela.Domain.Entities.Chicken;

namespace Famtela.Application.Mappings
{
    public class LayersProfile : Profile
    {
        public LayersProfile()
        {
            CreateMap<AddEditLayerCommand, Layer>().ReverseMap();
            CreateMap<GetLayerByIdResponse, Layer>().ReverseMap();
            CreateMap<GetAllLayersResponse, Layer>().ReverseMap();
        }
    }
}