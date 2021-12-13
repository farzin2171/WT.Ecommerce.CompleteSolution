using System;
using System.Threading.Tasks;

namespace WT.Ecommerce.Services.Cache
{
    public interface ICache<T>
	{
		Task<T> GetOrAddAsync(string cacheKey, Func<Task<T>> populator, TimeSpan? expire = null);

		Task RemoveAsync(string cacheKey);
	}
}
