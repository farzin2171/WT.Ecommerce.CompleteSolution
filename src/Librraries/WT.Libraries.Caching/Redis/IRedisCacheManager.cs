using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using RedLockNet;

namespace WT.Libraries.Caching.Redis
{
    public interface IRedisCacheManager
    {
        /// <summary>
        /// Acquires a blocking distributed lock, giving up immediately if the lock is not available
        /// </summary>
        /// <param name="resource">The lock key</param>
        /// <param name="expiry">The expiration of the lock in case of a problem (crash, etc)</param>
        /// <returns>A disposable lock</returns>
        /// <code>
        /// using (var lock = await redisCacheManager.AcquireLockAsync(resource, expiry))
        /// {
        ///    if (lock.IsAcquired) // make sure we got the lock
        ///    {
        ///        // do stuff
        ///    }
        /// } // the lock is automatically released at the end of the using block
        /// </code>
        Task<IRedLock> AcquireLockAsync(string resource, TimeSpan expiry);

        /// <summary>
        /// Acquires a blocking distributed lock, blocking and retrying every <see cref="retry"/> seconds until the lock is available, or <see cref="wait"/> seconds have passed
        /// </summary>
        /// <param name="resource">The lock key</param>
        /// <param name="expiry">The expiration of the lock in case of a problem (crash, etc)</param>
        /// <param name="wait">The time to max wait timeout</param>
        /// <param name="retry">The retry interval</param>
        /// <returns>A disposable lock</returns>
        /// <code>
        /// using (var lock = await redisCacheManager.AcquireLockAsync(resource, expiry, wait, retry))
        /// {
        ///    if (lock.IsAcquired) // make sure we got the lock
        ///    {
        ///        // do stuff
        ///    }
        /// } // the lock is automatically released at the end of the using block
        /// </code>
        Task<IRedLock> AcquireLockAsync(string resource, TimeSpan expiry, TimeSpan wait, TimeSpan retry);

        /// <summary>
        /// Adds a string to the cache
        /// </summary>
        /// <param name="key">The cache key</param>
        /// <param name="value">The cache value</param>
        /// <param name="expiry">The expiry of the item</param>
        /// <returns>True if the item was added to the cache, false otherwise</returns>
        Task<bool> AddStringAsync(string key, string value, TimeSpan? expiry);

        /// <summary>
        /// Retrieve an item as string from the cache
        /// </summary>
        /// <param name="key">The item key</param>
        /// <returns>The value of the item if found</returns>
        Task<string> GetStringAsync(string key);

        /// <summary>
        /// Determines whether a set contains the value
        /// </summary>
        /// <param name="setKey">The set key</param>
        /// <param name="value">The value to look for</param>
        /// <returns>True if the value is in the set, false otherwise</returns>
        Task<bool> SetContainsAsync(string setKey, string value);

        /// <summary>
        /// Removes an item from the cache
        /// </summary>
        /// <param name="key">The item key</param>
        /// <returns>True if the operation was successful, false if not</returns>
        Task<bool> RemoveAsync(string key);

        /// <summary>
        /// Checks if a key exists in the cache
        /// </summary>
        /// <param name="key">They cache key</param>
        /// <returns>True if key exists, false otherwise</returns>
        Task<bool> KeyExistsAsync(string key);

        /// <summary>
        /// Adds a set to the cache
        /// </summary>
        /// <param name="key">The cache key</param>
        /// <param name="values">The cache set value</param>
        /// <param name="expiry">Duration to keep the set in the cache</param>
        /// <returns>Number of affected records</returns>
        Task<long> SetAddAsync(string key, IEnumerable<string> values, TimeSpan expiry);

        /// <summary>
        /// Adds a set to the cache
        /// </summary>
        /// <param name="key">The cache key</param>
        /// <param name="value">The cache value</param>
        /// <param name="expiry">Duration to keep the set in the cache</param>
        /// <returns>true / false if succeed or not</returns>
        Task<bool> SetAddAsync(string key, string value, TimeSpan expiry);

        /// <summary>
        /// Removes an item of particular set from the cache
        /// </summary>
        /// <param name="key">The cache key</param>
        /// <param name="value">The cache value of a set</param>
        /// <returns>true / false if succeed or not</returns>
        Task<bool> SetRemoveAsync(string key, string value);

        /// <summary>
        /// Retrieves a set from the cache
        /// </summary>
        /// <param name="key">The cache key</param>
        /// <returns>Number of affected records</returns>
        Task<IEnumerable<string>> GetSetAsync(string key);

        /// <summary>
        /// Find if a value exists for a specific key
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        Task<bool> GetKeyExistsAsync(string key);

        /// <summary>
        /// Set a HashTable value in the Cache
        /// </summary>
        /// <param name="key">Cache Key</param>
        /// <param name="hashTable">Hash Table</param>
        /// <returns></returns>
        Task HashSetAsync(string key, IDictionary<string, string> hashTable);

        /// <summary>
        /// Get All Values and keys from a Hash Table
        /// </summary>
        /// <param name="key">Cache Key</param>
        /// <returns>Hash Table</returns>
        Task<IDictionary<string, string>> HashGetAllAsync(string key);

        /// <summary>
        /// Set Key Time To Live
        /// </summary>
        /// <param name="key">Cache Key</param>
        /// <param name="timeToLive">Time To Live in seconds</param>
        /// <returns>Is the call was successful</returns>
        Task<bool> KeyExpireAsync(string key, int timeToLive);

        /// <summary>
        /// Returns the keys from a pattern
        /// </summary>
        /// <param name="keyPattern">The key pattern</param>
        /// <returns>A list of keys matching the pattern</returns>
        IEnumerable<string> Keys(string keyPattern);

        /// <summary>
        /// Forces a reconnection of the redis multiplexer and lock manager
        /// </summary>
        void ForceReconnect();
    }
}
