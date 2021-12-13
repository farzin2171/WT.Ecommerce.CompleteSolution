using System;

namespace WT.Ecommerce.Domain.Models
{
    public class CustomerInformation:BaseEntity<Guid>
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public bool IsDeleted { get; set; }
        public string UserCode { get; set; }
    }
}
