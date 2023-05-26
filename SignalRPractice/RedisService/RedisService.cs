using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;

namespace SignalRPractice.RedisManager
{
    public class RedisService : IRedisService
    {
        private readonly IDistributedCache _redis;
        public RedisService(IDistributedCache redisCashe)
        {
            _redis = redisCashe;
        }
        public T Get<T>(string key)
        {
            var json = _redis.GetString(key);
            if (json == null)
            {
                return default(T);
            }

            return JsonConvert.DeserializeObject<T>(json);
        }
        public async Task<T> GetAsync<T>(string key)
        {
            var json = await _redis.GetStringAsync(key);
            if (json == null)
            {
                return default(T);
            }

            return JsonConvert.DeserializeObject<T>(json);
        }
        public void Remove(string key)
        {
            _redis.Remove(key);
        }

        public async Task RemoveAsync(string key)
        {
            await _redis.RemoveAsync(key);
        }
        public void Set(string key, object data, int absoluteExpiration, int slidingExpiration)
        {
            var json = JsonConvert.SerializeObject(data);
            var options = new DistributedCacheEntryOptions()
                          .SetAbsoluteExpiration(TimeSpan.FromMinutes(absoluteExpiration))
                          .SetSlidingExpiration(TimeSpan.FromMinutes(slidingExpiration));
            _redis.SetString(key, json, options);
        }
        public async Task SetAsync(string key, object data)
        {
            var json = JsonConvert.SerializeObject(data);
            await _redis.SetStringAsync(key, json);
        }
        public async Task SetAsync(string key, object data, int absoluteExpiration)
        {
            var json = JsonConvert.SerializeObject(data);
            var options = new DistributedCacheEntryOptions().SetAbsoluteExpiration(TimeSpan.FromMinutes(absoluteExpiration));
            await _redis.SetStringAsync(key, json, options);
        }
        public async Task SetAsync(string key, object data, int absoluteExpiration, int slidingExpiration)
        {
            var json = JsonConvert.SerializeObject(data);
            var options = new DistributedCacheEntryOptions()
                          .SetAbsoluteExpiration(TimeSpan.FromMinutes(absoluteExpiration))
                          .SetSlidingExpiration(TimeSpan.FromMinutes(slidingExpiration));
            await _redis.SetStringAsync(key, json, options);
        }
    }
}
