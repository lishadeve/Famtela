using AutoMapper;
using Famtela.Application.Features.Documents.Commands.AddEdit;
using Famtela.Application.Features.Documents.Queries.GetById;
using Famtela.Domain.Entities.Misc;

namespace Famtela.Application.Mappings
{
    public class DocumentProfile : Profile
    {
        public DocumentProfile()
        {
            CreateMap<AddEditDocumentCommand, Document>().ReverseMap();
            CreateMap<GetDocumentByIdResponse, Document>().ReverseMap();
        }
    }
}