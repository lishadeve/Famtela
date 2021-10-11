using Famtela.Domain.Contracts;
using Famtela.Domain.Entities.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Famtela.Domain.Entities.Dairy
{
    public class Milk : AuditableEntity<int>
    {
        public int CowId { get; set; }
        [Column(TypeName = "decimal(3,1)")]
        public decimal Morning { get; set; }
        [Column(TypeName = "decimal(3,1)")]
        public decimal Evening { get; set; }
        [MaxLength(120)]
        public string Remarks { get; set; }
        public virtual Cow Cow { get; set; }

        //public string UserId { get; set; }
        //public virtual FamtelaUser FamtelaUser { get; set; }
    }
}
