using Famtela.Application.Features.Chicks.Queries.GetAll;
using Famtela.Shared.Wrapper;
using System.Collections.Generic;
using System.Threading.Tasks;
using Famtela.Application.Features.Chicks.Commands.AddEdit;
using Famtela.Application.Features.Chicks.Commands.Import;

namespace Famtela.Client.Infrastructure.Managers.Chicken.Chicks
{
    public interface IChicksManager : IManager
    {
        Task<IResult<List<GetAllChicksResponse>>> GetAllAsync();

        Task<IResult<int>> SaveAsync(AddEditChickCommand request);

        Task<IResult<int>> DeleteAsync(int id);

        Task<IResult<string>> ExportToExcelAsync(string searchString = "");

        Task<IResult<int>> ImportAsync(ImportChicksCommand request);
    }
}