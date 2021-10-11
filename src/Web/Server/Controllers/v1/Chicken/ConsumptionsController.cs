using Famtela.Application.Features.Consumptions.Commands.AddEdit;
using Famtela.Application.Features.Consumptions.Commands.Delete;
using Famtela.Application.Features.Consumptions.Queries.Export;
using Famtela.Application.Features.Consumptions.Queries.GetAllPaged;
using Famtela.Shared.Constants.Permission;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Famtela.Server.Controllers.v1.Chicken
{
    public class ConsumptionsController : BaseApiController<ConsumptionsController>
    {
        /// <summary>
        /// Get All Consumptions
        /// </summary>
        /// <param name="pageNumber"></param>
        /// <param name="pageSize"></param>
        /// <param name="searchString"></param>
        /// <param name="orderBy"></param>
        /// <returns>Status 200 OK</returns>
        [Authorize(Policy = Permissions.Consumptions.View)]
        [HttpGet]
        public async Task<IActionResult> GetAll(int pageNumber, int pageSize, string searchString, string orderBy = null)
        {
            var consumptions = await _mediator.Send(new GetAllConsumptionsQuery(pageNumber, pageSize, searchString, orderBy));
            return Ok(consumptions);
        }

        /// <summary>
        /// Add/Edit a Consumption
        /// </summary>
        /// <param name="command"></param>
        /// <returns>Status 200 OK</returns>
        [Authorize(Policy = Permissions.Consumptions.Create)]
        [HttpPost]
        public async Task<IActionResult> Post(AddEditConsumptionCommand command)
        {
            return Ok(await _mediator.Send(command));
        }

        /// <summary>
        /// Delete a Consumption
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Status 200 OK response</returns>
        [Authorize(Policy = Permissions.Consumptions.Delete)]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            return Ok(await _mediator.Send(new DeleteConsumptionCommand { Id = id }));
        }

        /// <summary>
        /// Search Consumptions and Export to Excel
        /// </summary>
        /// <param name="searchString"></param>
        /// <returns>Status 200 OK</returns>
        [Authorize(Policy = Permissions.Consumptions.Export)]
        [HttpGet("export")]
        public async Task<IActionResult> Export(string searchString = "")
        {
            return Ok(await _mediator.Send(new ExportConsumptionsQuery(searchString)));
        }
    }
}