using Famtela.Application.Features.Ages.Queries.GetAll;
using Famtela.Client.Infrastructure.Extensions;
using Famtela.Shared.Wrapper;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Famtela.Application.Features.Ages.Commands.AddEdit;
using Famtela.Application.Features.Ages.Commands.Import;

namespace Famtela.Client.Infrastructure.Managers.Chicken.Age
{
    public class AgeManager : IAgeManager
    {
        private readonly HttpClient _httpClient;

        public AgeManager(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<IResult<string>> ExportToExcelAsync(string searchString = "")
        {
            var response = await _httpClient.GetAsync(string.IsNullOrWhiteSpace(searchString)
                ? Routes.AgesEndpoints.Export
                : Routes.AgesEndpoints.ExportFiltered(searchString));
            return await response.ToResult<string>();
        }

        public async Task<IResult<int>> DeleteAsync(int id)
        {
            var response = await _httpClient.DeleteAsync($"{Routes.AgesEndpoints.Delete}/{id}");
            return await response.ToResult<int>();
        }

        public async Task<IResult<List<GetAllAgesResponse>>> GetAllAsync()
        {
            var response = await _httpClient.GetAsync(Routes.AgesEndpoints.GetAll);
            return await response.ToResult<List<GetAllAgesResponse>>();
        }

        public async Task<IResult<int>> SaveAsync(AddEditAgeCommand request)
        {
            var response = await _httpClient.PostAsJsonAsync(Routes.AgesEndpoints.Save, request);
            return await response.ToResult<int>();
        }

        public async Task<IResult<int>> ImportAsync(ImportAgesCommand request)
        {
            var response = await _httpClient.PostAsJsonAsync(Routes.AgesEndpoints.Import, request);
            return await response.ToResult<int>();
        }
    }
}