using Famtela.Application.Features.TypesofFarming.Queries.GetAll;
using Famtela.Shared.Wrapper;
using System.Collections.Generic;
using System.Threading.Tasks;
using Famtela.Application.Features.TypesofFarming.Commands.AddEdit;
using Famtela.Application.Features.TypesofFarming.Commands.Import;

namespace Famtela.Client.Infrastructure.Managers.Catalog.TypeofFarming
{
    public interface ITypeofFarmingManager : IManager
    {
        Task<IResult<List<GetAllTypesofFarmingResponse>>> GetAllAsync();

        Task<IResult<int>> SaveAsync(AddEditTypeofFarmingCommand request);

        Task<IResult<int>> DeleteAsync(int id);

        Task<IResult<string>> ExportToExcelAsync(string searchString = "");

        Task<IResult<int>> ImportAsync(ImportTypesofFarmingCommand request);
    }
}