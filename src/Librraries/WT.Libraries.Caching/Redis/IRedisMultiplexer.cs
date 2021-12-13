using StackExchange.Redis;

namespace WT.Libraries.Caching.Redis
{
    public interface IRedisMultiplexer
    {
        /// <summary>
        /// Gets the Redis connection
        /// </summary>
        ConnectionMultiplexer Connection { get; }

        /// <summary>
        /// Gets the Redis database
        /// </summary>
        IDatabase Database { get; }

        /// <summary>
        /// Force a new ConnectionMultiplexer to be created.
        /// </summary>    
        /// <remarks> 
        ///     1. Users of the ConnectionMultiplexer MUST handle ObjectDisposedExceptions, which can now happen as a result of calling ForceReconnect()
        ///     2. Don't call ForceReconnect for Timeouts, just for RedisConnectionExceptions or SocketExceptions
        ///     3. Call this method every time you see a connection exception, the code will wait to reconnect:
        ///         a. for at least the "ReconnectErrorThreshold" time of repeated errors before actually reconnecting
        ///         b. not reconnect more frequently than configured in "ReconnectMinFrequency"
        /// </remarks>
        bool ForceReconnect();
    }
}
