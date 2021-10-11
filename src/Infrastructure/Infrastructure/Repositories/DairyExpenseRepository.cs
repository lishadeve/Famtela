using Famtela.Application.Interfaces.Repositories;
using Famtela.Domain.Entities.Dairy;

namespace Famtela.Infrastructure.Repositories
{
    public class DairyExpenseRepository : IDairyExpenseRepository
    {
        private readonly IRepositoryAsync<DairyExpense, int> _repository;

        public DairyExpenseRepository(IRepositoryAsync<DairyExpense, int> repository)
        {
            _repository = repository;
        }
    }
}