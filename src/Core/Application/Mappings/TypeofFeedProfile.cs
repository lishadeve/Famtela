using AutoMapper;
using Famtela.Application.Features.TypesofFeed.Commands.AddEdit;
using Famtela.Application.Features.TypesofFeed.Queries.GetAll;
using Famtela.Application.Features.TypesofFeed.Queries.GetById;
using Famtela.Domain.Entities.Chicken;

namespace Famtela.Application.Mappings
{
    public class TypeofFeedProfile : Profile
    {
        public TypeofFeedProfile()
        {
            CreateMap<AddEditTypeofFeedCommand, TypeofFeed>().ReverseMap();
            CreateMap<GetTypeofFeedByIdResponse, TypeofFeed>().ReverseMap();
            CreateMap<GetAllTypesofFeedResponse, TypeofFeed>().ReverseMap();
        }
    }
}