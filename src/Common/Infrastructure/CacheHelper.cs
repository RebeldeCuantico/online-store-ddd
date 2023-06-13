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

            await AddId(cache, recordId, typeof(T).FullName);
        }

        public static async Task UpdateRecord<T>(this IDistributedCache cache,
                                                string recordId,
                                                T data)
        {
            await RemoveRecord<T>(cache, recordId);
            await SetRecord<T>(cache, recordId, data);
        }

        public static async Task<T?> GetRecord<T>(this IDistributedCache cache,
                                                string recordId)
        {
            var jsonData = await cache.GetStringAsync(recordId);
            if (jsonData == null)
            {
                return default;
            }

            return JsonSerializer.Deserialize<T>(jsonData);
        }

        public static async Task RemoveRecord<T>(this IDistributedCache cache,
                                                string recordId)
        {
            await cache.RemoveAsync(recordId);
            await RemoveIdFromIds(cache, recordId, typeof(T).FullName);
        }

        public static async Task<List<T>> GetAll<T>(this IDistributedCache cache)
        {
            var ids = await GetAllIds(cache, typeof(T).FullName);
            var result = new List<T>();
            foreach (var id in ids)
            {
                result.Add(await GetRecord<T>(cache, id));
            }

            return result;
        }

        private static async Task RemoveIdFromIds(this IDistributedCache cache,
                                                  string recordId,
                                                  string type)
        {
            var ids = await GetAllIds(cache, type);
            var isDeleted = ids.Remove(recordId);
            await cache.RemoveAsync(type);
            await SaveIds(cache, type, ids);
        }

        private static async Task AddId(this IDistributedCache cache,
                                                string recordId,
                                                string type)
        {
            var ids = await GetAllIds(cache, type);
            ids.Add(recordId);
            await SaveIds(cache, type, ids);
        }

        private static async Task SaveIds(IDistributedCache cache, string type, List<string> ids)
        {
            var jsonData = JsonSerializer.Serialize(ids);
            await cache.SetStringAsync(type, jsonData);
        }

        private static async Task<List<string>> GetAllIds(this IDistributedCache cache,
                                                string type)
        {
            var ids = await GetRecord<List<string>>(cache, type);
            if (ids == null)
            {
                ids = new List<string>();
            }

            return ids;
        }
    }
}
