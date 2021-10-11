using Famtela.Application.Features.Cows.Commands.AddEdit;
using Famtela.Application.Features.Cows.Queries.GetAll;
using Famtela.Application.Features.Cows.Queries.GetAllPaged;
using Famtela.Application.Requests.Dairy;
using Famtela.Shared.Wrapper;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Famtela.Client.Infrastructure.Managers.Dairy.Cow
{
    public interface ICowManager : IManager
    {
        Task<IResult<List<GetAllCowsResponse>>> GetAllAsync();
        Task<PaginatedResult<GetAllPagedCowsResponse>> GetCowsAsync(GetAllPagedCowsRequest request);

        Task<IResult<int>> SaveAsync(AddEditCowCommand request);

        Task<IResult<int>> DeleteAsync(int id);

        Task<IResult<string>> ExportToExcelAsync(string searchString = "");
    }
}