using Famtela.Application.Features.Milks.Commands.AddEdit;
using Famtela.Application.Features.Milks.Commands.Delete;
using Famtela.Application.Features.Milks.Queries.Export;
using Famtela.Application.Features.Milks.Queries.GetAllPaged;
using Famtela.Shared.Constants.Permission;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Famtela.Server.Controllers.v1.Dairy
{
    public class MilkController : BaseApiController<MilkController>
    {
        /// <summary>
        /// Get All Milk
        /// </summary>
        /// <param name="pageNumber"></param>
        /// <param name="pageSize"></param>
        /// <param name="searchString"></param>
        /// <param name="orderBy"></param>
        /// <returns>Status 200 OK</returns>
        [Authorize(Policy = Permissions.Milk.View)]
        [HttpGet]
        public async Task<IActionResult> GetAll(int pageNumber, int pageSize, string searchString, string orderBy = null)
        {
            var milk = await _mediator.Send(new GetAllMilksQuery(pageNumber, pageSize, searchString, orderBy));
            return Ok(milk);
        }

        /// <summary>
        /// Add/Edit a Milk
        /// </summary>
        /// <param name="command"></param>
        /// <returns>Status 200 OK</returns>
        [Authorize(Policy = Permissions.Milk.Create)]
        [HttpPost]
        public async Task<IActionResult> Post(AddEditMilkCommand command)
        {
            return Ok(await _mediator.Send(command));
        }

        /// <summary>
        /// Delete a Milk
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Status 200 OK response</returns>
        [Authorize(Policy = Permissions.Milk.Delete)]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            return Ok(await _mediator.Send(new DeleteMilkCommand { Id = id }));
        }

        /// <summary>
        /// Search Milk and Export to Excel
        /// </summary>
        /// <param name="searchString"></param>
        /// <returns>Status 200 OK</returns>
        [Authorize(Policy = Permissions.Milk.Export)]
        [HttpGet("export")]
        public async Task<IActionResult> Export(string searchString = "")
        {
            return Ok(await _mediator.Send(new ExportMilksQuery(searchString)));
        }
    }
}