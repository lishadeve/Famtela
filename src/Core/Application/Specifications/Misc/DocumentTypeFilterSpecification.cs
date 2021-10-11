using Famtela.Application.Specifications.Base;
using Famtela.Domain.Entities.Misc;

namespace Famtela.Application.Specifications.Misc
{
    public class DocumentTypeFilterSpecification : FamtelaSpecification<DocumentType>
    {
        public DocumentTypeFilterSpecification(string searchString)
        {
            if (!string.IsNullOrEmpty(searchString))
            {
                Criteria = p => p.Name.Contains(searchString) || p.Description.Contains(searchString);
            }
            else
            {
                Criteria = p => true;
            }
        }
    }
}