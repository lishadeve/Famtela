using Famtela.Application.Interfaces.Repositories;
using Famtela.Domain.Entities.Catalog;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace Famtela.Infrastructure.Repositories
{
    public class FarmProfileRepository : IFarmProfileRepository
    {
        private readonly IRepositoryAsync<FarmProfile, int> _repository;

        public FarmProfileRepository(IRepositoryAsync<FarmProfile, int> repository)
        {
            _repository = repository;
        }

        public async Task<bool> IsCountyUsed(int countyId)
        {
            return await _repository.Entities.AnyAsync(b => b.CountyId == countyId);
        }

        public async Task<bool> IsTypeofFarmingUsed(int typeoffarmingId)
        {
            return await _repository.Entities.AnyAsync(b => b.TypeofFarmingId == typeoffarmingId);
        }
    }
}