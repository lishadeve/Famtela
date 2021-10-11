using System;
using Famtela.Domain.Contracts;
using Microsoft.AspNetCore.Identity;

namespace Famtela.Infrastructure.Models.Identity
{
    public class FamtelaRoleClaim : IdentityRoleClaim<string>, IAuditableEntity<int>
    {
        public string Description { get; set; }
        public string Group { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public string LastModifiedBy { get; set; }
        public DateTime? LastModifiedOn { get; set; }
        public virtual FamtelaRole Role { get; set; }

        public FamtelaRoleClaim() : base()
        {
        }

        public FamtelaRoleClaim(string roleClaimDescription = null, string roleClaimGroup = null) : base()
        {
            Description = roleClaimDescription;
            Group = roleClaimGroup;
        }
    }
}