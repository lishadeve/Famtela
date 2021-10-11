using Famtela.Domain.Contracts;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Famtela.Domain.Entities.Dairy
{
    public class WeightEstimate : AuditableEntity<int>
    {
        [Column(TypeName = "decimal(5,2)")]
        public decimal CM { get; set; }
        [Column(TypeName = "decimal(6,2)")]
        public decimal KG { get; set; }
        [MaxLength(120)]
        public string Remarks { get; set; }
    }
}
