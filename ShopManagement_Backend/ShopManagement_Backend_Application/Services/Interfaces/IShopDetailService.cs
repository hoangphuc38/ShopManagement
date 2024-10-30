using ShopManagement_Backend_Application.Models;
using ShopManagement_Backend_Application.Models.ShopDetail;

namespace ShopManagement_Backend_Application.Services.Interfaces
{
    public interface IShopDetailService
    {
        BaseResponse GetAllOfShop(int id);

        BaseResponse UpdateDetail(ShopDetailRequest request);

        BaseResponse DeleteDetail(int shopID, int productID);

        BaseResponse CreateDetail(ShopDetailRequest request);
    }
}
