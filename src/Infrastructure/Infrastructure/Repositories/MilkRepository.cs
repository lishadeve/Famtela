using Famtela.Application.Interfaces.Repositories;
using Famtela.Domain.Entities.Dairy;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace Famtela.Infrastructure.Repositories
{
    public class MilkRepository : IMilkRepository
    {
        private readonly IRepositoryAsync<Milk, int> _repository;

        public MilkRepository(IRepositoryAsync<Milk, int> repository)
        {
            _repository = repository;
        }

        public async Task<bool> IsCowUsed(int cowId)
        {
            return await _repository.Entities.AnyAsync(b => b.CowId == cowId);
        }
    }
}