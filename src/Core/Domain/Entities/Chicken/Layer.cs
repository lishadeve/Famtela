using Famtela.Domain.Contracts;
using Famtela.Domain.Entities.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Famtela.Domain.Entities.Chicken
{
    public class Layer : AuditableEntity<int>
    {
        public int NumberofBirds { get; set; }
        [MaxLength(120)]
        public string Disease { get; set; }
        public int Mortality { get; set; }
        [MaxLength(120)]
        public string Vaccination { get; set; }
        [MaxLength(120)]
        public string Medication { get; set; }
        [Column(TypeName = "decimal(6,2)")]
        public decimal Feed { get; set; }
        [MaxLength(120)]
        public string TypeofFeed { get; set; }
        public int Eggs { get; set; }
        [MaxLength(120)]
        public string Remarks { get; set; }

        //public string UserId { get; set; }
        //public virtual FamtelaUser FamtelaUser { get; set; }
    }
}
