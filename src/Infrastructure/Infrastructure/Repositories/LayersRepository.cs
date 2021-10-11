using Famtela.Application.Interfaces.Repositories;
using Famtela.Domain.Entities.Chicken;

namespace Famtela.Infrastructure.Repositories
{
    public class LayersRepository : ILayersRepository
    {
        private readonly IRepositoryAsync<Layer, int> _repository;

        public LayersRepository(IRepositoryAsync<Layer, int> repository)
        {
            _repository = repository;
        }
    }
}