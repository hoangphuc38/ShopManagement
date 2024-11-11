using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using ShopManagement_Backend_Application.Models;
using ShopManagement_Backend_Application.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopManagement_Backend_Application.Services
{
    public class MemoryCacheService : IMemoryCacheService
    {
        private readonly IMemoryCache _cache;
        private readonly IConfiguration _config;
        private readonly MemoryCacheEntryOptions _cacheEntryOptions;

        public MemoryCacheService(
            IMemoryCache cache, 
            IConfiguration config)
        {
            _cache = cache;
            _config = config;
            _cacheEntryOptions = new MemoryCacheEntryOptions()
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.Parse(_config["CacheEntryOptions:AbsoluteExpiration"]),
                SlidingExpiration = TimeSpan.Parse(_config["CacheEntryOptions:SlidingExpiration"]),
            };
        }

        public void RemoveCache(string cacheKey)
        {
            _cache.Remove(cacheKey);
        }

        public bool CheckIfCacheExist(string cacheKey, object? result)
        {
            return _cache.TryGetValue(cacheKey, out result);
        }

        public BaseResponse? GetCacheData(string cacheKey)
        {
            if (_cache.TryGetValue(cacheKey, out BaseResponse? result))
            {
                return result;
            }

            return null;
        }

        public void SetCache(string cacheKey, object response)
        {
            _cache.Set(cacheKey, response, _cacheEntryOptions);
        }
    }
}
