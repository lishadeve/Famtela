using Famtela.Domain.Contracts;
using System.ComponentModel.DataAnnotations;

namespace Famtela.Domain.Entities.Dairy
{
    public class Status : AuditableEntity<int>
    {
        [MaxLength(120)]
        public string Name { get; set; }
    }
}
