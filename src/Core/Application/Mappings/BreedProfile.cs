using AutoMapper;
using Famtela.Application.Features.Breeds.Commands.AddEdit;
using Famtela.Application.Features.Breeds.Queries.GetAll;
using Famtela.Application.Features.Breeds.Queries.GetById;
using Famtela.Domain.Entities.Dairy;

namespace Famtela.Application.Mappings
{
    public class BreedProfile : Profile
    {
        public BreedProfile()
        {
            CreateMap<AddEditBreedCommand, Breed>().ReverseMap();
            CreateMap<GetBreedByIdResponse, Breed>().ReverseMap();
            CreateMap<GetAllBreedsResponse, Breed>().ReverseMap();
        }
    }
}