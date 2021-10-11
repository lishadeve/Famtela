using Famtela.Application.Specifications.Base;
using Famtela.Domain.Entities.Dairy;

namespace Famtela.Application.Specifications.Dairy
{
    public class MilkFilterSpecification : FamtelaSpecification<Milk>
    {
        public MilkFilterSpecification(string searchString)
        {
            Includes.Add(a => a.Cow);
            if (!string.IsNullOrEmpty(searchString))
            {
                Criteria = p => p.Remarks.Contains(searchString) || p.Cow.EarTagNumber.Contains(searchString);
            }
            else
            {
                Criteria = p => true;
            }
        }
    }
}
