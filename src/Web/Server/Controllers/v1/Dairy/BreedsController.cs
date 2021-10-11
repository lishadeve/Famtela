using Famtela.Application.Features.Breeds.Queries.GetAll;
using Famtela.Application.Features.Breeds.Queries.GetById;
using Famtela.Shared.Constants.Permission;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Famtela.Application.Features.Breeds.Commands.AddEdit;
using Famtela.Application.Features.Breeds.Commands.Delete;
using Famtela.Application.Features.Breeds.Queries.Export;
using Famtela.Application.Features.Breeds.Commands.Import;

namespace Famtela.Server.Controllers.v1.Dairy
{
    public class BreedsController : BaseApiController<BreedsController>
    {
        /// <summary>
        /// Get All Breeds
        /// </summary>
        /// <returns>Status 200 OK</returns>
        [Authorize(Policy = Permissions.Breeds.View)]
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var breeds = await _mediator.Send(new GetAllBreedsQuery());
            return Ok(breeds);
        }

        /// <summary>
        /// Get a Breed By Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Status 200 Ok</returns>
        [Authorize(Policy = Permissions.Breeds.View)]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var breed = await _mediator.Send(new GetBreedByIdQuery() { Id = id });
            return Ok(breed);
        }

        /// <summary>
        /// Create/Update a Breed
        /// </summary>
        /// <param name="command"></param>
        /// <returns>Status 200 OK</returns>
        [Authorize(Policy = Permissions.Breeds.Create)]
        [HttpPost]
        public async Task<IActionResult> Post(AddEditBreedCommand command)
        {
            return Ok(await _mediator.Send(command));
        }

        /// <summary>
        /// Delete a Breed
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Status 200 OK</returns>
        [Authorize(Policy = Permissions.Breeds.Delete)]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            return Ok(await _mediator.Send(new DeleteBreedCommand { Id = id }));
        }

        /// <summary>
        /// Search Breeds and Export to Excel
        /// </summary>
        /// <param name="searchString"></param>
        /// <returns></returns>
        [Authorize(Policy = Permissions.Breeds.Export)]
        [HttpGet("export")]
        public async Task<IActionResult> Export(string searchString = "")
        {
            return Ok(await _mediator.Send(new ExportBreedsQuery(searchString)));
        }

        /// <summary>
        /// Import Breeds from Excel
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [Authorize(Policy = Permissions.Breeds.Import)]
        [HttpPost("import")]
        public async Task<IActionResult> Import(ImportBreedsCommand command)
        {
            return Ok(await _mediator.Send(command));
        }
    }
}