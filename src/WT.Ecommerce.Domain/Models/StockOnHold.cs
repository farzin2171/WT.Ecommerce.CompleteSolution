using System;

namespace WT.Ecommerce.Domain.Models
{
    public class StockOnHold:BaseEntity<Guid>
    {
        public Guid StockId { get; set; }
        public Stock Stock { get; set; }

        public string SessionId { get; set; }
        public int Qty { get; set; }
        public DateTime ExpiryDate { get; set; }
    }
}
