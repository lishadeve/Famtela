using Famtela.Application.Features.TypesofFeed.Queries.GetAll;
using Famtela.Shared.Wrapper;
using System.Collections.Generic;
using System.Threading.Tasks;
using Famtela.Application.Features.TypesofFeed.Commands.AddEdit;
using Famtela.Application.Features.TypesofFeed.Commands.Import;

namespace Famtela.Client.Infrastructure.Managers.Chicken.TypeofFeed
{
    public interface ITypeofFeedManager : IManager
    {
        Task<IResult<List<GetAllTypesofFeedResponse>>> GetAllAsync();

        Task<IResult<int>> SaveAsync(AddEditTypeofFeedCommand request);

        Task<IResult<int>> DeleteAsync(int id);

        Task<IResult<string>> ExportToExcelAsync(string searchString = "");

        Task<IResult<int>> ImportAsync(ImportTypesofFeedCommand request);
    }
}