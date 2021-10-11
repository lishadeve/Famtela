using Famtela.Application.Features.Milks.Commands.AddEdit;
using Famtela.Application.Features.Milks.Queries.GetAllPaged;
using Famtela.Application.Requests.Dairy;
using Famtela.Shared.Wrapper;
using System.Threading.Tasks;

namespace Famtela.Client.Infrastructure.Managers.Dairy.Milk
{
    public interface IMilkManager : IManager
    {
        Task<PaginatedResult<GetAllPagedMilksResponse>> GetMilksAsync(GetAllPagedMilksRequest request);

        Task<IResult<int>> SaveAsync(AddEditMilkCommand request);

        Task<IResult<int>> DeleteAsync(int id);

        Task<IResult<string>> ExportToExcelAsync(string searchString = "");
    }
}