using Famtela.Application.Features.Growers.Queries.GetAll;
using Famtela.Application.Features.Growers.Queries.GetById;
using Famtela.Shared.Constants.Permission;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Famtela.Application.Features.Growers.Commands.AddEdit;
using Famtela.Application.Features.Growers.Commands.Delete;
using Famtela.Application.Features.Growers.Queries.Export;
using Famtela.Application.Features.Growers.Commands.Import;

namespace Famtela.Server.Controllers.v1.Chicken
{
    public class GrowersController : BaseApiController<GrowersController>
    {
        /// <summary>
        /// Get All Growers
        /// </summary>
        /// <returns>Status 200 OK</returns>
        [Authorize(Policy = Permissions.Growers.View)]
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var growers = await _mediator.Send(new GetAllGrowersQuery());
            return Ok(growers);
        }

        /// <summary>
        /// Get a Grower By Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Status 200 Ok</returns>
        [Authorize(Policy = Permissions.Growers.View)]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var grower = await _mediator.Send(new GetGrowerByIdQuery() { Id = id });
            return Ok(grower);
        }

        /// <summary>
        /// Create/Update a Grower
        /// </summary>
        /// <param name="command"></param>
        /// <returns>Status 200 OK</returns>
        [Authorize(Policy = Permissions.Growers.Create)]
        [HttpPost]
        public async Task<IActionResult> Post(AddEditGrowerCommand command)
        {
            return Ok(await _mediator.Send(command));
        }

        /// <summary>
        /// Delete a Grower
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Status 200 OK</returns>
        [Authorize(Policy = Permissions.Growers.Delete)]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            return Ok(await _mediator.Send(new DeleteGrowerCommand { Id = id }));
        }

        /// <summary>
        /// Search Growers and Export to Excel
        /// </summary>
        /// <param name="searchString"></param>
        /// <returns></returns>
        [Authorize(Policy = Permissions.Growers.Export)]
        [HttpGet("export")]
        public async Task<IActionResult> Export(string searchString = "")
        {
            return Ok(await _mediator.Send(new ExportGrowersQuery(searchString)));
        }

        /// <summary>
        /// Import Growers from Excel
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [Authorize(Policy = Permissions.Growers.Import)]
        [HttpPost("import")]
        public async Task<IActionResult> Import(ImportGrowersCommand command)
        {
            return Ok(await _mediator.Send(command));
        }
    }
}