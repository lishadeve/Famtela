using Famtela.Application.Specifications.Base;
using Famtela.Domain.Entities.Chicken;

namespace Famtela.Application.Specifications.Chicken
{
    public class VaccinationFilterSpecification : FamtelaSpecification<Vaccination>
    {
        public VaccinationFilterSpecification(string searchString)
        {
            Includes.Add(a => a.Age);
            Includes.Add(a => a.Disease);
            if (!string.IsNullOrEmpty(searchString))
            {
                Criteria = p => p.Remarks != null && (p.Administration.Contains(searchString) || p.Remarks.Contains(searchString) || p.Age.Name.Contains(searchString) || p.Disease.Name.Contains(searchString));
            }
            else
            {
                Criteria = p => p.Remarks != null;
            }
        }
    }
}
