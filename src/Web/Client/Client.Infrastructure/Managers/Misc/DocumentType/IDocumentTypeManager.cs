using System.Collections.Generic;
using System.Threading.Tasks;
using Famtela.Application.Features.DocumentTypes.Commands.AddEdit;
using Famtela.Application.Features.DocumentTypes.Queries.GetAll;
using Famtela.Shared.Wrapper;

namespace Famtela.Client.Infrastructure.Managers.Misc.DocumentType
{
    public interface IDocumentTypeManager : IManager
    {
        Task<IResult<List<GetAllDocumentTypesResponse>>> GetAllAsync();

        Task<IResult<int>> SaveAsync(AddEditDocumentTypeCommand request);

        Task<IResult<int>> DeleteAsync(int id);

        Task<IResult<string>> ExportToExcelAsync(string searchString = "");
    }
}