using Famtela.Application.Specifications.Base;
using Famtela.Domain.Entities.Dairy;

namespace Famtela.Application.Specifications.Dairy
{
    public class WeightEstimateFilterSpecification : FamtelaSpecification<WeightEstimate>
    {
        public WeightEstimateFilterSpecification(string searchString)
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
