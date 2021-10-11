using AutoMapper;
using Famtela.Application.Features.Consumptions.Commands.AddEdit;
using Famtela.Domain.Entities.Chicken;

namespace Famtela.Application.Mappings
{
    public class ConsumptionProfile : Profile
    {
        public ConsumptionProfile()
        {
            CreateMap<AddEditConsumptionCommand, Consumption>().ReverseMap();
        }
    }
}