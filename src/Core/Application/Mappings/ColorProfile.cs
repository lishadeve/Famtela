using AutoMapper;
using Famtela.Application.Features.Colors.Commands.AddEdit;
using Famtela.Application.Features.Colors.Queries.GetAll;
using Famtela.Application.Features.Colors.Queries.GetById;
using Famtela.Domain.Entities.Dairy;

namespace Famtela.Application.Mappings
{
    public class ColorProfile : Profile
    {
        public ColorProfile()
        {
            CreateMap<AddEditColorCommand, Color>().ReverseMap();
            CreateMap<GetColorByIdResponse, Color>().ReverseMap();
            CreateMap<GetAllColorsResponse, Color>().ReverseMap();
        }
    }
}