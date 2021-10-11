using Famtela.Domain.Contracts;
using Famtela.Domain.Entities.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Famtela.Domain.Entities.Chicken
{
    public class Egg : AuditableEntity<int>
    {
        public int Sold { get; set; }
        [Column(TypeName = "decimal(5,2)")]
        public decimal UnitPrice { get; set; }
        public int Retained { get; set; }
        public int Rejected { get; set; }
        public int Home { get; set; }
        [Column(TypeName = "decimal(10,2)")]
        public decimal Transport { get; set; }
        [MaxLength(120)]
        public string Remarks { get; set; }

        //public string UserId { get; set; }
        //public virtual FamtelaUser FamtelaUser { get; set; }
    }
}
