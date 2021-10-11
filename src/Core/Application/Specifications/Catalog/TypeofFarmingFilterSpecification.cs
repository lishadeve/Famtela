using Famtela.Application.Specifications.Base;
using Famtela.Domain.Entities.Catalog;

namespace Famtela.Application.Specifications.Catalog
{
    public class TypeofFarmingFilterSpecification : FamtelaSpecification<TypeofFarming>
    {
        public TypeofFarmingFilterSpecification(string searchString)
        {
            if (!string.IsNullOrEmpty(searchString))
            {
                Criteria = p => p.Name.Contains(searchString);
            }
            else
            {
                Criteria = p => true;
            }
        }
    }
}
