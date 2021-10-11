using Famtela.Application.Features.Tags.Queries.GetAll;
using Famtela.Client.Infrastructure.Extensions;
using Famtela.Shared.Wrapper;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Famtela.Application.Features.Tags.Commands.AddEdit;
using Famtela.Application.Features.Tags.Commands.Import;

namespace Famtela.Client.Infrastructure.Managers.Dairy.Tag
{
    public class TagManager : ITagManager
    {
        private readonly HttpClient _httpClient;

        public TagManager(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<IResult<string>> ExportToExcelAsync(string searchString = "")
        {
            var response = await _httpClient.GetAsync(string.IsNullOrWhiteSpace(searchString)
                ? Routes.TagsEndpoints.Export
                : Routes.TagsEndpoints.ExportFiltered(searchString));
            return await response.ToResult<string>();
        }

        public async Task<IResult<int>> DeleteAsync(int id)
        {
            var response = await _httpClient.DeleteAsync($"{Routes.TagsEndpoints.Delete}/{id}");
            return await response.ToResult<int>();
        }

        public async Task<IResult<List<GetAllTagsResponse>>> GetAllAsync()
        {
            var response = await _httpClient.GetAsync(Routes.TagsEndpoints.GetAll);
            return await response.ToResult<List<GetAllTagsResponse>>();
        }

        public async Task<IResult<int>> SaveAsync(AddEditTagCommand request)
        {
            var response = await _httpClient.PostAsJsonAsync(Routes.TagsEndpoints.Save, request);
            return await response.ToResult<int>();
        }

        public async Task<IResult<int>> ImportAsync(ImportTagsCommand request)
        {
            var response = await _httpClient.PostAsJsonAsync(Routes.TagsEndpoints.Import, request);
            return await response.ToResult<int>();
        }
    }
}