using Famtela.Domain.Contracts;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Famtela.Domain.Entities.Chicken
{
    public class Consumption : AuditableEntity<int>
    {
        public int AgeId { get; set; }
        public int TypeofFeedId { get; set; }
        [Column(TypeName = "decimal(6,2)")]
        public decimal Grams { get; set; }
        [MaxLength(120)]
        public string Remarks { get; set; }
        public virtual Age Age { get; set; }
        public virtual TypeofFeed TypeofFeed { get; set; }
    }
}
