using Famtela.Application.Interfaces.Repositories;
using Famtela.Domain.Entities.Chicken;

namespace Famtela.Infrastructure.Repositories
{
    public class ChicksRepository : IChicksRepository
    {
        private readonly IRepositoryAsync<Chick, int> _repository;

        public ChicksRepository(IRepositoryAsync<Chick, int> repository)
        {
            _repository = repository;
        }
    }
}