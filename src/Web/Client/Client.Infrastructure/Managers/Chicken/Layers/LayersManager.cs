using Famtela.Application.Features.Layers.Queries.GetAll;
using Famtela.Client.Infrastructure.Extensions;
using Famtela.Shared.Wrapper;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Famtela.Application.Features.Layers.Commands.AddEdit;
using Famtela.Application.Features.Layers.Commands.Import;

namespace Famtela.Client.Infrastructure.Managers.Chicken.Layers
{
    public class LayersManager : ILayersManager
    {
        private readonly HttpClient _httpClient;

        public LayersManager(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<IResult<string>> ExportToExcelAsync(string searchString = "")
        {
            var response = await _httpClient.GetAsync(string.IsNullOrWhiteSpace(searchString)
                ? Routes.LayersEndpoints.Export
                : Routes.LayersEndpoints.ExportFiltered(searchString));
            return await response.ToResult<string>();
        }

        public async Task<IResult<int>> DeleteAsync(int id)
        {
            var response = await _httpClient.DeleteAsync($"{Routes.LayersEndpoints.Delete}/{id}");
            return await response.ToResult<int>();
        }

        public async Task<IResult<List<GetAllLayersResponse>>> GetAllAsync()
        {
            var response = await _httpClient.GetAsync(Routes.LayersEndpoints.GetAll);
            return await response.ToResult<List<GetAllLayersResponse>>();
        }

        public async Task<IResult<int>> SaveAsync(AddEditLayerCommand request)
        {
            var response = await _httpClient.PostAsJsonAsync(Routes.LayersEndpoints.Save, request);
            return await response.ToResult<int>();
        }

        public async Task<IResult<int>> ImportAsync(ImportLayersCommand request)
        {
            var response = await _httpClient.PostAsJsonAsync(Routes.LayersEndpoints.Import, request);
            return await response.ToResult<int>();
        }
    }
}