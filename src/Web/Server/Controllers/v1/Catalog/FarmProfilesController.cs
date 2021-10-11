using Famtela.Application.Features.FarmProfiles.Commands.AddEdit;
using Famtela.Application.Features.FarmProfiles.Commands.Delete;
using Famtela.Application.Features.FarmProfiles.Queries.Export;
using Famtela.Application.Features.FarmProfiles.Queries.GetAllPaged;
using Famtela.Shared.Constants.Permission;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Famtela.Server.Controllers.v1.Catalog
{
    public class FarmProfilesController : BaseApiController<FarmProfilesController>
    {
        /// <summary>
        /// Get All Farm Profiles
        /// </summary>
        /// <param name="pageNumber"></param>
        /// <param name="pageSize"></param>
        /// <param name="searchString"></param>
        /// <param name="orderBy"></param>
        /// <returns>Status 200 OK</returns>
        [Authorize(Policy = Permissions.FarmProfiles.View)]
        [HttpGet]
        public async Task<IActionResult> GetAll(int pageNumber, int pageSize, string searchString, string orderBy = null)
        {
            var farmprofiles = await _mediator.Send(new GetAllFarmProfilesQuery(pageNumber, pageSize, searchString, orderBy));
            return Ok(farmprofiles);
        }

        /// <summary>
        /// Add/Edit a Farm Profile
        /// </summary>
        /// <param name="command"></param>
        /// <returns>Status 200 OK</returns>
        [Authorize(Policy = Permissions.FarmProfiles.Create)]
        [HttpPost]
        public async Task<IActionResult> Post(AddEditFarmProfileCommand command)
        {
            return Ok(await _mediator.Send(command));
        }

        /// <summary>
        /// Delete a Farm Profile
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Status 200 OK response</returns>
        [Authorize(Policy = Permissions.FarmProfiles.Delete)]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            return Ok(await _mediator.Send(new DeleteFarmProfileCommand { Id = id }));
        }

        /// <summary>
        /// Search Farm Profiles and Export to Excel
        /// </summary>
        /// <param name="searchString"></param>
        /// <returns>Status 200 OK</returns>
        [Authorize(Policy = Permissions.FarmProfiles.Export)]
        [HttpGet("export")]
        public async Task<IActionResult> Export(string searchString = "")
        {
            return Ok(await _mediator.Send(new ExportFarmProfilesQuery(searchString)));
        }
    }
}