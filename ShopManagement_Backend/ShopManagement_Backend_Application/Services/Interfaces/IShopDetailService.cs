using ShopManagement_Backend_Application.Models;
using ShopManagement_Backend_Application.Models.ShopDetail;

namespace ShopManagement_Backend_Application.Services.Interfaces
{
    public interface IShopDetailService
    {
        Task<BaseResponse> GetAllOfShop(int id);

        Task<BaseResponse> UpdateDetail(ShopDetailRequest request);

        Task<BaseResponse> DeleteDetail(int shopID, int productID);

        Task<BaseResponse> CreateDetail(ShopDetailRequest request);
    }
}
