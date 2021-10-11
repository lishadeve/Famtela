using Famtela.Application.Features.Growers.Queries.GetAll;
using Famtela.Client.Infrastructure.Extensions;
using Famtela.Shared.Wrapper;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Famtela.Application.Features.Growers.Commands.AddEdit;
using Famtela.Application.Features.Growers.Commands.Import;

namespace Famtela.Client.Infrastructure.Managers.Chicken.Growers
{
    public class GrowersManager : IGrowersManager
    {
        private readonly HttpClient _httpClient;

        public GrowersManager(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<IResult<string>> ExportToExcelAsync(string searchString = "")
        {
            var response = await _httpClient.GetAsync(string.IsNullOrWhiteSpace(searchString)
                ? Routes.GrowersEndpoints.Export
                : Routes.GrowersEndpoints.ExportFiltered(searchString));
            return await response.ToResult<string>();
        }

        public async Task<IResult<int>> DeleteAsync(int id)
        {
            var response = await _httpClient.DeleteAsync($"{Routes.GrowersEndpoints.Delete}/{id}");
            return await response.ToResult<int>();
        }

        public async Task<IResult<List<GetAllGrowersResponse>>> GetAllAsync()
        {
            var response = await _httpClient.GetAsync(Routes.GrowersEndpoints.GetAll);
            return await response.ToResult<List<GetAllGrowersResponse>>();
        }

        public async Task<IResult<int>> SaveAsync(AddEditGrowerCommand request)
        {
            var response = await _httpClient.PostAsJsonAsync(Routes.GrowersEndpoints.Save, request);
            return await response.ToResult<int>();
        }

        public async Task<IResult<int>> ImportAsync(ImportGrowersCommand request)
        {
            var response = await _httpClient.PostAsJsonAsync(Routes.GrowersEndpoints.Import, request);
            return await response.ToResult<int>();
        }
    }
}