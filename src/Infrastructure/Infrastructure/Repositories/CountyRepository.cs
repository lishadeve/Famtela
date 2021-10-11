using Famtela.Application.Interfaces.Repositories;
using Famtela.Domain.Entities.Catalog;

namespace Famtela.Infrastructure.Repositories
{
    public class CountyRepository : ICountyRepository
    {
        private readonly IRepositoryAsync<County, int> _repository;

        public CountyRepository(IRepositoryAsync<County, int> repository)
        {
            _repository = repository;
        }
    }
}