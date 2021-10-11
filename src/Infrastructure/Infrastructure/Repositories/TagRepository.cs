using Famtela.Application.Interfaces.Repositories;
using Famtela.Domain.Entities.Dairy;

namespace Famtela.Infrastructure.Repositories
{
    public class TagRepository : ITagRepository
    {
        private readonly IRepositoryAsync<Tag, int> _repository;

        public TagRepository(IRepositoryAsync<Tag, int> repository)
        {
            _repository = repository;
        }
    }
}