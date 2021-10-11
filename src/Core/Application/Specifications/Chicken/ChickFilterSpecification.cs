using Famtela.Application.Specifications.Base;
using Famtela.Domain.Entities.Chicken;

namespace Famtela.Application.Specifications.Chicken
{
    public class ChicksFilterSpecification : FamtelaSpecification<Chick>
    {
        public ChicksFilterSpecification(string searchString)
        {
            if (!string.IsNullOrEmpty(searchString))
            {
                Criteria = p => p.Disease.Contains(searchString) || p.Vaccination.Contains(searchString) || p.Medication.Contains(searchString) || p.TypeofFeed.Contains(searchString) || p.Remarks.Contains(searchString);
            }
            else
            {
                Criteria = p => true;
            }
        }
    }
}
