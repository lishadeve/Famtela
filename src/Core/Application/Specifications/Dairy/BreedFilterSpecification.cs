﻿using Famtela.Application.Specifications.Base;
using Famtela.Domain.Entities.Dairy;

namespace Famtela.Application.Specifications.Dairy
{
    public class BreedFilterSpecification : FamtelaSpecification<Breed>
    {
        public BreedFilterSpecification(string searchString)
        {
            if (!string.IsNullOrEmpty(searchString))
            {
                Criteria = p => p.Name.Contains(searchString);
            }
            else
            {
                Criteria = p => true;
            }
        }
    }
}