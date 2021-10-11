using AutoMapper;
using Famtela.Application.Features.Statuses.Commands.AddEdit;
using Famtela.Application.Features.Statuses.Queries.GetAll;
using Famtela.Application.Features.Statuses.Queries.GetById;
using Famtela.Domain.Entities.Dairy;

namespace Famtela.Application.Mappings
{
    public class StatusProfile : Profile
    {
        public StatusProfile()
        {
            CreateMap<AddEditStatusCommand, Status>().ReverseMap();
            CreateMap<GetStatusByIdResponse, Status>().ReverseMap();
            CreateMap<GetAllStatusesResponse, Status>().ReverseMap();
        }
    }
}