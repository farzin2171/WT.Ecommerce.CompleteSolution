using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using WT.Ecommerce.Infrastructure.Extentions;
using WT.Ecommerce.Infrastructure.StartupTasks;

namespace WT.Ecommerce.Infrastructure.Installers
{
    public class StartupTaskInstaller : IInstaller
    {
        public void InstallServices(IServiceCollection services, IConfiguration configuration)
        {
            services.AddStartupTask<InformationStartupTask>();
        }
    }
}
