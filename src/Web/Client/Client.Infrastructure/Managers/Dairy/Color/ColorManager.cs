using Famtela.Application.Features.Colors.Queries.GetAll;
using Famtela.Client.Infrastructure.Extensions;
using Famtela.Shared.Wrapper;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Famtela.Application.Features.Colors.Commands.AddEdit;
using Famtela.Application.Features.Colors.Commands.Import;

namespace Famtela.Client.Infrastructure.Managers.Dairy.Color
{
    public class ColorManager : IColorManager
    {
        private readonly HttpClient _httpClient;

        public ColorManager(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<IResult<string>> ExportToExcelAsync(string searchString = "")
        {
            var response = await _httpClient.GetAsync(string.IsNullOrWhiteSpace(searchString)
                ? Routes.ColorsEndpoints.Export
                : Routes.ColorsEndpoints.ExportFiltered(searchString));
            return await response.ToResult<string>();
        }

        public async Task<IResult<int>> DeleteAsync(int id)
        {
            var response = await _httpClient.DeleteAsync($"{Routes.ColorsEndpoints.Delete}/{id}");
            return await response.ToResult<int>();
        }

        public async Task<IResult<List<GetAllColorsResponse>>> GetAllAsync()
        {
            var response = await _httpClient.GetAsync(Routes.ColorsEndpoints.GetAll);
            return await response.ToResult<List<GetAllColorsResponse>>();
        }

        public async Task<IResult<int>> SaveAsync(AddEditColorCommand request)
        {
            var response = await _httpClient.PostAsJsonAsync(Routes.ColorsEndpoints.Save, request);
            return await response.ToResult<int>();
        }

        public async Task<IResult<int>> ImportAsync(ImportColorsCommand request)
        {
            var response = await _httpClient.PostAsJsonAsync(Routes.ColorsEndpoints.Import, request);
            return await response.ToResult<int>();
        }
    }
}