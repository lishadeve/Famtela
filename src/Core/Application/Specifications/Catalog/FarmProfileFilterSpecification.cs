using Famtela.Application.Specifications.Base;
using Famtela.Domain.Entities.Catalog;

namespace Famtela.Application.Specifications.Catalog
{
    public class FarmProfileFilterSpecification : FamtelaSpecification<FarmProfile>
    {
        public FarmProfileFilterSpecification(string searchString)
        {
            Includes.Add(a => a.County);
            Includes.Add(a => a.TypeofFarming);
            if (!string.IsNullOrEmpty(searchString))
            {
                Criteria = p => p.FarmName != null && (p.FarmName.Contains(searchString) || p.County.Name.Contains(searchString) || p.TypeofFarming.Name.Contains(searchString));
            }
            else
            {
                Criteria = p => p.FarmName != null;
            }
        }
    }
}