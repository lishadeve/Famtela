using System.Threading.Tasks;

namespace Famtela.Application.Interfaces.Repositories
{
    public interface IFarmProfileRepository
    {
        Task<bool> IsCountyUsed(int countyId);
        Task<bool> IsTypeofFarmingUsed(int typeoffarmingId);
    }
}