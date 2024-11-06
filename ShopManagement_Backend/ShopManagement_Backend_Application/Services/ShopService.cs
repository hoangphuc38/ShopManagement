using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using ShopManagement_Backend_Application.Models;
using ShopManagement_Backend_Application.Models.Shop;
using ShopManagement_Backend_Application.Services.Interfaces;
using ShopManagement_Backend_Core.Entities;
using ShopManagement_Backend_DataAccess.Repositories.Interfaces;

namespace ShopManagement_Backend_Application.Services
{
    public class ShopService : IShopService
    {
        private readonly IMapper _mapper;
        private readonly IShopRepository _shopRepo;
        private readonly IUserRepository _userRepo;
        private readonly IShopDetailRepository _shopDetailRepo;
        private readonly ILogger<ShopService> _logger;

        public ShopService(
            IMapper mapper,
            IShopRepository shopRepo,
            IUserRepository userRepo,
            IShopDetailRepository shopDetailRepo,
            ILogger<ShopService> logger)
        {
            _mapper = mapper;
            _shopRepo = shopRepo;
            _userRepo = userRepo;
            _shopDetailRepo = shopDetailRepo;
            _logger = logger;
        }

        public BaseResponse GetAll()
        {
            try
            {
                _logger.LogInformation($"[GetAllShop] Start to get all shops.");
                var shopList = _shopRepo.GetAllAsync(c => c.IsDeleted == false);

                foreach (var shop in shopList)
                {
                    var user = _userRepo.GetFirstAsync(c => c.Id == shop.UserId && c.IsDeleted == false);
                }

                var responseList = _mapper.Map<List<ShopResponse>>(shopList);
                return new BaseResponse(responseList);
            }
            catch (Exception ex)
            {
                _logger.LogError($"[GetAllShop] Error: {ex.Message}");
                return new BaseResponse(StatusCodes.Status500InternalServerError, "Failed to get all");
            }
            
        }

        public BaseResponse GetShopOfUser(int userID)
        {
            try
            {
                _logger.LogInformation($"[GetShopOfUser] Start to get shops of user with id: {userID}.");
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
            catch (Exception ex)
            {
                _logger.LogError($"[GetShopOfUser] Error: {ex.Message}");
                return new BaseResponse(StatusCodes.Status500InternalServerError, "Failed to get shop of user");
            }
        }

        public BaseResponse UpdateShop(int shopID, ShopRequest request)
        {
            try
            {
                _logger.LogInformation($"[UpdateShop] Start to update shop with id: {shopID}");
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
            catch (Exception ex)
            {
                _logger.LogError($"[UpdateShop] Error: {ex.Message}");
                return new BaseResponse(StatusCodes.Status500InternalServerError, "Failed to update shop");
            }
        }

        public BaseResponse CreateShop(int userID, ShopRequest request)
        {
            try
            {
                _logger.LogInformation($"[CreateShop] Start to create shop of user with id: {userID}");
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
            catch (Exception ex)
            {
                _logger.LogError($"[CreateShop] Error: {ex.Message}");
                return new BaseResponse(StatusCodes.Status500InternalServerError, "Failed to create shop");
            }
        }

        public BaseResponse DeleteShop(int shopID)
        {
            try
            {
                _logger.LogInformation($"[DeleteShop] Start to delete shop with id: {shopID}");
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
            catch (Exception ex)
            {
                _logger.LogError($"[DeleteShop] Error: {ex.Message}");
                return new BaseResponse(StatusCodes.Status500InternalServerError, "Failed to delete shop");
            }
        }
    }
}
