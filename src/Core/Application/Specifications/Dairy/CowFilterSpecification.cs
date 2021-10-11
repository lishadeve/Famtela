using Famtela.Application.Specifications.Base;
using Famtela.Domain.Entities.Dairy;

namespace Famtela.Application.Specifications.Dairy
{
    public class CowFilterSpecification : FamtelaSpecification<Cow>
    {
        public CowFilterSpecification(string searchString)
        {
            Includes.Add(a => a.Breed);
            Includes.Add(a => a.Color);
            Includes.Add(a => a.Status);
            Includes.Add(a => a.Tag);
            if (!string.IsNullOrEmpty(searchString))
            {
                Criteria = p => p.EarTagNumber != null && (p.EarTagNumber.Contains(searchString) || p.Dam.Contains(searchString) || p.Sire.Contains(searchString) || p.Breed.Name.Contains(searchString) || p.Color.Name.Contains(searchString) || p.Status.Name.Contains(searchString) || p.Tag.Name.Contains(searchString));
            }
            else
            {
                Criteria = p => p.EarTagNumber != null;
            }
        }
    }
}
