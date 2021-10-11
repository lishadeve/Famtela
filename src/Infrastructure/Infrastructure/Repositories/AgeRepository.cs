using Famtela.Application.Interfaces.Repositories;
using Famtela.Domain.Entities.Chicken;

namespace Famtela.Infrastructure.Repositories
{
    public class AgeRepository : IAgeRepository
    {
        private readonly IRepositoryAsync<Age, int> _repository;

        public AgeRepository(IRepositoryAsync<Age, int> repository)
        {
            _repository = repository;
        }
    }
}