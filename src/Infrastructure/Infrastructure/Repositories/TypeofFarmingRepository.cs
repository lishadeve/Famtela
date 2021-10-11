using Famtela.Application.Interfaces.Repositories;
using Famtela.Domain.Entities.Catalog;

namespace Famtela.Infrastructure.Repositories
{
    public class TypeofFarmingRepository : ITypeofFarmingRepository
    {
        private readonly IRepositoryAsync<TypeofFarming, int> _repository;

        public TypeofFarmingRepository(IRepositoryAsync<TypeofFarming, int> repository)
        {
            _repository = repository;
        }
    }
}