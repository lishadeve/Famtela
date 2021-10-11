using Famtela.Application.Features.Counties.Queries.GetAll;
using Famtela.Client.Infrastructure.Extensions;
using Famtela.Shared.Wrapper;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Famtela.Application.Features.Counties.Commands.AddEdit;
using Famtela.Application.Features.Counties.Commands.Import;

namespace Famtela.Client.Infrastructure.Managers.Catalog.County
{
    public class CountyManager : ICountyManager
    {
        private readonly HttpClient _httpClient;

        public CountyManager(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<IResult<string>> ExportToExcelAsync(string searchString = "")
        {
            var response = await _httpClient.GetAsync(string.IsNullOrWhiteSpace(searchString)
                ? Routes.CountiesEndpoints.Export
                : Routes.CountiesEndpoints.ExportFiltered(searchString));
            return await response.ToResult<string>();
        }

        public async Task<IResult<int>> DeleteAsync(int id)
        {
            var response = await _httpClient.DeleteAsync($"{Routes.CountiesEndpoints.Delete}/{id}");
            return await response.ToResult<int>();
        }

        public async Task<IResult<List<GetAllCountiesResponse>>> GetAllAsync()
        {
            var response = await _httpClient.GetAsync(Routes.CountiesEndpoints.GetAll);
            return await response.ToResult<List<GetAllCountiesResponse>>();
        }

        public async Task<IResult<int>> SaveAsync(AddEditCountyCommand request)
        {
            var response = await _httpClient.PostAsJsonAsync(Routes.CountiesEndpoints.Save, request);
            return await response.ToResult<int>();
        }

        public async Task<IResult<int>> ImportAsync(ImportCountiesCommand request)
        {
            var response = await _httpClient.PostAsJsonAsync(Routes.CountiesEndpoints.Import, request);
            return await response.ToResult<int>();
        }
    }
}