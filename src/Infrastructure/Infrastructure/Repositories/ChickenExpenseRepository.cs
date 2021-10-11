using Famtela.Application.Interfaces.Repositories;
using Famtela.Domain.Entities.Chicken;

namespace Famtela.Infrastructure.Repositories
{
    public class ChickenExpenseRepository : IChickenExpenseRepository
    {
        private readonly IRepositoryAsync<ChickenExpense, int> _repository;

        public ChickenExpenseRepository(IRepositoryAsync<ChickenExpense, int> repository)
        {
            _repository = repository;
        }
    }
}