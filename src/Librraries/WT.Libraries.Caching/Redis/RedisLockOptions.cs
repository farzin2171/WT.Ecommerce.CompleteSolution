using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace WT.Libraries.Caching.Redis
{
    /// <summary>
    /// Provides the lock time options for acquiring a lock through the <see cref="IRedisCacheManager"/> AcquireLockAsync method
    /// </summary>
    public class RedisLockOptions
    {
        /// <summary>
        /// Gets or sets the lock expiry. Defaults to 30 seconds
        /// </summary>
        public int ExpiryTimeSpanInSecs { get; set; } = 30;

        /// <summary>
        /// Gets or sets the lock acquire timeout. Defaults to 30 seconds
        /// </summary>
        public int WaitTimeSpanInSecs { get; set; } = 30;

        /// <summary>
        /// Gets or sets the lock acquire retry attempt. Defaults to 5 seconds
        /// </summary>
        public int RetryTimeSpanInSecs { get; set; } = 5;
    }

    public class RedisLockPostConfigureOptions : IPostConfigureOptions<RedisLockOptions>
    {
        private readonly ILogger<RedisLockPostConfigureOptions> _logger;

        public RedisLockPostConfigureOptions(ILogger<RedisLockPostConfigureOptions> logger)
        {
            _logger = logger;
        }

        public void PostConfigure(string name, RedisLockOptions options)
        {
            if (options.ExpiryTimeSpanInSecs <= 0)
            {
                _logger.LogWarning($"RedisLock option {nameof(options.ExpiryTimeSpanInSecs)} contains an unacceptable value. Resetting the value to the default of 30");
                options.ExpiryTimeSpanInSecs = 30;
            }

            if (options.WaitTimeSpanInSecs <= 0)
            {
                _logger.LogWarning($"RedisLock option {nameof(options.WaitTimeSpanInSecs)} contains an unacceptable value. Resetting the value to the default of 30");
                options.WaitTimeSpanInSecs = 30;
            }

            if (options.RetryTimeSpanInSecs <= 0)
            {
                _logger.LogWarning($"RedisLock option {nameof(options.RetryTimeSpanInSecs)} contains an unacceptable value. Resetting the value to the default of 5");
                options.RetryTimeSpanInSecs = 30;
            }
        }
    }
}
