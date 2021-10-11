using Famtela.Application.Features.Counties.Queries.GetAll;
using Famtela.Application.Features.Counties.Queries.GetById;
using Famtela.Shared.Constants.Permission;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Famtela.Application.Features.Counties.Commands.AddEdit;
using Famtela.Application.Features.Counties.Commands.Delete;
using Famtela.Application.Features.Counties.Queries.Export;
using Famtela.Application.Features.Counties.Commands.Import;

namespace Famtela.Server.Controllers.v1.Catalog
{
    public class CountiesController : BaseApiController<CountiesController>
    {
        /// <summary>
        /// Get All Counties
        /// </summary>
        /// <returns>Status 200 OK</returns>
        [Authorize(Policy = Permissions.Counties.View)]
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var counties = await _mediator.Send(new GetAllCountiesQuery());
            return Ok(counties);
        }

        /// <summary>
        /// Get a County By Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Status 200 Ok</returns>
        [Authorize(Policy = Permissions.Counties.View)]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var county = await _mediator.Send(new GetCountyByIdQuery() { Id = id });
            return Ok(county);
        }

        /// <summary>
        /// Create/Update a County
        /// </summary>
        /// <param name="command"></param>
        /// <returns>Status 200 OK</returns>
        [Authorize(Policy = Permissions.Counties.Create)]
        [HttpPost]
        public async Task<IActionResult> Post(AddEditCountyCommand command)
        {
            return Ok(await _mediator.Send(command));
        }

        /// <summary>
        /// Delete a County
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Status 200 OK</returns>
        [Authorize(Policy = Permissions.Counties.Delete)]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            return Ok(await _mediator.Send(new DeleteCountyCommand { Id = id }));
        }

        /// <summary>
        /// Search Counties and Export to Excel
        /// </summary>
        /// <param name="searchString"></param>
        /// <returns></returns>
        [Authorize(Policy = Permissions.Counties.Export)]
        [HttpGet("export")]
        public async Task<IActionResult> Export(string searchString = "")
        {
            return Ok(await _mediator.Send(new ExportCountiesQuery(searchString)));
        }

        /// <summary>
        /// Import Counties from Excel
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [Authorize(Policy = Permissions.Counties.Import)]
        [HttpPost("import")]
        public async Task<IActionResult> Import(ImportCountiesCommand command)
        {
            return Ok(await _mediator.Send(command));
        }
    }
}