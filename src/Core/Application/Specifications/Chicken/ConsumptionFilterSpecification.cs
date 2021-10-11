using Famtela.Application.Specifications.Base;
using Famtela.Domain.Entities.Chicken;

namespace Famtela.Application.Specifications.Chicken
{
    public class ConsumptionFilterSpecification : FamtelaSpecification<Consumption>
    {
        public ConsumptionFilterSpecification(string searchString)
        {
            Includes.Add(a => a.Age);
            Includes.Add(a => a.TypeofFeed);
            if (!string.IsNullOrEmpty(searchString))
            {
                Criteria = p => p.Remarks.Contains(searchString) || p.Age.Name.Contains(searchString) || p.TypeofFeed.Name.Contains(searchString);
            }
            else
            {
                Criteria = p => true;
            }
        }
    }
}
