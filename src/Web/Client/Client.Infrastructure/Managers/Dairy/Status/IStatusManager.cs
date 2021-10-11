using Famtela.Application.Features.Statuses.Queries.GetAll;
using Famtela.Shared.Wrapper;
using System.Collections.Generic;
using System.Threading.Tasks;
using Famtela.Application.Features.Statuses.Commands.AddEdit;
using Famtela.Application.Features.Statuses.Commands.Import;

namespace Famtela.Client.Infrastructure.Managers.Dairy.Status
{
    public interface IStatusManager : IManager
    {
        Task<IResult<List<GetAllStatusesResponse>>> GetAllAsync();

        Task<IResult<int>> SaveAsync(AddEditStatusCommand request);

        Task<IResult<int>> DeleteAsync(int id);

        Task<IResult<string>> ExportToExcelAsync(string searchString = "");

        Task<IResult<int>> ImportAsync(ImportStatusesCommand request);
    }
}