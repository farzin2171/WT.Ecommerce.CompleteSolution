using System.Threading.Tasks;
using WT.Ecommerce.Data.Repositories.Interfaces;
using WT.Ecommerce.Domain.Identity;
using WT.Ecommerce.Domain.Models;

namespace WT.Ecommerce.Services.Customer
{
    public class CustomerInformationService : ICustomerInformationService
    {
        private readonly ICustomerInformationRepository _customerInformationRepository;
        private readonly IIdentityContext _identityContext;

        public CustomerInformationService(ICustomerInformationRepository customerInformationRepository,
                                          IIdentityContext identityContext)
        {

            _customerInformationRepository = customerInformationRepository;
            _identityContext = identityContext;

        }

        public Task<CustomerInformation> CreateCustomerAsync(CustomerInformation customerInformation)
        {
            customerInformation.UserCode = _identityContext.UserCode;
            return _customerInformationRepository.AddAsync(customerInformation);
        }
    }
}
