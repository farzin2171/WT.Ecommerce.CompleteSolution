using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WT.Ecommerce.Services.Cache;
using WT.Libraries.Caching.Redis;

namespace WT.Ecommerce.Infrastructure.Installers
{
    public class RedisInstaller : IInstaller
    {
        public void InstallServices(IServiceCollection services, IConfiguration configuration)
        {

            try
            {
                services.AddTransient(typeof(IRedisCache<>), typeof(RedisCache<>));
                services.AddTransient(typeof(IMemoryCache<>), typeof(MemoryCache<>));


                services.AddRedisCacheManager(configuration, options =>
                {
                    options.ConnectionString = configuration.GetConnectionString("Redis");
                });
                

            }
            catch (Exception e)
            {
                // If the Redis instance is unavailable, prevent the app from crashing, this will at least
                // allow us to call the health checks endpoints
                Console.WriteLine(e);
            }
        }
    }
}
