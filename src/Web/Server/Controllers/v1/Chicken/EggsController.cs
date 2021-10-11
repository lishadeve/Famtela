using Famtela.Application.Features.Eggs.Queries.GetAll;
using Famtela.Application.Features.Eggs.Queries.GetById;
using Famtela.Shared.Constants.Permission;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Famtela.Application.Features.Eggs.Commands.AddEdit;
using Famtela.Application.Features.Eggs.Commands.Delete;
using Famtela.Application.Features.Eggs.Queries.Export;
using Famtela.Application.Features.Eggs.Commands.Import;

namespace Famtela.Server.Controllers.v1.Chicken
{
    public class EggsController : BaseApiController<EggsController>
    {
        /// <summary>
        /// Get All Eggs
        /// </summary>
        /// <returns>Status 200 OK</returns>
        [Authorize(Policy = Permissions.Eggs.View)]
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var Eggs = await _mediator.Send(new GetAllEggsQuery());
            return Ok(Eggs);
        }

        /// <summary>
        /// Get a Egg By Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Status 200 Ok</returns>
        [Authorize(Policy = Permissions.Eggs.View)]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var Egg = await _mediator.Send(new GetEggByIdQuery() { Id = id });
            return Ok(Egg);
        }

        /// <summary>
        /// Create/Update a Egg
        /// </summary>
        /// <param name="command"></param>
        /// <returns>Status 200 OK</returns>
        [Authorize(Policy = Permissions.Eggs.Create)]
        [HttpPost]
        public async Task<IActionResult> Post(AddEditEggCommand command)
        {
            return Ok(await _mediator.Send(command));
        }

        /// <summary>
        /// Delete a Egg
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Status 200 OK</returns>
        [Authorize(Policy = Permissions.Eggs.Delete)]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            return Ok(await _mediator.Send(new DeleteEggCommand { Id = id }));
        }

        /// <summary>
        /// Search Eggs and Export to Excel
        /// </summary>
        /// <param name="searchString"></param>
        /// <returns></returns>
        [Authorize(Policy = Permissions.Eggs.Export)]
        [HttpGet("export")]
        public async Task<IActionResult> Export(string searchString = "")
        {
            return Ok(await _mediator.Send(new ExportEggsQuery(searchString)));
        }

        /// <summary>
        /// Import Eggs from Excel
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [Authorize(Policy = Permissions.Eggs.Import)]
        [HttpPost("import")]
        public async Task<IActionResult> Import(ImportEggsCommand command)
        {
            return Ok(await _mediator.Send(command));
        }
    }
}