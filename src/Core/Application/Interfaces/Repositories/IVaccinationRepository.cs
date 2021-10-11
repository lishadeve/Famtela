using System.Threading.Tasks;

namespace Famtela.Application.Interfaces.Repositories
{
    public interface IVaccinationRepository
    {
        Task<bool> IsAgeUsed(int ageId);
        Task<bool> IsDiseaseUsed(int typeoffeedId);
    }
}
