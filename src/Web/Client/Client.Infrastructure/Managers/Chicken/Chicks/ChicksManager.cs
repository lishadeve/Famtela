using Famtela.Application.Features.Chicks.Queries.GetAll;
using Famtela.Client.Infrastructure.Extensions;
using Famtela.Shared.Wrapper;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Famtela.Application.Features.Chicks.Commands.AddEdit;
using Famtela.Application.Features.Chicks.Commands.Import;

namespace Famtela.Client.Infrastructure.Managers.Chicken.Chicks
{
    public class ChicksManager : IChicksManager
    {
        private readonly HttpClient _httpClient;

        public ChicksManager(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<IResult<string>> ExportToExcelAsync(string searchString = "")
        {
            var response = await _httpClient.GetAsync(string.IsNullOrWhiteSpace(searchString)
                ? Routes.ChicksEndpoints.Export
                : Routes.ChicksEndpoints.ExportFiltered(searchString));
            return await response.ToResult<string>();
        }

        public async Task<IResult<int>> DeleteAsync(int id)
        {
            var response = await _httpClient.DeleteAsync($"{Routes.ChicksEndpoints.Delete}/{id}");
            return await response.ToResult<int>();
        }

        public async Task<IResult<List<GetAllChicksResponse>>> GetAllAsync()
        {
            var response = await _httpClient.GetAsync(Routes.ChicksEndpoints.GetAll);
            return await response.ToResult<List<GetAllChicksResponse>>();
        }

        public async Task<IResult<int>> SaveAsync(AddEditChickCommand request)
        {
            var response = await _httpClient.PostAsJsonAsync(Routes.ChicksEndpoints.Save, request);
            return await response.ToResult<int>();
        }

        public async Task<IResult<int>> ImportAsync(ImportChicksCommand request)
        {
            var response = await _httpClient.PostAsJsonAsync(Routes.ChicksEndpoints.Import, request);
            return await response.ToResult<int>();
        }
    }
}