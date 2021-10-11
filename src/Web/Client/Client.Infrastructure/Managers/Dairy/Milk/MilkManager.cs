using Famtela.Application.Features.Milks.Commands.AddEdit;
using Famtela.Application.Features.Milks.Queries.GetAllPaged;
using Famtela.Application.Requests.Dairy;
using Famtela.Client.Infrastructure.Extensions;
using Famtela.Shared.Wrapper;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace Famtela.Client.Infrastructure.Managers.Dairy.Milk
{
    public class MilkManager : IMilkManager
    {
        private readonly HttpClient _httpClient;

        public MilkManager(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<IResult<int>> DeleteAsync(int id)
        {
            var response = await _httpClient.DeleteAsync($"{Routes.MilkEndpoints.Delete}/{id}");
            return await response.ToResult<int>();
        }

        public async Task<IResult<string>> ExportToExcelAsync(string searchString = "")
        {
            var response = await _httpClient.GetAsync(string.IsNullOrWhiteSpace(searchString)
                ? Routes.MilkEndpoints.Export
                : Routes.MilkEndpoints.ExportFiltered(searchString));
            return await response.ToResult<string>();
        }

        public async Task<PaginatedResult<GetAllPagedMilksResponse>> GetMilksAsync(GetAllPagedMilksRequest request)
        {
            var response = await _httpClient.GetAsync(Routes.MilkEndpoints.GetAllPaged(request.PageNumber, request.PageSize, request.SearchString, request.Orderby));
            return await response.ToPaginatedResult<GetAllPagedMilksResponse>();
        }

        public async Task<IResult<int>> SaveAsync(AddEditMilkCommand request)
        {
            var response = await _httpClient.PostAsJsonAsync(Routes.MilkEndpoints.Save, request);
            return await response.ToResult<int>();
        }
    }
}