using Famtela.Application.Features.Colors.Queries.GetAll;
using Famtela.Application.Features.Colors.Queries.GetById;
using Famtela.Shared.Constants.Permission;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Famtela.Application.Features.Colors.Commands.AddEdit;
using Famtela.Application.Features.Colors.Commands.Delete;
using Famtela.Application.Features.Colors.Queries.Export;
using Famtela.Application.Features.Colors.Commands.Import;

namespace Famtela.Server.Controllers.v1.Dairy
{
    public class ColorsController : BaseApiController<ColorsController>
    {
        /// <summary>
        /// Get All Colors
        /// </summary>
        /// <returns>Status 200 OK</returns>
        [Authorize(Policy = Permissions.Colors.View)]
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var colors = await _mediator.Send(new GetAllColorsQuery());
            return Ok(colors);
        }

        /// <summary>
        /// Get a Color By Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Status 200 Ok</returns>
        [Authorize(Policy = Permissions.Colors.View)]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var color = await _mediator.Send(new GetColorByIdQuery() { Id = id });
            return Ok(color);
        }

        /// <summary>
        /// Create/Update a Color
        /// </summary>
        /// <param name="command"></param>
        /// <returns>Status 200 OK</returns>
        [Authorize(Policy = Permissions.Colors.Create)]
        [HttpPost]
        public async Task<IActionResult> Post(AddEditColorCommand command)
        {
            return Ok(await _mediator.Send(command));
        }

        /// <summary>
        /// Delete a Color
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Status 200 OK</returns>
        [Authorize(Policy = Permissions.Colors.Delete)]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            return Ok(await _mediator.Send(new DeleteColorCommand { Id = id }));
        }

        /// <summary>
        /// Search Colors and Export to Excel
        /// </summary>
        /// <param name="searchString"></param>
        /// <returns></returns>
        [Authorize(Policy = Permissions.Colors.Export)]
        [HttpGet("export")]
        public async Task<IActionResult> Export(string searchString = "")
        {
            return Ok(await _mediator.Send(new ExportColorsQuery(searchString)));
        }

        /// <summary>
        /// Import Colors from Excel
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [Authorize(Policy = Permissions.Colors.Import)]
        [HttpPost("import")]
        public async Task<IActionResult> Import(ImportColorsCommand command)
        {
            return Ok(await _mediator.Send(command));
        }
    }
}