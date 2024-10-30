using ShopManagement_Backend.Requests;
using ShopManagement_Backend.Responses;

namespace ShopManagement_Backend.Services.Interfaces
{
    public interface IShopService
    {
        BaseResponse GetAll();

        BaseResponse GetShopOfUser(int userID);

        BaseResponse UpdateShop(int shopID, ShopRequest request);

        BaseResponse CreateShop(int userID, ShopRequest request);

        BaseResponse DeleteShop(int shopID);
    }
}
