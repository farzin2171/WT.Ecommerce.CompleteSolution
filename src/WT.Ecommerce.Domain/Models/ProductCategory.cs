using System;
using System.Collections.Generic;

namespace WT.Ecommerce.Domain.Models
{
    public class ProductCategory:BaseEntity<Guid>
    {
        public bool IsDeleted { get; set; }

        public string Description { get; set; }
        public string Name { get; set; }
        public ICollection<Product> Products { get; set; }
    }
}
