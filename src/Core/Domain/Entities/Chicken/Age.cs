using Famtela.Domain.Contracts;
using System.ComponentModel.DataAnnotations;

namespace Famtela.Domain.Entities.Chicken
{
    public class Age : AuditableEntity<int>
    {
        [MaxLength(120)]
        public string Name { get; set; }
    }
}
