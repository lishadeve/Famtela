using Famtela.Application.Features.Cows.Commands.AddEdit;
using Famtela.Application.Features.Cows.Queries.GetAll;
using Famtela.Application.Features.Cows.Queries.GetAllPaged;
using Famtela.Application.Requests.Dairy;
using Famtela.Client.Infrastructure.Extensions;
using Famtela.Shared.Wrapper;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace Famtela.Client.Infrastructure.Managers.Dairy.Cow
{
    public class CowManager : ICowManager
    {
        private readonly HttpClient _httpClient;

        public CowManager(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<IResult<int>> DeleteAsync(int id)
        {
            var response = await _httpClient.DeleteAsync($"{Routes.CowsEndpoints.Delete}/{id}");
            return await response.ToResult<int>();
        }

        public async Task<IResult<List<GetAllCowsResponse>>> GetAllAsync()
        {
            var response = await _httpClient.GetAsync(Routes.CowsEndpoints.GetAll);
            return await response.ToResult<List<GetAllCowsResponse>>();
        }

        public async Task<IResult<string>> ExportToExcelAsync(string searchString = "")
        {
            var response = await _httpClient.GetAsync(string.IsNullOrWhiteSpace(searchString)
                ? Routes.CowsEndpoints.Export
                : Routes.CowsEndpoints.ExportFiltered(searchString));
            return await response.ToResult<string>();
        }

        public async Task<PaginatedResult<GetAllPagedCowsResponse>> GetCowsAsync(GetAllPagedCowsRequest request)
        {
            var response = await _httpClient.GetAsync(Routes.CowsEndpoints.GetAllPaged(request.PageNumber, request.PageSize, request.SearchString, request.Orderby));
            return await response.ToPaginatedResult<GetAllPagedCowsResponse>();
        }

        public async Task<IResult<int>> SaveAsync(AddEditCowCommand request)
        {
            var response = await _httpClient.PostAsJsonAsync(Routes.CowsEndpoints.Save, request);
            return await response.ToResult<int>();
        }
    }
}