using Famtela.Application.Features.TypesofFarming.Queries.GetAll;
using Famtela.Client.Infrastructure.Extensions;
using Famtela.Shared.Wrapper;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Famtela.Application.Features.TypesofFarming.Commands.AddEdit;
using Famtela.Application.Features.TypesofFarming.Commands.Import;

namespace Famtela.Client.Infrastructure.Managers.Catalog.TypeofFarming
{
    public class TypeofFarmingManager : ITypeofFarmingManager
    {
        private readonly HttpClient _httpClient;

        public TypeofFarmingManager(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<IResult<string>> ExportToExcelAsync(string searchString = "")
        {
            var response = await _httpClient.GetAsync(string.IsNullOrWhiteSpace(searchString)
                ? Routes.TypesofFarmingEndpoints.Export
                : Routes.TypesofFarmingEndpoints.ExportFiltered(searchString));
            return await response.ToResult<string>();
        }

        public async Task<IResult<int>> DeleteAsync(int id)
        {
            var response = await _httpClient.DeleteAsync($"{Routes.TypesofFarmingEndpoints.Delete}/{id}");
            return await response.ToResult<int>();
        }

        public async Task<IResult<List<GetAllTypesofFarmingResponse>>> GetAllAsync()
        {
            var response = await _httpClient.GetAsync(Routes.TypesofFarmingEndpoints.GetAll);
            return await response.ToResult<List<GetAllTypesofFarmingResponse>>();
        }

        public async Task<IResult<int>> SaveAsync(AddEditTypeofFarmingCommand request)
        {
            var response = await _httpClient.PostAsJsonAsync(Routes.TypesofFarmingEndpoints.Save, request);
            return await response.ToResult<int>();
        }

        public async Task<IResult<int>> ImportAsync(ImportTypesofFarmingCommand request)
        {
            var response = await _httpClient.PostAsJsonAsync(Routes.TypesofFarmingEndpoints.Import, request);
            return await response.ToResult<int>();
        }
    }
}