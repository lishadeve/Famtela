using Famtela.Application.Features.Growers.Queries.GetAll;
using Famtela.Shared.Wrapper;
using System.Collections.Generic;
using System.Threading.Tasks;
using Famtela.Application.Features.Growers.Commands.AddEdit;
using Famtela.Application.Features.Growers.Commands.Import;

namespace Famtela.Client.Infrastructure.Managers.Chicken.Growers
{
    public interface IGrowersManager : IManager
    {
        Task<IResult<List<GetAllGrowersResponse>>> GetAllAsync();

        Task<IResult<int>> SaveAsync(AddEditGrowerCommand request);

        Task<IResult<int>> DeleteAsync(int id);

        Task<IResult<string>> ExportToExcelAsync(string searchString = "");

        Task<IResult<int>> ImportAsync(ImportGrowersCommand request);
    }
}