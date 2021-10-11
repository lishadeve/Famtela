using Famtela.Application.Features.ChickenExpenses.Queries.GetAll;
using Famtela.Application.Features.ChickenExpenses.Queries.GetById;
using Famtela.Shared.Constants.Permission;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Famtela.Application.Features.ChickenExpenses.Commands.AddEdit;
using Famtela.Application.Features.ChickenExpenses.Commands.Delete;
using Famtela.Application.Features.ChickenExpenses.Queries.Export;
using Famtela.Application.Features.ChickenExpenses.Commands.Import;

namespace Famtela.Server.Controllers.v1.Chicken
{
    public class ChickenExpensesController : BaseApiController<ChickenExpensesController>
    {
        /// <summary>
        /// Get All Chicken Expenses
        /// </summary>
        /// <returns>Status 200 OK</returns>
        [Authorize(Policy = Permissions.ChickenExpenses.View)]
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var chickenexpenses = await _mediator.Send(new GetAllChickenExpensesQuery());
            return Ok(chickenexpenses);
        }

        /// <summary>
        /// Get a Chicken Expense By Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Status 200 Ok</returns>
        [Authorize(Policy = Permissions.ChickenExpenses.View)]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var chickenExpense = await _mediator.Send(new GetChickenExpenseByIdQuery() { Id = id });
            return Ok(chickenExpense);
        }

        /// <summary>
        /// Create/Update a Chicken Expense
        /// </summary>
        /// <param name="command"></param>
        /// <returns>Status 200 OK</returns>
        [Authorize(Policy = Permissions.ChickenExpenses.Create)]
        [HttpPost]
        public async Task<IActionResult> Post(AddEditChickenExpenseCommand command)
        {
            return Ok(await _mediator.Send(command));
        }

        /// <summary>
        /// Delete a Chicken Expense
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Status 200 OK</returns>
        [Authorize(Policy = Permissions.ChickenExpenses.Delete)]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            return Ok(await _mediator.Send(new DeleteChickenExpenseCommand { Id = id }));
        }

        /// <summary>
        /// Search Chicken Expenses and Export to Excel
        /// </summary>
        /// <param name="searchString"></param>
        /// <returns></returns>
        [Authorize(Policy = Permissions.ChickenExpenses.Export)]
        [HttpGet("export")]
        public async Task<IActionResult> Export(string searchString = "")
        {
            return Ok(await _mediator.Send(new ExportChickenExpensesQuery(searchString)));
        }

        /// <summary>
        /// Import Chicken Expenses from Excel
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [Authorize(Policy = Permissions.ChickenExpenses.Import)]
        [HttpPost("import")]
        public async Task<IActionResult> Import(ImportChickenExpensesCommand command)
        {
            return Ok(await _mediator.Send(command));
        }
    }
}