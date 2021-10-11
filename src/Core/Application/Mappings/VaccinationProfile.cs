using AutoMapper;
using Famtela.Application.Features.Vaccinations.Commands.AddEdit;
using Famtela.Domain.Entities.Chicken;

namespace Famtela.Application.Mappings
{
    public class VaccinationProfile : Profile
    {
        public VaccinationProfile()
        {
            CreateMap<AddEditVaccinationCommand, Vaccination>().ReverseMap();
        }
    }
}