using Famtela.Application.Features.TypesofFeed.Queries.GetAll;
using Famtela.Client.Infrastructure.Extensions;
using Famtela.Shared.Wrapper;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Famtela.Application.Features.TypesofFeed.Commands.AddEdit;
using Famtela.Application.Features.TypesofFeed.Commands.Import;

namespace Famtela.Client.Infrastructure.Managers.Chicken.TypeofFeed
{
    public class TypeofFeedManager : ITypeofFeedManager
    {
        private readonly HttpClient _httpClient;

        public TypeofFeedManager(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<IResult<string>> ExportToExcelAsync(string searchString = "")
        {
            var response = await _httpClient.GetAsync(string.IsNullOrWhiteSpace(searchString)
                ? Routes.TypesofFeedEndpoints.Export
                : Routes.TypesofFeedEndpoints.ExportFiltered(searchString));
            return await response.ToResult<string>();
        }

        public async Task<IResult<int>> DeleteAsync(int id)
        {
            var response = await _httpClient.DeleteAsync($"{Routes.TypesofFeedEndpoints.Delete}/{id}");
            return await response.ToResult<int>();
        }

        public async Task<IResult<List<GetAllTypesofFeedResponse>>> GetAllAsync()
        {
            var response = await _httpClient.GetAsync(Routes.TypesofFeedEndpoints.GetAll);
            return await response.ToResult<List<GetAllTypesofFeedResponse>>();
        }

        public async Task<IResult<int>> SaveAsync(AddEditTypeofFeedCommand request)
        {
            var response = await _httpClient.PostAsJsonAsync(Routes.TypesofFeedEndpoints.Save, request);
            return await response.ToResult<int>();
        }

        public async Task<IResult<int>> ImportAsync(ImportTypesofFeedCommand request)
        {
            var response = await _httpClient.PostAsJsonAsync(Routes.TypesofFeedEndpoints.Import, request);
            return await response.ToResult<int>();
        }
    }
}