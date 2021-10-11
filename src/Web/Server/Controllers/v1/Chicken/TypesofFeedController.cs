using Famtela.Application.Features.TypesofFeed.Queries.GetAll;
using Famtela.Application.Features.TypesofFeed.Queries.GetById;
using Famtela.Shared.Constants.Permission;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Famtela.Application.Features.TypesofFeed.Commands.AddEdit;
using Famtela.Application.Features.TypesofFeed.Commands.Delete;
using Famtela.Application.Features.TypesofFeed.Queries.Export;
using Famtela.Application.Features.TypesofFeed.Commands.Import;

namespace Famtela.Server.Controllers.v1.Chicken
{
    public class TypesofFeedController : BaseApiController<TypesofFeedController>
    {
        /// <summary>
        /// Get All Types of Feed
        /// </summary>
        /// <returns>Status 200 OK</returns>
        [Authorize(Policy = Permissions.TypesofFeed.View)]
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var typesofFeed = await _mediator.Send(new GetAllTypesofFeedQuery());
            return Ok(typesofFeed);
        }

        /// <summary>
        /// Get a Type of Feed By Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Status 200 Ok</returns>
        [Authorize(Policy = Permissions.TypesofFeed.View)]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var typeofFeed = await _mediator.Send(new GetTypeofFeedByIdQuery() { Id = id });
            return Ok(typeofFeed);
        }

        /// <summary>
        /// Create/Update a Type of Feed
        /// </summary>
        /// <param name="command"></param>
        /// <returns>Status 200 OK</returns>
        [Authorize(Policy = Permissions.TypesofFeed.Create)]
        [HttpPost]
        public async Task<IActionResult> Post(AddEditTypeofFeedCommand command)
        {
            return Ok(await _mediator.Send(command));
        }

        /// <summary>
        /// Delete a Type of Feed
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Status 200 OK</returns>
        [Authorize(Policy = Permissions.TypesofFeed.Delete)]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            return Ok(await _mediator.Send(new DeleteTypeofFeedCommand { Id = id }));
        }

        /// <summary>
        /// Search Types of Feed and Export to Excel
        /// </summary>
        /// <param name="searchString"></param>
        /// <returns></returns>
        [Authorize(Policy = Permissions.TypesofFeed.Export)]
        [HttpGet("export")]
        public async Task<IActionResult> Export(string searchString = "")
        {
            return Ok(await _mediator.Send(new ExportTypesofFeedQuery(searchString)));
        }

        /// <summary>
        /// Import Types of Feed from Excel
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [Authorize(Policy = Permissions.TypesofFeed.Import)]
        [HttpPost("import")]
        public async Task<IActionResult> Import(ImportTypesofFeedCommand command)
        {
            return Ok(await _mediator.Send(command));
        }
    }
}