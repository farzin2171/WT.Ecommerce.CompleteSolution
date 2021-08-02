using System;
using System.Collections.Generic;
using RedLockNet.SERedis;
using RedLockNet.SERedis.Configuration;

namespace WT.Libraries.Caching.Redis
{
    public class RedisLockManager : RedisResiliencyBase, IRedisLockManager
    {
        private Lazy<RedLockFactory> _lock;

        /// <summary>
        /// Gets the lock for Redis
        /// </summary>
        public RedLockFactory Lock => _lock.Value;

        public RedisLockManager(RedisCacheOptions options) : base(options)
        {
            _lock = CreateLock();
        }

        /// <inheritdoc cref="RedisResiliencyBase.ForceReconnect" />
        public override bool ForceReconnect()
        {
            lock (ReconnectLock)
            {
                var hasReconnected = base.ForceReconnect();
                if (!hasReconnected)
                {
                    return false;
                }

                var oldLock = _lock;
                CloseLock(oldLock);
                _lock = CreateLock();

                return true;
            }
        }

        private Lazy<RedLockFactory> CreateLock()
        {
            return new Lazy<RedLockFactory>(() => RedLockFactory.Create(new List<RedLockMultiplexer>(1) { Connection }));
        }

        private void CloseLock(Lazy<RedLockFactory> oldLock)
        {
            if (oldLock == null)
            {
                return;
            }

            try
            {
                oldLock.Value.Dispose();
            }
            catch (Exception)
            {
                // Example error condition: if accessing old.Value causes a connection attempt and that fails.
            }
        }
    }
}
