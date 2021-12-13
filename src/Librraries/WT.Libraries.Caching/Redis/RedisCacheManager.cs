using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using RedLockNet;
using StackExchange.Redis;

namespace WT.Libraries.Caching.Redis
{
    /// <summary>
    /// Encapsulates all the Redis cache management
    /// </summary>
    public class RedisCacheManager : IRedisCacheManager
    {
        private readonly IRedisMultiplexer _redis;
        private readonly IRedisLockManager _lockManager;
        private readonly object _lock = new object();

        public IDatabase Database => _redis.Database;

        /// <summary>
        /// Creates a <see cref="RedisCacheManager"/>
        /// </summary>
        /// <param name="redis">The redis connection</param>
        /// <param name="lockManager">The redis lock manager</param>
        public RedisCacheManager(IRedisMultiplexer redis, IRedisLockManager lockManager)
        {
            _redis = redis;
            _lockManager = lockManager;
        }

        /// <inheritdoc />
        public async Task<bool> AddStringAsync(string key, string value, TimeSpan? expiry)
        {
            return await Database.StringSetAsync(key, value, expiry);
        }

        /// <inheritdoc />
        public async Task<string> GetStringAsync(string key)
        {
            return await Database.StringGetAsync(key);
        }

        /// <inheritdoc />
        public async Task<IRedLock> AcquireLockAsync(string resource, TimeSpan expiry)
        {
            return await _lockManager.Lock.CreateLockAsync(resource, expiry);
        }

        /// <inheritdoc />
        public async Task<IRedLock> AcquireLockAsync(string resource, TimeSpan expiry, TimeSpan wait, TimeSpan retry)
        {
            return await _lockManager.Lock.CreateLockAsync(resource, expiry, wait, retry);
        }

        /// <inheritdoc />
        public async Task<bool> SetContainsAsync(string setKey, string value)
        {
            return await Database.SetContainsAsync(setKey, value);
        }

        /// <inheritdoc />
        public async Task<bool> RemoveAsync(string key)
        {
            return await Database.KeyDeleteAsync(key);
        }

        /// <inheritdoc />
        public async Task<bool> KeyExistsAsync(string key)
        {
            return await Database.KeyExistsAsync(key);
        }

        /// <inheritdoc />
        public async Task<long> SetAddAsync(string key, IEnumerable<string> values, TimeSpan expiry)
        {
            var items = values.Select(value => (RedisValue)value).ToArray();
            var addSetResult = await Database.SetAddAsync(key, items);
            await Database.KeyExpireAsync(key, expiry);
            return addSetResult;
        }

        /// <inheritdoc />
        public async Task<bool> SetAddAsync(string key, string value, TimeSpan expiry)
        {
            var success = await Database.SetAddAsync(key, (RedisValue)value);
            if (success)
            {
                await Database.KeyExpireAsync(key, expiry);
            }
            return success;
        }

        /// <inheritdoc />
        public async Task<bool> SetRemoveAsync(string key, string value)
        {
            return await Database.SetRemoveAsync(key, (RedisValue)value);
        }


        /// <inheritdoc />
        public async Task<IEnumerable<string>> GetSetAsync(string key)
        {
            return (await Database.SetMembersAsync(key)).Select(r => r.ToString()).ToArray();
        }

        /// <inheritdoc />
        public async Task<bool> GetKeyExistsAsync(string key)
        {
            return await Database.KeyExistsAsync(key);
        }

        /// <inheritdoc />
        public async Task HashSetAsync(string key, IDictionary<string, string> hashTable)
        {
            HashEntry[] hashEntries = hashTable.Select(he => new HashEntry(he.Key, he.Value)).ToArray();
            await Database.HashSetAsync(key, hashEntries);
        }

        /// <inheritdoc />
        public async Task<IDictionary<string, string>> HashGetAllAsync(string key)
        {
            return (await Database.HashGetAllAsync(key)).ToDictionary(he => he.Name.ToString(), he => he.Value.ToString());
        }

        /// <inheritdoc />
        public async Task<bool> KeyExpireAsync(string key, int timeToLive)
        {
            return await Database.KeyExpireAsync(key, TimeSpan.FromSeconds(timeToLive));
        }

        /// <inheritdoc />
        public IEnumerable<string> Keys(string keyPattern)
        {
            var endpoints = _redis.Connection.GetEndPoints();
            foreach (var endPoint in endpoints)
            {
                var keys = _redis.Connection.GetServer(endPoint).Keys(pattern: keyPattern);
                foreach (var key in keys)
                {
                    yield return key;
                }
            }
        }

        public void ForceReconnect()
        {
            lock (_lock)
            {
                _redis.ForceReconnect();
                _lockManager.ForceReconnect();
            }
        }
    }
}
