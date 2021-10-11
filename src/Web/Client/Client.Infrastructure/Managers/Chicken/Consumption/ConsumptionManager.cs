using Famtela.Application.Features.Consumptions.Commands.AddEdit;
using Famtela.Application.Features.Consumptions.Queries.GetAllPaged;
using Famtela.Application.Requests.Chicken;
using Famtela.Client.Infrastructure.Extensions;
using Famtela.Shared.Wrapper;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace Famtela.Client.Infrastructure.Managers.Chicken.Consumption
{
    public class ConsumptionManager : IConsumptionManager
    {
        private readonly HttpClient _httpClient;

        public ConsumptionManager(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<IResult<int>> DeleteAsync(int id)
        {
            var response = await _httpClient.DeleteAsync($"{Routes.ConsumptionsEndpoints.Delete}/{id}");
            return await response.ToResult<int>();
        }

        public async Task<IResult<string>> ExportToExcelAsync(string searchString = "")
        {
            var response = await _httpClient.GetAsync(string.IsNullOrWhiteSpace(searchString)
                ? Routes.ConsumptionsEndpoints.Export
                : Routes.ConsumptionsEndpoints.ExportFiltered(searchString));
            return await response.ToResult<string>();
        }

        public async Task<PaginatedResult<GetAllPagedConsumptionsResponse>> GetConsumptionsAsync(GetAllPagedConsumptionsRequest request)
        {
            var response = await _httpClient.GetAsync(Routes.ConsumptionsEndpoints.GetAllPaged(request.PageNumber, request.PageSize, request.SearchString, request.Orderby));
            return await response.ToPaginatedResult<GetAllPagedConsumptionsResponse>();
        }

        public async Task<IResult<int>> SaveAsync(AddEditConsumptionCommand request)
        {
            var response = await _httpClient.PostAsJsonAsync(Routes.ConsumptionsEndpoints.Save, request);
            return await response.ToResult<int>();
        }
    }
}