using System;
using System.Collections.Generic;
using Famtela.Domain.Contracts;
using Microsoft.AspNetCore.Identity;

namespace Famtela.Infrastructure.Models.Identity
{
    public class FamtelaRole : IdentityRole, IAuditableEntity<string>
    {
        public string Description { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public string LastModifiedBy { get; set; }
        public DateTime? LastModifiedOn { get; set; }
        public virtual ICollection<FamtelaRoleClaim> RoleClaims { get; set; }

        public FamtelaRole() : base()
        {
            RoleClaims = new HashSet<FamtelaRoleClaim>();
        }

        public FamtelaRole(string roleName, string roleDescription = null) : base(roleName)
        {
            RoleClaims = new HashSet<FamtelaRoleClaim>();
            Description = roleDescription;
        }
    }
}