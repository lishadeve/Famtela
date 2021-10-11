using Famtela.Application.Features.Ages.Queries.GetAll;
using Famtela.Shared.Wrapper;
using System.Collections.Generic;
using System.Threading.Tasks;
using Famtela.Application.Features.Ages.Commands.AddEdit;
using Famtela.Application.Features.Ages.Commands.Import;

namespace Famtela.Client.Infrastructure.Managers.Chicken.Age
{
    public interface IAgeManager : IManager
    {
        Task<IResult<List<GetAllAgesResponse>>> GetAllAsync();

        Task<IResult<int>> SaveAsync(AddEditAgeCommand request);

        Task<IResult<int>> DeleteAsync(int id);

        Task<IResult<string>> ExportToExcelAsync(string searchString = "");

        Task<IResult<int>> ImportAsync(ImportAgesCommand request);
    }
}