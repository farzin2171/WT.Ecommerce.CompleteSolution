using System;

namespace WT.Ecommerce.Domain.Models
{
    public class ProductImage:BaseEntity<Guid>
    {
        public bool IsDeleted { get; set; }

        public string Name { get; set; }
        public string Description { get; set; }
        public Guid ProductId { get; set; }
        public Product Product { get; set; }
    }
}
