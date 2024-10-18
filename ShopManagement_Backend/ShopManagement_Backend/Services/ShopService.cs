using ShopManagement_Backend.Models;
using ShopManagement_Backend.Responses;

namespace ShopManagement_Backend.Services
{
    public class ShopService
    {
        private readonly ShopManagementDbContext _context;

        public ShopService(ShopManagementDbContext context)
        {
            _context = context;
        }

        public BaseResponse GetAll()
        {
            var shopList = _context.Shops.Where(c => c.IsDeleted == false).ToList();
            var responseList = new List<ShopResponse>();

            foreach (var shop in shopList)
            {
                ShopResponse response = new ShopResponse
                {
                    ShopName = shop.ShopName,
                    ShopAddress = shop.ShopAddress,
                    CreatedDate = shop.CreatedDate,
                };

                var user = _context.Users.Find(shop.UserId);

                if (user == null)
                {
                    return new BaseResponse(
                        StatusCodes.Status404NotFound, "Not found owner of the shop");
                }

                response.OwnerName = user.FullName; 

                responseList.Add(response);
            }

            return new BaseResponse(responseList);
        }

        public BaseResponse GetShopOfUser(int userID)
        {
            var shopList = _context.Shops.Where(c => c.UserId == userID).ToList();
            var responseList = new List<ShopResponse>();

            if (shopList == null)
            {
                return new BaseResponse(StatusCodes.Status404NotFound, "Not found user");
            }

            foreach (var shop in shopList)
            {
                ShopResponse response = new ShopResponse
                {
                    ShopName = shop.ShopName,
                    ShopAddress = shop.ShopAddress,
                    CreatedDate = shop.CreatedDate,
                };
                var user = _context.Users.Find(shop.UserId);

                if (user == null)
                {
                    return new BaseResponse(
                        StatusCodes.Status404NotFound, "Not found owner of the shop");
                }

                response.OwnerName = user.FullName;

                responseList.Add(response);
            }

            return new BaseResponse(responseList);
        }
    }
}
