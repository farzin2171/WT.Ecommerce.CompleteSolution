using System;
using System.Collections.Generic;
using WT.Ecommerce.Domain.Enums;

namespace WT.Ecommerce.Domain.Models
{
    public class Order:BaseEntity<Guid>
    {
        public bool IsDeleted { get; set; }
        public string OrderRef { get; set; }
        public string PaymentRef { get; set; }
        public Guid CustomerInformationId { get; set; }
        public CustomerInformation CustomerInformation { get; set; }
        public OrderStatus Status { get; set; }
        public ICollection<OrderStock> OrderStocks { get; set; }
    }
}
