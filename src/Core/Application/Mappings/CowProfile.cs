using AutoMapper;
using Famtela.Application.Features.Cows.Commands.AddEdit;
using Famtela.Application.Features.Cows.Queries.GetAll;
using Famtela.Domain.Entities.Dairy;

namespace Famtela.Application.Mappings
{
    public class CowProfile : Profile
    {
        public CowProfile()
        {
            CreateMap<AddEditCowCommand, Cow>().ReverseMap();
            CreateMap<GetAllCowsResponse, Cow>().ReverseMap();
        }
    }
}