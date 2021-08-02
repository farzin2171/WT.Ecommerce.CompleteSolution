using StackExchange.Redis;

namespace WT.Libraries.Caching.Redis
{
    // inspired from https://gist.github.com/JonCole/925630df72be1351b21440625ff2671f#reconnecting-with-lazyt-pattern
    public class RedisMultiplexer : RedisResiliencyBase, IRedisMultiplexer
    {
        public IDatabase Database => Connection.GetDatabase();

        public RedisMultiplexer(RedisCacheOptions options) : base(options)
        {
        }
    }
}
