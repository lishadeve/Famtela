using Famtela.Application.Features.FarmProfiles.Commands.AddEdit;
using Famtela.Application.Features.FarmProfiles.Queries.GetAllPaged;
using Famtela.Application.Requests.Catalog;
using Famtela.Shared.Wrapper;
using System.Threading.Tasks;

namespace Famtela.Client.Infrastructure.Managers.Catalog.FarmProfile
{
    public interface IFarmProfileManager : IManager
    {
        Task<PaginatedResult<GetAllPagedFarmProfilesResponse>> GetFarmProfilesAsync(GetAllPagedFarmProfilesRequest request);

        Task<IResult<int>> SaveAsync(AddEditFarmProfileCommand request);

        Task<IResult<int>> DeleteAsync(int id);

        Task<IResult<string>> ExportToExcelAsync(string searchString = "");
    }
}