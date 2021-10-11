using Famtela.Application.Features.ChickenExpenses.Queries.GetAll;
using Famtela.Client.Infrastructure.Extensions;
using Famtela.Shared.Wrapper;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Famtela.Application.Features.ChickenExpenses.Commands.AddEdit;
using Famtela.Application.Features.ChickenExpenses.Commands.Import;

namespace Famtela.Client.Infrastructure.Managers.Chicken.ChickenExpense
{
    public class ChickenExpenseManager : IChickenExpenseManager
    {
        private readonly HttpClient _httpClient;

        public ChickenExpenseManager(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<IResult<string>> ExportToExcelAsync(string searchString = "")
        {
            var response = await _httpClient.GetAsync(string.IsNullOrWhiteSpace(searchString)
                ? Routes.ChickenExpensesEndpoints.Export
                : Routes.ChickenExpensesEndpoints.ExportFiltered(searchString));
            return await response.ToResult<string>();
        }

        public async Task<IResult<int>> DeleteAsync(int id)
        {
            var response = await _httpClient.DeleteAsync($"{Routes.ChickenExpensesEndpoints.Delete}/{id}");
            return await response.ToResult<int>();
        }

        public async Task<IResult<List<GetAllChickenExpensesResponse>>> GetAllAsync()
        {
            var response = await _httpClient.GetAsync(Routes.ChickenExpensesEndpoints.GetAll);
            return await response.ToResult<List<GetAllChickenExpensesResponse>>();
        }

        public async Task<IResult<int>> SaveAsync(AddEditChickenExpenseCommand request)
        {
            var response = await _httpClient.PostAsJsonAsync(Routes.ChickenExpensesEndpoints.Save, request);
            return await response.ToResult<int>();
        }

        public async Task<IResult<int>> ImportAsync(ImportChickenExpensesCommand request)
        {
            var response = await _httpClient.PostAsJsonAsync(Routes.ChickenExpensesEndpoints.Import, request);
            return await response.ToResult<int>();
        }
    }
}