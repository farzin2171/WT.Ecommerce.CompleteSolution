using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace WT.Ecommerce.Infrastructure.Installers
{
    public interface IInstaller
    {
        void InstallServices(IServiceCollection services, IConfiguration configuration);
    }
}
