using Famtela.Application.Features.Eggs.Queries.GetAll;
using Famtela.Client.Infrastructure.Extensions;
using Famtela.Shared.Wrapper;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Famtela.Application.Features.Eggs.Commands.AddEdit;
using Famtela.Application.Features.Eggs.Commands.Import;

namespace Famtela.Client.Infrastructure.Managers.Chicken.Egg
{
    public class EggManager : IEggManager
    {
        private readonly HttpClient _httpClient;

        public EggManager(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<IResult<string>> ExportToExcelAsync(string searchString = "")
        {
            var response = await _httpClient.GetAsync(string.IsNullOrWhiteSpace(searchString)
                ? Routes.EggsEndpoints.Export
                : Routes.EggsEndpoints.ExportFiltered(searchString));
            return await response.ToResult<string>();
        }

        public async Task<IResult<int>> DeleteAsync(int id)
        {
            var response = await _httpClient.DeleteAsync($"{Routes.EggsEndpoints.Delete}/{id}");
            return await response.ToResult<int>();
        }

        public async Task<IResult<List<GetAllEggsResponse>>> GetAllAsync()
        {
            var response = await _httpClient.GetAsync(Routes.EggsEndpoints.GetAll);
            return await response.ToResult<List<GetAllEggsResponse>>();
        }

        public async Task<IResult<int>> SaveAsync(AddEditEggCommand request)
        {
            var response = await _httpClient.PostAsJsonAsync(Routes.EggsEndpoints.Save, request);
            return await response.ToResult<int>();
        }

        public async Task<IResult<int>> ImportAsync(ImportEggsCommand request)
        {
            var response = await _httpClient.PostAsJsonAsync(Routes.EggsEndpoints.Import, request);
            return await response.ToResult<int>();
        }
    }
}