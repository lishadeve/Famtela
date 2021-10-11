using Famtela.Application.Features.Breeds.Queries.GetAll;
using Famtela.Client.Infrastructure.Extensions;
using Famtela.Shared.Wrapper;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Famtela.Application.Features.Breeds.Commands.AddEdit;
using Famtela.Application.Features.Breeds.Commands.Import;

namespace Famtela.Client.Infrastructure.Managers.Dairy.Breed
{
    public class BreedManager : IBreedManager
    {
        private readonly HttpClient _httpClient;

        public BreedManager(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<IResult<string>> ExportToExcelAsync(string searchString = "")
        {
            var response = await _httpClient.GetAsync(string.IsNullOrWhiteSpace(searchString)
                ? Routes.BreedsEndpoints.Export
                : Routes.BreedsEndpoints.ExportFiltered(searchString));
            return await response.ToResult<string>();
        }

        public async Task<IResult<int>> DeleteAsync(int id)
        {
            var response = await _httpClient.DeleteAsync($"{Routes.BreedsEndpoints.Delete}/{id}");
            return await response.ToResult<int>();
        }

        public async Task<IResult<List<GetAllBreedsResponse>>> GetAllAsync()
        {
            var response = await _httpClient.GetAsync(Routes.BreedsEndpoints.GetAll);
            return await response.ToResult<List<GetAllBreedsResponse>>();
        }

        public async Task<IResult<int>> SaveAsync(AddEditBreedCommand request)
        {
            var response = await _httpClient.PostAsJsonAsync(Routes.BreedsEndpoints.Save, request);
            return await response.ToResult<int>();
        }

        public async Task<IResult<int>> ImportAsync(ImportBreedsCommand request)
        {
            var response = await _httpClient.PostAsJsonAsync(Routes.BreedsEndpoints.Import, request);
            return await response.ToResult<int>();
        }
    }
}