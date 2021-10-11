using Famtela.Application.Features.Breeds.Queries.GetAll;
using Famtela.Shared.Wrapper;
using System.Collections.Generic;
using System.Threading.Tasks;
using Famtela.Application.Features.Breeds.Commands.AddEdit;
using Famtela.Application.Features.Breeds.Commands.Import;

namespace Famtela.Client.Infrastructure.Managers.Dairy.Breed
{
    public interface IBreedManager : IManager
    {
        Task<IResult<List<GetAllBreedsResponse>>> GetAllAsync();

        Task<IResult<int>> SaveAsync(AddEditBreedCommand request);

        Task<IResult<int>> DeleteAsync(int id);

        Task<IResult<string>> ExportToExcelAsync(string searchString = "");

        Task<IResult<int>> ImportAsync(ImportBreedsCommand request);
    }
}