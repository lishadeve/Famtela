using Famtela.Application.Interfaces.Repositories;
using Famtela.Domain.Entities.Chicken;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace Famtela.Infrastructure.Repositories
{
    public class ConsumptionRepository : IConsumptionRepository
    {
        private readonly IRepositoryAsync<Consumption, int> _repository;

        public ConsumptionRepository(IRepositoryAsync<Consumption, int> repository)
        {
            _repository = repository;
        }

        public async Task<bool> IsAgeUsed(int ageId)
        {
            return await _repository.Entities.AnyAsync(b => b.AgeId == ageId);
        }

        public async Task<bool> IsTypeofFeedUsed(int typeoffeedId)
        {
            return await _repository.Entities.AnyAsync(b => b.TypeofFeedId == typeoffeedId);
        }
    }
}