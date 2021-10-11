using AutoMapper;
using Famtela.Application.Features.Tags.Commands.AddEdit;
using Famtela.Application.Features.Tags.Queries.GetAll;
using Famtela.Application.Features.Tags.Queries.GetById;
using Famtela.Domain.Entities.Dairy;

namespace Famtela.Application.Mappings
{
    public class TagProfile : Profile
    {
        public TagProfile()
        {
            CreateMap<AddEditTagCommand, Tag>().ReverseMap();
            CreateMap<GetTagByIdResponse, Tag>().ReverseMap();
            CreateMap<GetAllTagsResponse, Tag>().ReverseMap();
        }
    }
}