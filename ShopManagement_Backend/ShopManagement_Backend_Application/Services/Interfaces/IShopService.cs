using ShopManagement_Backend_Application.Models;
using ShopManagement_Backend_Application.Models.Shop;

namespace ShopManagement_Backend_Application.Services.Interfaces
{
    public interface IShopService
    {
        Task<BaseResponse> GetShopsWithPagination(ShopPaginationRequest request);

        Task<BaseResponse> GetShopOfUser(int userID, ShopPaginationRequest request);

        Task<BaseResponse> UpdateShop(int shopID, ShopRequest request);

        Task<BaseResponse> CreateShop(int userID, ShopRequest request);

        Task<BaseResponse> DeleteShop(int shopID);
    }
}
