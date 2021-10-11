using Famtela.Application.Features.WeightEstimates.Queries.GetAll;
using Famtela.Client.Infrastructure.Extensions;
using Famtela.Shared.Wrapper;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Famtela.Application.Features.WeightEstimates.Commands.AddEdit;
using Famtela.Application.Features.WeightEstimates.Commands.Import;

namespace Famtela.Client.Infrastructure.Managers.Dairy.WeightEstimate
{
    public class WeightEstimateManager : IWeightEstimateManager
    {
        private readonly HttpClient _httpClient;

        public WeightEstimateManager(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<IResult<string>> ExportToExcelAsync(string searchString = "")
        {
            var response = await _httpClient.GetAsync(string.IsNullOrWhiteSpace(searchString)
                ? Routes.WeightEstimatesEndpoints.Export
                : Routes.WeightEstimatesEndpoints.ExportFiltered(searchString));
            return await response.ToResult<string>();
        }

        public async Task<IResult<int>> DeleteAsync(int id)
        {
            var response = await _httpClient.DeleteAsync($"{Routes.WeightEstimatesEndpoints.Delete}/{id}");
            return await response.ToResult<int>();
        }

        public async Task<IResult<List<GetAllWeightEstimatesResponse>>> GetAllAsync()
        {
            var response = await _httpClient.GetAsync(Routes.WeightEstimatesEndpoints.GetAll);
            return await response.ToResult<List<GetAllWeightEstimatesResponse>>();
        }

        public async Task<IResult<int>> SaveAsync(AddEditWeightEstimateCommand request)
        {
            var response = await _httpClient.PostAsJsonAsync(Routes.WeightEstimatesEndpoints.Save, request);
            return await response.ToResult<int>();
        }

        public async Task<IResult<int>> ImportAsync(ImportWeightEstimatesCommand request)
        {
            var response = await _httpClient.PostAsJsonAsync(Routes.WeightEstimatesEndpoints.Import, request);
            return await response.ToResult<int>();
        }
    }
}