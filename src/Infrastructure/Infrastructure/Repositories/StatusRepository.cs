using Famtela.Application.Interfaces.Repositories;
using Famtela.Domain.Entities.Dairy;

namespace Famtela.Infrastructure.Repositories
{
    public class StatusRepository : IStatusRepository
    {
        private readonly IRepositoryAsync<Status, int> _repository;

        public StatusRepository(IRepositoryAsync<Status, int> repository)
        {
            _repository = repository;
        }
    }
}