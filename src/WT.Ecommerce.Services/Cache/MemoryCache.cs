using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.Threading;

namespace WT.Ecommerce.Services.Cache
{
	public class MemoryCache<T> : IMemoryCache<T>
	{
		[System.Diagnostics.CodeAnalysis.SuppressMessage("Usage", "VSTHRD012:Provide JoinableTaskFactory where allowed", Justification = "Not needed here.")]
		private readonly AsyncReaderWriterLock _lock = new AsyncReaderWriterLock();

		private readonly IMemoryCache _innerMemoryCache;
		private readonly ILogger _logger;

		public MemoryCache(ILogger<MemoryCache<T>> logger, IMemoryCache innerMemoryCache)
		{
			_logger = logger;
			_innerMemoryCache = innerMemoryCache;
		}

		public async Task<T> GetOrAddAsync(string cacheKey, Func<Task<T>> populator, TimeSpan? expire = null)
		{
			_logger.LogDebug("Requesting cache entry {CacheKey}", cacheKey);

			T value;
			var readLock = await _lock.ReadLockAsync();
			try
			{
				if (_innerMemoryCache.TryGetValue(cacheKey, out value))
				{
					return value;
				}
			}
			finally
			{
				await readLock.ReleaseAsync();
			}

			var upgradeableReadLock = await _lock.UpgradeableReadLockAsync();
			try
			{
				if (_innerMemoryCache.TryGetValue(cacheKey, out value))
				{
					return value;
				}

				var writeLock = await _lock.WriteLockAsync();
				try
				{
					if (_innerMemoryCache.TryGetValue(cacheKey, out value))
					{
						return value;
					}

					_logger.LogDebug("Generating new entry");
					var entity = await populator();

					if (entity == null)
					{
						_logger.LogError("New generated entry is null");
						throw new Exception("New generated entry is null");
					}

					_logger.LogDebug("Adding new entry {CacheKey} in cache", cacheKey);
					if (expire.HasValue)
					{
						_innerMemoryCache.Set(cacheKey, entity, expire.Value);
					}
					else
					{
						_innerMemoryCache.Set(cacheKey, entity);
					}

					return entity;
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

		public Task RemoveAsync(string cacheKey)
		{
			_logger.LogDebug("Removing entry {CacheKey} from cache", cacheKey);
			_innerMemoryCache.Remove(cacheKey);

			return Task.CompletedTask;
		}
	}
}
