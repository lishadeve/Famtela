using Famtela.Application.Features.TypesofFarming.Queries.GetAll;
using Famtela.Application.Features.TypesofFarming.Queries.GetById;
using Famtela.Shared.Constants.Permission;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Famtela.Application.Features.TypesofFarming.Commands.AddEdit;
using Famtela.Application.Features.TypesofFarming.Commands.Delete;
using Famtela.Application.Features.TypesofFarming.Queries.Export;
using Famtela.Application.Features.TypesofFarming.Commands.Import;

namespace Famtela.Server.Controllers.v1.Catalog
{
    public class TypesofFarmingController : BaseApiController<TypesofFarmingController>
    {
        /// <summary>
        /// Get All TypesofFarming
        /// </summary>
        /// <returns>Status 200 OK</returns>
        [Authorize(Policy = Permissions.TypesofFarming.View)]
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var typesoffarming = await _mediator.Send(new GetAllTypesofFarmingQuery());
            return Ok(typesoffarming);
        }

        /// <summary>
        /// Get a TypeofFarming By Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Status 200 Ok</returns>
        [Authorize(Policy = Permissions.TypesofFarming.View)]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var typeoffarming = await _mediator.Send(new GetTypeofFarmingByIdQuery() { Id = id });
            return Ok(typeoffarming);
        }

        /// <summary>
        /// Create/Update a TypeofFarming
        /// </summary>
        /// <param name="command"></param>
        /// <returns>Status 200 OK</returns>
        [Authorize(Policy = Permissions.TypesofFarming.Create)]
        [HttpPost]
        public async Task<IActionResult> Post(AddEditTypeofFarmingCommand command)
        {
            return Ok(await _mediator.Send(command));
        }

        /// <summary>
        /// Delete a TypeofFarming
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Status 200 OK</returns>
        [Authorize(Policy = Permissions.TypesofFarming.Delete)]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            return Ok(await _mediator.Send(new DeleteTypeofFarmingCommand { Id = id }));
        }

        /// <summary>
        /// Search TypesofFarming and Export to Excel
        /// </summary>
        /// <param name="searchString"></param>
        /// <returns></returns>
        [Authorize(Policy = Permissions.TypesofFarming.Export)]
        [HttpGet("export")]
        public async Task<IActionResult> Export(string searchString = "")
        {
            return Ok(await _mediator.Send(new ExportTypesofFarmingQuery(searchString)));
        }

        /// <summary>
        /// Import TypesofFarming from Excel
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [Authorize(Policy = Permissions.TypesofFarming.Import)]
        [HttpPost("import")]
        public async Task<IActionResult> Import(ImportTypesofFarmingCommand command)
        {
            return Ok(await _mediator.Send(command));
        }
    }
}