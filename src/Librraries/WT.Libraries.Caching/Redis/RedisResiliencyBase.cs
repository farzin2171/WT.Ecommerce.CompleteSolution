using System;
using System.Threading;
using StackExchange.Redis;

namespace WT.Libraries.Caching.Redis
{
    public abstract class RedisResiliencyBase
    {
        private readonly RedisCacheOptions _options;

        private Lazy<ConnectionMultiplexer> _multiplexer;

        protected long LastReconnectTicks = DateTimeOffset.MinValue.UtcTicks;
        protected DateTimeOffset FirstError = DateTimeOffset.MinValue;
        protected DateTimeOffset PreviousError = DateTimeOffset.MinValue;

        protected readonly object ReconnectLock = new object();

        /// <summary>
        /// Gets the Redis connection
        /// </summary>
        public ConnectionMultiplexer Connection => _multiplexer.Value;

        protected RedisResiliencyBase(RedisCacheOptions options)
        {
            _options = options;
            _multiplexer = CreateMultiplexer();
        }

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
        public virtual bool ForceReconnect()
        {
            var utcNow = DateTimeOffset.UtcNow;
            var previousTicks = Interlocked.Read(ref LastReconnectTicks);
            var previousReconnect = new DateTimeOffset(previousTicks, TimeSpan.Zero);
            var elapsedSinceLastReconnect = utcNow - previousReconnect;

            if (elapsedSinceLastReconnect <= _options.ReconnectMinFrequency)
            {
                return false;
            }

            // If multiple threads call ForceReconnect at the same time, we only want to honor one of them.
            lock (ReconnectLock)
            {
                utcNow = DateTimeOffset.UtcNow;
                elapsedSinceLastReconnect = utcNow - previousReconnect;

                if (FirstError == DateTimeOffset.MinValue)
                {
                    // We haven't seen an error since last reconnect, so set initial values.
                    FirstError = utcNow;
                    PreviousError = utcNow;
                    return false;
                }

                if (elapsedSinceLastReconnect < _options.ReconnectMinFrequency)
                {
                    // Some other thread made it through the check and the lock, so nothing to do.
                    return false;
                }

                var elapsedSinceFirstError = utcNow - FirstError;
                var elapsedSinceMostRecentError = utcNow - PreviousError;

                var shouldReconnect =
                    elapsedSinceFirstError >= _options.ReconnectErrorThreshold   // make sure we gave the multiplexer enough time to reconnect on its own if it can
                    && elapsedSinceMostRecentError <= _options.ReconnectErrorThreshold; //make sure we aren't working on stale data (e.g. if there was a gap in errors, don't reconnect yet).

                // Update the previousError timestamp to be now (e.g. this reconnect request)
                PreviousError = utcNow;

                if (!shouldReconnect)
                {
                    return false;
                }

                FirstError = DateTimeOffset.MinValue;
                PreviousError = DateTimeOffset.MinValue;

                var oldMultiplexer = _multiplexer;
                CloseMultiplexer(oldMultiplexer);
                _multiplexer = CreateMultiplexer();
                Interlocked.Exchange(ref LastReconnectTicks, utcNow.UtcTicks);
                return true;
            }
        }

        private Lazy<ConnectionMultiplexer> CreateMultiplexer()
        {
            return new Lazy<ConnectionMultiplexer>(() => ConnectionMultiplexer.Connect(_options.ConnectionString));
        }

        private void CloseMultiplexer(Lazy<ConnectionMultiplexer> oldMultiplexer)
        {
            if (oldMultiplexer == null)
            {
                return;
            }

            try
            {
                oldMultiplexer.Value.Close();
            }
            catch (Exception)
            {
                // Example error condition: if accessing old.Value causes a connection attempt and that fails.
            }
        }
    }
}
