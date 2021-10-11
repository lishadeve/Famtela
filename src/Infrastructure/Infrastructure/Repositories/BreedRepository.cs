using Famtela.Application.Interfaces.Repositories;
using Famtela.Domain.Entities.Dairy;

namespace Famtela.Infrastructure.Repositories
{
    public class BreedRepository : IBreedRepository
    {
        private readonly IRepositoryAsync<Breed, int> _repository;

        public BreedRepository(IRepositoryAsync<Breed, int> repository)
        {
            _repository = repository;
        }
    }
}