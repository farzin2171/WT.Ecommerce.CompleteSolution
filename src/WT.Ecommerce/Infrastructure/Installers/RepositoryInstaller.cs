using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using WT.Ecommerce.Data.Repositories;
using WT.Ecommerce.Data.Repositories.Interfaces;

namespace WT.Ecommerce.Infrastructure.Installers
{
    public class RepositoryInstaller : IInstaller
    {
        public void InstallServices(IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<ICustomerInformationRepository, CustomerInformationRepository>();
        }
    }
}
