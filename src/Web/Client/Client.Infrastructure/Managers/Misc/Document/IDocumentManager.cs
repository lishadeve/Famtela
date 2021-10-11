using Famtela.Application.Features.Documents.Commands.AddEdit;
using Famtela.Application.Features.Documents.Queries.GetAll;
using Famtela.Application.Requests.Documents;
using Famtela.Shared.Wrapper;
using System.Threading.Tasks;
using Famtela.Application.Features.Documents.Queries.GetById;

namespace Famtela.Client.Infrastructure.Managers.Misc.Document
{
    public interface IDocumentManager : IManager
    {
        Task<PaginatedResult<GetAllDocumentsResponse>> GetAllAsync(GetAllPagedDocumentsRequest request);

        Task<IResult<GetDocumentByIdResponse>> GetByIdAsync(GetDocumentByIdQuery request);

        Task<IResult<int>> SaveAsync(AddEditDocumentCommand request);

        Task<IResult<int>> DeleteAsync(int id);
    }
}