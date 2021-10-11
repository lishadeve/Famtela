using Famtela.Application.Features.WeightEstimates.Queries.GetAll;
using Famtela.Application.Features.WeightEstimates.Queries.GetById;
using Famtela.Shared.Constants.Permission;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Famtela.Application.Features.WeightEstimates.Commands.AddEdit;
using Famtela.Application.Features.WeightEstimates.Commands.Delete;
using Famtela.Application.Features.WeightEstimates.Queries.Export;
using Famtela.Application.Features.WeightEstimates.Commands.Import;

namespace Famtela.Server.Controllers.v1.Dairy
{
    public class WeightEstimatesController : BaseApiController<WeightEstimatesController>
    {
        /// <summary>
        /// Get All Weight Estimates
        /// </summary>
        /// <returns>Status 200 OK</returns>
        [Authorize(Policy = Permissions.WeightEstimates.View)]
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var weightestimates = await _mediator.Send(new GetAllWeightEstimatesQuery());
            return Ok(weightestimates);
        }

        /// <summary>
        /// Get a Weight Estimate By Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Status 200 Ok</returns>
        [Authorize(Policy = Permissions.WeightEstimates.View)]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var weightestimate = await _mediator.Send(new GetWeightEstimateByIdQuery() { Id = id });
            return Ok(weightestimate);
        }

        /// <summary>
        /// Create/Update a Weight Estimate
        /// </summary>
        /// <param name="command"></param>
        /// <returns>Status 200 OK</returns>
        [Authorize(Policy = Permissions.WeightEstimates.Create)]
        [HttpPost]
        public async Task<IActionResult> Post(AddEditWeightEstimateCommand command)
        {
            return Ok(await _mediator.Send(command));
        }

        /// <summary>
        /// Delete a Weight Estimate
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Status 200 OK</returns>
        [Authorize(Policy = Permissions.WeightEstimates.Delete)]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            return Ok(await _mediator.Send(new DeleteWeightEstimateCommand { Id = id }));
        }

        /// <summary>
        /// Search Weight Estimates and Export to Excel
        /// </summary>
        /// <param name="searchString"></param>
        /// <returns></returns>
        [Authorize(Policy = Permissions.WeightEstimates.Export)]
        [HttpGet("export")]
        public async Task<IActionResult> Export(string searchString = "")
        {
            return Ok(await _mediator.Send(new ExportWeightEstimatesQuery(searchString)));
        }

        /// <summary>
        /// Import Weight Estimates from Excel
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [Authorize(Policy = Permissions.WeightEstimates.Import)]
        [HttpPost("import")]
        public async Task<IActionResult> Import(ImportWeightEstimatesCommand command)
        {
            return Ok(await _mediator.Send(command));
        }
    }
}