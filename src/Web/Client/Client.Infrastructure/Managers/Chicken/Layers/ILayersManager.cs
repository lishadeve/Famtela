using Famtela.Application.Features.Layers.Queries.GetAll;
using Famtela.Shared.Wrapper;
using System.Collections.Generic;
using System.Threading.Tasks;
using Famtela.Application.Features.Layers.Commands.AddEdit;
using Famtela.Application.Features.Layers.Commands.Import;

namespace Famtela.Client.Infrastructure.Managers.Chicken.Layers
{
    public interface ILayersManager : IManager
    {
        Task<IResult<List<GetAllLayersResponse>>> GetAllAsync();

        Task<IResult<int>> SaveAsync(AddEditLayerCommand request);

        Task<IResult<int>> DeleteAsync(int id);

        Task<IResult<string>> ExportToExcelAsync(string searchString = "");

        Task<IResult<int>> ImportAsync(ImportLayersCommand request);
    }
}