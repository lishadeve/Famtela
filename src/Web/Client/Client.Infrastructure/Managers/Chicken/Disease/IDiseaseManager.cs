using Famtela.Application.Features.Diseases.Queries.GetAll;
using Famtela.Shared.Wrapper;
using System.Collections.Generic;
using System.Threading.Tasks;
using Famtela.Application.Features.Diseases.Commands.AddEdit;
using Famtela.Application.Features.Diseases.Commands.Import;

namespace Famtela.Client.Infrastructure.Managers.Chicken.Disease
{
    public interface IDiseaseManager : IManager
    {
        Task<IResult<List<GetAllDiseasesResponse>>> GetAllAsync();

        Task<IResult<int>> SaveAsync(AddEditDiseaseCommand request);

        Task<IResult<int>> DeleteAsync(int id);

        Task<IResult<string>> ExportToExcelAsync(string searchString = "");

        Task<IResult<int>> ImportAsync(ImportDiseasesCommand request);
    }
}