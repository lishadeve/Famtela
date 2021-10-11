using AutoMapper;
using Famtela.Infrastructure.Models.Audit;
using Famtela.Application.Responses.Audit;

namespace Famtela.Infrastructure.Mappings
{
    public class AuditProfile : Profile
    {
        public AuditProfile()
        {
            CreateMap<AuditResponse, Audit>().ReverseMap();
        }
    }
}