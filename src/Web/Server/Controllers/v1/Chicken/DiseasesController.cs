using Famtela.Application.Features.Diseases.Queries.GetAll;
using Famtela.Application.Features.Diseases.Queries.GetById;
using Famtela.Shared.Constants.Permission;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Famtela.Application.Features.Diseases.Commands.AddEdit;
using Famtela.Application.Features.Diseases.Commands.Delete;
using Famtela.Application.Features.Diseases.Queries.Export;
using Famtela.Application.Features.Diseases.Commands.Import;

namespace Famtela.Server.Controllers.v1.Diseases
{
    public class DiseasesController : BaseApiController<DiseasesController>
    {
        /// <summary>
        /// Get All Diseases
        /// </summary>
        /// <returns>Status 200 OK</returns>
        [Authorize(Policy = Permissions.Diseases.View)]
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var diseases = await _mediator.Send(new GetAllDiseasesQuery());
            return Ok(diseases);
        }

        /// <summary>
        /// Get a Disease By Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Status 200 Ok</returns>
        [Authorize(Policy = Permissions.Diseases.View)]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var disease = await _mediator.Send(new GetDiseaseByIdQuery() { Id = id });
            return Ok(disease);
        }

        /// <summary>
        /// Create/Update a Disease
        /// </summary>
        /// <param name="command"></param>
        /// <returns>Status 200 OK</returns>
        [Authorize(Policy = Permissions.Diseases.Create)]
        [HttpPost]
        public async Task<IActionResult> Post(AddEditDiseaseCommand command)
        {
            return Ok(await _mediator.Send(command));
        }

        /// <summary>
        /// Delete a Disease
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Status 200 OK</returns>
        [Authorize(Policy = Permissions.Diseases.Delete)]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            return Ok(await _mediator.Send(new DeleteDiseaseCommand { Id = id }));
        }

        /// <summary>
        /// Search Diseases and Export to Excel
        /// </summary>
        /// <param name="searchString"></param>
        /// <returns></returns>
        [Authorize(Policy = Permissions.Diseases.Export)]
        [HttpGet("export")]
        public async Task<IActionResult> Export(string searchString = "")
        {
            return Ok(await _mediator.Send(new ExportDiseasesQuery(searchString)));
        }

        /// <summary>
        /// Import Diseases from Excel
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [Authorize(Policy = Permissions.Diseases.Import)]
        [HttpPost("import")]
        public async Task<IActionResult> Import(ImportDiseasesCommand command)
        {
            return Ok(await _mediator.Send(command));
        }
    }
}