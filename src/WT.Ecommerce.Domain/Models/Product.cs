using System;
using System.Collections.Generic;

namespace WT.Ecommerce.Domain.Models
{
    public class Product:BaseEntity<Guid>
    {
        public bool IsDeleted { get; set; }

        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Value { get; set; }
        public ProductCategory ProductCategory { get; set; }
        public int ProductCategoryId { get; set; }
        public ICollection<Stock> Stocks { get; set; }
        public ICollection<ProductImage> productImages { get; set; }
    }
}
