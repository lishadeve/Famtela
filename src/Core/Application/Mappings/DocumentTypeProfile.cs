using AutoMapper;
using Famtela.Application.Features.DocumentTypes.Commands.AddEdit;
using Famtela.Application.Features.DocumentTypes.Queries.GetAll;
using Famtela.Application.Features.DocumentTypes.Queries.GetById;
using Famtela.Domain.Entities.Misc;

namespace Famtela.Application.Mappings
{
    public class DocumentTypeProfile : Profile
    {
        public DocumentTypeProfile()
        {
            CreateMap<AddEditDocumentTypeCommand, DocumentType>().ReverseMap();
            CreateMap<GetDocumentTypeByIdResponse, DocumentType>().ReverseMap();
            CreateMap<GetAllDocumentTypesResponse, DocumentType>().ReverseMap();
        }
    }
}