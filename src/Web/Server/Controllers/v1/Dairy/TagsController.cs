using Famtela.Application.Features.Tags.Queries.GetAll;
using Famtela.Application.Features.Tags.Queries.GetById;
using Famtela.Shared.Constants.Permission;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Famtela.Application.Features.Tags.Commands.AddEdit;
using Famtela.Application.Features.Tags.Commands.Delete;
using Famtela.Application.Features.Tags.Queries.Export;
using Famtela.Application.Features.Tags.Commands.Import;

namespace Famtela.Server.Controllers.v1.Dairy
{
    public class TagsController : BaseApiController<TagsController>
    {
        /// <summary>
        /// Get All Tags
        /// </summary>
        /// <returns>Status 200 OK</returns>
        [Authorize(Policy = Permissions.Tags.View)]
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var tags = await _mediator.Send(new GetAllTagsQuery());
            return Ok(tags);
        }

        /// <summary>
        /// Get a Tag By Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Status 200 Ok</returns>
        [Authorize(Policy = Permissions.Tags.View)]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var tag = await _mediator.Send(new GetTagByIdQuery() { Id = id });
            return Ok(tag);
        }

        /// <summary>
        /// Create/Update a Tag
        /// </summary>
        /// <param name="command"></param>
        /// <returns>Status 200 OK</returns>
        [Authorize(Policy = Permissions.Tags.Create)]
        [HttpPost]
        public async Task<IActionResult> Post(AddEditTagCommand command)
        {
            return Ok(await _mediator.Send(command));
        }

        /// <summary>
        /// Delete a Tag
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Status 200 OK</returns>
        [Authorize(Policy = Permissions.Tags.Delete)]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            return Ok(await _mediator.Send(new DeleteTagCommand { Id = id }));
        }

        /// <summary>
        /// Search Tags and Export to Excel
        /// </summary>
        /// <param name="searchString"></param>
        /// <returns></returns>
        [Authorize(Policy = Permissions.Tags.Export)]
        [HttpGet("export")]
        public async Task<IActionResult> Export(string searchString = "")
        {
            return Ok(await _mediator.Send(new ExportTagsQuery(searchString)));
        }

        /// <summary>
        /// Import Tags from Excel
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [Authorize(Policy = Permissions.Tags.Import)]
        [HttpPost("import")]
        public async Task<IActionResult> Import(ImportTagsCommand command)
        {
            return Ok(await _mediator.Send(command));
        }
    }
}