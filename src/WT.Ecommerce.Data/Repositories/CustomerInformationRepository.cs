using System;
using WT.Ecommerce.Data.Repositories.Interfaces;
using WT.Ecommerce.Domain.Models;

namespace WT.Ecommerce.Data.Repositories
{
    public class CustomerInformationRepository: BaseRepository<CustomerInformation, Guid>, ICustomerInformationRepository
    {
        public CustomerInformationRepository(ApplicationDbContext dbContext) : base(dbContext)
        {}
    }
}
