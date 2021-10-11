using Famtela.Application.Features.Cows.Commands.AddEdit;
using Famtela.Application.Features.Cows.Commands.Delete;
using Famtela.Application.Features.Cows.Queries.Export;
using Famtela.Application.Features.Cows.Queries.GetAllPaged;
using Famtela.Shared.Constants.Permission;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Famtela.Server.Controllers.v1.Dairy
{
    public class CowsController : BaseApiController<CowsController>
    {
        /// <summary>
        /// Get All Cows
        /// </summary>
        /// <param name="pageNumber"></param>
        /// <param name="pageSize"></param>
        /// <param name="searchString"></param>
        /// <param name="orderBy"></param>
        /// <returns>Status 200 OK</returns>
        [Authorize(Policy = Permissions.Cows.View)]
        [HttpGet]
        public async Task<IActionResult> GetAll(int pageNumber, int pageSize, string searchString, string orderBy = null)
        {
            var cows = await _mediator.Send(new GetAllCowsQuery(pageNumber, pageSize, searchString, orderBy));
            return Ok(cows);
        }

        /// <summary>
        /// Add/Edit a Cow
        /// </summary>
        /// <param name="command"></param>
        /// <returns>Status 200 OK</returns>
        [Authorize(Policy = Permissions.Cows.Create)]
        [HttpPost]
        public async Task<IActionResult> Post(AddEditCowCommand command)
        {
            return Ok(await _mediator.Send(command));
        }

        /// <summary>
        /// Delete a Cow
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Status 200 OK response</returns>
        [Authorize(Policy = Permissions.Cows.Delete)]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            return Ok(await _mediator.Send(new DeleteCowCommand { Id = id }));
        }

        /// <summary>
        /// Search Cows and Export to Excel
        /// </summary>
        /// <param name="searchString"></param>
        /// <returns>Status 200 OK</returns>
        [Authorize(Policy = Permissions.Cows.Export)]
        [HttpGet("export")]
        public async Task<IActionResult> Export(string searchString = "")
        {
            return Ok(await _mediator.Send(new ExportCowsQuery(searchString)));
        }
    }
}