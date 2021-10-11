using Famtela.Application.Features.Statuses.Queries.GetAll;
using Famtela.Client.Infrastructure.Extensions;
using Famtela.Shared.Wrapper;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Famtela.Application.Features.Statuses.Commands.AddEdit;
using Famtela.Application.Features.Statuses.Commands.Import;

namespace Famtela.Client.Infrastructure.Managers.Dairy.Status
{
    public class StatusManager : IStatusManager
    {
        private readonly HttpClient _httpClient;

        public StatusManager(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<IResult<string>> ExportToExcelAsync(string searchString = "")
        {
            var response = await _httpClient.GetAsync(string.IsNullOrWhiteSpace(searchString)
                ? Routes.StatusesEndpoints.Export
                : Routes.StatusesEndpoints.ExportFiltered(searchString));
            return await response.ToResult<string>();
        }

        public async Task<IResult<int>> DeleteAsync(int id)
        {
            var response = await _httpClient.DeleteAsync($"{Routes.StatusesEndpoints.Delete}/{id}");
            return await response.ToResult<int>();
        }

        public async Task<IResult<List<GetAllStatusesResponse>>> GetAllAsync()
        {
            var response = await _httpClient.GetAsync(Routes.StatusesEndpoints.GetAll);
            return await response.ToResult<List<GetAllStatusesResponse>>();
        }

        public async Task<IResult<int>> SaveAsync(AddEditStatusCommand request)
        {
            var response = await _httpClient.PostAsJsonAsync(Routes.StatusesEndpoints.Save, request);
            return await response.ToResult<int>();
        }

        public async Task<IResult<int>> ImportAsync(ImportStatusesCommand request)
        {
            var response = await _httpClient.PostAsJsonAsync(Routes.StatusesEndpoints.Import, request);
            return await response.ToResult<int>();
        }
    }
}