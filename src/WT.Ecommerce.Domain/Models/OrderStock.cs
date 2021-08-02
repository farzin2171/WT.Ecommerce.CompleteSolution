using System;

namespace WT.Ecommerce.Domain.Models
{
    public class OrderStock:BaseEntity<Guid>
    {
        public bool IsDeleted { get; set; }

        public Guid OrderId { get; set; }
        public Order Order { get; set; }

        public int Qty { get; set; }
        public Guid StockId { get; set; }
        public Stock Stock { get; set; }
    }
}
