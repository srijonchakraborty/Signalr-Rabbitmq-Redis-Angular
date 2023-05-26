namespace SignalRPractice.RedisManager
{
    public interface IRedisService
    {
        T Get<T>(string key);
        Task<T> GetAsync<T>(string key);
        void Remove(string key);
        Task RemoveAsync(string key);
        void Set(string key, object data, int absoluteExpiration, int slidingExpiration);
        Task SetAsync(string key, object data);
        Task SetAsync(string key, object data, int absoluteExpiration);
        Task SetAsync(string key, object data, int absoluteExpiration, int slidingExpiration);
    }
}
