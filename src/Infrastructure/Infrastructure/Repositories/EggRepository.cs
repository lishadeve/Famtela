using Famtela.Application.Interfaces.Repositories;
using Famtela.Domain.Entities.Chicken;

namespace Famtela.Infrastructure.Repositories
{
    public class EggRepository : IEggRepository
    {
        private readonly IRepositoryAsync<Egg, int> _repository;

        public EggRepository(IRepositoryAsync<Egg, int> repository)
        {
            _repository = repository;
        }
    }
}