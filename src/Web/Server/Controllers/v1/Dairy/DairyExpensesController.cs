using Famtela.Application.Features.DairyExpenses.Queries.GetAll;
using Famtela.Application.Features.DairyExpenses.Queries.GetById;
using Famtela.Shared.Constants.Permission;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Famtela.Application.Features.DairyExpenses.Commands.AddEdit;
using Famtela.Application.Features.DairyExpenses.Commands.Delete;
using Famtela.Application.Features.DairyExpenses.Queries.Export;
using Famtela.Application.Features.DairyExpenses.Commands.Import;

namespace Famtela.Server.Controllers.v1.Dairy
{
    public class DairyExpensesController : BaseApiController<DairyExpensesController>
    {
        /// <summary>
        /// Get All Dairy Expenses
        /// </summary>
        /// <returns>Status 200 OK</returns>
        [Authorize(Policy = Permissions.DairyExpenses.View)]
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var dairyexpenses = await _mediator.Send(new GetAllDairyExpensesQuery());
            return Ok(dairyexpenses);
        }

        /// <summary>
        /// Get a Dairy Expense By Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Status 200 Ok</returns>
        [Authorize(Policy = Permissions.DairyExpenses.View)]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var dairyExpense = await _mediator.Send(new GetDairyExpenseByIdQuery() { Id = id });
            return Ok(dairyExpense);
        }

        /// <summary>
        /// Create/Update a Dairy Expense
        /// </summary>
        /// <param name="command"></param>
        /// <returns>Status 200 OK</returns>
        [Authorize(Policy = Permissions.DairyExpenses.Create)]
        [HttpPost]
        public async Task<IActionResult> Post(AddEditDairyExpenseCommand command)
        {
            return Ok(await _mediator.Send(command));
        }

        /// <summary>
        /// Delete a Dairy Expense
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Status 200 OK</returns>
        [Authorize(Policy = Permissions.DairyExpenses.Delete)]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            return Ok(await _mediator.Send(new DeleteDairyExpenseCommand { Id = id }));
        }

        /// <summary>
        /// Search Dairy Expenses and Export to Excel
        /// </summary>
        /// <param name="searchString"></param>
        /// <returns></returns>
        [Authorize(Policy = Permissions.DairyExpenses.Export)]
        [HttpGet("export")]
        public async Task<IActionResult> Export(string searchString = "")
        {
            return Ok(await _mediator.Send(new ExportDairyExpensesQuery(searchString)));
        }

        /// <summary>
        /// Import Dairy Expenses from Excel
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [Authorize(Policy = Permissions.DairyExpenses.Import)]
        [HttpPost("import")]
        public async Task<IActionResult> Import(ImportDairyExpensesCommand command)
        {
            return Ok(await _mediator.Send(command));
        }
    }
}