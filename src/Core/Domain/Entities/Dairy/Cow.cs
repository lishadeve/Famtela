using Famtela.Domain.Contracts;
using Famtela.Domain.Entities.Identity;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Famtela.Domain.Entities.Dairy
{
    public class Cow : AuditableEntity<int>
    {
        [MaxLength(120)]
        public string EarTagNumber { get; set; }
        [DataType(DataType.Date)]
        public DateTime? DateofBirth { get; set; }
        [Column(TypeName = "decimal(5,2)")]
        public decimal BirthWeight { get; set; }
        [MaxLength(120)]
        public string Sire { get; set; }
        [MaxLength(120)]
        public string Dam { get; set; }
        public int BreedId { get; set; }
        public int ColorId { get; set; }
        public int StatusId { get; set; }
        public int TagId { get; set; }
        public virtual Breed Breed { get; set; }
        public virtual Color Color { get; set; }
        public virtual Status Status { get; set; }
        public virtual Tag Tag { get; set; }

        //public string UserId { get; set; }
        //public virtual FamtelaUser FamtelaUser { get; set; }
    }
}
