using Famtela.Application.Features.Layers.Queries.GetAll;
using Famtela.Application.Features.Layers.Queries.GetById;
using Famtela.Shared.Constants.Permission;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Famtela.Application.Features.Layers.Commands.AddEdit;
using Famtela.Application.Features.Layers.Commands.Delete;
using Famtela.Application.Features.Layers.Queries.Export;
using Famtela.Application.Features.Layers.Commands.Import;

namespace Famtela.Server.Controllers.v1.Chicken
{
    public class LayersController : BaseApiController<LayersController>
    {
        /// <summary>
        /// Get All Layers
        /// </summary>
        /// <returns>Status 200 OK</returns>
        [Authorize(Policy = Permissions.Layers.View)]
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var layers = await _mediator.Send(new GetAllLayersQuery());
            return Ok(layers);
        }

        /// <summary>
        /// Get a Layer By Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Status 200 Ok</returns>
        [Authorize(Policy = Permissions.Layers.View)]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var layer = await _mediator.Send(new GetLayerByIdQuery() { Id = id });
            return Ok(layer);
        }

        /// <summary>
        /// Create/Update a Layer
        /// </summary>
        /// <param name="command"></param>
        /// <returns>Status 200 OK</returns>
        [Authorize(Policy = Permissions.Layers.Create)]
        [HttpPost]
        public async Task<IActionResult> Post(AddEditLayerCommand command)
        {
            return Ok(await _mediator.Send(command));
        }

        /// <summary>
        /// Delete a Layer
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Status 200 OK</returns>
        [Authorize(Policy = Permissions.Layers.Delete)]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            return Ok(await _mediator.Send(new DeleteLayerCommand { Id = id }));
        }

        /// <summary>
        /// Search Layers and Export to Excel
        /// </summary>
        /// <param name="searchString"></param>
        /// <returns></returns>
        [Authorize(Policy = Permissions.Layers.Export)]
        [HttpGet("export")]
        public async Task<IActionResult> Export(string searchString = "")
        {
            return Ok(await _mediator.Send(new ExportLayersQuery(searchString)));
        }

        /// <summary>
        /// Import Layers from Excel
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [Authorize(Policy = Permissions.Layers.Import)]
        [HttpPost("import")]
        public async Task<IActionResult> Import(ImportLayersCommand command)
        {
            return Ok(await _mediator.Send(command));
        }
    }
}