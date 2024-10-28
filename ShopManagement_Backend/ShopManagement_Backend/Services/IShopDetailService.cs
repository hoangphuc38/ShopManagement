using ShopManagement_Backend.Requests;
using ShopManagement_Backend.Responses;

namespace ShopManagement_Backend.Services
{
    public interface IShopDetailService
    {
        BaseResponse GetAllOfShop(int id);

        BaseResponse UpdateDetail(ShopDetailRequest request);

        BaseResponse DeleteDetail(int shopID, int productID);

        BaseResponse CreateDetail(ShopDetailRequest request);
    }
}
