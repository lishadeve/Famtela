using AutoMapper;
using Famtela.Application.Features.Milks.Commands.AddEdit;
using Famtela.Domain.Entities.Dairy;

namespace Famtela.Application.Mappings
{
    public class MilkProfile : Profile
    {
        public MilkProfile()
        {
            CreateMap<AddEditMilkCommand, Milk>().ReverseMap();
        }
    }
}