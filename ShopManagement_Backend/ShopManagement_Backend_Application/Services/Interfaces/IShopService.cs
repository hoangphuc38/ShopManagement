using ShopManagement_Backend_Application.Models;
using ShopManagement_Backend_Application.Models.Shop;

namespace ShopManagement_Backend_Application.Services.Interfaces
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
