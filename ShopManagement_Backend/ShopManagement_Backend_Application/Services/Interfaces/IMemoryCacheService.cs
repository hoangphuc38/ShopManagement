using ShopManagement_Backend_Application.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopManagement_Backend_Application.Services.Interfaces
{
    public interface IMemoryCacheService
    {
        void SetCache(string cacheKey, object response);

        bool CheckIfCacheExist(string cacheKey, object? result);

        void RemoveCache(string cacheKey);

        BaseResponse? GetCacheData(string cacheKey);
    }
}
