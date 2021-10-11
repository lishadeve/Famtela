using Famtela.Application.Specifications.Base;
using Famtela.Domain.Entities.Chicken;

namespace Famtela.Application.Specifications.Chicken
{
    public class EggFilterSpecification : FamtelaSpecification<Egg>
    {
        public EggFilterSpecification(string searchString)
        {
            if (!string.IsNullOrEmpty(searchString))
            {
                Criteria = p => p.Remarks.Contains(searchString);
            }
            else
            {
                Criteria = p => true;
            }
        }
    }
}
