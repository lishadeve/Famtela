using Famtela.Application.Features.FarmProfiles.Commands.AddEdit;
using Famtela.Application.Features.FarmProfiles.Queries.GetAllPaged;
using Famtela.Application.Requests.Catalog;
using Famtela.Client.Infrastructure.Extensions;
using Famtela.Shared.Wrapper;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace Famtela.Client.Infrastructure.Managers.Catalog.FarmProfile
{
    public class FarmProfileManager : IFarmProfileManager
    {
        private readonly HttpClient _httpClient;

        public FarmProfileManager(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<IResult<int>> DeleteAsync(int id)
        {
            var response = await _httpClient.DeleteAsync($"{Routes.FarmProfilesEndpoints.Delete}/{id}");
            return await response.ToResult<int>();
        }

        public async Task<IResult<string>> ExportToExcelAsync(string searchString = "")
        {
            var response = await _httpClient.GetAsync(string.IsNullOrWhiteSpace(searchString)
                ? Routes.FarmProfilesEndpoints.Export
                : Routes.FarmProfilesEndpoints.ExportFiltered(searchString));
            return await response.ToResult<string>();
        }

        public async Task<PaginatedResult<GetAllPagedFarmProfilesResponse>> GetFarmProfilesAsync(GetAllPagedFarmProfilesRequest request)
        {
            var response = await _httpClient.GetAsync(Routes.FarmProfilesEndpoints.GetAllPaged(request.PageNumber, request.PageSize, request.SearchString, request.Orderby));
            return await response.ToPaginatedResult<GetAllPagedFarmProfilesResponse>();
        }

        public async Task<IResult<int>> SaveAsync(AddEditFarmProfileCommand request)
        {
            var response = await _httpClient.PostAsJsonAsync(Routes.FarmProfilesEndpoints.Save, request);
            return await response.ToResult<int>();
        }
    }
}