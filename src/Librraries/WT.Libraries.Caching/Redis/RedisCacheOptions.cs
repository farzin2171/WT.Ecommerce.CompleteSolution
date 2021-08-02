using System;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;


namespace WT.Libraries.Caching.Redis
{
    public class RedisCacheOptions
    {
        /// <summary>
        /// Gets or sets the frequency, in seconds, at which we will reconnect. In general, StackExchange.Redis handles most reconnects. Defaults to 60.
        /// </summary>
        public int ReconnectMinFrequencyInSeconds { get; set; } = 60;

        /// <summary>
        /// Gets the frequency at which we will reconnect. In general, StackExchange.Redis handles most reconnects. Defaults to 60 seconds.
        /// </summary>
        public TimeSpan ReconnectMinFrequency => TimeSpan.FromSeconds(ReconnectMinFrequencyInSeconds);

        /// <summary>
        /// Gets or sets the threshold, in seconds, at which the multiplexer will be re-created if a reconnection fails. Defaults to 30.
        /// </summary>
        public int ReconnectErrorThresholdInSeconds { get; set; } = 30;

        /// <summary>
        /// Gets the threshold at which the multiplexer will be re-created if a reconnection fails. Defaults to 30 seconds.
        /// </summary>
        public TimeSpan ReconnectErrorThreshold => TimeSpan.FromSeconds(ReconnectErrorThresholdInSeconds);

        /// <summary>
        /// Gets or sets the key prefix of the keys that will be set by the application in the ache. Defaults to empty
        /// </summary>
        public string KeyPrefix { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the connection string
        /// </summary>
        public string ConnectionString { get; set; }
    }

    public class RedisCachePostConfigureOptions : IPostConfigureOptions<RedisCacheOptions>
    {
        private readonly ILogger<RedisCachePostConfigureOptions> _logger;

        public RedisCachePostConfigureOptions(ILogger<RedisCachePostConfigureOptions> logger)
        {
            _logger = logger;
        }

        public void PostConfigure(string name, RedisCacheOptions options)
        {
            if (string.IsNullOrWhiteSpace(options.ConnectionString))
            {
                throw new Exception($"RedisCache option {nameof(options.ConnectionString)} is null or empty");
            }

            if (options.KeyPrefix == null)
            {
                _logger.LogWarning($"RedisCache option {nameof(options.KeyPrefix)} is null. Setting it to empty");
                options.KeyPrefix = string.Empty;
            }
        }
    }
}
