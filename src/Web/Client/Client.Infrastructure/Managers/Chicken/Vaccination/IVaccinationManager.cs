using Famtela.Application.Features.Vaccinations.Commands.AddEdit;
using Famtela.Application.Features.Vaccinations.Queries.GetAllPaged;
using Famtela.Application.Requests.Chicken;
using Famtela.Shared.Wrapper;
using System.Threading.Tasks;

namespace Famtela.Client.Infrastructure.Managers.Chicken.Vaccination
{
    public interface IVaccinationManager : IManager
    {
        Task<PaginatedResult<GetAllPagedVaccinationsResponse>> GetVaccinationsAsync(GetAllPagedVaccinationsRequest request);

        Task<IResult<int>> SaveAsync(AddEditVaccinationCommand request);

        Task<IResult<int>> DeleteAsync(int id);

        Task<IResult<string>> ExportToExcelAsync(string searchString = "");
    }
}