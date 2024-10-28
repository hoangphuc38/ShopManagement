using AutoMapper;
using ShopManagement_Backend.Models;
using ShopManagement_Backend.Repositories;
using ShopManagement_Backend.Requests;
using ShopManagement_Backend.Responses;

namespace ShopManagement_Backend.Services.Impl
{
    public class ShopService : IShopService
    {
        private readonly IMapper _mapper;
        private readonly IShopRepository _shopRepo;
        private readonly IUserRepository _userRepo;
        private readonly IShopDetailRepository _shopDetailRepo;

        public ShopService(
            IMapper mapper,
            IShopRepository shopRepo,
            IUserRepository userRepo,
            IShopDetailRepository shopDetailRepo)
        {
            _mapper = mapper;
            _shopRepo = shopRepo;
            _userRepo = userRepo;
            _shopDetailRepo = shopDetailRepo;
        }

        public BaseResponse GetAll()
        {
            var shopList = _shopRepo.GetAllAsync(c => c.IsDeleted == false);

            foreach (var shop in shopList)
            {
                var user = _userRepo.GetFirstAsync(c => c.Id == shop.UserId && c.IsDeleted == false);
            }

            var responseList = _mapper.Map<List<ShopResponse>>(shopList);
            return new BaseResponse(responseList);
        }

        public BaseResponse GetShopOfUser(int userID)
        {
            var shopList = _shopRepo.GetAllAsync(c => c.UserId == userID && c.IsDeleted == false);
            var responseList = new List<ShopResponse>();

            if (shopList == null)
            {
                return new BaseResponse(StatusCodes.Status404NotFound, "Not found user");
            }

            foreach (var shop in shopList)
            {
                var response = _mapper.Map<ShopResponse>(shop);
                var user = _userRepo.GetFirstAsync(c => c.Id == userID);

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
            var shop = _shopRepo.GetFirstAsync(c => c.ShopId == shopID);

            if (shop == null)
            {
                return new BaseResponse(StatusCodes.Status404NotFound, "Shop not found");
            }

            shop.ShopName = request.ShopName;
            shop.ShopAddress = request.ShopAddress;

            _shopRepo.UpdateAsync(shop);

            return new BaseResponse("Update shop successfully");
        }

        public BaseResponse CreateShop(int userID, ShopRequest request)
        {
            var user = _userRepo.GetFirstAsync(c => c.Id == userID);

            if (user == null)
            {
                return new BaseResponse(StatusCodes.Status404NotFound, "User not found");
            }

            var shop = _mapper.Map<Shop>(request);
            shop.UserId = userID;
            shop.CreatedDate = DateOnly.FromDateTime(DateTime.Now);
            shop.IsDeleted = false;

            _shopRepo.AddAsync(shop);

            return new BaseResponse("Create new shop successfully");
        }

        public BaseResponse DeleteShop(int shopID)
        {
            var shop = _shopRepo.GetFirstAsync(c => c.ShopId == shopID);

            if (shop == null)
            {
                return new BaseResponse(StatusCodes.Status404NotFound, "Shop not found");
            }

            shop.IsDeleted = true;
            _shopRepo.UpdateAsync(shop);

            var detailList = _shopDetailRepo.GetAllAsync(c => c.ShopId == shopID);
            foreach (var detail in detailList)
            {
                detail.IsDeleted = true;
                _shopDetailRepo.UpdateAsync(detail);
            }

            return new BaseResponse("Delete shop successfully");
        }
    }
}
