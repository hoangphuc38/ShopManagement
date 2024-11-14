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
        private readonly IMemoryCacheService _memoryCacheService;

        public ShopService(
            IMapper mapper,
            IShopRepository shopRepo,
            IUserRepository userRepo,
            IShopDetailRepository shopDetailRepo,
            ILogger<ShopService> logger,
            IMemoryCacheService memoryCacheService)
        {
            _mapper = mapper;
            _shopRepo = shopRepo;
            _userRepo = userRepo;
            _shopDetailRepo = shopDetailRepo;
            _logger = logger;
            _memoryCacheService = memoryCacheService;
        }

        public BaseResponse GetAll()
        {
            try
            {
                _logger.LogInformation($"[GetAllShop] Start to get all shops.");
                var response = _memoryCacheService.GetCacheData("ShopList");

                if (response == null)
                {
                    var shopList = _shopRepo.GetAllAsync(t => !t.IsDeleted);

                    foreach (var shop in shopList)
                    {
                        var user = _userRepo.GetFirstAsync(c => c.Id == shop.UserId && !c.IsDeleted);
                    }

                    var shopListMapper = _mapper.Map<List<ShopResponse>>(shopList);

                    response = new BaseResponse(shopListMapper);

                    _memoryCacheService.SetCache("ShopList", response);
                }
                
                return response;
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
                var response = _memoryCacheService.GetCacheData($"Shop_{userID}");

                if (response == null)
                {
                    var shopList = _shopRepo.GetAllAsync(c => c.UserId == userID && !c.IsDeleted);
                    var responseShopList = new List<ShopResponse>();

                    if (shopList == null)
                    {
                        return new BaseResponse(StatusCodes.Status404NotFound, "Not found user");
                    }

                    foreach (var shop in shopList)
                    {
                        var shopMapper = _mapper.Map<ShopResponse>(shop);
                        var user = _userRepo.GetFirstAsync(c => c.Id == userID && !c.IsDeleted);

                        if (user == null)
                        {
                            return new BaseResponse(
                                StatusCodes.Status404NotFound, "Not found owner of the shop");
                        }

                        shopMapper.OwnerName = user.FullName;

                        responseShopList.Add(shopMapper);
                    }

                    response = new BaseResponse(responseShopList);

                    _memoryCacheService.SetCache($"Shop_{userID}", response);
                }
                
                return response;
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
                _logger.LogInformation($"[UpdateShop] Start to update shop with id: {shopID} " +
                    $"ShopName: {request.ShopName}, ShopAddress: {request.ShopAddress}");
                var shop = _shopRepo.GetFirstAsync(c => c.ShopId == shopID && !c.IsDeleted);

                if (shop == null)
                {
                    return new BaseResponse(StatusCodes.Status404NotFound, "Shop not found");
                }

                shop.ShopName = request.ShopName;
                shop.ShopAddress = request.ShopAddress;

                _shopRepo.UpdateAsync(shop);

                _memoryCacheService.RemoveCache($"Shop_{shop.UserId}");
                _memoryCacheService.RemoveCache("ShopList");

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
                _logger.LogInformation($"[CreateShop] Start to create shop of user with id: {userID} " +
                    $"ShopName: {request.ShopName}, ShopAddress: {request.ShopAddress}");
                var user = _userRepo.GetFirstAsync(c => c.Id == userID && !c.IsDeleted);

                if (user == null)
                {
                    return new BaseResponse(StatusCodes.Status404NotFound, "User not found");
                }

                var shop = _mapper.Map<Shop>(request);
                shop.UserId = userID;
                shop.CreatedDate = DateOnly.FromDateTime(DateTime.Now);
                shop.IsDeleted = false;

                _shopRepo.AddAsync(shop);

                _memoryCacheService.RemoveCache($"Shop_{userID}");
                _memoryCacheService.RemoveCache("ShopList");

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
                var shop = _shopRepo.GetFirstAsync(c => c.ShopId == shopID && !c.IsDeleted);

                if (shop == null)
                {
                    return new BaseResponse(StatusCodes.Status404NotFound, "Shop not found");
                }

                shop.IsDeleted = true;
                _shopRepo.UpdateAsync(shop);

                var detailList = _shopDetailRepo.GetAllAsync(c => c.ShopId == shopID && !c.IsDeleted);
                foreach (var detail in detailList)
                {
                    detail.IsDeleted = true;
                    _shopDetailRepo.UpdateAsync(detail);
                }

                _memoryCacheService.RemoveCache($"Shop_{shop.UserId}");
                _memoryCacheService.RemoveCache("ShopList");

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
