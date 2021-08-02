using System;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;


namespace WT.Libraries.Caching.Redis
{
    public static class Extensions
    {
        /// <summary>
        /// Adds the redis cache manager.
        /// </summary>
        /// <param name="services"></param>
        /// <param name="cacheOptions"></param>
        /// <param name="lockOptions"></param>
        /// <returns></returns>
        public static IServiceCollection AddRedisCacheManager(this IServiceCollection services, Action<RedisCacheOptions> cacheOptions, Action<RedisLockOptions> lockOptions)
        {
            services.Configure<RedisCacheOptions>(cacheOptions);
            services.AddSingleton<RedisCacheOptions>(sp => sp.GetRequiredService<IOptions<RedisCacheOptions>>().Value);
            services.Configure<RedisLockOptions>(lockOptions);

            services.AddSingleton<IPostConfigureOptions<RedisCacheOptions>, RedisCachePostConfigureOptions>();
            services.AddSingleton<IPostConfigureOptions<RedisLockOptions>, RedisLockPostConfigureOptions>();

            var redisLockOptions = new RedisLockOptions();
            lockOptions(redisLockOptions);
            services.AddSingleton(redisLockOptions);

            services.AddSingleton<IRedisMultiplexer, RedisMultiplexer>();
            services.AddSingleton<IRedisLockManager, RedisLockManager>();
            services.AddSingleton<IRedisCacheManager, RedisCacheManager>();

            return services;
        }

        /// <summary>
        /// Adds the redis cache manager. The lock options come from the <see cref="IConfiguration"/>
        /// </summary>
        /// <param name="services"></param>
        /// <param name="configuration"></param>
        /// <param name="cacheOptions"></param>
        /// <returns></returns>
        public static IServiceCollection AddRedisCacheManager(this IServiceCollection services, IConfiguration configuration, Action<RedisCacheOptions> cacheOptions)
        {
            services.Configure<RedisCacheOptions>(cacheOptions);
            services.AddSingleton<RedisCacheOptions>(sp => sp.GetRequiredService<IOptions<RedisCacheOptions>>().Value);
            services.AddSingleton<IPostConfigureOptions<RedisCacheOptions>, RedisCachePostConfigureOptions>();

            services.Configure<RedisLockOptions>(configuration.GetSection("Redis")?.GetSection("Lock"));
            services.AddSingleton<IPostConfigureOptions<RedisLockOptions>, RedisLockPostConfigureOptions>();

            services.AddSingleton<IRedisMultiplexer, RedisMultiplexer>();
            services.AddSingleton<IRedisLockManager, RedisLockManager>();
            services.AddSingleton<IRedisCacheManager, RedisCacheManager>();

            return services;
        }
    }
}
