using Famtela.Application.Interfaces.Repositories;
using Famtela.Domain.Entities.Chicken;

namespace Famtela.Infrastructure.Repositories
{
    public class DiseaseRepository : IDiseaseRepository
    {
        private readonly IRepositoryAsync<Disease, int> _repository;

        public DiseaseRepository(IRepositoryAsync<Disease, int> repository)
        {
            _repository = repository;
        }
    }
}