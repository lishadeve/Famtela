using Famtela.Application.Features.Vaccinations.Commands.AddEdit;
using Famtela.Application.Features.Vaccinations.Commands.Delete;
using Famtela.Application.Features.Vaccinations.Queries.Export;
using Famtela.Application.Features.Vaccinations.Queries.GetAllPaged;
using Famtela.Shared.Constants.Permission;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Famtela.Server.Controllers.v1.Chicken
{
    public class VaccinationsController : BaseApiController<VaccinationsController>
    {
        /// <summary>
        /// Get All Vaccinations
        /// </summary>
        /// <param name="pageNumber"></param>
        /// <param name="pageSize"></param>
        /// <param name="searchString"></param>
        /// <param name="orderBy"></param>
        /// <returns>Status 200 OK</returns>
        [Authorize(Policy = Permissions.Vaccinations.View)]
        [HttpGet]
        public async Task<IActionResult> GetAll(int pageNumber, int pageSize, string searchString, string orderBy = null)
        {
            var vaccinations = await _mediator.Send(new GetAllVaccinationsQuery(pageNumber, pageSize, searchString, orderBy));
            return Ok(vaccinations);
        }

        /// <summary>
        /// Add/Edit a Vaccination
        /// </summary>
        /// <param name="command"></param>
        /// <returns>Status 200 OK</returns>
        [Authorize(Policy = Permissions.Vaccinations.Create)]
        [HttpPost]
        public async Task<IActionResult> Post(AddEditVaccinationCommand command)
        {
            return Ok(await _mediator.Send(command));
        }

        /// <summary>
        /// Delete a Vaccination
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Status 200 OK response</returns>
        [Authorize(Policy = Permissions.Vaccinations.Delete)]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            return Ok(await _mediator.Send(new DeleteVaccinationCommand { Id = id }));
        }

        /// <summary>
        /// Search Vaccinations and Export to Excel
        /// </summary>
        /// <param name="searchString"></param>
        /// <returns>Status 200 OK</returns>
        [Authorize(Policy = Permissions.Vaccinations.Export)]
        [HttpGet("export")]
        public async Task<IActionResult> Export(string searchString = "")
        {
            return Ok(await _mediator.Send(new ExportVaccinationsQuery(searchString)));
        }
    }
}