using System;
using WT.Ecommerce.Domain.Models;

namespace WT.Ecommerce.Data.Repositories.Interfaces
{
    public interface ICustomerInformationRepository:IRepository<CustomerInformation,Guid>
    {
    }
}
