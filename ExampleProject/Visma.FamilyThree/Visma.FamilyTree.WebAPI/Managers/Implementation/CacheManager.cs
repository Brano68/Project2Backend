using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using System;
using Visma.FamilyTree.Composition;
using Visma.FamilyTree.WebAPI.Managers.Interfaces;

namespace Visma.FamilyTree.WebAPI.Managers.Implementation
{
    public class CacheManager : ICacheManager
    {
        public CacheManager(
            IOptions<AppConfiguration> options)
        {
            AppConfiguration = options.Value;
            Cache = new MemoryCache(new MemoryCacheOptions());
        }

        private MemoryCache Cache { get; }

        private AppConfiguration AppConfiguration { get; }

        public bool GetCacheMemoryObject<T>(string key, out T cacheObject)
        {
            bool exist = Cache.TryGetValue<T>(key, out T cache);

            cacheObject = cache;
            return exist;
        }

        public void SetMemory<T>(string key, T cacheObject) =>
                Cache.Set<T>(key, cacheObject, TimeSpan.FromSeconds(AppConfiguration.CacheTimeOutSeconds));

        public void CleanCachedItem(string key) =>
                Cache.Remove(key);
    }
}
