using Famtela.Application.Features.DairyExpenses.Queries.GetAll;
using Famtela.Shared.Wrapper;
using System.Collections.Generic;
using System.Threading.Tasks;
using Famtela.Application.Features.DairyExpenses.Commands.AddEdit;
using Famtela.Application.Features.DairyExpenses.Commands.Import;

namespace Famtela.Client.Infrastructure.Managers.Dairy.DairyExpense
{
    public interface IDairyExpenseManager : IManager
    {
        Task<IResult<List<GetAllDairyExpensesResponse>>> GetAllAsync();

        Task<IResult<int>> SaveAsync(AddEditDairyExpenseCommand request);

        Task<IResult<int>> DeleteAsync(int id);

        Task<IResult<string>> ExportToExcelAsync(string searchString = "");

        Task<IResult<int>> ImportAsync(ImportDairyExpensesCommand request);
    }
}