using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.Threading;
using System.Text.Json;
using System;
using System.Threading.Tasks;
using WT.Libraries.Caching.Redis;

namespace WT.Ecommerce.Services.Cache
{
    public class RedisCache<T> : IRedisCache<T>
	{
		[System.Diagnostics.CodeAnalysis.SuppressMessage("Usage", "VSTHRD012:Provide JoinableTaskFactory where allowed", Justification = "Not needed here.")]
		private readonly AsyncReaderWriterLock _lock = new AsyncReaderWriterLock();

		private readonly IRedisCacheManager _redisCacheManager;
		private readonly ILogger _logger;

		public int LockCacheExpiryInSec { get; set; } = 30;
		public int LockCacheWaitInSec { get; set; } = 30;
		public int LockCacheRetryInSec { get; set; } = 5;

		public RedisCache(IRedisCacheManager redisCacheManager,
			ILogger<RedisCache<T>> logger)
		{
			_redisCacheManager = redisCacheManager;
			_logger = logger;
		}

		public async Task<T> GetOrAddAsync(string cacheKey, Func<Task<T>> populator, TimeSpan? expire = null)
		{
			_logger.LogDebug("Requesting cache entry {CacheKey}", cacheKey);

			var readLock = await _lock.ReadLockAsync();
			try
			{
				var cacheResult = await _redisCacheManager.GetStringAsync(cacheKey);
				if (cacheResult != null)
				{
					var cacheObject = Deserialize(cacheResult);
					return cacheObject;
				}
			}
			finally
			{
				await readLock.ReleaseAsync();
			}

			var upgradeableReadLock = await _lock.UpgradeableReadLockAsync();
			try
			{
				var cacheResult = await _redisCacheManager.GetStringAsync(cacheKey);
				if (cacheResult != null)
				{
					var cacheObject = Deserialize(cacheResult);
					return cacheObject;
				}

				var writeLock = await _lock.WriteLockAsync();
				try
				{
					cacheResult = await _redisCacheManager.GetStringAsync(cacheKey);
					if (cacheResult != null)
					{
						var cacheObject = Deserialize(cacheResult);
						return cacheObject;
					}

					var lockKey = $"{cacheKey}_lock";
					_logger.LogDebug("Requesting lock {LockKey}", lockKey);
					using (var cacheLock = await _redisCacheManager.AcquireLockAsync(lockKey, TimeSpan.FromSeconds(LockCacheExpiryInSec), TimeSpan.FromSeconds(LockCacheWaitInSec), TimeSpan.FromSeconds(LockCacheRetryInSec)))
					{
						cacheResult = await _redisCacheManager.GetStringAsync(cacheKey);
						if (cacheResult != null)
						{
							var cacheObject = Deserialize(cacheResult);
							return cacheObject;
						}

						if (!cacheLock.IsAcquired)
						{
							_logger.LogError("Could not acquire lock");
							throw new Exception("Could not acquire lock");
						}
						_logger.LogDebug("Lock acquired");

						_logger.LogDebug("Generating new entry");
						var entity = await populator();

						if (entity == null)
						{
							_logger.LogError("New generated entry is null");
							throw new Exception("New generated entry is null");
						}

						var serializedEntity = Serialize(entity);

						_logger.LogDebug("Adding new entry {CacheKey} in cache", cacheKey);
						await _redisCacheManager.AddStringAsync(cacheKey, serializedEntity, expire);

						return entity;
					}
				}
				finally
				{
					await writeLock.ReleaseAsync();
				}
			}
			finally
			{
				await upgradeableReadLock.ReleaseAsync();
			}
		}

		public async Task RemoveAsync(string cacheKey)
		{
			_logger.LogDebug("Removing entry {CacheKey} from cache", cacheKey);
			await _redisCacheManager.RemoveAsync(cacheKey);
		}

		private static string Serialize(T entity)
		{
			if (typeof(T) == typeof(string))
			{
				return entity.ToString();
			}
			else
			{
				return JsonSerializer.Serialize(entity);
			}
		}

		private static dynamic Deserialize(string serializedEntity)
		{
			if (typeof(T) == typeof(string))
			{
				return serializedEntity;
			}
			else
			{
				return JsonSerializer.Deserialize<T>(serializedEntity);
			}
		}
	}
}
