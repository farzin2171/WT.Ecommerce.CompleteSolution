namespace WT.Ecommerce.Services.Cache
{
    public interface IRedisCache<T> : ICache<T>
	{
		int LockCacheExpiryInSec { get; set; }
		int LockCacheWaitInSec { get; set; }
		int LockCacheRetryInSec { get; set; }
	}
}
