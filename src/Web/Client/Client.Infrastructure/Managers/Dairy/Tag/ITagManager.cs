using Famtela.Application.Features.Tags.Queries.GetAll;
using Famtela.Shared.Wrapper;
using System.Collections.Generic;
using System.Threading.Tasks;
using Famtela.Application.Features.Tags.Commands.AddEdit;
using Famtela.Application.Features.Tags.Commands.Import;

namespace Famtela.Client.Infrastructure.Managers.Dairy.Tag
{
    public interface ITagManager : IManager
    {
        Task<IResult<List<GetAllTagsResponse>>> GetAllAsync();

        Task<IResult<int>> SaveAsync(AddEditTagCommand request);

        Task<IResult<int>> DeleteAsync(int id);

        Task<IResult<string>> ExportToExcelAsync(string searchString = "");

        Task<IResult<int>> ImportAsync(ImportTagsCommand request);
    }
}