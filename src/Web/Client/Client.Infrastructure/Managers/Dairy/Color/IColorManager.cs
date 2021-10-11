using Famtela.Application.Features.Colors.Queries.GetAll;
using Famtela.Shared.Wrapper;
using System.Collections.Generic;
using System.Threading.Tasks;
using Famtela.Application.Features.Colors.Commands.AddEdit;
using Famtela.Application.Features.Colors.Commands.Import;

namespace Famtela.Client.Infrastructure.Managers.Dairy.Color
{
    public interface IColorManager : IManager
    {
        Task<IResult<List<GetAllColorsResponse>>> GetAllAsync();

        Task<IResult<int>> SaveAsync(AddEditColorCommand request);

        Task<IResult<int>> DeleteAsync(int id);

        Task<IResult<string>> ExportToExcelAsync(string searchString = "");

        Task<IResult<int>> ImportAsync(ImportColorsCommand request);
    }
}