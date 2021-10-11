using Famtela.Domain.Contracts;
using Famtela.Domain.Entities.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Famtela.Domain.Entities.Dairy
{
    public class DairyExpense : AuditableEntity<int>
    {
        [MaxLength(120)]
        public string Description { get; set; }
        public int Quantity { get; set; }
        [Required]
        [Column(TypeName = "decimal(10,2)")]
        public decimal UnitCost { get; set; }
        [Column(TypeName = "decimal(10,2)")]
        public decimal Transport { get; set; }

        //public string UserId { get; set; }
        //public virtual FamtelaUser FamtelaUser { get; set; }
    }
}
