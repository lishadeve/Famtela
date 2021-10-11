using System.Threading.Tasks;

namespace Famtela.Application.Interfaces.Repositories
{
    public interface IConsumptionRepository
    {
        Task<bool> IsAgeUsed(int ageId);
        Task<bool> IsTypeofFeedUsed(int typeoffeedId);
    }
}
