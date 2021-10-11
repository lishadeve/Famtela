using Famtela.Application.Interfaces.Repositories;
using Famtela.Domain.Entities.Chicken;

namespace Famtela.Infrastructure.Repositories
{
    public class TypeofFeedRepository : ITypeofFeedRepository
    {
        private readonly IRepositoryAsync<TypeofFeed, int> _repository;

        public TypeofFeedRepository(IRepositoryAsync<TypeofFeed, int> repository)
        {
            _repository = repository;
        }
    }
}