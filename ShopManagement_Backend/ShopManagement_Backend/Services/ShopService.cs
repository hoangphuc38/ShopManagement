using AutoMapper;
using ShopManagement_Backend.Models;
using ShopManagement_Backend.Requests;
using ShopManagement_Backend.Responses;

namespace ShopManagement_Backend.Services
{
    public class ShopService
    {
        private readonly ShopManagementDbContext _context;
        private readonly IMapper _mapper;

        public ShopService(ShopManagementDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public BaseResponse GetAll()
        {
            var shopList = _context.Shops.Where(c => c.IsDeleted == false).ToList();

            foreach (var shop in shopList)
            {
                var user = _context.Users
                                   .Where(c => c.Id == shop.UserId && c.IsDeleted == false)
                                   .FirstOrDefault();                
            }

            var responseList = _mapper.Map<List<ShopResponse>>(shopList);
            return new BaseResponse(responseList);
        }

        public BaseResponse GetShopOfUser(int userID)
        {
            var shopList = _context.Shops
                                   .Where(c => c.UserId == userID && c.IsDeleted == false)
                                   .ToList();
            var responseList = new List<ShopResponse>();

            if (shopList == null)
            {
                return new BaseResponse(StatusCodes.Status404NotFound, "Not found user");
            }

            foreach (var shop in shopList)
            {
                var response = _mapper.Map<ShopResponse>(shop);
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

            var shop = _mapper.Map<Shop>(request);
            shop.UserId = userID;
            shop.CreatedDate = DateOnly.FromDateTime(DateTime.Now);
            shop.IsDeleted = false;

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

            var detailList = _context.ShopDetails.Where(c => c.ShopId == shopID).ToList();
            foreach (var detail in detailList)
            {
                detail.IsDeleted = true;
                _context.ShopDetails.Update(detail);
            }

            _context.SaveChanges();

            return new BaseResponse("Delete shop successfully");
        }
    }
}
