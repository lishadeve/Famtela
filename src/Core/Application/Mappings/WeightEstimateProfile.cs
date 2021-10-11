using AutoMapper;
using Famtela.Application.Features.WeightEstimates.Commands.AddEdit;
using Famtela.Application.Features.WeightEstimates.Queries.GetAll;
using Famtela.Application.Features.WeightEstimates.Queries.GetById;
using Famtela.Domain.Entities.Dairy;

namespace Famtela.Application.Mappings
{
    public class WeightEstimateProfile : Profile
    {
        public WeightEstimateProfile()
        {
            CreateMap<AddEditWeightEstimateCommand, WeightEstimate>().ReverseMap();
            CreateMap<GetWeightEstimateByIdResponse, WeightEstimate>().ReverseMap();
            CreateMap<GetAllWeightEstimatesResponse, WeightEstimate>().ReverseMap();
        }
    }
}