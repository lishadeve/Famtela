using Famtela.Application.Features.DairyExpenses.Queries.GetAll;
using Famtela.Client.Infrastructure.Extensions;
using Famtela.Shared.Wrapper;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Famtela.Application.Features.DairyExpenses.Commands.AddEdit;
using Famtela.Application.Features.DairyExpenses.Commands.Import;

namespace Famtela.Client.Infrastructure.Managers.Dairy.DairyExpense
{
    public class DairyExpenseManager : IDairyExpenseManager
    {
        private readonly HttpClient _httpClient;

        public DairyExpenseManager(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<IResult<string>> ExportToExcelAsync(string searchString = "")
        {
            var response = await _httpClient.GetAsync(string.IsNullOrWhiteSpace(searchString)
                ? Routes.DairyExpensesEndpoints.Export
                : Routes.DairyExpensesEndpoints.ExportFiltered(searchString));
            return await response.ToResult<string>();
        }

        public async Task<IResult<int>> DeleteAsync(int id)
        {
            var response = await _httpClient.DeleteAsync($"{Routes.DairyExpensesEndpoints.Delete}/{id}");
            return await response.ToResult<int>();
        }

        public async Task<IResult<List<GetAllDairyExpensesResponse>>> GetAllAsync()
        {
            var response = await _httpClient.GetAsync(Routes.DairyExpensesEndpoints.GetAll);
            return await response.ToResult<List<GetAllDairyExpensesResponse>>();
        }

        public async Task<IResult<int>> SaveAsync(AddEditDairyExpenseCommand request)
        {
            var response = await _httpClient.PostAsJsonAsync(Routes.DairyExpensesEndpoints.Save, request);
            return await response.ToResult<int>();
        }

        public async Task<IResult<int>> ImportAsync(ImportDairyExpensesCommand request)
        {
            var response = await _httpClient.PostAsJsonAsync(Routes.DairyExpensesEndpoints.Import, request);
            return await response.ToResult<int>();
        }
    }
}