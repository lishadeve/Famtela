using Famtela.Domain.Contracts;
using System.ComponentModel.DataAnnotations;

namespace Famtela.Domain.Entities.Chicken
{
    public class Vaccination : AuditableEntity<int>
    {
        public int AgeId { get; set; }
        public int DiseaseId { get; set; }
        [MaxLength(120)]
        public string Administration { get; set; }
        [MaxLength(120)]
        public string Remarks { get; set; }
        public virtual Age Age { get; set; }
        public virtual Disease Disease { get; set; }
    }
}
