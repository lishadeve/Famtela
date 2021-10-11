using AutoMapper;
using Famtela.Application.Features.Diseases.Commands.AddEdit;
using Famtela.Application.Features.Diseases.Queries.GetAll;
using Famtela.Application.Features.Diseases.Queries.GetById;
using Famtela.Domain.Entities.Chicken;

namespace Famtela.Application.Mappings
{
    public class DiseaseProfile : Profile
    {
        public DiseaseProfile()
        {
            CreateMap<AddEditDiseaseCommand, Disease>().ReverseMap();
            CreateMap<GetDiseaseByIdResponse, Disease>().ReverseMap();
            CreateMap<GetAllDiseasesResponse, Disease>().ReverseMap();
        }
    }
}