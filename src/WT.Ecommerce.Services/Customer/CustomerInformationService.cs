using System.Threading.Tasks;
using WT.Ecommerce.Data.Repositories.Interfaces;
using WT.Ecommerce.Domain.Identity;
using WT.Ecommerce.Domain.Models;
using WT.Ecommerce.Services.Cache;

namespace WT.Ecommerce.Services.Customer
{
    public class CustomerInformationService : ICustomerInformationService
    {
        private readonly ICustomerInformationRepository _customerInformationRepository;
        private readonly IIdentityContext _identityContext;
        private readonly IRedisCache<CustomerInformation> _redisCache;

        public CustomerInformationService(ICustomerInformationRepository customerInformationRepository,
                                          IIdentityContext identityContext,
                                          IRedisCache<CustomerInformation> redisCache)
        {

            _customerInformationRepository = customerInformationRepository;
            _identityContext = identityContext;
            _redisCache = redisCache;

        }

        public async Task<CustomerInformation> CreateCustomerAsync(CustomerInformation customerInformation)
        {
            customerInformation.UserCode = _identityContext.UserCode;

            //var x = await _redisCache.GetOrAddAsync("TestCashe", () => _customerInformationRepository.GetByIdAsync(new System.Guid("F1766AFD-FC35-470A-91CE-08D955B66526")));
            
            return await _customerInformationRepository.AddAsync(customerInformation);
        }
    }
}
