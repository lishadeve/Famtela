using Famtela.Application.Features.Chicks.Queries.GetAll;
using Famtela.Application.Features.Chicks.Queries.GetById;
using Famtela.Shared.Constants.Permission;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Famtela.Application.Features.Chicks.Commands.AddEdit;
using Famtela.Application.Features.Chicks.Commands.Delete;
using Famtela.Application.Features.Chicks.Queries.Export;
using Famtela.Application.Features.Chicks.Commands.Import;

namespace Famtela.Server.Controllers.v1.Chicken
{
    public class ChicksController : BaseApiController<ChicksController>
    {
        /// <summary>
        /// Get All Chicks
        /// </summary>
        /// <returns>Status 200 OK</returns>
        [Authorize(Policy = Permissions.Chicks.View)]
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var chicks = await _mediator.Send(new GetAllChicksQuery());
            return Ok(chicks);
        }

        /// <summary>
        /// Get a Chick By Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Status 200 Ok</returns>
        [Authorize(Policy = Permissions.Chicks.View)]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var chick = await _mediator.Send(new GetChickByIdQuery() { Id = id });
            return Ok(chick);
        }

        /// <summary>
        /// Create/Update a Chick
        /// </summary>
        /// <param name="command"></param>
        /// <returns>Status 200 OK</returns>
        [Authorize(Policy = Permissions.Chicks.Create)]
        [HttpPost]
        public async Task<IActionResult> Post(AddEditChickCommand command)
        {
            return Ok(await _mediator.Send(command));
        }

        /// <summary>
        /// Delete a Chick
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Status 200 OK</returns>
        [Authorize(Policy = Permissions.Chicks.Delete)]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            return Ok(await _mediator.Send(new DeleteChickCommand { Id = id }));
        }

        /// <summary>
        /// Search Chicks and Export to Excel
        /// </summary>
        /// <param name="searchString"></param>
        /// <returns></returns>
        [Authorize(Policy = Permissions.Chicks.Export)]
        [HttpGet("export")]
        public async Task<IActionResult> Export(string searchString = "")
        {
            return Ok(await _mediator.Send(new ExportChicksQuery(searchString)));
        }

        /// <summary>
        /// Import Chicks from Excel
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [Authorize(Policy = Permissions.Chicks.Import)]
        [HttpPost("import")]
        public async Task<IActionResult> Import(ImportChicksCommand command)
        {
            return Ok(await _mediator.Send(command));
        }
    }
}