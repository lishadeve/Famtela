using Famtela.Application.Features.Diseases.Queries.GetAll;
using Famtela.Client.Infrastructure.Extensions;
using Famtela.Shared.Wrapper;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Famtela.Application.Features.Diseases.Commands.AddEdit;
using Famtela.Application.Features.Diseases.Commands.Import;

namespace Famtela.Client.Infrastructure.Managers.Chicken.Disease
{
    public class DiseaseManager : IDiseaseManager
    {
        private readonly HttpClient _httpClient;

        public DiseaseManager(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<IResult<string>> ExportToExcelAsync(string searchString = "")
        {
            var response = await _httpClient.GetAsync(string.IsNullOrWhiteSpace(searchString)
                ? Routes.DiseasesEndpoints.Export
                : Routes.DiseasesEndpoints.ExportFiltered(searchString));
            return await response.ToResult<string>();
        }

        public async Task<IResult<int>> DeleteAsync(int id)
        {
            var response = await _httpClient.DeleteAsync($"{Routes.DiseasesEndpoints.Delete}/{id}");
            return await response.ToResult<int>();
        }

        public async Task<IResult<List<GetAllDiseasesResponse>>> GetAllAsync()
        {
            var response = await _httpClient.GetAsync(Routes.DiseasesEndpoints.GetAll);
            return await response.ToResult<List<GetAllDiseasesResponse>>();
        }

        public async Task<IResult<int>> SaveAsync(AddEditDiseaseCommand request)
        {
            var response = await _httpClient.PostAsJsonAsync(Routes.DiseasesEndpoints.Save, request);
            return await response.ToResult<int>();
        }

        public async Task<IResult<int>> ImportAsync(ImportDiseasesCommand request)
        {
            var response = await _httpClient.PostAsJsonAsync(Routes.DiseasesEndpoints.Import, request);
            return await response.ToResult<int>();
        }
    }
}