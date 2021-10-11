using Famtela.Application.Features.Ages.Queries.GetAll;
using Famtela.Application.Features.Ages.Queries.GetById;
using Famtela.Shared.Constants.Permission;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Famtela.Application.Features.Ages.Commands.AddEdit;
using Famtela.Application.Features.Ages.Commands.Delete;
using Famtela.Application.Features.Ages.Queries.Export;
using Famtela.Application.Features.Ages.Commands.Import;

namespace Famtela.Server.Controllers.v1.Chicken
{
    public class AgesController : BaseApiController<AgesController>
    {
        /// <summary>
        /// Get All Ages
        /// </summary>
        /// <returns>Status 200 OK</returns>
        [Authorize(Policy = Permissions.Ages.View)]
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var ages = await _mediator.Send(new GetAllAgesQuery());
            return Ok(ages);
        }

        /// <summary>
        /// Get a Age By Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Status 200 Ok</returns>
        [Authorize(Policy = Permissions.Ages.View)]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var age = await _mediator.Send(new GetAgeByIdQuery() { Id = id });
            return Ok(age);
        }

        /// <summary>
        /// Create/Update a Age
        /// </summary>
        /// <param name="command"></param>
        /// <returns>Status 200 OK</returns>
        [Authorize(Policy = Permissions.Ages.Create)]
        [HttpPost]
        public async Task<IActionResult> Post(AddEditAgeCommand command)
        {
            return Ok(await _mediator.Send(command));
        }

        /// <summary>
        /// Delete a Age
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Status 200 OK</returns>
        [Authorize(Policy = Permissions.Ages.Delete)]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            return Ok(await _mediator.Send(new DeleteAgeCommand { Id = id }));
        }

        /// <summary>
        /// Search Ages and Export to Excel
        /// </summary>
        /// <param name="searchString"></param>
        /// <returns></returns>
        [Authorize(Policy = Permissions.Ages.Export)]
        [HttpGet("export")]
        public async Task<IActionResult> Export(string searchString = "")
        {
            return Ok(await _mediator.Send(new ExportAgesQuery(searchString)));
        }

        /// <summary>
        /// Import Ages from Excel
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [Authorize(Policy = Permissions.Ages.Import)]
        [HttpPost("import")]
        public async Task<IActionResult> Import(ImportAgesCommand command)
        {
            return Ok(await _mediator.Send(command));
        }
    }
}