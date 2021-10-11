using Famtela.Application.Interfaces.Repositories;
using Famtela.Domain.Entities.Dairy;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace Famtela.Infrastructure.Repositories
{
    public class CowRepository : ICowRepository
    {
        private readonly IRepositoryAsync<Cow, int> _repository;

        public CowRepository(IRepositoryAsync<Cow, int> repository)
        {
            _repository = repository;
        }

        public async Task<bool> IsColorUsed(int colorId)
        {
            return await _repository.Entities.AnyAsync(b => b.ColorId == colorId);
        }

        public async Task<bool> IsBreedUsed(int breedId)
        {
            return await _repository.Entities.AnyAsync(b => b.BreedId == breedId);
        }

        public async Task<bool> IsStatusUsed(int statusId)
        {
            return await _repository.Entities.AnyAsync(b => b.StatusId == statusId);
        }

        public async Task<bool> IsTagUsed(int tagId)
        {
            return await _repository.Entities.AnyAsync(b => b.TagId == tagId);
        }
    }
}