using Microsoft.Extensions.DependencyInjection;
using WT.Ecommerce.Infrastructure.StartupTasks;

namespace WT.Ecommerce.Infrastructure.Extentions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddStartupTask<T>(this IServiceCollection services) where T : class, IStartupTask => services.AddTransient<IStartupTask, T>();
    }
}
