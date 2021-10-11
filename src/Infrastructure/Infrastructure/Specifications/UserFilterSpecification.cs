//using Famtela.Infrastructure.Models.Identity;
using Famtela.Application.Specifications.Base;
using Famtela.Domain.Entities.Identity;

namespace Famtela.Infrastructure.Specifications
{
    public class UserFilterSpecification : FamtelaSpecification<FamtelaUser>
    {
        public UserFilterSpecification(string searchString)
        {
            if (!string.IsNullOrEmpty(searchString))
            {
                Criteria = p => p.FirstName.Contains(searchString) || p.LastName.Contains(searchString) || p.Email.Contains(searchString) || p.PhoneNumber.Contains(searchString) || p.UserName.Contains(searchString);
            }
            else
            {
                Criteria = p => true;
            }
        }
    }
}