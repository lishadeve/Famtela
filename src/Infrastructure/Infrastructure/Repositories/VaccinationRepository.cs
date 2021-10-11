using Famtela.Application.Interfaces.Repositories;
using Famtela.Domain.Entities.Chicken;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace Famtela.Infrastructure.Repositories
{
    public class VaccinationRepository : IVaccinationRepository
    {
        private readonly IRepositoryAsync<Vaccination, int> _repository;

        public VaccinationRepository(IRepositoryAsync<Vaccination, int> repository)
        {
            _repository = repository;
        }

        public async Task<bool> IsAgeUsed(int ageId)
        {
            return await _repository.Entities.AnyAsync(b => b.AgeId == ageId);
        }

        public async Task<bool> IsDiseaseUsed(int diseaseId)
        {
            return await _repository.Entities.AnyAsync(b => b.DiseaseId == diseaseId);
        }
    }
}