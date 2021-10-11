using Famtela.Application.Features.Statuses.Queries.GetAll;
using Famtela.Application.Features.Statuses.Queries.GetById;
using Famtela.Shared.Constants.Permission;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Famtela.Application.Features.Statuses.Commands.AddEdit;
using Famtela.Application.Features.Statuses.Commands.Delete;
using Famtela.Application.Features.Statuses.Queries.Export;
using Famtela.Application.Features.Statuses.Commands.Import;

namespace Famtela.Server.Controllers.v1.Dairy
{
    public class StatusesController : BaseApiController<StatusesController>
    {
        /// <summary>
        /// Get All Statuses
        /// </summary>
        /// <returns>Status 200 OK</returns>
        [Authorize(Policy = Permissions.Statuses.View)]
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var statuses = await _mediator.Send(new GetAllStatusesQuery());
            return Ok(statuses);
        }

        /// <summary>
        /// Get a Status By Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Status 200 Ok</returns>
        [Authorize(Policy = Permissions.Statuses.View)]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var status = await _mediator.Send(new GetStatusByIdQuery() { Id = id });
            return Ok(status);
        }

        /// <summary>
        /// Create/Update a Status
        /// </summary>
        /// <param name="command"></param>
        /// <returns>Status 200 OK</returns>
        [Authorize(Policy = Permissions.Statuses.Create)]
        [HttpPost]
        public async Task<IActionResult> Post(AddEditStatusCommand command)
        {
            return Ok(await _mediator.Send(command));
        }

        /// <summary>
        /// Delete a Status
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Status 200 OK</returns>
        [Authorize(Policy = Permissions.Statuses.Delete)]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            return Ok(await _mediator.Send(new DeleteStatusCommand { Id = id }));
        }

        /// <summary>
        /// Search Statuses and Export to Excel
        /// </summary>
        /// <param name="searchString"></param>
        /// <returns></returns>
        [Authorize(Policy = Permissions.Statuses.Export)]
        [HttpGet("export")]
        public async Task<IActionResult> Export(string searchString = "")
        {
            return Ok(await _mediator.Send(new ExportStatusesQuery(searchString)));
        }

        /// <summary>
        /// Import Statuses from Excel
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [Authorize(Policy = Permissions.Statuses.Import)]
        [HttpPost("import")]
        public async Task<IActionResult> Import(ImportStatusesCommand command)
        {
            return Ok(await _mediator.Send(command));
        }
    }
}