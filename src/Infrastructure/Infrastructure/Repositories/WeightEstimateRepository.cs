using Famtela.Application.Interfaces.Repositories;
using Famtela.Domain.Entities.Dairy;

namespace Famtela.Infrastructure.Repositories
{
    public class WeightEstimateRepository : IWeightEstimateRepository
    {
        private readonly IRepositoryAsync<WeightEstimate, int> _repository;

        public WeightEstimateRepository(IRepositoryAsync<WeightEstimate, int> repository)
        {
            _repository = repository;
        }
    }
}