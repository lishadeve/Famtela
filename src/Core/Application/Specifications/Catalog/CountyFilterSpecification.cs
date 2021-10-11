using Famtela.Application.Specifications.Base;
using Famtela.Domain.Entities.Catalog;

namespace Famtela.Application.Specifications.Catalog
{
    public class CountyFilterSpecification : FamtelaSpecification<County>
    {
        public CountyFilterSpecification(string searchString)
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
