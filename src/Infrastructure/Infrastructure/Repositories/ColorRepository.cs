using Famtela.Application.Interfaces.Repositories;
using Famtela.Domain.Entities.Dairy;

namespace Famtela.Infrastructure.Repositories
{
    public class ColorRepository : IColorRepository
    {
        private readonly IRepositoryAsync<Color, int> _repository;

        public ColorRepository(IRepositoryAsync<Color, int> repository)
        {
            _repository = repository;
        }
    }
}