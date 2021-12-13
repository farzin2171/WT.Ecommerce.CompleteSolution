using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using WT.Ecommerce.Domain.Identity;
using WT.Ecommerce.Services.Customer;

namespace WT.Ecommerce.Infrastructure.Installers
{
    public class ServiceInstaller : IInstaller
    {
        public void InstallServices(IServiceCollection services, IConfiguration configuration)
        {
            services.AddHttpClient();
            services.AddHttpContextAccessor();
            services.AddTransient<IIdentityContext>(s => new IdentityContext(s.GetService<IHttpContextAccessor>().HttpContext?.User));
            services.AddScoped<ICustomerInformationService, CustomerInformationService>();
        }
    }
}
