using ShopManagement_Backend.Models;
using ShopManagement_Backend.Requests;
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
                    ShopID = shop.ShopId,
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
                    ShopID = shop.ShopId,
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

        public BaseResponse UpdateShop(int shopID, ShopRequest request)
        {
            var shop = _context.Shops.Find(shopID);

            if (shop == null)
            {
                return new BaseResponse(StatusCodes.Status404NotFound, "Shop not found");
            }

            shop.ShopName = request.ShopName;
            shop.ShopAddress = request.ShopAddress;
            
            _context.Shops.Update(shop);
            _context.SaveChanges();

            return new BaseResponse(shop);
        }

        public BaseResponse CreateShop(int userID, ShopRequest request)
        {
            var user = _context.Users.Find(userID);

            if (user == null)
            {
                return new BaseResponse(StatusCodes.Status404NotFound, "User not found");
            }

            Shop shop = new Shop
            {
                UserId = userID,
                ShopName = request.ShopName,
                ShopAddress = request.ShopAddress,
                CreatedDate = DateOnly.FromDateTime(DateTime.Now),
                IsDeleted = false,
            };

            _context.Shops.Add(shop);
            _context.SaveChanges();

            return new BaseResponse("Create new shop successfully");
        }

        public BaseResponse DeleteShop(int shopID)
        {
            var shop = _context.Shops.Find(shopID);

            if (shop == null)
            {
                return new BaseResponse(StatusCodes.Status404NotFound, "Shop not found");
            }

            shop.IsDeleted = true;

            _context.Shops.Update(shop);
            _context.SaveChanges();

            return new BaseResponse("Delete shop successfully");
        }
    }
}
