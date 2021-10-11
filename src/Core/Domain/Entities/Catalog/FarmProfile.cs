using Famtela.Domain.Contracts;
using Famtela.Domain.Entities.Identity;
using System.ComponentModel.DataAnnotations;

namespace Famtela.Domain.Entities.Catalog
{
    public class FarmProfile : AuditableEntity<int>
    {
        [MaxLength(120)]
        public string FarmName { get; set; }
        public int TypeofFarmingId { get; set; }
        public int CountyId { get; set; }
        public virtual TypeofFarming TypeofFarming { get; set; }
        public virtual County County { get; set; }

        //public string UserId { get; set; }
        //public virtual FamtelaUser FamtelaUser { get; set; }
    }
}
