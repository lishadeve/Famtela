using Famtela.Application.Features.Vaccinations.Commands.AddEdit;
using Famtela.Application.Features.Vaccinations.Queries.GetAllPaged;
using Famtela.Application.Requests.Chicken;
using Famtela.Client.Infrastructure.Extensions;
using Famtela.Shared.Wrapper;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace Famtela.Client.Infrastructure.Managers.Chicken.Vaccination
{
    public class VaccinationManager : IVaccinationManager
    {
        private readonly HttpClient _httpClient;

        public VaccinationManager(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<IResult<int>> DeleteAsync(int id)
        {
            var response = await _httpClient.DeleteAsync($"{Routes.VaccinationsEndpoints.Delete}/{id}");
            return await response.ToResult<int>();
        }

        public async Task<IResult<string>> ExportToExcelAsync(string searchString = "")
        {
            var response = await _httpClient.GetAsync(string.IsNullOrWhiteSpace(searchString)
                ? Routes.VaccinationsEndpoints.Export
                : Routes.VaccinationsEndpoints.ExportFiltered(searchString));
            return await response.ToResult<string>();
        }

        public async Task<PaginatedResult<GetAllPagedVaccinationsResponse>> GetVaccinationsAsync(GetAllPagedVaccinationsRequest request)
        {
            var response = await _httpClient.GetAsync(Routes.VaccinationsEndpoints.GetAllPaged(request.PageNumber, request.PageSize, request.SearchString, request.Orderby));
            return await response.ToPaginatedResult<GetAllPagedVaccinationsResponse>();
        }

        public async Task<IResult<int>> SaveAsync(AddEditVaccinationCommand request)
        {
            var response = await _httpClient.PostAsJsonAsync(Routes.VaccinationsEndpoints.Save, request);
            return await response.ToResult<int>();
        }
    }
}