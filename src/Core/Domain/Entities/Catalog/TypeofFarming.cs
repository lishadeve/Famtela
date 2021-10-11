﻿using Famtela.Domain.Contracts;
using System.ComponentModel.DataAnnotations;

namespace Famtela.Domain.Entities.Catalog
{
    public class TypeofFarming : AuditableEntity<int>
    {
        [MaxLength(120)]
        public string Name { get; set; }
    }
}
