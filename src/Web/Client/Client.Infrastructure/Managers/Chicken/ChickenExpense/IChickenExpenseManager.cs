using Famtela.Application.Features.ChickenExpenses.Queries.GetAll;
using Famtela.Shared.Wrapper;
using System.Collections.Generic;
using System.Threading.Tasks;
using Famtela.Application.Features.ChickenExpenses.Commands.AddEdit;
using Famtela.Application.Features.ChickenExpenses.Commands.Import;

namespace Famtela.Client.Infrastructure.Managers.Chicken.ChickenExpense
{
    public interface IChickenExpenseManager : IManager
    {
        Task<IResult<List<GetAllChickenExpensesResponse>>> GetAllAsync();

        Task<IResult<int>> SaveAsync(AddEditChickenExpenseCommand request);

        Task<IResult<int>> DeleteAsync(int id);

        Task<IResult<string>> ExportToExcelAsync(string searchString = "");

        Task<IResult<int>> ImportAsync(ImportChickenExpensesCommand request);
    }
}