using Famtela.Application.Interfaces.Repositories;
using Famtela.Domain.Entities.Chicken;

namespace Famtela.Infrastructure.Repositories
{
    public class GrowersRepository : IGrowersRepository
    {
        private readonly IRepositoryAsync<Grower, int> _repository;

        public GrowersRepository(IRepositoryAsync<Grower, int> repository)
        {
            _repository = repository;
        }
    }
}