using Famtela.Application.Features.Consumptions.Commands.AddEdit;
using Famtela.Application.Features.Consumptions.Queries.GetAllPaged;
using Famtela.Application.Requests.Chicken;
using Famtela.Shared.Wrapper;
using System.Threading.Tasks;

namespace Famtela.Client.Infrastructure.Managers.Chicken.Consumption
{
    public interface IConsumptionManager : IManager
    {
        Task<PaginatedResult<GetAllPagedConsumptionsResponse>> GetConsumptionsAsync(GetAllPagedConsumptionsRequest request);

        Task<IResult<int>> SaveAsync(AddEditConsumptionCommand request);

        Task<IResult<int>> DeleteAsync(int id);

        Task<IResult<string>> ExportToExcelAsync(string searchString = "");
    }
}