using System;

namespace WT.Ecommerce.Domain.Models
{
    public class Stock:BaseEntity<Guid>
    {
        public bool IsDeleted { get; set; }

        public string Description { get; set; }
        public int Qty { get; set; }
        public Guid ProductId { get; set; }
        public Product Product { get; set; }
    }
}
