using Famtela.Application.Specifications.Base;
using Famtela.Domain.Entities.Chicken;

namespace Famtela.Application.Specifications.Chicken
{
    public class ChickenExpenseFilterSpecification : FamtelaSpecification<ChickenExpense>
    {
        public ChickenExpenseFilterSpecification(string searchString)
        {
            if (!string.IsNullOrEmpty(searchString))
            {
                Criteria = p => p.Description.Contains(searchString);
            }
            else
            {
                Criteria = p => true;
            }
        }
    }
}
