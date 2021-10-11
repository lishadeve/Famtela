using Famtela.Application.Features.Eggs.Queries.GetAll;
using Famtela.Shared.Wrapper;
using System.Collections.Generic;
using System.Threading.Tasks;
using Famtela.Application.Features.Eggs.Commands.AddEdit;
using Famtela.Application.Features.Eggs.Commands.Import;

namespace Famtela.Client.Infrastructure.Managers.Chicken.Egg
{
    public interface IEggManager : IManager
    {
        Task<IResult<List<GetAllEggsResponse>>> GetAllAsync();

        Task<IResult<int>> SaveAsync(AddEditEggCommand request);

        Task<IResult<int>> DeleteAsync(int id);

        Task<IResult<string>> ExportToExcelAsync(string searchString = "");

        Task<IResult<int>> ImportAsync(ImportEggsCommand request);
    }
}