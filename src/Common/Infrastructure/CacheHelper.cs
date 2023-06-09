using Microsoft.Extensions.Caching.Distributed;
using System.Text.Json;

namespace Common.Infrastructure
{
    public static class CacheHelper
    {
        public static async Task SetRecord<T>(this IDistributedCache cache,
                                                string recordId,
                                                T data,
                                                TimeSpan? absoluteExpireTime = null,
                                                TimeSpan? slidingExpireTime = null)
        {
            //TODO: implement expiration time

            var jsonData = JsonSerializer.Serialize(data);
            await cache.SetStringAsync(recordId, jsonData);
        }

        public static async Task<T?> GetRecord<T>(this IDistributedCache cache,
                                                string recordId)
        {
            var jsonData = await cache.GetStringAsync(recordId);
            if(jsonData == null)
            {
                return default;
            }

            return JsonSerializer.Deserialize<T>(jsonData);
        }

        public static async Task RemoveRecord<T>(this IDistributedCache cache,
                                                string recordId)
        {
            await cache.RemoveAsync(recordId);
        }


    }
}
