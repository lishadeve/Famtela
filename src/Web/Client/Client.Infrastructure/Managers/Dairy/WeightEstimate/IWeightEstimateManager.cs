using Famtela.Application.Features.WeightEstimates.Queries.GetAll;
using Famtela.Shared.Wrapper;
using System.Collections.Generic;
using System.Threading.Tasks;
using Famtela.Application.Features.WeightEstimates.Commands.AddEdit;
using Famtela.Application.Features.WeightEstimates.Commands.Import;

namespace Famtela.Client.Infrastructure.Managers.Dairy.WeightEstimate
{
    public interface IWeightEstimateManager : IManager
    {
        Task<IResult<List<GetAllWeightEstimatesResponse>>> GetAllAsync();

        Task<IResult<int>> SaveAsync(AddEditWeightEstimateCommand request);

        Task<IResult<int>> DeleteAsync(int id);

        Task<IResult<string>> ExportToExcelAsync(string searchString = "");

        Task<IResult<int>> ImportAsync(ImportWeightEstimatesCommand request);
    }
}