using StackExchange.Redis;

namespace EventPlaner_UI.Helper.Redis
{
    public class RedisConfig: IRedisConfig
    {
        private readonly IConnectionMultiplexer _redis;

        public RedisConfig(IConnectionMultiplexer redis)
        {
            _redis = redis;
        }

        public async Task<string> GetValueAsync(string key)
        {
            var db = _redis.GetDatabase();
            return await db.StringGetAsync(key);
        }

        public async Task SetValueAsync(string key, string value)
        {
            var db = _redis.GetDatabase();
            await db.StringSetAsync(key, value);
        }
    }
}
