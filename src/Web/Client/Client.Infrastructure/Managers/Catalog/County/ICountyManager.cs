using Famtela.Application.Features.Counties.Queries.GetAll;
using Famtela.Shared.Wrapper;
using System.Collections.Generic;
using System.Threading.Tasks;
using Famtela.Application.Features.Counties.Commands.AddEdit;
using Famtela.Application.Features.Counties.Commands.Import;

namespace Famtela.Client.Infrastructure.Managers.Catalog.County
{
    public interface ICountyManager : IManager
    {
        Task<IResult<List<GetAllCountiesResponse>>> GetAllAsync();

        Task<IResult<int>> SaveAsync(AddEditCountyCommand request);

        Task<IResult<int>> DeleteAsync(int id);

        Task<IResult<string>> ExportToExcelAsync(string searchString = "");

        Task<IResult<int>> ImportAsync(ImportCountiesCommand request);
    }
}