using System.Threading.Tasks;
using WT.Ecommerce.Domain.Models;

namespace WT.Ecommerce.Services.Customer
{
    public interface ICustomerInformationService
    {
        Task<CustomerInformation> CreateCustomerAsync(CustomerInformation customerInformation);
    }
}
